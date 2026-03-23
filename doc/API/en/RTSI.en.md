# RTSI Interface

## Introduction

RTSI is the real-time communication interface of Elite robots. It can be used to obtain robot status, set IO, and so on. The SDK provides two RTSI interfaces: `RtsiClientInterface` and `RtsiIOInterface`. `RtsiClientInterface` requires manual handling of connection, version negotiation, and related steps. `RtsiIOInterface` wraps most of the interfaces. In actual testing, `RtsiIOInterface` is slightly less real-time, while the real-time behavior of `RtsiClientInterface` depends on the user's code.

- `RtsiClientInterface`: basic RTSI client.
- `RtsiRecipe`: recipe read/write object.
- `RtsiIoInterface`: high-level wrapper for IO and status read/write.

## Import

```csharp
using EliteRobots.CSharp;
```

## RtsiClientInterface

### RtsiClientInterface

```csharp
public RtsiClientInterface()
```

- ***Function***
  - Create an RTSI client.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

### connect

```csharp
public void connect(string ip, int port = 30004)
```

- ***Function***
  - Connect to the RTSI service.
- ***Parameters***
  - `ip`: robot IP address.
  - `port`: port number, default `30004`.
- ***Return Value***
  - None.

### disconnect

```csharp
public void disconnect()
```

- ***Function***
  - Disconnect.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

### negotiateProtocolVersion

```csharp
public bool negotiateProtocolVersion(ushort version = 1)
```

- ***Function***
  - Negotiate the RTSI protocol version.
- ***Parameters***
  - `version`: protocol version, default `1`.
- ***Return Value***
  - Returns `true` if negotiation succeeds, otherwise `false`.

### getControllerVersion

```csharp
public RtsiVersionInfo getControllerVersion()
```

- ***Function***
  - Get the controller version.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `RtsiVersionInfo`.

### setupOutputRecipe

```csharp
public RtsiRecipe setupOutputRecipe(IEnumerable<string> recipe_list, double frequency = 250)
```

- ***Function***
  - Configure an output subscription recipe.
- ***Parameters***
  - `recipe_list`: recipe field strings. Refer to the official Elite documentation "RTSI User Manual" for specific content.
  - `frequency`: update frequency.
- ***Return Value***
  - Returns a `RtsiRecipe` object.

### setupInputRecipe

```csharp
public RtsiRecipe setupInputRecipe(IEnumerable<string> recipe)
```

- ***Function***
  - Configure an input recipe.
- ***Parameters***
  - `recipe`: list of input fields.
- ***Return Value***
  - Returns a `RtsiRecipe` object.

### start

```csharp
public bool start()
```

- ***Function***
  - Start RTSI data exchange.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### pause

```csharp
public bool pause()
```

- ***Function***
  - Pause RTSI data exchange.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### send

```csharp
public void send(RtsiRecipe recipe)
```

- ***Function***
  - Send input recipe data.
- ***Parameters***
  - `recipe`: recipe to send.
- ***Return Value***
  - None.

### receiveData (single recipe)

```csharp
public bool receiveData(RtsiRecipe recipe, bool read_newest = false)
```

- ***Function***
  - Receive data for a single recipe.
- ***Parameters***
  - `recipe`: subscribed output recipe list. Only one recipe is received, and the recipe data in the list is updated. It is recommended to keep `read_newest` as `false`.
  - `read_newest`: whether to read the newest packet.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### receiveData (multiple recipes)

```csharp
public int receiveData(IReadOnlyList<RtsiRecipe> recipes, bool read_newest = false)
```

- ***Function***
  - Receive data from subscribed output recipes.
- ***Parameters***
  - `recipes`: subscribed output recipes. When multiple recipes are used, if the received recipe is not one of the input recipes, the data of that recipe will not be updated.
  - `read_newest`: whether to read the newest packet.
- ***Return Value***
  - Returns the index of the matched recipe.

### isConnected

```csharp
public bool isConnected()
```

- ***Function***
  - Query the connection status.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if connected, otherwise `false`.

### isStarted

```csharp
public bool isStarted()
```

- ***Function***
  - Whether synchronization of robot data has started.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if started, otherwise `false`.

### isReadAvailable

```csharp
public bool isReadAvailable()
```

- ***Function***
  - Query whether readable data is available.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `true` if data is available, otherwise `false`.

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release client resources.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## RtsiRecipe

### getID

```csharp
public int getID()
```

- ***Function***
  - Get the recipe ID.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the recipe ID.

### getRecipe

```csharp
public string[] getRecipe()
```

- ***Function***
  - Get the list of subscribed field names in the recipe.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the list of recipe field names.

