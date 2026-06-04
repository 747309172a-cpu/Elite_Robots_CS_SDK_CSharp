# PoseAlgebraBase Class

## Introduction

`PoseAlgebraBase` is the C# wrapper for the pose algebra plugin interface. Its public method names match the C++ SDK. It provides pose vector/matrix conversion, pose composition, inverse, frame conversion, addition/subtraction, and distance calculation.

## Import

```csharp
using EliteRobots.CSharp;
```

## Constructor

### PoseAlgebraBase

```csharp
public PoseAlgebraBase(string plugin_lib_path, string? plugin_class_name = null)
```

- ***Function***
  - Load the pose algebra plugin and create a `PoseAlgebraBase` instance.
- ***Parameters***
  - `plugin_lib_path`: path to the pose algebra plugin dynamic library.
  - `plugin_class_name`: plugin class name, such as `ELITE::EigenPoseAlgebra`.
- ***Return Value***
  - None.
- ***Exceptions***
  - `ArgumentNullException`: `plugin_lib_path` is null.
  - `EliteSdkException`: plugin loading or instance creation failed.

## Plugin Library

Common plugin library and class names:

| Platform | Plugin library | Plugin class name |
| --- | --- | --- |
| Windows | `elite_eigen_pose_algebra.dll` | `ELITE::EigenPoseAlgebra` |
| Windows | `elite_pose_algebra.dll` | `ELITE::ElitePoseAlgebra` |
| Linux | `libelite_eigen_pose_algebra.so` | `ELITE::EigenPoseAlgebra` |
| Linux | `libelite_pose_algebra.so` | `ELITE::ElitePoseAlgebra` |

On Windows, the runtime directory must also contain `elite_cs_sdk.dll`, `elite_cs_series_sdk_c.dll`, and `elite_cs_series_sdk.dll`. The plugin DLL should be built with `/p:EliteLinkUpstreamStatic=false`; otherwise plugin instance creation can fail even when all DLL files exist.

## Related Types

### PoseMatrix

```csharp
public struct PoseMatrix
{
    public double[] data;
}
```

- ***Function***
  - Represents a 4x4 pose matrix.
- ***Notes***
  - `data` must have length 16 and is stored in row-major order.

### PoseDistance

```csharp
public struct PoseDistance
{
    public double linear_distance;
    public double angular_distance;
}
```

- ***Function***
  - Represents linear and angular distance between two poses.

### PoseAlgebraResult

```csharp
public struct PoseAlgebraResult
{
    public PoseAlgebraError error;
    public string message;
}
```

- ***Function***
  - Represents the result of a pose algebra operation.

### PoseAlgebraError

```csharp
public enum PoseAlgebraError
{
    SUCCESS = 0,
    INVALID_INPUT = 1,
    SINGULAR_MATRIX = 2,
    INVALID_ROTATION_MATRIX = 3,
    NUMERICAL_ERROR = 4,
    UNSUPPORTED_OPERATION = 5,
    INTERNAL_ERROR = 6,
}
```

- ***Function***
  - Represents the operation result code returned in `PoseAlgebraResult.error`.

## Methods

### inverse

