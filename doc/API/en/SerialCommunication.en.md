# SerialCommunication (RS485)

## Introduction

Robot tool and control box expose RS485 channels. SDK reads/writes serial data via TCP forwarding.

## Import

```csharp
using EliteRobots.CSharp;
```

## SerialConfig

```csharp
public sealed class SerialConfig
```

Properties:

```csharp
public SerialConfigBaudRate baud_rate { get; set; }
public SerialConfigParity parity { get; set; }
public SerialConfigStopBits stop_bits { get; set; }
```

## EliteSerialCommunication

```csharp
public bool connect(int timeout_ms)
public void disconnect()
public bool isConnected()
public int getSocatPid()
public int write(byte[] data)
public byte[] read(int size, int timeout_ms)
public void Dispose()
```

## Entry Interfaces from EliteDriver

```csharp
public EliteSerialCommunication? startToolRs485(SerialConfig config, string ssh_password, int tcp_port = 54321)
public bool endToolRs485(EliteSerialCommunication comm, string ssh_password)
public EliteSerialCommunication? startBoardRs485(SerialConfig config, string ssh_password, int tcp_port = 54322)
public bool endBoardRs485(EliteSerialCommunication comm, string ssh_password)
```
