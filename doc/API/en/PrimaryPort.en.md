# PrimaryClientInterface Class

## Introduction

`PrimaryClientInterface` communicates with robot Primary port, supports script sending, kinematics package reading, and exception callbacks.

## Import

```csharp
using EliteRobots.CSharp;
```

## Interfaces

```csharp
public bool connect(string ip, int port = 30001)
public void disconnect()
public string getLocalIP()
public bool sendScript(string script)
public bool getPackage(out PrimaryKinematicsInfo info, int timeoutMs = 1000)
public void registerRobotExceptionCallback(Action<PrimaryRobotException> callback)
public void registerWrappedRobotExceptionCallback(Action<RobotException> callback)
public void clearRobotExceptionCallback()
public void Dispose()
```

## Data Types

### PrimaryKinematicsInfo

```csharp
public sealed class PrimaryKinematicsInfo
{
    public double[] DhA { get; init; }
    public double[] DhD { get; init; }
    public double[] DhAlpha { get; init; }
}
```

### PrimaryRobotException

```csharp
public sealed class PrimaryRobotException
```
