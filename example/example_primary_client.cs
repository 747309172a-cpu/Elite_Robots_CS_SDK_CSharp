using EliteRobots.CSharp;

internal static class PrimaryClientFlowExample
{
    internal static void Run(string[] args)
    {
        if (!TryParseArgs(args, out var ip, out var port, out var runControlDemo, out var speedScaling))
        {
            PrintUsage();
            return;
        }

        using var pr = new PrimaryClientInterface();

        if (pr.connect(ip, port))
        {
            Console.WriteLine("Success connected robot");
        }
        else
        {
            Console.WriteLine("Connect robot fail");
            return;
        }

        Console.WriteLine("Wait get robot cartesian info");
        Console.WriteLine("Note: current C# wrapper only exposes kinematics package directly.");
        var gotKinematics = pr.getPackage(out var kine, 1000);
        Console.WriteLine($"getPackage(KinematicsInfo): {gotKinematics}");
        if (gotKinematics)
        {
            Console.WriteLine($"DH A: [{string.Join(", ", kine.DhA)}]");
            Console.WriteLine($"DH D: [{string.Join(", ", kine.DhD)}]");
            Console.WriteLine($"DH Alpha: [{string.Join(", ", kine.DhAlpha)}]");
        }

        if (runControlDemo)
        {
            Console.WriteLine($"setSpeedScaling({speedScaling}): {pr.setSpeedScaling(speedScaling)}");
            Console.WriteLine($"powerOn(): {pr.powerOn()}");
            Console.WriteLine($"brakeRelease(): {pr.brakeRelease()}");
            Console.WriteLine($"pauseProgram(): {pr.pauseProgram()}");
            Console.WriteLine($"stopProgram(): {pr.stopProgram()}");
            Console.WriteLine($"unlockProtectiveStop(): {pr.unlockProtectiveStop()}");
            Console.WriteLine($"safetySystemRestart(): {pr.safetySystemRestart()}");
        }

        pr.registerRobotExceptionCallback(ExceptionCb);

        Console.WriteLine("Send \"hello\" script to robot");
        pr.sendScript("def hello():\n\ttextmsg(\"hello world\")\nend\n");
        Console.WriteLine("Sended \"hello\" script to robot");
        Thread.Sleep(3000);

        Console.WriteLine("Send a script with anomalies to the robot");
        pr.sendScript("def func():\n\tabcd(123)\nend\n");
        Console.WriteLine("Sended a script with anomalies to the robot");
        Thread.Sleep(3000);

        pr.disconnect();
    }

    private static void ExceptionCb(PrimaryRobotException ex)
    {
        Console.WriteLine($"Exception: {ex.Type}");
        Console.WriteLine($"\ttimestamp: {ex.Timestamp}");
        Console.WriteLine($"\terror code: {ex.ErrorCode}");
        Console.WriteLine($"\terror sub-code: {ex.SubErrorCode}");
        Console.WriteLine($"\tmessage: {ex.Message}");
    }

    private static bool TryParseArgs(string[] args, out string ip, out int port, out bool runControlDemo, out int speedScaling)
    {
        ip = string.Empty;
        port = 30001;
        runControlDemo = false;
        speedScaling = 70;

        if (args.Length < 2)
        {
            return false;
        }

        if (args[1] == "--ip")
        {
            for (var i = 1; i < args.Length; i++)
            {
                if (args[i] == "--ip" && i + 1 < args.Length)
                {
                    ip = args[++i];
                }
                else if (args[i] == "--port" && i + 1 < args.Length && int.TryParse(args[++i], out var parsedPort))
                {
                    port = parsedPort;
                }
                else if (args[i] == "--control-demo")
                {
                    runControlDemo = true;
                }
                else if (args[i] == "--speed-scaling" && i + 1 < args.Length && int.TryParse(args[++i], out var parsedScaling))
                {
                    speedScaling = parsedScaling;
                }
            }
            return !string.IsNullOrWhiteSpace(ip);
        }

        ip = args[1];
        for (var i = 2; i < args.Length; i++)
        {
            if (args[i] == "--port" && i + 1 < args.Length && int.TryParse(args[++i], out var parsedPort))
            {
                port = parsedPort;
            }
            else if (int.TryParse(args[i], out parsedPort))
            {
                port = parsedPort;
            }
            else if (args[i] == "--control-demo")
            {
                runControlDemo = true;
            }
            else if (args[i] == "--speed-scaling" && i + 1 < args.Length && int.TryParse(args[++i], out var parsedScaling))
            {
                speedScaling = parsedScaling;
            }
        }
        return true;
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- primary_client <robot-ip> [--port <primary-port>] [--control-demo] [--speed-scaling <percent>]");
        Console.WriteLine("  dotnet run -- primary_client --ip <robot-ip> [--port <primary-port>] [--control-demo] [--speed-scaling <percent>]");
    }
}
