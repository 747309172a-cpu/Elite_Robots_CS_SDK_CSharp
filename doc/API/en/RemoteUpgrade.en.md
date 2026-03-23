# RemoteUpgrade Module

## Introduction

The RemoteUpgrade module provides remote upgrade functionality for robot control software.

## Import

```csharp
using EliteRobots.CSharp;
```

## Interface

### upgradeControlSoftware

```csharp
public static bool upgradeControlSoftware(string ip, string file, string password)
```

- ***Function***
  - Trigger a remote upgrade of the control software.
- ***Parameters***
  - `ip`: robot IP address.
  - `file`: upgrade package file path.
  - `password`: robot SSH password.
- ***Return Value***
  - Returns `true` if the request is executed successfully, otherwise `false`.
- ***Notes***

  1. On Linux, if `libssh` is not installed, the computer running the SDK must have the `scp`, `ssh`, and `sshpass` commands available.
  2. On Windows, if `libssh` is not installed, this interface is unavailable.
