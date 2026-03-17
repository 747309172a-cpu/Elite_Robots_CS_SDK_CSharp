# EliteDriver Class

## Introduction

`EliteDriver` is the main class for robot data/control exchange. It creates required sockets, sends control scripts, and handles motion and callbacks.

## Import

```csharp
using EliteRobots.CSharp;
```

## Constructor

### EliteDriver

```csharp
public EliteDriver(EliteDriverConfig config)
```

- ***Function***
  - Create `EliteDriver` and initialize required communication channels.
- ***Parameters***
  - `config`: driver configuration. See [EliteDriverConfig](./EliteDriverConfig.en.md).
- ***Exceptions***
  - `ArgumentNullException`
  - `ArgumentException`
  - `EliteSdkException`

## Connection and Control

```csharp
public bool isRobotConnected()
public bool sendExternalControlScript()
public bool stopControl(int wait_ms = 10000)
```

## Motion Control

```csharp
public bool writeServoj(double[] pos, int timeout_ms, bool cartesian = false)
public bool writeSpeedj(double[] vel, int timeout_ms)
public bool writeSpeedl(double[] vel, int timeout_ms)
public bool writeIdle(int timeout_ms)
public bool writeFreedrive(FreedriveAction action, int timeout_ms)
```

## Trajectory Control

```csharp
public void setTrajectoryResultCallback(Action<TrajectoryMotionResult> cb)
public bool writeTrajectoryPoint(double[] positions, float time, float blend_radius, bool cartesian)
public bool writeTrajectoryControlAction(TrajectoryControlAction action, int point_number, int timeout_ms)
```

## Force and Tool

```csharp
public bool zeroFTSensor()
public bool setPayload(double mass, double[] cog)
public bool setToolVoltage(ToolVoltage vol)
public bool startForceMode(double[] reference_frame, int[] selection_vector, double[] wrench, ForceMode mode, double[] limits)
public bool endForceMode()
```

## Script and Primary

```csharp
public bool sendScript(string script)
public bool getPrimaryPackage(PrimaryKinematicsInfo pkg, int timeout_ms)
public bool primaryReconnect()
```

## Exception Callbacks

```csharp
public void registerRobotExceptionCallback(Action<EliteDriverRobotException> cb)
public void registerWrappedRobotExceptionCallback(Action<RobotException> cb)
```

## RS485

```csharp
public EliteSerialCommunication? startToolRs485(SerialConfig config, string ssh_password, int tcp_port = 54321)
public bool endToolRs485(EliteSerialCommunication comm, string ssh_password)
public EliteSerialCommunication? startBoardRs485(SerialConfig config, string ssh_password, int tcp_port = 54322)
public bool endBoardRs485(EliteSerialCommunication comm, string ssh_password)
```
