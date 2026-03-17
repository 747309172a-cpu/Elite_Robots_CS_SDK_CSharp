# RTSI Interfaces

## Introduction

RTSI is the realtime interface for robot states and IO operations.

- `RtsiClientInterface`: low-level client.
- `RtsiRecipe`: recipe object for field read/write.
- `RtsiIoInterface`: high-level IO/state wrapper.

## Import

```csharp
using EliteRobots.CSharp;
```

## RtsiClientInterface

```csharp
public void connect(string ip, int port = 30004)
public void disconnect()
public bool negotiateProtocolVersion(ushort version = 1)
public RtsiVersionInfo getControllerVersion()
public RtsiRecipe setupOutputRecipe(IEnumerable<string> recipe_list, double frequency = 250)
public RtsiRecipe setupInputRecipe(IEnumerable<string> recipe)
public bool start()
public bool pause()
public void send(RtsiRecipe recipe)
public bool receiveData(RtsiRecipe recipe, bool read_newest = false)
public int receiveData(IReadOnlyList<RtsiRecipe> recipes, bool read_newest = false)
public bool isConnected()
public bool isStarted()
public bool isReadAvailable()
```

## RtsiRecipe

```csharp
public int getID()
public string[] getRecipe()
public bool getValue(string name, out double out_value)
public bool getValue(string name, out int out_value)
public bool getValue(string name, out uint out_value)
public bool getValue(string name, out bool out_value)
public bool getValue(string name, double[] out_value6)
public bool setValue(string name, double value)
public bool setValue(string name, int value)
public bool setValue(string name, uint value)
public bool setValue(string name, bool value)
public bool setValue(string name, double[] value6)
```

## RtsiIoInterface (Common)

```csharp
public bool connect(string ip)
public void disconnect()
public bool isConnected()
public bool isStarted()
public RtsiVersionInfo getControllerVersion()

public bool setSpeedScaling(double scaling)
public bool setStandardDigital(int index, bool level)
public bool setConfigureDigital(int index, bool level)
public bool setAnalogOutputVoltage(int index, double value)
public bool setAnalogOutputCurrent(int index, double value)
public bool setExternalForceTorque(double[] value6)
public bool setToolDigitalOutput(int index, bool level)

public double[] getActualJointPositions()
public double[] getActualTCPPose()
public double[] getActualTCPVelocity()
public double[] getActualTCPForce()
public RobotMode getRobotMode()
public SafetyMode getSafetyStatus()
public TaskStatus getRuntimeState()
```
