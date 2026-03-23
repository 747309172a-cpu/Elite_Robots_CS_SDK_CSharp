# RTSI 接口

## 简介

RTSI 是 Elite 机器人的实时通讯接口，可以获取机器人状态、设置IO等。SDK中提供了RTSI的两种接口：`RtsiClientInterface`和`RtsiIOInterface`。`RtsiClientInterface`需要手动操作连接、版本验证等。`RtsiIOInterface`则封装了大部分的接口。实际测试中`RtsiIOInterface`的实时性要差一点，而`RtsiClientInterface`的实时性取决于使用者的代码。

- `RtsiClientInterface`：基础 RTSI 客户端。
- `RtsiRecipe`：recipe 读写对象。
- `RtsiIoInterface`：高层 IO/状态读写封装。

## 导入

```csharp
using EliteRobots.CSharp;
```

## RtsiClientInterface

### RtsiClientInterface

```csharp
public RtsiClientInterface()
```

- ***功能***
  - 创建 RTSI 客户端。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### connect

```csharp
public void connect(string ip, int port = 30004)
```

- ***功能***
  - 连接 RTSI 服务。
- ***参数***
  - `ip`：机器人 IP。
  - `port`：端口，默认 `30004`。
- ***返回值***
  - 无。

### disconnect

```csharp
public void disconnect()
```

- ***功能***
  - 断开连接。
- ***参数***
  - 无。
- ***返回值***
  - 无。

### negotiateProtocolVersion

```csharp
public bool negotiateProtocolVersion(ushort version = 1)
```

- ***功能***
  - 协商 RTSI 协议版本。
- ***参数***
  - `version`：协议版本，默认 `1`。
- ***返回值***
  - 协商成功返回 `true`，失败返回 `false`。

### getControllerVersion

```csharp
public RtsiVersionInfo getControllerVersion()
```

- ***功能***
  - 获取控制器版本。
- ***参数***
  - 无。
- ***返回值***
  - 返回 `RtsiVersionInfo`。

### setupOutputRecipe

```csharp
public RtsiRecipe setupOutputRecipe(IEnumerable<string> recipe_list, double frequency = 250)
```

- ***功能***
  - 配置输出订阅配方。
- ***参数***
  - recipe_list：配方字符串。具体内容参考Elite官方文档“RTSI用户手册”

  - frequency：更新频率
- ***返回值***
  - 返回 `RtsiRecipe` 对象。

### setupInputRecipe

```csharp
public RtsiRecipe setupInputRecipe(IEnumerable<string> recipe)
```

- ***功能***
  - 配置输入 recipe。
- ***参数***
  - `recipe`：输入字段列表。
- ***返回值***
  - 返回 `RtsiRecipe` 对象。

### start

```csharp
public bool start()
```

- ***功能***
  - 启动 RTSI 数据交换。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### pause

```csharp
public bool pause()
```

- ***功能***
  - 暂停 RTSI 数据交换。
- ***参数***
  - 无。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### send

```csharp
public void send(RtsiRecipe recipe)
```

- ***功能***
  - 发送输入 recipe 数据。
- ***参数***
  - `recipe`：待发送 recipe。
- ***返回值***
  - 无。

### receiveData（单 recipe）

```csharp
public bool receiveData(RtsiRecipe recipe, bool read_newest = false)
```

- ***功能***
  - 接收单个 recipe 数据。
- ***参数***
  - `recipe`：输出订阅的配方列表。仅接收一个配方，并更新列表中配方的数据。建议read_newest为false。
  - `read_newest`：是否读取最新包。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### receiveData（多 recipe）

```csharp
public int receiveData(IReadOnlyList<RtsiRecipe> recipes, bool read_newest = false)
```

- ***功能***
  - 接收输出订阅的配方数据。
- ***参数***
  - `recipes`：输出订阅的配方。多配方的情况下，如果接收到的不是输入的配方，则不会更新此配方的数据。
  - `read_newest`：是否读取最新包。
- ***返回值***
  - 返回命中 recipe 的索引。

### isConnected

```csharp
public bool isConnected()
```

- ***功能***
  - 查询连接状态。
- ***参数***
  - 无。
- ***返回值***
  - 已连接返回 `true`，否则 `false`。

### isStarted

```csharp
public bool isStarted()
```

- ***功能***
  - 是否已开始同步机器人数据。
- ***参数***
  - 无。
- ***返回值***
  - 已启动返回 `true`，否则 `false`。

### isReadAvailable

```csharp
public bool isReadAvailable()
```

- ***功能***
  - 查询是否有可读数据。
- ***参数***
  - 无。
- ***返回值***
  - 有数据返回 `true`，否则 `false`。

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放客户端资源。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## RtsiRecipe

### getID

```csharp
public int getID()
```

- ***功能***
  - 获取配方ID。
- ***参数***
  - 无。
- ***返回值***
  - 返回配方ID。

### getRecipe

