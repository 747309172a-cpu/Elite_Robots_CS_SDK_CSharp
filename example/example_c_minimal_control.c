// SPDX-License-Identifier: MIT
// Copyright (c) 2026, Elite Robots.

#include <Elite_Sdk_C.h>

#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#if defined(_WIN32) || defined(_WIN64)
#include <windows.h>
static void sleep_ms(int32_t ms)
{
    Sleep((DWORD)ms);
}
#else
#include <time.h>
static void sleep_ms(int32_t ms)
{
    struct timespec ts;
    ts.tv_sec = ms / 1000;
    ts.tv_nsec = (long)(ms % 1000) * 1000000L;
    nanosleep(&ts, NULL);
}
#endif

static void print_status_error(const char* step, elite_c_status_t status, const char* detail)
{
    fprintf(stderr, "[ERROR] %s failed, status=%d", step, (int)status);
    if (detail != NULL && detail[0] != '\0') {
        fprintf(stderr, ", detail=%s", detail);
    }
    fprintf(stderr, "\n");
}

static int check_bool_result(const char* step, elite_c_status_t status, int32_t ok, const char* detail)
{
    if (status != ELITE_C_STATUS_OK) {
        print_status_error(step, status, detail);
        return 0;
    }
    if (!ok) {
        fprintf(stderr, "[ERROR] %s returned false\n", step);
        return 0;
    }
    printf("[OK] %s\n", step);
    return 1;
}

int main(int argc, char** argv)
{
    elite_dashboard_handle_t* dashboard = NULL;
    elite_driver_handle_t* driver = NULL;
    elite_driver_config_t config;
    elite_c_status_t status;
    int32_t ok = 0;
    int32_t connected = 0;
    int attempt = 0;

    if (argc < 3) {
        fprintf(stderr, "Usage:\n");
        fprintf(stderr, "  %s <robot-ip> <external_control.script> [local-ip]\n", argv[0]);
        fprintf(stderr, "\n");
        fprintf(stderr, "Example:\n");
        fprintf(stderr, "  %s 172.16.102.156 /home/user/Elite_Robots_CS_SDK/source/resources/external_control.script\n", argv[0]);
        return 1;
    }

    const char* robot_ip = argv[1];
    const char* script_file = argv[2];
    const char* local_ip = argc >= 4 ? argv[3] : "";

    printf("[INFO] robot_ip=%s\n", robot_ip);
    printf("[INFO] script_file=%s\n", script_file);
    if (local_ip[0] != '\0') {
        printf("[INFO] local_ip=%s\n", local_ip);
    }

    status = elite_dashboard_create(&dashboard);
    if (status != ELITE_C_STATUS_OK || dashboard == NULL) {
        print_status_error("elite_dashboard_create", status, elite_c_last_error_message());
        return 1;
    }

    status = elite_dashboard_connect(dashboard, robot_ip, 29999, &ok);
    if (!check_bool_result("dashboard connect", status, ok, elite_dashboard_last_error_message(dashboard))) {
        goto cleanup;
    }

    status = elite_dashboard_power_on(dashboard, &ok);
    if (!check_bool_result("dashboard power_on", status, ok, elite_dashboard_last_error_message(dashboard))) {
        goto cleanup;
    }

    sleep_ms(1000);

    status = elite_dashboard_brake_release(dashboard, &ok);
    if (!check_bool_result("dashboard brake_release", status, ok, elite_dashboard_last_error_message(dashboard))) {
        goto cleanup;
    }

    sleep_ms(1000);

    elite_driver_config_set_default(&config);
    config.robot_ip = robot_ip;
    config.script_file_path = script_file;
    config.local_ip = local_ip;
    config.headless_mode = 1;

    status = elite_driver_create(&config, &driver);
    if (status != ELITE_C_STATUS_OK || driver == NULL) {
        print_status_error("elite_driver_create", status, elite_c_last_error_message());
        goto cleanup;
    }
    printf("[OK] driver create\n");

    status = elite_driver_send_external_control_script(driver, &ok);
    if (!check_bool_result("driver send_external_control_script", status, ok, elite_driver_last_error_message(driver))) {
        goto cleanup;
    }

    printf("[INFO] waiting for robot reverse connection...\n");
    for (attempt = 0; attempt < 100; ++attempt) {
        status = elite_driver_is_robot_connected(driver, &connected);
        if (status != ELITE_C_STATUS_OK) {
            print_status_error("driver is_robot_connected", status, elite_driver_last_error_message(driver));
            goto cleanup;
        }
        if (connected) {
            printf("[OK] robot connected to external control ports\n");
            break;
        }
        sleep_ms(100);
    }

    if (!connected) {
        fprintf(stderr, "[ERROR] robot did not connect back within timeout\n");
        goto cleanup;
    }

    {
        const double down[6] = {0.0, 0.0, -0.05, 0.0, 0.0, 0.0};
        const double up[6] = {0.0, 0.0, 0.05, 0.0, 0.0, 0.0};

        status = elite_driver_write_speedl(driver, down, 2000, &ok);
        if (!check_bool_result("driver write_speedl(down)", status, ok, elite_driver_last_error_message(driver))) {
            goto cleanup;
        }
        sleep_ms(2000);

        status = elite_driver_write_speedl(driver, up, 2000, &ok);
        if (!check_bool_result("driver write_speedl(up)", status, ok, elite_driver_last_error_message(driver))) {
            goto cleanup;
        }
        sleep_ms(2000);
    }

    status = elite_driver_stop_control(driver, 10000, &ok);
    if (!check_bool_result("driver stop_control", status, ok, elite_driver_last_error_message(driver))) {
        goto cleanup;
    }

    printf("[INFO] minimal C control example finished successfully\n");

cleanup:
    if (driver != NULL) {
        elite_driver_destroy(driver);
        driver = NULL;
    }

    if (dashboard != NULL) {
        elite_dashboard_disconnect(dashboard);
        elite_dashboard_destroy(dashboard);
        dashboard = NULL;
    }

    return 0;
}
