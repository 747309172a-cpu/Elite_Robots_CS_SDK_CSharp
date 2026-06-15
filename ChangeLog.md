## [Unrelease]
### Added
- Added `EliteDriver` trajectory feedback callback, speed/acceleration-based `writeTrajectoryPoint` overload, and four new `servoj` configuration fields.
- Added the `KinematicsBase` C# wrapper with `setMDH`, `getPositionFK`, `getPositionIK`, and default-timeout APIs.
- Added the `PoseAlgebraBase` C# wrapper with pose matrix/vector conversion, composition, inverse, add/subtract, frame conversion, and distance calculation.
- Added Primary port control APIs for power on/off, brake release, pause, stop, protective stop unlock, safety restart, and speed scaling.
- Added `kinematics` and `pose_algebra` C# examples.
- Updated the `primary_client` C# example with an optional Primary control demo.
- Updated the `trajectory` C# example to match the C++ SDK sample flow, covering trajectory feedback, time-based trajectory, and speed-based trajectory.
- Added native bootstrap build properties `EliteCompileKinPlugin` and `EliteCompilePoseAlgPlugin` to enable plugin builds when the upstream C++ SDK is built from source.
- Updated Chinese and English user guides and API documentation for the new interfaces and examples.

### Initial Release
- First public version release
