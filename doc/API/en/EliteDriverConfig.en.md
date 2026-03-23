# EliteDriverConfig

## Introduction

`EliteDriverConfig` is used to configure the network parameters, script path, and servo parameters for `EliteDriver`.

## Import

```csharp
using EliteRobots.CSharp;
```

## Definition

### EliteDriverConfig

```csharp
public sealed class EliteDriverConfig
```

- ***Function***
  - Used as the constructor argument of `EliteDriver` to define connection and control behavior.

## Property Description

### RobotIp

```csharp
public string RobotIp { get; set; }
```

- ***Function***
  - Robot IP address.
- ***Description***
  - Required.

### ScriptFilePath

```csharp
public string ScriptFilePath { get; set; }
```

- ***Function***
  - External control script file path.
- ***Description***
  - Required.

### LocalIp

```csharp
public string LocalIp { get; set; }
```

- ***Function***
  - Local NIC IP address, which can be specified in multi-NIC scenarios.

### HeadlessMode

```csharp
public bool HeadlessMode { get; set; }
```

- ***Function***
  - Whether to enable headless mode.

### ScriptSenderPort

```csharp
public int ScriptSenderPort { get; set; } = 50002;
```

- ***Function***
  - Script sending port.

### ReversePort

```csharp
public int ReversePort { get; set; } = 50001;
```

- ***Function***
  - Reverse connection port.

### TrajectoryPort

```csharp
public int TrajectoryPort { get; set; } = 50003;
```

- ***Function***
  - Port used to send trajectory points.

### ScriptCommandPort

```csharp
public int ScriptCommandPort { get; set; } = 50004;
```

- ***Function***
  - Port used to send script commands.

### ServojTime

```csharp
public float ServojTime { get; set; } = 0.008f;
```

- ***Function***
  - Time interval of the `servoj()` command.

### ServojLookaheadTime

```csharp
public float ServojLookaheadTime { get; set; } = 0.1f;
```

- ***Function***
  - Lookahead time of the `servoj()` command, in the range [0.03, 0.2] seconds.

### ServojGain

```csharp
public int ServojGain { get; set; } = 300;
```

- ***Function***
  - Servo gain.

### StopjAcc

```csharp
public float StopjAcc { get; set; } = 8.0f;
```

- ***Function***
  - Deceleration used to stop motion (`rad/s²`).

## Notes

- `RobotIp` and `ScriptFilePath` must be assigned valid values.
- Port conflicts will cause `EliteDriver` creation to fail.
