# RemoteUpgrade Module

## Introduction

The RemoteUpgrade module provides remote controller software upgrade functionality.

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
  - Trigger controller software remote upgrade.
- ***Parameters***
  - `ip`: Robot IP.
  - `file`: Upgrade package path.
  - `password`: Robot SSH password.
- ***Return***
  - `true` on success, `false` on failure.
- ***Notes***
  1. On Linux, if `libssh` is not installed, ensure `scp`, `ssh`, and `sshpass` are available.
  2. On Windows, if `libssh` is not installed, this interface is unavailable.
