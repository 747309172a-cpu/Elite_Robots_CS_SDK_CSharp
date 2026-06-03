# PoseAlgebraBase 类

## 简介

`PoseAlgebraBase` 是位姿代数插件接口的 C# 封装，接口名称与 C++ SDK 保持一致。它用于位姿向量和矩阵的转换、位姿复合、求逆、坐标系转换、加减和距离计算。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 构造函数

### PoseAlgebraBase

```csharp
public PoseAlgebraBase(string plugin_lib_path, string? plugin_class_name = null)
```

- ***功能***
  - 加载位姿代数插件并创建 `PoseAlgebraBase` 实例。
- ***参数***
  - `plugin_lib_path`：位姿代数插件动态库路径。
  - `plugin_class_name`：插件类名；可传入 `ELITE::EigenPoseAlgebra`。
- ***返回值***
  - 无。
- ***异常***
  - `ArgumentNullException`：`plugin_lib_path` 为空。
  - `EliteSdkException`：插件加载或实例创建失败。

## 相关类型

### PoseMatrix

```csharp
public struct PoseMatrix
{
    public double[] data;
}
```

- ***功能***
  - 表示 4x4 位姿矩阵。
- ***说明***
  - `data` 长度必须为 16，按行优先顺序存储。

### PoseDistance

```csharp
public struct PoseDistance
{
    public double linear_distance;
    public double angular_distance;
}
```

- ***功能***
  - 表示两个位姿之间的线性距离和角度距离。

### PoseAlgebraResult

```csharp
public struct PoseAlgebraResult
{
    public PoseAlgebraError error;
    public string message;
}
```

- ***功能***
  - 表示位姿代数计算结果。

## 方法

### inverse

