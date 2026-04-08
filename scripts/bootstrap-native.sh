#!/usr/bin/env bash
set -euo pipefail

repo_root="${1:?repo root is required}"
project_output_dir="${2:?project output dir is required}"
repo_url="${3:-}"
repo_ref="${4:-main}"
force_rebuild="${5:-false}"
toolchain_file="${CMAKE_TOOLCHAIN_FILE:-}"
vcpkg_root="${VCPKG_ROOT:-}"
vcpkg_triplet="${VCPKG_TARGET_TRIPLET:-}"
cmake_prefix_path="${CMAKE_PREFIX_PATH:-}"

default_github_repo="https://github.com/747309172a-cpu/Elite_Robots_CS_SDK_C.git"
default_gitee_repo="https://gitee.com/elibot_dukang/Elite_Robots_CS_SDK_C.git"

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
        https://gitee.com/*/*.git)
            printf '%s\n' "${remote_url%/*}/Elite_Robots_CS_SDK_C.git"
            return 0
            ;;
        git@gitee.com:*.git)
            printf '%s\n' "${remote_url%/*}/Elite_Robots_CS_SDK_C.git"
            return 0
            ;;
    esac

    return 1
}

counterpart_repo_url() {
    local url="${1:-}"
    case "${url}" in
        https://github.com/747309172a-cpu/Elite_Robots_CS_SDK_C.git)
            printf '%s\n' "${default_gitee_repo}"
            ;;
        https://gitee.com/elibot_dukang/Elite_Robots_CS_SDK_C.git)
            printf '%s\n' "${default_github_repo}"
            ;;
        https://github.com/*/Elite_Robots_CS_SDK_C.git)
            printf '%s\n' "${default_gitee_repo}"
            ;;
        https://gitee.com/*/Elite_Robots_CS_SDK_C.git)
            printf '%s\n' "${default_github_repo}"
            ;;
    esac
}

declare -a repo_candidates=()
add_repo_candidate() {
    local candidate="${1:-}"
    [[ -z "${candidate}" ]] && return 0
    local existing
    for existing in "${repo_candidates[@]:-}"; do
        [[ "${existing}" == "${candidate}" ]] && return 0
    done
    repo_candidates+=("${candidate}")
}

add_repo_candidate "${repo_url}"
add_repo_candidate "$(derive_repo_url || true)"
add_repo_candidate "${default_github_repo}"
add_repo_candidate "${default_gitee_repo}"

if [[ ${#repo_candidates[@]} -eq 0 ]]; then
    echo "Native repository URL is not set. Pass EliteNativeRepoUrl or configure origin so it can be derived automatically." >&2
    exit 1
fi

for candidate in "${repo_candidates[@]}"; do
    add_repo_candidate "$(counterpart_repo_url "${candidate}")"
done

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
    clone_ok="false"
    for candidate in "${repo_candidates[@]}"; do
        echo "[bootstrap-native] Cloning ${candidate} (${repo_ref})..."
        rm -rf "${source_dir}"
        if git clone --depth 1 --branch "${repo_ref}" "${candidate}" "${source_dir}"; then
            repo_url="${candidate}"
            clone_ok="true"
            break
        fi
        echo "[bootstrap-native] Clone failed from ${candidate}, trying next mirror..."
    done

    if [[ "${clone_ok}" != "true" ]]; then
        echo "Failed to clone native repository from all configured mirrors." >&2
        exit 1
    fi
fi

if [[ "${force_rebuild}" == "true" ]]; then
    echo "[bootstrap-native] Force rebuild requested."
    rm -rf "${build_dir}" "${cache_dir}"
    mkdir -p "${cache_dir}"
fi

if [[ ! -f "${stamp_file}" ]]; then
    if [[ -z "${toolchain_file}" && -n "${vcpkg_root}" ]]; then
        candidate_toolchain_file="${vcpkg_root}/scripts/buildsystems/vcpkg.cmake"
        if [[ -f "${candidate_toolchain_file}" ]]; then
            toolchain_file="${candidate_toolchain_file}"
        fi
    fi

    if [[ -n "${toolchain_file}" ]]; then
        if [[ ! -f "${toolchain_file}" ]]; then
            echo "CMAKE_TOOLCHAIN_FILE does not exist: ${toolchain_file}" >&2
            exit 1
        fi
        echo "[bootstrap-native] Using CMake toolchain file: ${toolchain_file}"
    fi

    configure_args=(
        -S "${source_dir}"
        -B "${build_dir}"
        -DELITE_AUTO_FETCH_SDK=ON
        -DELITE_BUILD_EXAMPLES=OFF
        -DCMAKE_BUILD_TYPE=Release
        -DCMAKE_BUILD_RPATH="\$ORIGIN"
        -DCMAKE_INSTALL_RPATH="\$ORIGIN"
    )

    if [[ -n "${toolchain_file}" ]]; then
        configure_args+=("-DCMAKE_TOOLCHAIN_FILE:FILEPATH=${toolchain_file}")
    fi
    if [[ -n "${vcpkg_root}" ]]; then
        configure_args+=("-DVCPKG_ROOT:PATH=${vcpkg_root}")
    fi
    if [[ -n "${vcpkg_triplet}" ]]; then
        configure_args+=("-DVCPKG_TARGET_TRIPLET:STRING=${vcpkg_triplet}")
    fi
    if [[ -n "${cmake_prefix_path}" ]]; then
        configure_args+=("-DCMAKE_PREFIX_PATH:PATH=${cmake_prefix_path}")
    fi

    echo "[bootstrap-native] Configuring native library for ${rid}..."
    cmake "${configure_args[@]}"

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
