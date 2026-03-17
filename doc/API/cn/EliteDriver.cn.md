# EliteDriver 类

## 简介

`EliteDriver` 是用于与机器人进行数据交互的主要类。它负责建立所有必要的套接字连接，并处理与机器人的数据交换。EliteDriver 会向机器人发送控制脚本，机器人在运行控制脚本后，会和 EliteDriver 建立通讯，接收运动数据，并且必要时会发送运动结果。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 构造函数

### EliteDriver

```csharp
public EliteDriver(EliteDriverConfig config)
```

- ***功能***
  - 创建 EliteDriver 对象，并初始化与机器人通信的必要连接。  
    以下情况此函数会抛出异常：  
    1. TCP server 创建失败，通常是因为端口被占用导致的。
    2. 连接机器人的primary port失败。
- ***参数***
  - `config`：驱动配置，参考[配置](./EliteDriverConfig.cn.md)
- ***返回值***
  - 无。
- ***异常***
  - `ArgumentNullException`：`config` 为空。
  - `ArgumentException`：`RobotIp` 或 `ScriptFilePath` 为空。
  - `EliteSdkException`：底层创建失败。

## 连接与控制

### isRobotConnected

```csharp
public bool isRobotConnected()
```

- ***功能***
  - 查询机器人当前连接状态。
- ***参数***
  - 无。
- ***返回值***
  - 已连接返回 `true`，否则 `false`。

### sendExternalControlScript

```csharp
public bool sendExternalControlScript()
```

- ***功能***
  - 向机器人发送外部控制脚本。
- ***参数***
  - 无。
- ***返回值***
  - 发送成功返回 `true`，失败返回 `false`。

### stopControl

```csharp
public bool stopControl(int wait_ms = 10000)
```

- ***功能***
  - 停止当前控制流程。
- ***参数***
  - `wait_ms`：等待超时时间（毫秒）。
- ***返回值***
  - 停止成功返回 `true`，失败返回 `false`。

## 运动控制

### writeServoj

```csharp
public bool writeServoj(double[] pos, int timeout_ms, bool cartesian = false)
```

- ***功能***
  - 向机器人发送伺服目标点。
- ***参数***
  - `pos`：目标点位，长度必须为 6。
  - `timeout_ms`：读取下一条指令超时时间（毫秒）。
  - `cartesian`：`true` 表示笛卡尔点位，`false` 表示关节点位。
- ***返回值***
  - 指令发送成功返回 `true`，失败返回 `false`。


### writeSpeedj

```csharp
public bool writeSpeedj(double[] vel, int timeout_ms)
```

- ***功能***
  - 发送关节空间速度控制指令。
- ***参数***
  - `vel`：关节速度，长度必须为 6。
  - `timeout_ms`：超时时间（毫秒）。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### writeSpeedl

```csharp
public bool writeSpeedl(double[] vel, int timeout_ms)
```

- ***功能***
  - 发送笛卡尔空间速度控制指令。
- ***参数***
  - `vel`：笛卡尔速度，长度必须为 6。
  - `timeout_ms`：超时时间（毫秒）。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### writeIdle

```csharp
public bool writeIdle(int timeout_ms)
```

- ***功能***
  - 发送空闲控制指令，如果机器人正在运动会使机器人停止运动。
- ***参数***
  - `timeout_ms`：超时时间（毫秒）。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### writeFreedrive

```csharp
public bool writeFreedrive(FreedriveAction action, int timeout_ms)
```

- ***功能***
  - 控制拖动模式（进入/退出）。
- ***参数***
  - `action`：Freedrive动作，有：开启（START）、停止(END)、空操作(NOOP)。
  - `timeout_ms`：设置机器人读取下一条指令的超时时间，小于等于0时会无限等待。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。
- ***注意***
    写入START动作之后，需要在超时时间内写入下一条指令，可以写入NOOP。

## 轨迹控制

### setTrajectoryResultCallback

```csharp
public void setTrajectoryResultCallback(Action<TrajectoryMotionResult> cb)
```

- ***功能***
  - 注册轨迹完成时的回调函数。控制机器人的一种方式是将路点一次性发给机器人，当执行完成时，这里注册的回调函数将被触发。

- ***参数***
  - `cb`：轨迹结果回调函数。
- ***返回值***
  - 无。

### writeTrajectoryPoint

```csharp
public bool writeTrajectoryPoint(double[] positions, float time, float blend_radius, bool cartesian)
```

- ***功能***
  - 向专门的socket写入轨迹路点。
- ***参数***
  - `positions`：轨迹路点，长度必须为 6。
  - `time`：该点执行时间。
  - `blend_radius`：过渡半径。
  - `cartesian`：是否笛卡尔点位。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### writeTrajectoryControlAction

```csharp
public bool writeTrajectoryControlAction(TrajectoryControlAction action, int point_number, int timeout_ms)
```

- ***功能***
  - 发送轨迹控制指令。
- ***参数***
  - `action`：轨迹控制动作。
  - `point_number`：轨迹点数量。
  - `timeout_ms`：超时时间（毫秒）。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

## 力控与工具配置

### zeroFTSensor

```csharp
public bool zeroFTSensor()
```

