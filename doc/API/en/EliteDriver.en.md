# EliteDriver Class

## Introduction

`EliteDriver` is the main class for robot data exchange. It is responsible for establishing all necessary socket connections and handling data exchange with the robot. `EliteDriver` sends control scripts to the robot. After the robot runs the control script, it establishes communication with `EliteDriver`, receives motion data, and sends motion results when needed.

## Import

```csharp
using EliteRobots.CSharp;
```

## Constructor

### EliteDriver

```csharp
public EliteDriver(EliteDriverConfig config)
```

- ***Function***
  - Create an `EliteDriver` object and initialize the required connections for communication with the robot.
    This constructor throws in the following cases:
    1. TCP server creation fails, usually because the port is already in use.
    2. Connection to the robot primary port fails.
- ***Parameters***
  - `config`: driver configuration. See [EliteDriverConfig](./EliteDriverConfig.en.md).
- ***Return Value***
  - None.
- ***Exceptions***
  - `ArgumentNullException`: `config` is null.
  - `ArgumentException`: `RobotIp` or `ScriptFilePath` is empty.
  - `EliteSdkException`: native-layer creation failed.

## Connection and Control

### isRobotConnected

```csharp
public bool isRobotConnected()
```

- ***Function***
  - Query the current connection status of the robot.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if connected, otherwise `false`.

### sendExternalControlScript

```csharp
public bool sendExternalControlScript()
```

- ***Function***
  - Send the external control script to the robot.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### stopControl

```csharp
public bool stopControl(int wait_ms = 10000)
```

- ***Function***
  - Stop the current control flow.
- ***Parameters***
  - `wait_ms`: wait timeout in milliseconds.
- ***Return Value***
  - Returns `true` if stopping succeeds, otherwise `false`.

## Motion Control

### writeServoj

```csharp
public bool writeServoj(double[] pos, int timeout_ms, bool cartesian = false)
```

- ***Function***
  - Send a servo target point to the robot.
- ***Parameters***
  - `pos`: target point, length must be 6.
  - `timeout_ms`: timeout in milliseconds for reading the next command.
  - `cartesian`: `true` for a Cartesian point, `false` for a joint point.
- ***Return Value***
  - Returns `true` if the command is sent successfully, otherwise `false`.

### writeSpeedj

```csharp
public bool writeSpeedj(double[] vel, int timeout_ms)
```

- ***Function***
  - Send a joint-space speed control command.
- ***Parameters***
  - `vel`: joint velocity, length must be 6.
  - `timeout_ms`: timeout in milliseconds.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### writeSpeedl

```csharp
public bool writeSpeedl(double[] vel, int timeout_ms)
```

- ***Function***
  - Send a Cartesian-space speed control command.
- ***Parameters***
  - `vel`: Cartesian velocity, length must be 6.
  - `timeout_ms`: timeout in milliseconds.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### writeIdle

```csharp
public bool writeIdle(int timeout_ms)
```

- ***Function***
  - Send an idle control command. If the robot is moving, it will stop the motion.
- ***Parameters***
  - `timeout_ms`: timeout in milliseconds.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### writeFreedrive

```csharp
public bool writeFreedrive(FreedriveAction action, int timeout_ms)
```

- ***Function***
  - Control freedrive mode (enter or exit).
- ***Parameters***
  - `action`: freedrive action, including start (`START`), stop (`END`), and no-op (`NOOP`).
  - `timeout_ms`: timeout in milliseconds for the robot to read the next command. If less than or equal to 0, it waits indefinitely.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.
- ***Notes***
    After writing the `START` action, the next command must be written within the timeout. `NOOP` can be written.

## Trajectory Control

### setTrajectoryResultCallback

```csharp
public void setTrajectoryResultCallback(Action<TrajectoryMotionResult> cb)
```

- ***Function***
  - Register a callback function that is triggered when a trajectory finishes. One way to control the robot is to send all waypoints to the robot at once. When execution is completed, the callback registered here will be triggered.

- ***Parameters***
  - `cb`: trajectory result callback function.
- ***Return Value***
  - None.

### writeTrajectoryPoint

```csharp
public bool writeTrajectoryPoint(double[] positions, float time, float blend_radius, bool cartesian)
```

- ***Function***
  - Write a trajectory waypoint to the dedicated socket.
- ***Parameters***
  - `positions`: trajectory waypoint, length must be 6.
  - `time`: execution time for this point.
  - `blend_radius`: blend radius.
  - `cartesian`: whether the point is a Cartesian point.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### writeTrajectoryControlAction

```csharp
public bool writeTrajectoryControlAction(TrajectoryControlAction action, int point_number, int timeout_ms)
```

- ***Function***
  - Send a trajectory control command.
- ***Parameters***
  - `action`: trajectory control action.
  - `point_number`: number of trajectory points.
  - `timeout_ms`: timeout in milliseconds.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

## Force and Tool Configuration

### zeroFTSensor

```csharp
public bool zeroFTSensor()
```

