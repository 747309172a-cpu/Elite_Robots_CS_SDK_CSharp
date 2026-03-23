# SerialCommunication (RS485)

## Introduction

The robot tool end and control cabinet provide RS485 communication interfaces, and the SDK provides corresponding features.

The SDK reads and writes the robot serial port by having the robot forward RS485 interface data to a specified TCP port, and the SDK directly reads and writes that TCP port.

## Import

```csharp
using EliteRobots.CSharp;
```

## Serial Configuration

### SerialConfig

```csharp
public sealed class SerialConfig
```

- ***Function***
  - Configure serial port parameters.
- ***Properties***
  - `baud_rate`: baud rate (`SerialConfigBaudRate`).
  - `parity`: parity (`SerialConfigParity`).
  - `stop_bits`: stop bits (`SerialConfigStopBits`).

## Communication Object

### connect

```csharp
public bool connect(int timeout_ms)
```

- ***Function***
  - Connect to the forwarded serial port.
- ***Parameters***
  - `timeout_ms`: connection timeout in milliseconds.
- ***Return Value***
  - Returns `true` if the connection succeeds, otherwise `false`.

### disconnect

```csharp
public void disconnect()
```

- ***Function***
  - Disconnect the serial connection.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

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

### getSocatPid

```csharp
public int getSocatPid()
```

- ***Function***
  - Get the PID of the underlying forwarding process.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the PID.

### write

```csharp
public int write(byte[] data)
```

- ***Function***
  - Write byte data to the serial port.
- ***Parameters***
  - `data`: byte array to write.
- ***Return Value***
  - Returns the actual number of bytes written.

### read

```csharp
public byte[] read(int size, int timeout_ms)
```

- ***Function***
  - Read byte data from the serial port.
- ***Parameters***
  - `size`: expected number of bytes to read.
  - `timeout_ms`: read timeout in milliseconds.
- ***Return Value***
  - Returns the bytes read. On timeout or when no data is available, it may return an empty array.

### Dispose

```csharp
public void Dispose()
```

- ***Function***
  - Release communication object resources.
- ***Parameters***
  - None.
- ***Return Value***
  - None.

## Entry Points in EliteDriver

### startToolRs485

```csharp
public EliteSerialCommunication? startToolRs485(SerialConfig config, string ssh_password, int tcp_port = 54321)
```

- ***Function***
  - Start RS485 forwarding on the tool side.
- ***Parameters***
  - `config`: serial configuration.
  - `ssh_password`: robot SSH password.
  - `tcp_port`: local forwarding port.
- ***Return Value***
  - Returns the communication object on success, and may return `null` on failure.
