# DashboardClientInterface Class

## Introduction

`DashboardClientInterface` is used to communicate with the robot Dashboard service and perform operations such as power-on, program execution, status query, and configuration loading.

## Import

```csharp
using EliteRobots.CSharp;
```

## Constructor

### DashboardClientInterface

```csharp
public DashboardClientInterface()
```

- ***Function***
  - Create a Dashboard client instance.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## Communication Interface

### connect

```csharp
public bool connect(string ip, int port = 29999)
```

- ***Function***
  - Connect to the Dashboard server.
- ***Parameters***
  - `ip`: robot IP address.
  - `port`: port number, default `29999`.
- ***Return Value***
  - Returns `true` if the connection succeeds, otherwise `false`.

### disconnect

```csharp
public void disconnect()
```

- ***Function***
  - Disconnect the Dashboard connection.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

### echo

```csharp
public bool echo()
```

- ***Function***
  - Check the connection status with the dashboard shell server.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if reachable, otherwise `false`.

## Robot Control

### powerOn

```csharp
public bool powerOn()
```

- ***Function***
  - Power on the robot.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### powerOff

```csharp
public bool powerOff()
```

- ***Function***
  - Power off the robot.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### brakeRelease

```csharp
public bool brakeRelease()
```

- ***Function***
  - Release the brakes.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### closeSafetyDialog

```csharp
public bool closeSafetyDialog()
```

- ***Function***
  - Close the safety popup dialog.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### unlockProtectiveStop

```csharp
public bool unlockProtectiveStop()
```

- ***Function***
  - Release the robot from protective stop.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### safetySystemRestart

```csharp
public bool safetySystemRestart()
```

- ***Function***
  - Restart the safety system.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

## Program Control

### playProgram

```csharp
public bool playProgram()
```

- ***Function***
  - Start the program.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### pauseProgram

```csharp
public bool pauseProgram()
```

- ***Function***
  - Pause the program.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### stopProgram

```csharp
public bool stopProgram()
```

- ***Function***
  - Stop the program.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### taskIsRunning

```csharp
public bool taskIsRunning()
```

- ***Function***
  - Query whether the current task is running.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if running, otherwise `false`.

### getTaskStatus

```csharp
public TaskStatus getTaskStatus()
```

- ***Function***
  - Get the task status.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns a `TaskStatus` enum value.

## Configuration and Task

### loadConfiguration

```csharp
public bool loadConfiguration(string path)
```

- ***Function***
  - Load a robot configuration file.
- ***Parameters***
  - `path`: configuration file path.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### loadTask

```csharp
public bool loadTask(string path)
```

- ***Function***
  - Load a task file.
- ***Parameters***
  - `path`: task file path.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### isConfigurationModify

```csharp
public bool isConfigurationModify()
```

- ***Function***
  - Query whether the configuration has been modified.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if modified, otherwise `false`.

### isTaskSaved

```csharp
public bool isTaskSaved()
```

- ***Function***
  - Query whether the task has been saved.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if saved, otherwise `false`.

### getTaskPath

```csharp
public string getTaskPath()
```

- ***Function***
  - Get the current task path.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the task path string.

## Status Query

### robotMode

```csharp
public RobotMode robotMode()
```

- ***Function***
  - Get the robot mode.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns a `RobotMode` enum value.

### safetyMode

```csharp
public SafetyMode safetyMode()
```

- ***Function***
  - Get the safety mode.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns a `SafetyMode` enum value.

### runningStatus

```csharp
public TaskStatus runningStatus()
```

- ***Function***
  - Get the task running status.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns a `TaskStatus` enum value.

### speedScaling

```csharp
public int speedScaling()
```

- ***Function***
  - Get the current speed scaling percentage.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the integer scaling value.

### setSpeedScaling

```csharp
public bool setSpeedScaling(int scaling)
```

- ***Function***
  - Set the speed scaling percentage.
- ***Parameters***
  - `scaling`: target scaling value.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

## Other Interfaces

### log

```csharp
public bool log(string message)
```

- ***Function***
  - Write a message to the controller log.
- ***Parameters***
  - `message`: log content.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### popup

```csharp
public bool popup(string arg, string message = "")
```

- ***Function***
  - Show a popup message box.
- ***Parameters***
  - `arg`: popup argument.
  - `message`: popup text.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### quit

```csharp
public void quit()
```

- ***Function***
  - Quit the dashboard and disconnect.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

### reboot

```csharp
public void reboot()
```

- ***Function***
  - Reboot the robot system.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

### shutdown

```csharp
public void shutdown()
```

- ***Function***
  - Shut down the system.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

### help

```csharp
public string help(string cmd)
```

- ***Function***
  - Get command help information.
- ***Parameters***
  - `cmd`: command to query help for.
- ***Return Value***
  - Returns the help text.

### usage

```csharp
public string usage(string cmd)
```

- ***Function***
  - Get command usage information.
- ***Parameters***
  - `cmd`: command name.
- ***Return Value***
  - Returns the usage text.

### version

```csharp
public string version()
```

- ***Function***
  - Get version information.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the version string.

### robotType

```csharp
public string robotType()
```

- ***Function***
  - Get the robot model.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the model string.

### robotSerialNumber

```csharp
public string robotSerialNumber()
```

- ***Function***
  - Get the robot serial number.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the serial number string.

### robotID

```csharp
public string robotID()
```

- ***Function***
  - Get the robot ID.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the ID string.

### configurationPath

```csharp
public string configurationPath()
```

- ***Function***
  - Get the current configuration path.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the path string.

### sendAndReceive

```csharp
public string sendAndReceive(string cmd)
```

- ***Function***
  - Send a Dashboard command and receive the response.
- ***Parameters***
  - `cmd`: Dashboard command.
- ***Return Value***
  - Returns the response text.

## Resource Disposal

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release the client handle.
- ***Parameters***
  - None.
- ***Return Value***
  - None.
