# VersionInfo

## 简介

VersionInfo类用于包含和管理版本信息，提供了版本号的存储、比较和转换功能。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 结构定义

### VersionInfo

```csharp
public struct VersionInfo
{
    public uint major;
    public uint minor;
    public uint bugfix;
    public uint build;
}
```

- ***功能***
  - 保存四段式版本号。

## 构造与转换

### VersionInfo(uint major, uint minor, uint bugfix, uint build)

```csharp
public VersionInfo(uint major, uint minor, uint bugfix, uint build)
```

- ***功能***
  - 通过四段数字创建版本对象。
- ***参数***
  - `major/minor/bugfix/build`：版本号各段。
- ***返回值***
  - 无。

### VersionInfo(string version)

```csharp
public VersionInfo(string version)
```

- ***功能***
  - 从字符串解析版本对象。
- ***参数***
  - `version`：版本字符串。
- ***返回值***
  - 无。

### ToString

```csharp
public override string ToString()
```

- ***功能***
  - 转为字符串。
- ***参数***
  - 无。
- ***返回值***
  - 返回版本字符串。

### fromString

```csharp
public static VersionInfo fromString(string version)
```

- ***功能***
  - 从字符串创建 `VersionInfo`。
- ***参数***
  - `version`：版本字符串。
- ***返回值***
  - 返回版本对象。

## 比较运算符

```csharp
==  !=  <  <=  >  >=
```

- ***功能***
  - 支持版本对象比较。
- ***返回值***
  - 返回布尔值。

## 相关类型

### RtsiVersionInfo

```csharp
public sealed class RtsiVersionInfo
```

- ***功能***
  - RTSI 版本信息对象（`Major`、`Minor`、`Bugfix`、`Build`）。
