using EliteRobots.CSharp;

internal static class KinematicsExample
{
    internal static void Run(string[] args)
    {
        if (args.Length < 3)
        {
            PrintUsage();
            return;
        }

        var robotIp = args[1];
        var pluginLibPath = args[2];
        var pluginClassName = args.Length >= 4 ? args[3] : "ELITE::KdlKinematicsPlugin";

        try
        {
            using var primary = new PrimaryClientInterface();
            if (!primary.connect(robotIp, 30001))
            {
                Console.WriteLine("[FATAL] Connect robot 30001 port fail.");
                return;
            }

            if (!primary.getPackage(out var kinInfo, 200))
            {
                Console.WriteLine("[FATAL] Get robot kinematics info fail.");
                return;
            }
            primary.disconnect();
            Console.WriteLine("[INFO] Got robot kinematics info.");

            using var io = new RtsiIoInterface(
                new[] { "actual_joint_positions", "actual_TCP_pose" },
                Array.Empty<string>(),
                250.0);
            if (!io.connect(robotIp))
            {
                Console.WriteLine("[FATAL] Connect robot RTSI port fail.");
                return;
            }

            var currentJoint = io.getActualJointPositions();
            Console.WriteLine("[INFO] Got robot actual joint positions.");

            var currentTcp = io.getActualTCPPose();
            Console.WriteLine("[INFO] Got robot actual TCP positions.");

            using var kinSolver = new KinematicsBase(pluginLibPath, pluginClassName);
            kinSolver.setMDH(kinInfo.DhAlpha, kinInfo.DhA, kinInfo.DhD);

            var fkPose = new double[6];
            if (!kinSolver.getPositionFK(currentJoint, fkPose))
            {
                Console.WriteLine("[FATAL] Get FK fail.");
                return;
            }

            var ikJoints = new double[6];
            if (!kinSolver.getPositionIK(currentTcp, currentJoint, ikJoints, out var ikResult))
            {
                Console.WriteLine($"[FATAL] Get IK fail. error={ikResult.kinematic_error}");
                return;
            }

            PrintVector6("Current TCP Pose", currentTcp);
            PrintVector6("FK Pose", fkPose);
            PrintVector6("IK Result Joints", ikJoints);
            PrintVector6("Current Joints", currentJoint);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
        }
    }

    private static void PrintVector6(string name, double[] value)
    {
        Console.WriteLine($"{name}: [{string.Join(", ", value.Select(v => v.ToString("F6")))}]");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- kinematics <robot-ip> <kinematics-plugin-lib> [plugin-class]");
    }
}
