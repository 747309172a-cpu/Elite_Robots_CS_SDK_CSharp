[English](./README.md)
# Elite Robots CS SDK (C#)

本仓库提供 Elite Robots CS SDK 的 C# 绑定。
默认情况下，`dotnet build` 和 `dotnet run` 会在缺少原生 C 封装库 `elite_cs_series_sdk_c` 时自动拉取并编译。


## 要求
- ***CS Controller*** (机器人的控制软件) 如果使用 2.13.x 则需要 >= **2.13.4**(for CS-Series)，如果使用2.14.x 则需要 >= **2.14.2**。 如果机器人的控制软件版本低于此，建议升级。
- .NET SDK 8.0+
- git、cmake、C/C++ 编译器

## 编译与安装
编译安装的方式参考：[构建指南](./doc/BuildGuide/BuildGuide.cn.md)

## 使用说明
运行方式、示例说明、常见问题和外部项目接入请参考：[使用指南](./doc/UserGuide/UserGuide.cn.md)


## API手册
[API手册](./doc/API/cn/API.cn.md)
