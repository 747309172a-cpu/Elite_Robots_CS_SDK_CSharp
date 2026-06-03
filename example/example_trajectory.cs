using EliteRobots.CSharp;

internal static class TrajectoryExample
{
    private const float JointSpeed = 0.5f;
    private const float JointAcceleration = 0.8f;
    private const float PointTime = 3.0f;
    private const float CartesianSpeed = 0.15f;
    private const float CartesianAcceleration = 0.3f;

    private sealed class TrajectoryControl : IDisposable
    {
        private readonly EliteDriverConfig _config;
        private readonly DashboardClientInterface _dashboard;
        private readonly EliteDriver _driver;
        private readonly object _feedbackLock = new();
        private int _currentPoint = -1;
        private int _totalPoints;
        private TrajectoryMotionResult? _lastResult;
        private TrajectoryMotionFeedback? _lastFeedback;

        public TrajectoryControl(EliteDriverConfig config)
        {
            _config = config;
            _driver = new EliteDriver(config);
            _dashboard = new DashboardClientInterface();

            Console.WriteLine("[INFO] Connecting to the dashboard");
            if (!_dashboard.connect(_config.RobotIp))
            {
                throw new InvalidOperationException("Failed to connect to the dashboard.");
            }
            Console.WriteLine("[INFO] Successfully connected to the dashboard");
        }

        public bool startControl()
        {
            Console.WriteLine("[INFO] Start powering on...");
            if (!_dashboard.powerOn())
            {
                Console.WriteLine("[FATAL] Power-on failed");
                return false;
            }
            Console.WriteLine("[INFO] Power-on succeeded");

            Console.WriteLine("[INFO] Start releasing brake...");
            if (!_dashboard.brakeRelease())
            {
                Console.WriteLine("[FATAL] Brake release failed");
                return false;
            }
            Console.WriteLine("[INFO] Brake released");

            if (_config.HeadlessMode)
            {
                if (!_driver.isRobotConnected() && !_driver.sendExternalControlScript())
                {
                    Console.WriteLine("[FATAL] Fail to send external control script");
                    return false;
                }
            }
            else
            {
                if (!_dashboard.playProgram())
                {
                    Console.WriteLine("[FATAL] Fail to play program");
                    return false;
                }
            }

            Console.WriteLine("[INFO] Wait external control script run...");
            while (!_driver.isRobotConnected())
            {
                Thread.Sleep(10);
            }
            Console.WriteLine("[INFO] External control script is running");
            return true;
        }

        public bool moveTrajectoryByTime(IReadOnlyList<double[]> targetPoints, float pointTime, float blendRadius, bool isCartesian)
        {
            return moveTrajectory(targetPoints, pointTime, blendRadius, isCartesian, 0.0f, 0.0f);
        }

        public bool moveTrajectoryBySpeed(IReadOnlyList<double[]> targetPoints, float blendRadius, bool isCartesian, float speed, float acceleration)
        {
            return moveTrajectory(targetPoints, 0.0f, blendRadius, isCartesian, speed, acceleration);
        }

        public bool moveToJointTargetBySpeed(double[] point, float speed, float acceleration)
        {
            return moveTrajectoryBySpeed(new[] { point }, 0.0f, false, speed, acceleration);
        }

