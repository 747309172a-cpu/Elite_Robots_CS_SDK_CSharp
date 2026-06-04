## [Unrelease]
### Added
- 新增 `EliteDriver` 轨迹反馈回调、带速度/加速度的 `writeTrajectoryPoint` 重载，以及 4 个 `servoj` 配置字段。
- 新增 `KinematicsBase` C# 封装，支持 `setMDH`、`getPositionFK`、`getPositionIK` 和默认超时时间接口。
- 新增 `PoseAlgebraBase` C# 封装，支持位姿矩阵/向量转换、复合、求逆、加减、坐标系转换和距离计算。
- 新增 `kinematics` 与 `pose_algebra` C# 示例。
- 更新 `trajectory` C# 示例，使其对齐 C++ SDK 示例流程，覆盖轨迹反馈、按时间轨迹和按速度轨迹。
- 新增 native bootstrap 构建参数 `EliteCompileKinPlugin` 和 `EliteCompilePoseAlgPlugin`，用于在源码构建上游 C++ SDK 时启用插件编译。
- 更新中英文使用指南和 API 文档，补充新增接口与示例说明。

### Initial Release
- 首次公开版本发布。