```csharp
public bool inverse(PoseMatrix pose, ref PoseMatrix inverse_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 计算位姿矩阵的逆。
- ***参数***
  - `pose`：输入位姿矩阵。
  - `inverse_pose`：输出逆位姿矩阵。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### inverse

```csharp
public bool inverse(double[] pose, double[] inverse_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 计算位姿向量的逆。
- ***参数***
  - `pose`：输入位姿向量，长度必须为 6。
  - `inverse_pose`：输出逆位姿向量，长度必须为 6。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### multiply

```csharp
public bool multiply(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 复合两个位姿矩阵。
- ***参数***
  - `left_pose`：左侧位姿矩阵。
  - `right_pose`：右侧位姿矩阵。
  - `out_pose`：输出复合后的位姿矩阵。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### multiply

```csharp
public bool multiply(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 复合两个位姿向量。
- ***参数***
  - `left_pose`：左侧位姿向量，长度必须为 6。
  - `right_pose`：右侧位姿向量，长度必须为 6。
  - `out_pose`：输出复合后的位姿向量，长度必须为 6。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### add

```csharp
public bool add(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 对两个位姿矩阵执行加法运算。
- ***参数***
  - `left_pose`：左侧位姿矩阵。
  - `right_pose`：右侧位姿矩阵。
  - `out_pose`：输出位姿矩阵。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### add

```csharp
public bool add(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 对两个位姿向量执行加法运算。
- ***参数***
  - `left_pose`：左侧位姿向量，长度必须为 6。
  - `right_pose`：右侧位姿向量，长度必须为 6。
  - `out_pose`：输出位姿向量，长度必须为 6。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### subtract

```csharp
public bool subtract(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 对两个位姿矩阵执行减法运算。
- ***参数***
  - `left_pose`：左侧位姿矩阵。
  - `right_pose`：右侧位姿矩阵。
  - `out_pose`：输出位姿矩阵。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### subtract

```csharp
public bool subtract(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 对两个位姿向量执行减法运算。
- ***参数***
  - `left_pose`：左侧位姿向量，长度必须为 6。
  - `right_pose`：右侧位姿向量，长度必须为 6。
  - `out_pose`：输出位姿向量，长度必须为 6。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### vectorToMatrix

```csharp
public bool vectorToMatrix(double[] pose_vector, ref PoseMatrix pose_matrix, out PoseAlgebraResult result)
```

- ***功能***
  - 将位姿向量转换为位姿矩阵。
- ***参数***
  - `pose_vector`：输入位姿向量，长度必须为 6。
  - `pose_matrix`：输出位姿矩阵。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### matrixToVector

```csharp
public bool matrixToVector(PoseMatrix pose_matrix, double[] pose_vector, out PoseAlgebraResult result)
```

- ***功能***
  - 将位姿矩阵转换为位姿向量。
- ***参数***
  - `pose_matrix`：输入位姿矩阵。
  - `pose_vector`：输出位姿向量，长度必须为 6。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### distance

```csharp
public bool distance(PoseMatrix pose_a, PoseMatrix pose_b, out PoseDistance out_distance, out PoseAlgebraResult result)
```

- ***功能***
  - 计算两个位姿矩阵之间的距离。
- ***参数***
  - `pose_a`：第一个位姿矩阵。
  - `pose_b`：第二个位姿矩阵。
  - `out_distance`：输出距离。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### distance

```csharp
public bool distance(double[] pose_a, double[] pose_b, out PoseDistance out_distance, out PoseAlgebraResult result)
```

- ***功能***
  - 计算两个位姿向量之间的距离。
- ***参数***
  - `pose_a`：第一个位姿向量，长度必须为 6。
  - `pose_b`：第二个位姿向量，长度必须为 6。
  - `out_distance`：输出距离。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### worldToLocal

```csharp
public bool worldToLocal(PoseMatrix world_ref_pose, PoseMatrix world_pose, ref PoseMatrix local_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 将世界坐标系下的位姿矩阵转换到参考位姿的局部坐标系。
- ***参数***
  - `world_ref_pose`：世界坐标系下的参考位姿矩阵。
  - `world_pose`：世界坐标系下的目标位姿矩阵。
  - `local_pose`：输出局部坐标系位姿矩阵。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### worldToLocal

```csharp
public bool worldToLocal(double[] world_ref_pose, double[] world_pose, double[] local_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 将世界坐标系下的位姿向量转换到参考位姿的局部坐标系。
- ***参数***
  - `world_ref_pose`：世界坐标系下的参考位姿向量，长度必须为 6。
  - `world_pose`：世界坐标系下的目标位姿向量，长度必须为 6。
  - `local_pose`：输出局部坐标系位姿向量，长度必须为 6。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### localToWorld

```csharp
public bool localToWorld(PoseMatrix world_ref_pose, PoseMatrix local_pose, ref PoseMatrix world_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 将参考位姿局部坐标系下的位姿矩阵转换到世界坐标系。
- ***参数***
  - `world_ref_pose`：世界坐标系下的参考位姿矩阵。
  - `local_pose`：局部坐标系下的位姿矩阵。
  - `world_pose`：输出世界坐标系位姿矩阵。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### localToWorld

```csharp
public bool localToWorld(double[] world_ref_pose, double[] local_pose, double[] world_pose, out PoseAlgebraResult result)
```

- ***功能***
  - 将参考位姿局部坐标系下的位姿向量转换到世界坐标系。
- ***参数***
  - `world_ref_pose`：世界坐标系下的参考位姿向量，长度必须为 6。
  - `local_pose`：局部坐标系下的位姿向量，长度必须为 6。
  - `world_pose`：输出世界坐标系位姿向量，长度必须为 6。
  - `result`：计算结果。
- ***返回值***
  - 成功返回 `true`，失败返回 `false`。

### Dispose

```csharp
public void Dispose()
```

- ***功能***
  - 释放 native 位姿代数插件实例。
- ***参数***
  - 无。
- ***返回值***
  - 无。

## 注意事项

- 所有位姿向量数组长度必须为 6。
- 所有 `PoseMatrix.data` 数组长度必须为 16。
- 当返回 `false` 时，可查看 `PoseAlgebraResult.error` 和 `PoseAlgebraResult.message`。

## 示例

```bash
dotnet run --project example -- pose_algebra /path/to/libelite_eigen_pose_algebra.so ELITE::EigenPoseAlgebra
```