        private bool moveTrajectory(IReadOnlyList<double[]> targetPoints, float pointTime, float blendRadius, bool isCartesian, float speed, float acceleration)
        {
            _currentPoint = -1;
            _totalPoints = 0;
            _lastResult = null;
            _lastFeedback = null;

            var moveDone = new TaskCompletionSource<TrajectoryMotionResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            _driver.setTrajectoryResultCallback(result => moveDone.TrySetResult(result));
            _driver.setTrajectoryFeedbackCallback(feedback =>
            {
                lock (_feedbackLock)
                {
                    _lastFeedback = feedback;
                }

                if (feedback.MessageType == TrajectoryFeedbackMessageType.ACTIVE_POINT)
                {
                    _currentPoint = feedback.PointIndex;
                    _totalPoints = feedback.TotalPoints;
                    Console.WriteLine($"[INFO] Trajectory point {feedback.PointIndex + 1}/{feedback.TotalPoints} is active");
                    Console.WriteLine($"[INFO] Active point target = {FormatVector(feedback.Point)}");
                }
                else if (feedback.MessageType == TrajectoryFeedbackMessageType.POINT_DONE)
                {
                    _currentPoint = feedback.PointIndex;
                    _totalPoints = feedback.TotalPoints;
                    Console.WriteLine($"[INFO] Trajectory point {feedback.PointIndex + 1}/{feedback.TotalPoints} is done");
                }
                else if (feedback.MessageType == TrajectoryFeedbackMessageType.RESULT)
                {
                    _lastResult = feedback.Result;
                    Console.WriteLine($"[INFO] Trajectory result frame received: {(int)feedback.Result}");
                }
            });

            Console.WriteLine("[INFO] Trajectory motion start");
            if (!_driver.writeTrajectoryControlAction(TrajectoryControlAction.START, targetPoints.Count, 200))
            {
                Console.WriteLine("[ERROR] Failed to start trajectory motion");
                return false;
            }

            foreach (var point in targetPoints)
            {
                var pointSent = pointTime > 0.0f
                    ? _driver.writeTrajectoryPoint(point, pointTime, blendRadius, isCartesian)
                    : _driver.writeTrajectoryPoint(point, blendRadius, isCartesian, speed, acceleration);

                if (!pointSent)
                {
                    Console.WriteLine("[ERROR] Failed to write trajectory point");
                    return false;
                }

                if (!_driver.writeTrajectoryControlAction(TrajectoryControlAction.NOOP, 0, 200))
                {
                    Console.WriteLine("[ERROR] Failed to send NOOP command");
                    return false;
                }
            }

            var lastLoggedPoint = -2;
            while (!moveDone.Task.Wait(50))
            {
                if (_currentPoint != lastLoggedPoint && _currentPoint >= 0 && _totalPoints > 0)
                {
                    var feedback = getLastFeedback();
                    Console.WriteLine($"[INFO] Cached progress says current point is {_currentPoint + 1}/{_totalPoints}");
                    if (feedback is not null)
                    {
                        Console.WriteLine($"[INFO] Cached point value = {FormatVector(feedback.Point)}");
                    }
                    lastLoggedPoint = _currentPoint;
                }

                if (!_driver.writeTrajectoryControlAction(TrajectoryControlAction.NOOP, 0, 200))
                {
                    Console.WriteLine("[ERROR] Failed to send NOOP command");
                    return false;
                }
            }

            var result = moveDone.Task.Result;
            Console.WriteLine($"[INFO] Trajectory motion completed with result: {(int)result}");

            if (!_driver.writeIdle(0))
            {
                Console.WriteLine("[ERROR] Failed to write idle command");
                return false;
            }

            return result == TrajectoryMotionResult.SUCCESS;
        }

        private TrajectoryMotionFeedback? getLastFeedback()
        {
            lock (_feedbackLock)
            {
                return _lastFeedback;
            }
        }

        public void Dispose()
        {
            try { _dashboard.disconnect(); } catch { }
            try { _driver.stopControl(); } catch { }
            _driver.Dispose();
            _dashboard.Dispose();
        }
    }

