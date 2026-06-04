# KinematicsBase Class

## Introduction

`KinematicsBase` is the C# wrapper for the kinematics plugin interface. Its public method names match the C++ SDK. It loads a kinematics plugin through the native C wrapper and provides MDH setup, forward kinematics, inverse kinematics, and default-timeout configuration.

## Import

```csharp
using EliteRobots.CSharp;
```

## Constructor

### KinematicsBase

```csharp
public KinematicsBase(string plugin_lib_path, string? plugin_class_name = null)
```

- ***Function***
  - Load the kinematics plugin and create a `KinematicsBase` instance.
- ***Parameters***
  - `plugin_lib_path`: path to the kinematics plugin dynamic library.
  - `plugin_class_name`: plugin class name, such as `ELITE::KdlKinematicsPlugin`.
- ***Return Value***
  - None.
- ***Exceptions***
  - `ArgumentNullException`: `plugin_lib_path` is null.
  - `EliteSdkException`: plugin loading or instance creation failed.

## Plugin Library

Common plugin library and class names:

| Platform | Plugin library | Plugin class name |
| --- | --- | --- |
| Windows | `elite_kdl_kinematics.dll` | `ELITE::KdlKinematicsPlugin` |
| Linux | `libelite_kdl_kinematics.so` | `ELITE::KdlKinematicsPlugin` |

On Windows, the runtime directory must also contain `elite_cs_sdk.dll`, `elite_cs_series_sdk_c.dll`, and `elite_cs_series_sdk.dll`. The plugin DLL should be built with `/p:EliteLinkUpstreamStatic=false`; otherwise plugin instance creation can fail even when all DLL files exist.

## Related Types

### KinematicsResult

```csharp
public struct KinematicsResult
{
    public KinematicError kinematic_error;
}
```

- ***Function***
  - Represents the kinematics solving result.

### KinematicError

```csharp
public enum KinematicError
{
    OK = 1,
    SOLVER_NOT_ACTIVE = 2,
    NO_SOLUTION = 3,
}
```

- ***Function***
  - Represents the kinematics error type.

## Methods

### setMDH

```csharp
public void setMDH(double[] alpha, double[] a, double[] d)
```

- ***Function***
  - Set robot MDH parameters.
- ***Parameters***
  - `alpha`: MDH alpha parameter, length must be 6.
  - `a`: MDH a parameter, length must be 6.
  - `d`: MDH d parameter, length must be 6.
- ***Return Value***
  - None.
- ***Exceptions***
  - `ArgumentNullException`: an array parameter is null.
  - `ArgumentException`: an array length is not 6.
  - `EliteSdkException`: native call failed.

### getPositionFK

```csharp
public bool getPositionFK(double[] joint_angles, double[] poses)
```

- ***Function***
  - Calculate TCP pose from joint angles.
- ***Parameters***
  - `joint_angles`: input joint angles, length must be 6.
  - `poses`: output TCP pose, length must be 6.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.
- ***Exceptions***
  - `ArgumentNullException`: an array parameter is null.
  - `ArgumentException`: an array length is not 6.
  - `EliteSdkException`: native call failed.

### getPositionIK

```csharp
public bool getPositionIK(double[] pose, double[] near, double[] solution, out KinematicsResult result)
```

- ***Function***
  - Calculate one inverse kinematics solution from TCP pose and a reference joint position.
- ***Parameters***
  - `pose`: input TCP pose, length must be 6.
  - `near`: reference joint position, length must be 6.
  - `solution`: output joint solution, length must be 6.
  - `result`: kinematics result containing `kinematic_error`.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.
- ***Exceptions***
  - `ArgumentNullException`: an array parameter is null.
  - `ArgumentException`: an array length is not 6.
  - `EliteSdkException`: native call failed.

### getPositionIK

```csharp
public bool getPositionIK(double[] pose, double[] near, List<double[]> solutions, out KinematicsResult result)
```

- ***Function***
  - Calculate multiple inverse kinematics solutions from TCP pose and a reference joint position, using the default maximum solution count.
- ***Parameters***
  - `pose`: input TCP pose, length must be 6.
  - `near`: reference joint position, length must be 6.
  - `solutions`: output solution list. It is cleared and filled with new solutions on success.
  - `result`: kinematics result containing `kinematic_error`.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.
- ***Exceptions***
  - `ArgumentNullException`: an array parameter or `solutions` is null.
  - `ArgumentException`: an array length is not 6.
  - `EliteSdkException`: native call failed.

### getPositionIK

```csharp
public bool getPositionIK(double[] pose, double[] near, List<double[]> solutions, int max_solutions, out KinematicsResult result)
```

- ***Function***
  - Calculate multiple inverse kinematics solutions from TCP pose and a reference joint position, with a specified maximum solution count.
- ***Parameters***
  - `pose`: input TCP pose, length must be 6.
  - `near`: reference joint position, length must be 6.
  - `solutions`: output solution list. It is cleared and filled with new solutions on success.
  - `max_solutions`: maximum number of solutions to return, must be greater than or equal to 0.
  - `result`: kinematics result containing `kinematic_error`.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.
- ***Exceptions***
  - `ArgumentNullException`: an array parameter or `solutions` is null.
  - `ArgumentException`: an array length is not 6, or `max_solutions` is less than 0.
  - `EliteSdkException`: native call failed.

### setDefaultTimeout

```csharp
public void setDefaultTimeout(double timeout)
```

- ***Function***
  - Set the default timeout used by the kinematics solver.
- ***Parameters***
  - `timeout`: timeout value.
- ***Return Value***
  - None.
- ***Exceptions***
  - `EliteSdkException`: native call failed.

### getDefaultTimeout

```csharp
public double getDefaultTimeout()
```

- ***Function***
  - Get the default timeout used by the kinematics solver.
- ***Parameters***
  - None.
- ***Return Value***
  - Default timeout value.
- ***Exceptions***
  - `EliteSdkException`: native call failed.

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release the native kinematics plugin instance.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## Example

Minimal C# usage on Windows:

```csharp
using System;
using System.IO;
using EliteRobots.CSharp;

var pluginPath = Path.Combine(AppContext.BaseDirectory, "elite_kdl_kinematics.dll");

using var kinematics = new KinematicsBase(pluginPath, "ELITE::KdlKinematicsPlugin");

double[] alpha = { 0, -Math.PI / 2, 0, -Math.PI / 2, Math.PI / 2, -Math.PI / 2 };
double[] a = { 0, 0, -0.425, -0.39225, 0, 0 };
double[] d = { 0.089159, 0, 0, 0.10915, 0.09465, 0.0823 };
kinematics.setMDH(alpha, a, d);

double[] joints = { 0, -1.57, 1.57, 0, 1.57, 0 };
double[] pose = new double[6];

if (kinematics.getPositionFK(joints, pose))
{
    Console.WriteLine(string.Join(", ", pose));
}
```

Example project command on Linux:

```bash
dotnet run --project example -- kinematics 172.16.102.156 /path/to/libelite_kdl_kinematics.so ELITE::KdlKinematicsPlugin
```

Example project command on Windows:

```powershell
dotnet run --project example -- kinematics 172.16.102.156 .\libs\elite_kdl_kinematics.dll ELITE::KdlKinematicsPlugin
```
