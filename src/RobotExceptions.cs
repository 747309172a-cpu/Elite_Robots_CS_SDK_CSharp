namespace EliteRobots.CSharp;

public enum RobotExceptionType
{
    Unknown = -1,
    RobotDisconnected = 0,
    RobotError = 1,
    ScriptRuntime = 2,
}

public enum RobotErrorType
{
    Unknown = -1,
    Safety = 0,
    Gui = 1,
    Controller = 2,
    Rtsi = 3,
    Joint = 4,
    Tool = 5,
    Tp = 6,
    JointFpga = 7,
    ToolFpga = 8,
}

public enum RobotErrorLevel
{
    Unknown = -1,
    Info = 0,
    Warning = 1,
    Error = 2,
    Fatal = 3,
}

public enum RobotErrorDataType
{
    Unknown = -1,
    None = 0,
    Unsigned = 1,
    Signed = 2,
    Float = 3,
    Hex = 4,
    String = 5,
    Joint = 6,
}

public abstract class RobotException
{
    protected RobotException(RobotExceptionType type, ulong timestamp)
    {
        Type = type;
        Timestamp = timestamp;
    }

    public RobotExceptionType Type { get; }
    public ulong Timestamp { get; }
}

public sealed class RobotDisconnectedException : RobotException
{
    public RobotDisconnectedException(ulong timestamp)
        : base(RobotExceptionType.RobotDisconnected, timestamp)
    {
    }
}

public sealed class RobotError : RobotException
{
    public RobotError(
        ulong timestamp,
        int errorCode,
        int subErrorCode,
        RobotErrorType errorSource,
        RobotErrorLevel errorLevel,
        RobotErrorDataType errorDataType,
        uint dataU32,
        int dataI32,
        float dataF32,
        string message)
        : base(RobotExceptionType.RobotError, timestamp)
    {
        ErrorCode = errorCode;
        SubErrorCode = subErrorCode;
        ErrorSource = errorSource;
        ErrorLevel = errorLevel;
        ErrorDataType = errorDataType;
        DataU32 = dataU32;
        DataI32 = dataI32;
        DataF32 = dataF32;
        Message = message;
    }

    public int ErrorCode { get; }
    public int SubErrorCode { get; }
    public RobotErrorType ErrorSource { get; }
    public RobotErrorLevel ErrorLevel { get; }
    public RobotErrorDataType ErrorDataType { get; }
    public uint DataU32 { get; }
    public int DataI32 { get; }
    public float DataF32 { get; }
    public string Message { get; }

    public object? Data => ErrorDataType switch
    {
        RobotErrorDataType.Unsigned => DataU32,
        RobotErrorDataType.Hex => DataU32,
        RobotErrorDataType.Signed => DataI32,
        RobotErrorDataType.Joint => DataI32,
        RobotErrorDataType.Float => DataF32,
        RobotErrorDataType.String => Message,
        _ => null,
    };
}

public sealed class RobotRuntimeException : RobotException
{
    public RobotRuntimeException(ulong timestamp, int line, int column, string message)
        : base(RobotExceptionType.ScriptRuntime, timestamp)
    {
        Line = line;
        Column = column;
        Message = message;
    }

    public int Line { get; }
    public int Column { get; }
    public string Message { get; }
}

public static class RobotExceptionMapper
{
    public static RobotException fromRaw(EliteDriverRobotException ex)
    {
        ArgumentNullException.ThrowIfNull(ex);
        return fromRaw(
            ex.Type,
            ex.Timestamp,
            ex.ErrorCode,
            ex.SubErrorCode,
            ex.ErrorSource,
            ex.ErrorLevel,
            ex.ErrorDataType,
            ex.DataU32,
            ex.DataI32,
            ex.DataF32,
            ex.Line,
            ex.Column,
            ex.Message);
    }

    public static RobotException fromRaw(PrimaryRobotException ex)
    {
        ArgumentNullException.ThrowIfNull(ex);
        return fromRaw(
            ex.Type,
            ex.Timestamp,
            ex.ErrorCode,
            ex.SubErrorCode,
            ex.ErrorSource,
            ex.ErrorLevel,
            ex.ErrorDataType,
            ex.DataU32,
            ex.DataI32,
            ex.DataF32,
            ex.Line,
            ex.Column,
            ex.Message);
    }

    private static RobotException fromRaw(
        int type,
        ulong timestamp,
        int errorCode,
        int subErrorCode,
        int errorSource,
        int errorLevel,
        int errorDataType,
        uint dataU32,
        int dataI32,
        float dataF32,
        int line,
        int column,
        string message)
    {
        var exceptionType = ToExceptionType(type);
        return exceptionType switch
        {
            RobotExceptionType.RobotDisconnected => new RobotDisconnectedException(timestamp),
            RobotExceptionType.ScriptRuntime => new RobotRuntimeException(timestamp, line, column, message ?? string.Empty),
            RobotExceptionType.RobotError => new RobotError(
                timestamp,
                errorCode,
                subErrorCode,
                ToErrorType(errorSource),
                ToErrorLevel(errorLevel),
                ToErrorDataType(errorDataType),
                dataU32,
                dataI32,
                dataF32,
                message ?? string.Empty),
            _ => new RobotError(
                timestamp,
                errorCode,
                subErrorCode,
                ToErrorType(errorSource),
                ToErrorLevel(errorLevel),
                ToErrorDataType(errorDataType),
                dataU32,
                dataI32,
                dataF32,
                message ?? string.Empty),
        };
    }

    private static RobotExceptionType ToExceptionType(int value) => value switch
    {
        0 => RobotExceptionType.RobotDisconnected,
        1 => RobotExceptionType.RobotError,
        2 => RobotExceptionType.ScriptRuntime,
        _ => RobotExceptionType.Unknown,
    };

    private static RobotErrorType ToErrorType(int value) => value switch
    {
        0 => RobotErrorType.Safety,
        1 => RobotErrorType.Gui,
        2 => RobotErrorType.Controller,
        3 => RobotErrorType.Rtsi,
        4 => RobotErrorType.Joint,
        5 => RobotErrorType.Tool,
        6 => RobotErrorType.Tp,
        7 => RobotErrorType.JointFpga,
        8 => RobotErrorType.ToolFpga,
        _ => RobotErrorType.Unknown,
    };

    private static RobotErrorLevel ToErrorLevel(int value) => value switch
    {
        0 => RobotErrorLevel.Info,
        1 => RobotErrorLevel.Warning,
        2 => RobotErrorLevel.Error,
        3 => RobotErrorLevel.Fatal,
        _ => RobotErrorLevel.Unknown,
    };

    private static RobotErrorDataType ToErrorDataType(int value) => value switch
    {
        0 => RobotErrorDataType.None,
        1 => RobotErrorDataType.Unsigned,
        2 => RobotErrorDataType.Signed,
        3 => RobotErrorDataType.Float,
        4 => RobotErrorDataType.Hex,
        5 => RobotErrorDataType.String,
        6 => RobotErrorDataType.Joint,
        _ => RobotErrorDataType.Unknown,
    };
}
