namespace EliteRobots.CSharp;

public sealed class EliteDriverConfig
{
    public string RobotIp { get; set; } = string.Empty;
    public string ScriptFilePath { get; set; } = string.Empty;
    public string LocalIp { get; set; } = string.Empty;

    public bool HeadlessMode { get; set; }
    public int ScriptSenderPort { get; set; } = 50002;
    public int ReversePort { get; set; } = 50001;
    public int TrajectoryPort { get; set; } = 50003;
    public int ScriptCommandPort { get; set; } = 50004;

    public float ServojTime { get; set; } = 0.008f;
    public float ServojLookaheadTime { get; set; } = 0.1f;
    public int ServojGain { get; set; } = 300;
    public float StopjAcc { get; set; } = 8.0f;
    public float ServojExtrapolateMaxTime { get; set; } = 0.08f;
    public float ServojDecelerateTime { get; set; } = 0.01f;
    public float ServojHoldVelocityThreshold { get; set; } = 0.05f;
    public float ServojHoldStableTime { get; set; } = 0.04f;
}
