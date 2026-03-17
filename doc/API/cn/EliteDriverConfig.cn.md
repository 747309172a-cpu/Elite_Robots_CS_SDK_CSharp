# EliteDriverConfig

## 简介

`EliteDriverConfig` 用于配置 `EliteDriver` 的网络参数、脚本路径和伺服参数。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 结构定义

### EliteDriverConfig

```csharp
public sealed class EliteDriverConfig
```

- ***功能***
  - 作为 `EliteDriver` 构造参数，定义连接和控制行为。

## 属性说明

### RobotIp

```csharp
public string RobotIp { get; set; }
```

- ***功能***
  - 机器人 IP 地址。
- ***说明***
  - 必填。

### ScriptFilePath

```csharp
public string ScriptFilePath { get; set; }
```

- ***功能***
  - 外部控制脚本文件路径。
- ***说明***
  - 必填。

### LocalIp

```csharp
public string LocalIp { get; set; }
```

- ***功能***
  - 本地网卡 IP（多网卡场景可指定）。

### HeadlessMode

```csharp
public bool HeadlessMode { get; set; }
```

- ***功能***
  - 是否启用无头模式。

### ScriptSenderPort

```csharp
public int ScriptSenderPort { get; set; } = 50002;
```

- ***功能***
  - 脚本发送端口。

### ReversePort

```csharp
public int ReversePort { get; set; } = 50001;
```

- ***功能***
  - 反向连接端口。

### TrajectoryPort

```csharp
public int TrajectoryPort { get; set; } = 50003;
```

- ***功能***
  - 发送轨迹点的端口。

### ScriptCommandPort

```csharp
public int ScriptCommandPort { get; set; } = 50004;
```

- ***功能***
  - 发送脚本命令的端口。

### ServojTime

```csharp
public float ServojTime { get; set; } = 0.008f;
```

- ***功能***
  - `servoj()`指令的时间间隔。

### ServojLookaheadTime

```csharp
public float ServojLookaheadTime { get; set; } = 0.1f;
```

- ***功能***
  - `servoj()`指令瞻时间，范围 [0.03, 0.2] 秒。

### ServojGain

```csharp
public int ServojGain { get; set; } = 300;
```

- ***功能***
  - 伺服增益。

### StopjAcc

```csharp
public float StopjAcc { get; set; } = 8.0f;
```

- ***功能***
  - 停止运动的加速度 (rad/s²)。

## 注意事项

- `RobotIp` 与 `ScriptFilePath` 需要提供有效值。
- 端口冲突会导致 `EliteDriver` 创建失败。
