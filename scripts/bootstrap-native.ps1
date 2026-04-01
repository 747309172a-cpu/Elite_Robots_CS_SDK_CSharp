param(
    [Parameter(Mandatory = $true)][string]$RepoRoot,
    [Parameter(Mandatory = $true)][string]$ProjectOutputDir,
    [string]$RepoUrl = "",
    [string]$RepoRef = "main",
    [string]$ForceRebuild = "false"
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($RepoUrl)) {
    $originUrl = ""
    try {
        $originUrl = (git -C $RepoRoot remote get-url origin).Trim()
    }
    catch {
        $originUrl = ""
    }

    if ($originUrl -match '^https://github\.com/.+/.+\.git$') {
        $RepoUrl = ($originUrl -replace '/[^/]+\.git$', '/Elite_Robots_CS_SDK_C.git')
    } elseif ($originUrl -match '^git@github\.com:.+/.+\.git$') {
        $RepoUrl = ($originUrl -replace '/[^/]+\.git$', '/Elite_Robots_CS_SDK_C.git')
    }
}

if ([string]::IsNullOrWhiteSpace($RepoUrl)) {
    throw "Native repository URL is not set. Pass EliteNativeRepoUrl or configure origin so it can be derived automatically."
}

$arch = [System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture.ToString().ToLowerInvariant()
switch ($arch) {
    "x64" { $ridArch = "x64" }
    "arm64" { $ridArch = "arm64" }
    default { throw "Unsupported architecture: $arch" }
}

$rid = "win-$ridArch"
$sourceDir = Join-Path $RepoRoot ".native-src/elite_cs_series_sdk_c"
$buildDir = Join-Path $RepoRoot ".native-build/$rid"
$cacheDir = Join-Path $RepoRoot ".native-out/$rid"
$stampFile = Join-Path $cacheDir ".bootstrap-complete"

New-Item -ItemType Directory -Force -Path (Join-Path $RepoRoot ".native-src") | Out-Null
New-Item -ItemType Directory -Force -Path (Join-Path $RepoRoot ".native-build") | Out-Null
New-Item -ItemType Directory -Force -Path $cacheDir | Out-Null
New-Item -ItemType Directory -Force -Path $ProjectOutputDir | Out-Null

if (-not (Test-Path (Join-Path $sourceDir ".git"))) {
    Write-Host "[bootstrap-native] Cloning $RepoUrl ($RepoRef)..."
    if (Test-Path $sourceDir) {
        Remove-Item -Recurse -Force $sourceDir
    }
    git clone --depth 1 --branch $RepoRef $RepoUrl $sourceDir
}

if ($ForceRebuild -eq "true") {
    Write-Host "[bootstrap-native] Force rebuild requested."
    if (Test-Path $buildDir) {
        Remove-Item -Recurse -Force $buildDir
    }
    if (Test-Path $cacheDir) {
        Remove-Item -Recurse -Force $cacheDir
    }
    New-Item -ItemType Directory -Force -Path $cacheDir | Out-Null
}

if (-not (Test-Path $stampFile)) {
    Write-Host "[bootstrap-native] Configuring native library for $rid..."
    cmake `
        -S $sourceDir `
        -B $buildDir `
        -DELITE_AUTO_FETCH_SDK=ON `
        -DELITE_BUILD_EXAMPLES=OFF

    Write-Host "[bootstrap-native] Building native library for $rid..."
    cmake --build $buildDir --config Release

    Get-ChildItem -Path $cacheDir -File -ErrorAction SilentlyContinue | Remove-Item -Force
    Get-ChildItem -Path $buildDir -Recurse -File -Filter *.dll | ForEach-Object {
        Copy-Item -Force $_.FullName (Join-Path $cacheDir $_.Name)
    }

    if (-not (Test-Path (Join-Path $cacheDir "elite_cs_series_sdk_c.dll"))) {
        throw "Failed to find built native library in $buildDir"
    }

    New-Item -ItemType File -Force -Path $stampFile | Out-Null
}

Get-ChildItem -Path $cacheDir -File | Where-Object { $_.Name -ne ".bootstrap-complete" } | ForEach-Object {
    Copy-Item -Force $_.FullName (Join-Path $ProjectOutputDir $_.Name)
}