- ***功能***
  - 将力/力矩传感器测量的施加在工具 TCP 上的力/力矩值清零（去皮），所述力/力矩值为 get_tcp_force(True) 脚本指令获取的施加在工具 TCP 上的力/力矩矢量，该矢量已进行负载补偿等处理。该指令执行后，当前的力/力矩测量值会被作为力/力矩参考值保存，后续所有的力/力矩测量值都会减去该力/力矩参考值（去皮）。请注意，上述力/力矩参考值会在该指令执行时更新，在控制器重启后将重置为 0。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### setPayload

```csharp
public bool setPayload(double mass, double[] cog)
```

- ***功能***
  - 设置负载质量与重心。
- ***参数***
  - `mass`：负载质量。
  - `cog`：有效载荷的重心坐标（相对于法兰框架）。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### setToolVoltage

```csharp
public bool setToolVoltage(ToolVoltage vol)
```

- ***功能***
  - 设置工具端电压。
- ***参数***
  - `vol`：工具电压枚举值。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### startForceMode

```csharp
public bool startForceMode(double[] reference_frame, int[] selection_vector, double[] wrench, ForceMode mode, double[] limits)
```

- ***功能***
  - 启动力控模式。
- ***参数***
  - `reference_frame`：参考坐标系，长度必须为 6。
  - `selection_vector`：选择向量，长度必须为 6。
  - `wrench`：目标力/力矩，长度必须为 6。
  - `mode`：力控模式。
  - `limits`：限制参数，长度必须为 6。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### endForceMode

```csharp
public bool endForceMode()
```

- ***功能***
  - 退出力控模式。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

## 其余

### sendScript

```csharp
public bool sendScript(string script)
```

- ***功能***
  - 向机器人发送脚本文本。
- ***参数***
  - `script`：脚本内容字符串。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### getPrimaryPackage

```csharp
public bool getPrimaryPackage(PrimaryKinematicsInfo pkg, int timeout_ms)
```

- ***功能***
  - 读取 Primary 运动学参数并写入 `pkg`。
- ***参数***
  - `pkg`：输出对象，内部 `DhA`/`DhD`/`DhAlpha` 数组长度必须为 6。
  - `timeout_ms`：超时时间（毫秒）。
- ***返回值***
  - 读取成功返回 `true`，失败返回 `false`。

### primaryReconnect

```csharp
public bool primaryReconnect()
```

- ***功能***
  - 重新建立 Primary 通道连接。
- ***参数***
  - 无。
- ***返回值***
  - 重连成功返回 `true`，失败返回 `false`。

## 异常回调

### registerRobotExceptionCallback

```csharp
public void registerRobotExceptionCallback(Action<EliteDriverRobotException> cb)
```

- ***功能***
  - 注册机器人异常回调。
- ***参数***
  - `cb`：异常回调函数。
- ***返回值***
  - 无。

### registerWrappedRobotExceptionCallback

```csharp
public void registerWrappedRobotExceptionCallback(Action<RobotException> cb)
```

- ***功能***
  - 注册高级异常回调，回调参数会被映射为 `RobotDisconnectedException` / `RobotError` / `RobotRuntimeException`。
- ***参数***
  - `cb`：高级异常回调函数。
- ***返回值***
  - 无。


### startToolRs485

```csharp
public EliteSerialCommunication? startToolRs485(SerialConfig config, string ssh_password, int tcp_port = 54321)
```

- ***功能***
  - 启用工具RS485通讯。此接口会在机器人控制器上启动一个 socat 进程，将工具RS485串口的数据转发到指定的 TCP/IP 端口。
- ***参数***
  - `config`：串口配置。详情可查看：[串口通讯](./SerialCommunication.cn.md)
  - `ssh_password`：机器人 SSH 密码。
  - `tcp_port`：TCP端口，默认 `54321`。
- ***返回值***
  - 一个可以操作串口的对象，其本质是一个 TCP 客户端。详情可查看：[串口通讯](./SerialCommunication.cn.md)

### endToolRs485

```csharp
public bool endToolRs485(EliteSerialCommunication comm, string ssh_password)
```

- ***功能***
  - 停止工具RS485通讯。
- ***参数***
  - `comm`：如果不为`None`，会调用`SerialCommunication.disconnect()`方法。详情可查看：[串口通讯](./SerialCommunication.cn.md)
  - `ssh_password`：机器人 SSH 密码。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### startBoardRs485

```csharp
public EliteSerialCommunication? startBoardRs485(SerialConfig config, string ssh_password, int tcp_port = 54322)
```

- ***功能***
  - 启动控制柜端 RS485 透传。
- ***参数***
  - `config`：串口参数（波特率、校验位、停止位）。
  - `ssh_password`：机器人 SSH 密码。
  - `tcp_port`：本地转发端口，默认 `54322`。
- ***返回值***
  - 成功返回 `EliteSerialCommunication` 对象，失败可能返回 `null`。

### endBoardRs485

```csharp
public bool endBoardRs485(EliteSerialCommunication comm, string ssh_password)
```

- ***功能***
  - 关闭控制柜端 RS485 透传。
- ***参数***
  - `comm`：已创建的串口通信对象。
  - `ssh_password`：机器人 SSH 密码。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

## 资源释放

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放驱动持有的托管和非托管资源。
- ***参数***
  - 无。
- ***返回值***
  - 无。