- ***Function***
  - Zero the force/torque values applied at the tool TCP as measured by the force/torque sensor. These values are obtained by the `get_tcp_force(True)` script command and have already been processed with payload compensation and related handling. After this command is executed, the current force/torque measurement is saved as the reference value, and all subsequent force/torque measurements are offset by subtracting this reference value. Note that this reference value is updated when this command is executed, and it is reset to 0 after the controller restarts.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### setPayload

```csharp
public bool setPayload(double mass, double[] cog)
```

- ***Function***
  - Set the payload mass and center of gravity.
- ***Parameters***
  - `mass`: payload mass.
  - `cog`: center-of-gravity coordinates of the payload relative to the flange frame.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### setToolVoltage

```csharp
public bool setToolVoltage(ToolVoltage vol)
```

- ***Function***
  - Set the tool-end voltage.
- ***Parameters***
  - `vol`: tool voltage enum value.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### startForceMode

```csharp
public bool startForceMode(double[] reference_frame, int[] selection_vector, double[] wrench, ForceMode mode, double[] limits)
```

- ***Function***
  - Start force mode.
- ***Parameters***
  - `reference_frame`: reference coordinate frame, length must be 6.
  - `selection_vector`: selection vector, length must be 6.
  - `wrench`: target force/torque, length must be 6.
  - `mode`: force mode.
  - `limits`: limit parameters, length must be 6.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### endForceMode

```csharp
public bool endForceMode()
```

- ***Function***
  - Exit force mode.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

## Others

### sendScript

```csharp
public bool sendScript(string script)
```

- ***Function***
  - Send script text to the robot.
- ***Parameters***
  - `script`: script content string.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### getPrimaryPackage

```csharp
public bool getPrimaryPackage(KinematicsInfo pkg, int timeout_ms)
```

- ***Function***
  - Read Primary kinematic parameters and write them into `pkg`.
- ***Parameters***
  - `pkg`: output object. The internal `DhA`, `DhD`, and `DhAlpha` arrays must each have length 6.
  - `timeout_ms`: timeout in milliseconds.
- ***Return Value***
  - Returns `true` if reading succeeds, otherwise `false`.

### primaryReconnect

```csharp
public bool primaryReconnect()
```

- ***Function***
  - Re-establish the Primary channel connection.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if reconnection succeeds, otherwise `false`.

## Exception Callback

### registerRobotExceptionCallback

```csharp
public void registerRobotExceptionCallback(Action<EliteDriverRobotException> cb)
```

- ***Function***
  - Register a robot exception callback.
- ***Parameters***
  - `cb`: exception callback function.
- ***Return Value***
  - None.

### registerWrappedRobotExceptionCallback

```csharp
public void registerWrappedRobotExceptionCallback(Action<RobotException> cb)
```

- ***Function***
  - Register a wrapped exception callback. The callback argument is mapped to `RobotDisconnectedException`, `RobotError`, or `RobotRuntimeException`.
- ***Parameters***
  - `cb`: wrapped exception callback function.
- ***Return Value***
  - None.

### startToolRs485

```csharp
public EliteSerialCommunication? startToolRs485(SerialConfig config, string ssh_password, int tcp_port = 54321)
```

- ***Function***
  - Enable tool RS485 communication. This interface starts a `socat` process on the robot controller to forward tool RS485 serial data to the specified TCP/IP port.
- ***Parameters***
  - `config`: serial configuration. See [SerialCommunication](./SerialCommunication.en.md) for details.
  - `ssh_password`: robot SSH password.
  - `tcp_port`: TCP port, default `54321`.
- ***Return Value***
  - Returns an object that can operate the serial port. It is essentially a TCP client. See [SerialCommunication](./SerialCommunication.en.md) for details.

### endToolRs485

```csharp
public bool endToolRs485(EliteSerialCommunication comm, string ssh_password)
```

- ***Function***
  - Stop tool RS485 communication.
- ***Parameters***
  - `comm`: if not `null`, `SerialCommunication.disconnect()` will be called. See [SerialCommunication](./SerialCommunication.en.md) for details.
  - `ssh_password`: robot SSH password.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### startBoardRs485

```csharp
public EliteSerialCommunication? startBoardRs485(SerialConfig config, string ssh_password, int tcp_port = 54322)
```

- ***Function***
  - Start control-cabinet-side RS485 passthrough.
- ***Parameters***
  - `config`: serial parameters (baud rate, parity, stop bits).
  - `ssh_password`: robot SSH password.
  - `tcp_port`: local forwarding port, default `54322`.
- ***Return Value***
  - Returns an `EliteSerialCommunication` object on success, and may return `null` on failure.

### endBoardRs485

```csharp
public bool endBoardRs485(EliteSerialCommunication comm, string ssh_password)
```

- ***Function***
  - Stop control-cabinet-side RS485 passthrough.
- ***Parameters***
  - `comm`: created serial communication object.
  - `ssh_password`: robot SSH password.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

## Resource Disposal

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release managed and unmanaged resources held by the driver.
- ***Parameters***
  - None.
- ***Return Value***
  - None.