```csharp
public string[] getRecipe()
```

- ***功能***
  - 获取配方订阅项名称的列表。
- ***参数***
  - 无。
- ***返回值***
  - 配方订阅项名称的列表。

### getValue（多重载）

```csharp
public bool getValue(string name, out double out_value)
public bool getValue(string name, out int out_value)
public bool getValue(string name, out uint out_value)
public bool getValue(string name, out bool out_value)
public bool getValue(string name, double[] out_value6)
```

- ***功能***
  - 获取配方中订阅项的值。
- ***参数***
  - `name`：订阅项名称。
  - `out_value/out_value6`：订阅项输出值，注意此值的类型需要和RTSI文档中的类型一致。
- ***返回值***
  - 读取成功返回 `true`，失败返回 `false`。

### setValue（多重载）

```csharp
public bool setValue(string name, double value)
public bool setValue(string name, int value)
public bool setValue(string name, uint value)
public bool setValue(string name, bool value)
public bool setValue(string name, double[] value6)
```

- ***功能***
  - 设置配方中订阅项的值。
- ***参数***
  - `name`：订阅项名称。
  - `value/value6`：订阅项设置值，注意此值的类型需要和RTSI文档中的类型一致。
- ***返回值***
  - 设置成功返回 `true`，失败返回 `false`。

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放 recipe 资源。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## RtsiIoInterface

### RtsiIoInterface

```csharp
public RtsiIoInterface(IEnumerable<string> output_recipe, IEnumerable<string> input_recipe, double frequency = 250)
```

- ***功能***
  - 创建 RTSI IO 高层接口。
- ***参数***
  - `output_recipe`：输出字段列表。
  - `input_recipe`：输入字段列表。
  - `frequency`：采样频率。
- ***返回值***
  - 无。

### connect / disconnect / isConnected / isStarted

```csharp
public bool connect(string ip)
public void disconnect()
public bool isConnected()
public bool isStarted()
```

- ***功能***
  - 连接、断开及查询 RTSI IO 状态。
- ***参数***
  - `connect` 的 `ip`：机器人 IP。
- ***返回值***
  - `connect/isConnected/isStarted`：布尔值。
  - `disconnect`：无返回。

### getControllerVersion

```csharp
public RtsiVersionInfo getControllerVersion()
```

- ***功能***
  - 获取控制器版本。
- ***参数***
  - 无。
- ***返回值***
  - 返回 `RtsiVersionInfo`。

### 常用写入接口

```csharp
public bool setSpeedScaling(double scaling)
public bool setStandardDigital(int index, bool level)
public bool setConfigureDigital(int index, bool level)
public bool setAnalogOutputVoltage(int index, double value)
public bool setAnalogOutputCurrent(int index, double value)
public bool setExternalForceTorque(double[] value6)
public bool setToolDigitalOutput(int index, bool level)
```

- ***功能***
  - 设置速度、数字 IO、模拟 IO、外力和工具端数字输出。
- ***参数***
  - `index`：通道索引。
  - `level`：开关量电平。
  - `value`：模拟量值。
  - `value6`：6 维外力向量。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### 常用读取接口（节选）

```csharp
public double[] getActualJointPositions()
public double[] getActualTCPPose()
public double[] getActualTCPVelocity()
public double[] getActualTCPForce()
public RobotMode getRobotMode()
public SafetyMode getSafetyStatus()
public TaskStatus getRuntimeState()
```

- ***功能***
  - 读取关节状态、TCP 状态与机器人状态枚举。
- ***参数***
  - 无。
- ***返回值***
  - 返回对应数组或枚举值。

### recipe 动态读写

```csharp
public bool getRecipeValue(string name, out double value)
public bool getRecipeValue(string name, out int value)
public bool getRecipeValue(string name, out uint value)
public bool getRecipeValue(string name, out bool value)
public bool getRecipeValue(string name, double[] value3or6)

public bool setInputRecipeValue(string name, double value)
public bool setInputRecipeValue(string name, int value)
public bool setInputRecipeValue(string name, uint value)
public bool setInputRecipeValue(string name, bool value)
public bool setInputRecipeValue(string name, double[] value6)
```

- ***功能***
  - 按字段名读取输出 recipe 值，并设置输入 recipe 值。
- ***参数***
  - `name`：字段名。
  - `value/value3or6/value6`：值容器或输入值。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放 RTSI IO 资源。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## 数据类型

### RtsiVersionInfo

```csharp
public sealed class RtsiVersionInfo
{
    public uint Major { get; init; }
    public uint Minor { get; init; }
    public uint Bugfix { get; init; }
    public uint Build { get; init; }
}
```

- ***功能***
  - 保存 RTSI 版本号。

### 相关枚举

```csharp
public enum JointMode
public enum ToolMode
public enum ToolDigitalMode
public enum ToolDigitalOutputMode
```

- ***功能***
  - 描述关节模式、工具模式和工具 IO 模式。