```csharp
public bool inverse(PoseMatrix pose, ref PoseMatrix inverse_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Calculate the inverse of a pose matrix.
- ***Parameters***
  - `pose`: input pose matrix.
  - `inverse_pose`: output inverse pose matrix.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### inverse

```csharp
public bool inverse(double[] pose, double[] inverse_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Calculate the inverse of a pose vector.
- ***Parameters***
  - `pose`: input pose vector, length must be 6.
  - `inverse_pose`: output inverse pose vector, length must be 6.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### multiply

```csharp
public bool multiply(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Compose two pose matrices.
- ***Parameters***
  - `left_pose`: left pose matrix.
  - `right_pose`: right pose matrix.
  - `out_pose`: output composed pose matrix.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### multiply

```csharp
public bool multiply(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Compose two pose vectors.
- ***Parameters***
  - `left_pose`: left pose vector, length must be 6.
  - `right_pose`: right pose vector, length must be 6.
  - `out_pose`: output composed pose vector, length must be 6.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### add

```csharp
public bool add(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Add two pose matrices.
- ***Parameters***
  - `left_pose`: left pose matrix.
  - `right_pose`: right pose matrix.
  - `out_pose`: output pose matrix.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### add

```csharp
public bool add(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Add two pose vectors.
- ***Parameters***
  - `left_pose`: left pose vector, length must be 6.
  - `right_pose`: right pose vector, length must be 6.
  - `out_pose`: output pose vector, length must be 6.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### subtract

```csharp
public bool subtract(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Subtract two pose matrices.
- ***Parameters***
  - `left_pose`: left pose matrix.
  - `right_pose`: right pose matrix.
  - `out_pose`: output pose matrix.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### subtract

```csharp
public bool subtract(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Subtract two pose vectors.
- ***Parameters***
  - `left_pose`: left pose vector, length must be 6.
  - `right_pose`: right pose vector, length must be 6.
  - `out_pose`: output pose vector, length must be 6.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### vectorToMatrix

```csharp
public bool vectorToMatrix(double[] pose_vector, ref PoseMatrix pose_matrix, out PoseAlgebraResult result)
```

- ***Function***
  - Convert a pose vector to a pose matrix.
- ***Parameters***
  - `pose_vector`: input pose vector, length must be 6.
  - `pose_matrix`: output pose matrix.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### matrixToVector

```csharp
public bool matrixToVector(PoseMatrix pose_matrix, double[] pose_vector, out PoseAlgebraResult result)
```

- ***Function***
  - Convert a pose matrix to a pose vector.
- ***Parameters***
  - `pose_matrix`: input pose matrix.
  - `pose_vector`: output pose vector, length must be 6.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### distance

```csharp
public bool distance(PoseMatrix pose_a, PoseMatrix pose_b, out PoseDistance out_distance, out PoseAlgebraResult result)
```

- ***Function***
  - Calculate the distance between two pose matrices.
- ***Parameters***
  - `pose_a`: first pose matrix.
  - `pose_b`: second pose matrix.
  - `out_distance`: output distance.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### distance

```csharp
public bool distance(double[] pose_a, double[] pose_b, out PoseDistance out_distance, out PoseAlgebraResult result)
```

- ***Function***
  - Calculate the distance between two pose vectors.
- ***Parameters***
  - `pose_a`: first pose vector, length must be 6.
  - `pose_b`: second pose vector, length must be 6.
  - `out_distance`: output distance.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### worldToLocal

```csharp
public bool worldToLocal(PoseMatrix world_ref_pose, PoseMatrix world_pose, ref PoseMatrix local_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Convert a world-frame pose matrix to the local frame of a reference pose.
- ***Parameters***
  - `world_ref_pose`: reference pose matrix in the world frame.
  - `world_pose`: target pose matrix in the world frame.
  - `local_pose`: output local-frame pose matrix.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### worldToLocal

```csharp
public bool worldToLocal(double[] world_ref_pose, double[] world_pose, double[] local_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Convert a world-frame pose vector to the local frame of a reference pose.
- ***Parameters***
  - `world_ref_pose`: reference pose vector in the world frame, length must be 6.
  - `world_pose`: target pose vector in the world frame, length must be 6.
  - `local_pose`: output local-frame pose vector, length must be 6.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### localToWorld

```csharp
public bool localToWorld(PoseMatrix world_ref_pose, PoseMatrix local_pose, ref PoseMatrix world_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Convert a local-frame pose matrix relative to a reference pose to the world frame.
- ***Parameters***
  - `world_ref_pose`: reference pose matrix in the world frame.
  - `local_pose`: pose matrix in the local frame.
  - `world_pose`: output world-frame pose matrix.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### localToWorld

```csharp
public bool localToWorld(double[] world_ref_pose, double[] local_pose, double[] world_pose, out PoseAlgebraResult result)
```

- ***Function***
  - Convert a local-frame pose vector relative to a reference pose to the world frame.
- ***Parameters***
  - `world_ref_pose`: reference pose vector in the world frame, length must be 6.
  - `local_pose`: pose vector in the local frame, length must be 6.
  - `world_pose`: output world-frame pose vector, length must be 6.
  - `result`: operation result.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release the native pose algebra plugin instance.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## Notes

- All pose vector arrays must have length 6.
- All `PoseMatrix.data` arrays must have length 16.
- If an operation returns `false`, check `PoseAlgebraResult.error` and `PoseAlgebraResult.message`.

## Example

Minimal C# usage on Windows:

```csharp
using System;
using System.IO;
using EliteRobots.CSharp;

var pluginPath = Path.Combine(AppContext.BaseDirectory, "elite_eigen_pose_algebra.dll");

using var poseAlgebra = new PoseAlgebraBase(pluginPath, "ELITE::EigenPoseAlgebra");

double[] pose = { 0.3, 0.1, 0.2, 0, 0, 1.57 };
double[] inversePose = new double[6];

if (!poseAlgebra.inverse(pose, inversePose, out var result))
{
    Console.WriteLine($"{result.error}: {result.message}");
    return;
}

Console.WriteLine(string.Join(", ", inversePose));
```

Example project command on Linux:

```bash
dotnet run --project example -- pose_algebra /path/to/libelite_eigen_pose_algebra.so ELITE::EigenPoseAlgebra
```

Example project command on Windows:

```powershell
dotnet run --project example -- pose_algebra .\libs\elite_eigen_pose_algebra.dll ELITE::EigenPoseAlgebra
```
