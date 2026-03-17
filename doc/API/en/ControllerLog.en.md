# EliteControllerLog Module

## Introduction

`EliteControllerLog` provides system log download capability from the robot controller.

## Import

```csharp
using EliteRobots.CSharp;
```

## Interface

### downloadSystemLog

```csharp
public static bool downloadSystemLog(string robot_ip, string password, string path, Action<int, int, string>? progress_cb = null)
```

- ***Function***
  - Download system logs from the robot to local path.
- ***Parameters***
  - `robot_ip`: Robot IP address.
  - `password`: Robot SSH password.
  - `path`: Local output path.
  - `progress_cb`: Optional progress callback.
- ***Callback Parameters***
  - `fileSize`: Total file size in bytes.
  - `recvSize`: Received size in bytes.
  - `err`: Error string.
- ***Return***
  - `true` on success, `false` on failure.
- ***Notes***
  1. On Linux, if `libssh` is not installed, ensure `scp`, `ssh`, and `sshpass` are available.
  2. On Windows, if `libssh` is not installed, this interface is unavailable.