    internal static void Run(string[] args)
    {
        if (args.Length < 2)
        {
            PrintUsage();
            return;
        }

        var ip = args[1];
        var localIp = string.Empty;
        var headless = false;
        var scriptFile = "external_control.script";

        for (var i = 2; i < args.Length; i++)
        {
            if (args[i] == "--local-ip" && i + 1 < args.Length)
            {
                localIp = args[++i];
            }
            else if ((args[i] == "--headless" || args[i] == "--use-headless-mode") && i + 1 < args.Length)
            {
                if (!bool.TryParse(args[++i], out headless))
                {
                    Console.WriteLine("Invalid headless value, use true or false.");
                    return;
                }
            }
            else if ((args[i] == "--headless" || args[i] == "--use-headless-mode") && i + 1 >= args.Length)
            {
                headless = true;
            }
            else if (args[i] == "--script-file" && i + 1 < args.Length)
            {
                scriptFile = args[++i];
            }
        }

        if (headless)
        {
            Console.WriteLine("[WARN] Use headless mode. Please ensure the robot is not in local mode.");
        }
        else
        {
            Console.WriteLine("[WARN] It needs to be correctly configured, and the External Control plugin should be inserted into the task tree.");
        }

        var outputRecipePath = ResolveRecipePath("output_recipe.txt");
        var inputRecipePath = ResolveRecipePath("input_recipe.txt");
        if (outputRecipePath is null || inputRecipePath is null)
        {
            Console.WriteLine("[FATAL] Recipe file not found. Expected under example/resource.");
            return;
        }

        var outputRecipe = LoadRecipe(outputRecipePath);
        var inputRecipe = LoadRecipe(inputRecipePath);
        if (outputRecipe.Count == 0 || inputRecipe.Count == 0)
        {
            Console.WriteLine("[FATAL] Recipe file is empty.");
            return;
        }

        var config = new EliteDriverConfig
        {
            RobotIp = ip,
            LocalIp = localIp,
            ScriptFilePath = scriptFile,
            HeadlessMode = headless,
        };

        try
        {
            using var trajectoryControl = new TrajectoryControl(config);
            using var rtsi = new RtsiIoInterface(outputRecipe, inputRecipe, 250.0);

            Console.WriteLine("[INFO] Connecting to the RTSI");
            if (!rtsi.connect(config.RobotIp))
            {
                Console.WriteLine("[FATAL] Fail to connect or config to the RTSI.");
                return;
            }
            Console.WriteLine("[INFO] Successfully connected to the RTSI");

            Console.WriteLine("[INFO] Starting trajectory control...");
            if (!trajectoryControl.startControl())
            {
                Console.WriteLine("[FATAL] Failed to start trajectory control.");
                return;
            }
            Console.WriteLine("[INFO] Trajectory control started");

            var actualJoints = rtsi.getActualJointPositions();
            actualJoints[3] = -1.57;

            Console.WriteLine($"[INFO] Moving joints to target: {FormatVector(actualJoints)}");
            if (!trajectoryControl.moveToJointTargetBySpeed(actualJoints, JointSpeed, JointAcceleration))
            {
                Console.WriteLine("[FATAL] Failed to move joints to target.");
                return;
            }
            Console.WriteLine("[INFO] Joints moved to target");

            var actualPose = rtsi.getActualTCPPose();
            var trajectory = new List<double[]>();

            actualPose[2] -= 0.2;
            trajectory.Add((double[])actualPose.Clone());

            actualPose[1] -= 0.2;
            trajectory.Add((double[])actualPose.Clone());

            actualPose[1] += 0.2;
            actualPose[2] += 0.2;
            trajectory.Add((double[])actualPose.Clone());

            Console.WriteLine($"[INFO] Executing trajectory with time control, {trajectory.Count} points");
            if (!trajectoryControl.moveTrajectoryByTime(trajectory, PointTime, 0.0f, true))
            {
                Console.WriteLine("[FATAL] Failed to move trajectory with time control.");
                return;
            }
            Console.WriteLine("[INFO] Time-controlled trajectory completed");

            Console.WriteLine($"[INFO] Executing trajectory with speed control, {trajectory.Count} points");
            if (!trajectoryControl.moveTrajectoryBySpeed(trajectory, 0.0f, true, CartesianSpeed, CartesianAcceleration))
            {
                Console.WriteLine("[FATAL] Failed to move trajectory with speed control.");
                return;
            }
            Console.WriteLine("[INFO] Speed-controlled trajectory completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {ex.Message}");
        }
    }

    private static string? ResolveRecipePath(string fileName)
    {
        var candidates = new[]
        {
            Path.Combine("example", "resource", fileName),
            Path.Combine("resource", fileName),
            Path.Combine(AppContext.BaseDirectory, "resource", fileName),
        };

        foreach (var path in candidates)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }
        return null;
    }

    private static List<string> LoadRecipe(string path)
    {
        return File.ReadAllLines(path)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s) && !s.StartsWith("#"))
            .ToList();
    }

    private static string FormatVector(double[] value)
    {
        return $"[{string.Join(", ", value.Select(v => v.ToString("F6")))}]";
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- trajectory <robot-ip> [--local-ip <ip>] [--headless <true|false>] [--use-headless-mode <true|false>] [--script-file <path>]");
    }
}
