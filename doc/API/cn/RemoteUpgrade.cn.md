# RemoteUpgrade 模块

## 简介

RemoteUpgrade模块提供了机器人控制软件的远程升级功能。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 接口

### upgradeControlSoftware

```csharp
public static bool upgradeControlSoftware(string ip, string file, string password)
```

- ***功能***
  - 触发控制软件远程升级。
- ***参数***
  - `ip`：机器人 IP。
  - `file`：升级包文件路径。
  - `password`：机器人 SSH 密码。
- ***返回值***
  - 请求执行成功返回 `true`，失败返回 `false`。
- ***注意事项***

  1. 在Linux系统下，如果未安装`libssh`，需要确保运行SDK的计算机具有`scp`、`ssh`和`sshpass`命令可用
  2. 在Windows系统下，如果未安装libssh，则此接口不可用