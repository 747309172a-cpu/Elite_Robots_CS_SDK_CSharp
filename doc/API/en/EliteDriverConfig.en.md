# EliteDriverConfig

## Introduction

`EliteDriverConfig` configures network, script path, and servoj parameters for `EliteDriver`.

## Import

```csharp
using EliteRobots.CSharp;
```

## Definition

```csharp
public sealed class EliteDriverConfig
```

## Properties

```csharp
public string RobotIp { get; set; }
public string ScriptFilePath { get; set; }
public string LocalIp { get; set; }

public bool HeadlessMode { get; set; }
public int ScriptSenderPort { get; set; } = 50002;
public int ReversePort { get; set; } = 50001;
public int TrajectoryPort { get; set; } = 50003;
public int ScriptCommandPort { get; set; } = 50004;

public float ServojTime { get; set; } = 0.008f;
public float ServojLookaheadTime { get; set; } = 0.1f;
public int ServojGain { get; set; } = 300;
public float StopjAcc { get; set; } = 8.0f;
```

## Notes

- `RobotIp` and `ScriptFilePath` are required.
- Port conflicts may cause driver creation failure.
