# EliteControllerLog 模块

## 简介

`EliteControllerLog` 提供机器人控制器日志下载能力。

## 导入

```csharp
using EliteRobots.CSharp;
```

## 接口

### downloadSystemLog

```csharp
public static bool downloadSystemLog(string robot_ip, string password, string path, Action<int, int, string>? progress_cb = null)
```

- ***功能***
  - 从机器人下载系统日志到本地。
- ***参数***
  - `robot_ip`：机器人 IP。
  - `password`：机器人 SSH 密码。
  - `path`：本地保存路径。
  - `progress_cb`：下载进度回调（可选）。
- ***回调参数***
  - `fileSize`：文件总大小（字节）。
  - `recvSize`：当前已接收大小（字节）。
  - `err`：错误信息字符串。
- ***返回值***
  - 下载成功返回 `true`，失败返回 `false`。

- ***注意事项***

  1. 在Linux系统下，如果未安装`libssh`，需要确保运行SDK的计算机具有`scp`、`ssh`和`sshpass`命令可用
  2. 在Windows系统下，如果未安装libssh，则此接口不可用