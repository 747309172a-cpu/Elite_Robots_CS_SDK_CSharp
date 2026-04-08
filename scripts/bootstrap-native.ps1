param(
    [Parameter(Mandatory = $true)][string]$RepoRoot,
    [Parameter(Mandatory = $true)][string]$ProjectOutputDir,
    [string]$RepoUrl = "",
    [string]$RepoRef = "main",
    [string]$ForceRebuild = "false"
)

$ErrorActionPreference = "Stop"

function Invoke-NativeCommand {
    param(
        [Parameter(Mandatory = $true)][scriptblock]$Command,
        [Parameter(Mandatory = $true)][string]$FailureMessage
    )

    & $Command
    if ($LASTEXITCODE -ne 0) {
        throw $FailureMessage
    }
}

function Get-CMakeGeneratorArgs {
    param([string]$RidArch)

    if (Get-Command ninja -ErrorAction SilentlyContinue) {
        return @("-G", "Ninja")
    }

    $vswherePath = Join-Path ${env:ProgramFiles(x86)} "Microsoft Visual Studio\Installer\vswhere.exe"
    if (-not (Test-Path $vswherePath)) {
        return @()
    }

    $installationVersion = & $vswherePath `
        -latest `
        -products * `
        -requires Microsoft.VisualStudio.Component.VC.Tools.x86.x64 `
        -property installationVersion
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($installationVersion)) {
        return @()
    }

    $generator = switch (($installationVersion.Trim() -split '\.')[0]) {
        "17" { "Visual Studio 17 2022" }
        "16" { "Visual Studio 16 2019" }
        default { "" }
    }

    if ([string]::IsNullOrWhiteSpace($generator)) {
        return @()
    }

    $platform = switch ($RidArch) {
        "x64" { "x64" }
        "arm64" { "ARM64" }
        default { "" }
    }

    if ([string]::IsNullOrWhiteSpace($platform)) {
        return @("-G", $generator)
    }

    return @("-G", $generator, "-A", $platform)
}

function Get-DerivedRepoUrl {
    param([string]$RemoteUrl)

    if ($RemoteUrl -match '^https://github\.com/.+/.+\.git$') {
        return ($RemoteUrl -replace '/[^/]+\.git$', '/Elite_Robots_CS_SDK_C.git')
    }
    if ($RemoteUrl -match '^git@github\.com:.+/.+\.git$') {
        return ($RemoteUrl -replace '/[^/]+\.git$', '/Elite_Robots_CS_SDK_C.git')
    }
    if ($RemoteUrl -match '^https://gitee\.com/.+/.+\.git$') {
        return ($RemoteUrl -replace '/[^/]+\.git$', '/Elite_Robots_CS_SDK_C.git')
    }
    if ($RemoteUrl -match '^git@gitee\.com:.+/.+\.git$') {
        return ($RemoteUrl -replace '/[^/]+\.git$', '/Elite_Robots_CS_SDK_C.git')
    }

    return ""
}

function Get-CounterpartRepoUrl {
    param([string]$Url)

    switch -Regex ($Url) {
        '^https://github\.com/.+/Elite_Robots_CS_SDK_C\.git$' { return 'https://gitee.com/elibot_dukang/Elite_Robots_CS_SDK_C.git' }
        '^https://gitee\.com/.+/Elite_Robots_CS_SDK_C\.git$' { return 'https://github.com/747309172a-cpu/Elite_Robots_CS_SDK_C.git' }
        default { return "" }
    }
}

function Add-RepoCandidate {
    param(
        [System.Collections.Generic.List[string]]$List,
        [string]$Candidate
    )

    if ([string]::IsNullOrWhiteSpace($Candidate)) {
        return
    }
    if (-not $List.Contains($Candidate)) {
        $List.Add($Candidate)
    }
}

$defaultGithubRepo = "https://github.com/747309172a-cpu/Elite_Robots_CS_SDK_C.git"
$defaultGiteeRepo = "https://gitee.com/elibot_dukang/Elite_Robots_CS_SDK_C.git"

$repoCandidates = [System.Collections.Generic.List[string]]::new()
Add-RepoCandidate -List $repoCandidates -Candidate $RepoUrl

if ([string]::IsNullOrWhiteSpace($RepoUrl)) {
    $originUrl = ""
    try {
        $originUrl = (git -C $RepoRoot remote get-url origin).Trim()
    }
    catch {
        $originUrl = ""
    }

    $RepoUrl = Get-DerivedRepoUrl -RemoteUrl $originUrl
}

Add-RepoCandidate -List $repoCandidates -Candidate $RepoUrl
Add-RepoCandidate -List $repoCandidates -Candidate $defaultGithubRepo
Add-RepoCandidate -List $repoCandidates -Candidate $defaultGiteeRepo

foreach ($candidate in @($repoCandidates)) {
    Add-RepoCandidate -List $repoCandidates -Candidate (Get-CounterpartRepoUrl -Url $candidate)
}

if ($repoCandidates.Count -eq 0) {
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
    $cloneOk = $false
    foreach ($candidate in $repoCandidates) {
        Write-Host "[bootstrap-native] Cloning $candidate ($RepoRef)..."
        if (Test-Path $sourceDir) {
            Remove-Item -Recurse -Force $sourceDir
        }
        git clone --depth 1 --branch $RepoRef $candidate $sourceDir
        if ($LASTEXITCODE -eq 0) {
            $RepoUrl = $candidate
            $cloneOk = $true
            break
        }
        Write-Host "[bootstrap-native] Clone failed from $candidate, trying next mirror..."
    }

    if (-not $cloneOk) {
        throw "Failed to clone native repository from all configured mirrors."
    }
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
    if (-not (Test-Path (Join-Path $sourceDir "CMakeLists.txt"))) {
        throw "Native repository does not contain CMakeLists.txt: $sourceDir"
    }

    $cmakeGeneratorArgs = Get-CMakeGeneratorArgs -RidArch $ridArch
    if ($cmakeGeneratorArgs.Count -gt 0) {
        Write-Host "[bootstrap-native] Using CMake generator: $($cmakeGeneratorArgs -join ' ')"
    }
    else {
        Write-Host "[bootstrap-native] Using CMake default generator. If configure fails on Windows, install Ninja or Visual Studio C++ build tools."
    }

    $configureArgs = @()
    $configureArgs += $cmakeGeneratorArgs
    $configureArgs += @(
        "-S", $sourceDir,
        "-B", $buildDir,
        "-DELITE_AUTO_FETCH_SDK=ON",
        "-DELITE_BUILD_EXAMPLES=OFF"
    )

    Write-Host "[bootstrap-native] Configuring native library for $rid..."
    Invoke-NativeCommand `
        -Command { cmake @configureArgs } `
        -FailureMessage "[bootstrap-native] CMake configure failed. Verify that CMake, Ninja or Visual Studio C++ build tools, and network access to the native dependencies are available."

    Write-Host "[bootstrap-native] Building native library for $rid..."
    Invoke-NativeCommand `
        -Command { cmake --build $buildDir --config Release } `
        -FailureMessage "[bootstrap-native] CMake build failed. Review the configure output above for the actual compiler or dependency error."

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
