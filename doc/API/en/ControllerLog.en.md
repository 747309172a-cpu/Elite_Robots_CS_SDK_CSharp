# EliteControllerLog Module

## Introduction

`EliteControllerLog` provides the ability to download robot controller logs.

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
  - Download system logs from the robot to a local path.
- ***Parameters***
  - `robot_ip`: robot IP address.
  - `password`: robot SSH password.
  - `path`: local save path.
  - `progress_cb`: download progress callback (optional).
- ***Callback Parameters***
  - `fileSize`: total file size in bytes.
  - `recvSize`: currently received size in bytes.
  - `err`: error message string.
- ***Return Value***
  - Returns `true` if the download succeeds, otherwise `false`.

- ***Notes***

  1. On Linux, if `libssh` is not installed, the computer running the SDK must have the `scp`, `ssh`, and `sshpass` commands available.
  2. On Windows, if `libssh` is not installed, this interface is unavailable.