### getValue (overloads)

```csharp
public bool getValue(string name, out double out_value)
public bool getValue(string name, out int out_value)
public bool getValue(string name, out uint out_value)
public bool getValue(string name, out bool out_value)
public bool getValue(string name, double[] out_value6)
```

- ***Function***
  - Get the value of a subscribed item in the recipe.
- ***Parameters***
  - `name`: subscribed item name.
  - `out_value/out_value6`: output value of the subscribed item. Note that the type of this value must match the type defined in the RTSI documentation.
- ***Return Value***
  - Returns `true` if reading succeeds, otherwise `false`.

### setValue (overloads)

```csharp
public bool setValue(string name, double value)
public bool setValue(string name, int value)
public bool setValue(string name, uint value)
public bool setValue(string name, bool value)
public bool setValue(string name, double[] value6)
```

- ***Function***
  - Set the value of a subscribed item in the recipe.
- ***Parameters***
  - `name`: subscribed item name.
  - `value/value6`: value to set for the subscribed item. Note that the type of this value must match the type defined in the RTSI documentation.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release recipe resources.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## RtsiIoInterface

### RtsiIoInterface

```csharp
public RtsiIoInterface(IEnumerable<string> output_recipe, IEnumerable<string> input_recipe, double frequency = 250)
```

- ***Function***
  - Create the high-level RTSI IO interface.
- ***Parameters***
  - `output_recipe`: list of output fields.
  - `input_recipe`: list of input fields.
  - `frequency`: sampling frequency.
- ***Return Value***
  - None.

### connect / disconnect / isConnected / isStarted

```csharp
public bool connect(string ip)
public void disconnect()
public bool isConnected()
public bool isStarted()
```

- ***Function***
  - Connect, disconnect, and query RTSI IO status.
- ***Parameters***
  - `ip` of `connect`: robot IP address.
- ***Return Value***
  - `connect/isConnected/isStarted`: boolean value.
  - `disconnect`: no return value.

### getControllerVersion

```csharp
public RtsiVersionInfo getControllerVersion()
```

- ***Function***
  - Get the controller version.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns `RtsiVersionInfo`.

### Common write interfaces

```csharp
public bool setSpeedScaling(double scaling)
public bool setStandardDigital(int index, bool level)
public bool setConfigureDigital(int index, bool level)
public bool setAnalogOutputVoltage(int index, double value)
public bool setAnalogOutputCurrent(int index, double value)
public bool setExternalForceTorque(double[] value6)
public bool setToolDigitalOutput(int index, bool level)
```

- ***Function***
  - Set speed, digital IO, analog IO, external force, and tool-side digital outputs.
- ***Parameters***
  - `index`: channel index.
  - `level`: digital signal level.
  - `value`: analog value.
  - `value6`: 6D external force vector.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### Common read interfaces (excerpt)

```csharp
public double[] getActualJointPositions()
public double[] getActualTCPPose()
public double[] getActualTCPVelocity()
public double[] getActualTCPForce()
public RobotMode getRobotMode()
public SafetyMode getSafetyStatus()
public TaskStatus getRuntimeState()
```

- ***Function***
  - Read joint states, TCP states, and robot-state enums.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the corresponding arrays or enum values.

### Recipe dynamic read/write

```csharp
public bool getRecipeValue(string name, out double value)
public bool getRecipeValue(string name, out int value)
public bool getRecipeValue(string name, out uint value)
public bool getRecipeValue(string name, out bool value)
public bool getRecipeValue(string name, double[] value3or6)

public bool setInputRecipeValue(string name, double value)
public bool setInputRecipeValue(string name, int value)
public bool setInputRecipeValue(string name, uint value)
public bool setInputRecipeValue(string name, bool value)
public bool setInputRecipeValue(string name, double[] value6)
```

- ***Function***
  - Read output recipe values by field name, and set input recipe values.
- ***Parameters***
  - `name`: field name.
  - `value/value3or6/value6`: value container or input value.
- ***Return Value***
  - Returns `true` on success, otherwise `false`.

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release RTSI IO resources.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## Data Types

### RtsiVersionInfo

```csharp
public sealed class RtsiVersionInfo
{
    public uint Major { get; init; }
    public uint Minor { get; init; }
    public uint Bugfix { get; init; }
    public uint Build { get; init; }
}
```

- ***Function***
  - Stores the RTSI version number.

### Related enums

```csharp
public enum JointMode
public enum ToolMode
public enum ToolDigitalMode
public enum ToolDigitalOutputMode
```

- ***Function***
  - Describe joint modes, tool modes, and tool IO modes.
