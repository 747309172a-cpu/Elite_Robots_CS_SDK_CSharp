#!/usr/bin/env bash
set -euo pipefail

repo_root="${1:?repo root is required}"
project_output_dir="${2:?project output dir is required}"
repo_url="${3:-}"
repo_ref="${4:-main}"
force_rebuild="${5:-false}"

derive_repo_url() {
    local remote_url
    remote_url="$(git -C "${repo_root}" remote get-url origin 2>/dev/null || true)"
    if [[ -z "${remote_url}" ]]; then
        return 1
    fi

    case "${remote_url}" in
        https://github.com/*/*.git)
            printf '%s\n' "${remote_url%/*}/Elite_Robots_CS_SDK_C.git"
            return 0
            ;;
        git@github.com:*.git)
            printf '%s\n' "${remote_url%/*}/Elite_Robots_CS_SDK_C.git"
            return 0
            ;;
    esac

    return 1
}

if [[ -z "${repo_url}" ]]; then
    repo_url="$(derive_repo_url || true)"
fi

if [[ -z "${repo_url}" ]]; then
    echo "Native repository URL is not set. Pass EliteNativeRepoUrl or configure origin so it can be derived automatically." >&2
    exit 1
fi

os_name="$(uname -s)"
arch_name="$(uname -m)"

case "${os_name}" in
    Linux) rid_os="linux"; lib_glob="*.so*" ;;
    Darwin) rid_os="osx"; lib_glob="*.dylib" ;;
    *)
        echo "Unsupported Unix platform: ${os_name}" >&2
        exit 1
        ;;
esac

case "${arch_name}" in
    x86_64|amd64) rid_arch="x64" ;;
    aarch64|arm64) rid_arch="arm64" ;;
    *)
        echo "Unsupported architecture: ${arch_name}" >&2
        exit 1
        ;;
esac

rid="${rid_os}-${rid_arch}"
source_dir="${repo_root}/.native-src/elite_cs_series_sdk_c"
build_dir="${repo_root}/.native-build/${rid}"
cache_dir="${repo_root}/.native-out/${rid}"
stamp_file="${cache_dir}/.bootstrap-complete"

case "${rid_os}" in
    linux) wrapper_pattern="libelite_cs_series_sdk_c.so*" ;;
    osx) wrapper_pattern="libelite_cs_series_sdk_c.dylib" ;;
esac

mkdir -p "${repo_root}/.native-src" "${repo_root}/.native-build" "${cache_dir}" "${project_output_dir}"

if [[ ! -d "${source_dir}/.git" ]]; then
    echo "[bootstrap-native] Cloning ${repo_url} (${repo_ref})..."
    rm -rf "${source_dir}"
    git clone --depth 1 --branch "${repo_ref}" "${repo_url}" "${source_dir}"
fi

if [[ "${force_rebuild}" == "true" ]]; then
    echo "[bootstrap-native] Force rebuild requested."
    rm -rf "${build_dir}" "${cache_dir}"
    mkdir -p "${cache_dir}"
fi

if [[ ! -f "${stamp_file}" ]]; then
    echo "[bootstrap-native] Configuring native library for ${rid}..."
    cmake \
        -S "${source_dir}" \
        -B "${build_dir}" \
        -DELITE_AUTO_FETCH_SDK=ON \
        -DELITE_BUILD_EXAMPLES=OFF \
        -DCMAKE_BUILD_TYPE=Release \
        -DCMAKE_BUILD_RPATH="\$ORIGIN" \
        -DCMAKE_INSTALL_RPATH="\$ORIGIN"

    echo "[bootstrap-native] Building native library for ${rid}..."
    cmake --build "${build_dir}" --config Release --parallel

    find "${cache_dir}" -maxdepth 1 -type f -delete
    while IFS= read -r -d '' file; do
        cp -f "${file}" "${cache_dir}/$(basename "${file}")"
    done < <(find "${build_dir}" -type f -name "${lib_glob}" -print0)

    if ! find "${cache_dir}" -maxdepth 1 -type f -name "${wrapper_pattern}" | grep -q .; then
        echo "Failed to find built native library in ${build_dir}" >&2
        exit 1
    fi

    touch "${stamp_file}"
fi

while IFS= read -r -d '' file; do
    cp -f "${file}" "${project_output_dir}/$(basename "${file}")"
done < <(find "${cache_dir}" -maxdepth 1 -type f ! -name '.bootstrap-complete' -print0)
