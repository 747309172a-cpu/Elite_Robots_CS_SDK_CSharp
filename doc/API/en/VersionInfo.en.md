# VersionInfo

## Introduction

`VersionInfo` stores and compares version numbers.

## Import

```csharp
using EliteRobots.CSharp;
```

## Definition

```csharp
public struct VersionInfo
{
    public uint major;
    public uint minor;
    public uint bugfix;
    public uint build;
}
```

## Constructors and Conversion

```csharp
public VersionInfo(uint major, uint minor, uint bugfix, uint build)
public VersionInfo(string version)
public override string ToString()
public static VersionInfo fromString(string version)
```

## Operators

`==`, `!=`, `<`, `<=`, `>`, `>=`

## Related Type

```csharp
public sealed class RtsiVersionInfo
```
