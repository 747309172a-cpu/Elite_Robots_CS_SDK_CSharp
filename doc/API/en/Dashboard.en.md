# DashboardClientInterface Class

## Introduction

`DashboardClientInterface` communicates with the robot Dashboard service for power, program control, and status queries.

## Import

```csharp
using EliteRobots.CSharp;
```

## Constructor

### DashboardClientInterface

```csharp
public DashboardClientInterface()
```

- ***Function***
  - Create a Dashboard client instance.
- ***Parameters***
  - None.
- ***Return***
  - None.

## Communication

### connect

```csharp
public bool connect(string ip, int port = 29999)
```

- ***Function***
  - Connect to Dashboard server.
- ***Parameters***
  - `ip`: Robot IP.
  - `port`: Port, default `29999`.
- ***Return***
  - `true` on success, otherwise `false`.

### disconnect

```csharp
public void disconnect()
```

- ***Function***
  - Disconnect from Dashboard server.
- ***Parameters***
  - None.
- ***Return***
  - None.

### echo

```csharp
public bool echo()
```

- ***Function***
  - Check dashboard shell connectivity.
- ***Return***
  - `true` if connected.

## Robot Control

```csharp
public bool powerOn()
public bool powerOff()
public bool brakeRelease()
public bool closeSafetyDialog()
public bool unlockProtectiveStop()
public bool safetySystemRestart()
```

- ***Function***
  - Power, brake, and safety operations.
- ***Return***
  - `true` on success, otherwise `false`.

## Program Control

```csharp
public bool playProgram()
public bool pauseProgram()
public bool stopProgram()
public bool taskIsRunning()
public TaskStatus getTaskStatus()
```

## Config and Task

```csharp
public bool loadConfiguration(string path)
public bool loadTask(string path)
public bool isConfigurationModify()
public bool isTaskSaved()
public string getTaskPath()
```

## Status

```csharp
public RobotMode robotMode()
public SafetyMode safetyMode()
public TaskStatus runningStatus()
public int speedScaling()
public bool setSpeedScaling(int scaling)
```

## Others

```csharp
public bool log(string message)
public bool popup(string arg, string message = "")
public void quit()
public void reboot()
public void shutdown()
public string help(string cmd)
public string usage(string cmd)
public string version()
public string robotType()
public string robotSerialNumber()
public string robotID()
public string configurationPath()
public string sendAndReceive(string cmd)
```
