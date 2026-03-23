# VersionInfo

## Introduction

The `VersionInfo` class is used to store and manage version information, and provides version storage, comparison, and conversion features.

## Import

```csharp
using EliteRobots.CSharp;
```

## Definition

### VersionInfo

```csharp
public struct VersionInfo
{
    public uint major;
    public uint minor;
    public uint bugfix;
    public uint build;
}
```

- ***Function***
  - Stores a four-part version number.

## Constructors and Conversion

### VersionInfo(uint major, uint minor, uint bugfix, uint build)

```csharp
public VersionInfo(uint major, uint minor, uint bugfix, uint build)
```

- ***Function***
  - Create a version object from four numeric parts.
- ***Parameters***
  - `major/minor/bugfix/build`: each part of the version number.
- ***Return Value***
  - None.

### VersionInfo(string version)

```csharp
public VersionInfo(string version)
```

- ***Function***
  - Parse a version object from a string.
- ***Parameters***
  - `version`: version string.
- ***Return Value***
  - None.

### ToString

```csharp
public override string ToString()
```

- ***Function***
  - Convert to a string.
- ***Parameters***
  - None.
- ***Return Value***
  - Returns the version string.

### fromString

```csharp
public static VersionInfo fromString(string version)
```

- ***Function***
  - Create a `VersionInfo` object from a string.
- ***Parameters***
  - `version`: version string.
- ***Return Value***
  - Returns the version object.

## Comparison Operators

```csharp
==  !=  <  <=  >  >=
```

- ***Function***
  - Supports comparison between version objects.
- ***Return Value***
  - Returns a boolean value.

## Related Type

### RtsiVersionInfo

```csharp
public sealed class RtsiVersionInfo
```

- ***Function***
  - RTSI version information object (`Major`, `Minor`, `Bugfix`, `Build`).
