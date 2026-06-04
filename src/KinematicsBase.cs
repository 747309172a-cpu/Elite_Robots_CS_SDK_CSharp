using System.Runtime.InteropServices;

namespace EliteRobots.CSharp;

public enum KinematicError
{
    OK = 1,
    SOLVER_NOT_ACTIVE = 2,
    NO_SOLUTION = 3,
}

public struct KinematicsResult
{
    public KinematicError kinematic_error;
}

public sealed class KinematicsBase : IDisposable
{
    private readonly EliteKinematicsSafeHandle _handle;

    public KinematicsBase(string plugin_lib_path, string? plugin_class_name = null)
    {
        ArgumentNullException.ThrowIfNull(plugin_lib_path);
        var status = NativeMethods.elite_kinematics_create(plugin_lib_path, plugin_class_name, out var rawHandle);
        ThrowIfError(status, rawHandle);
        _handle = new EliteKinematicsSafeHandle(rawHandle);
    }

    public void setMDH(double[] alpha, double[] a, double[] d)
    {
        ValidateVector6(alpha, nameof(alpha));
        ValidateVector6(a, nameof(a));
        ValidateVector6(d, nameof(d));
        var status = NativeMethods.elite_kinematics_set_mdh(_handle.DangerousGetHandle(), alpha, a, d);
        ThrowIfError(status, _handle.DangerousGetHandle());
    }

    public bool getPositionFK(double[] joint_angles, double[] poses)
    {
        ValidateVector6(joint_angles, nameof(joint_angles));
        ValidateVector6(poses, nameof(poses));
        var outPose = new double[6];
        var status = NativeMethods.elite_kinematics_get_position_fk(
            _handle.DangerousGetHandle(),
            joint_angles,
            outPose,
            out var success);
        ThrowIfError(status, _handle.DangerousGetHandle());
        if (success == 0)
        {
            return false;
        }
        Array.Copy(outPose, poses, 6);
        return true;
    }

    public bool getPositionIK(double[] pose, double[] near, double[] solution, out KinematicsResult result)
    {
        ValidateVector6(pose, nameof(pose));
        ValidateVector6(near, nameof(near));
        ValidateVector6(solution, nameof(solution));
        var outSolution = new double[6];
        var status = NativeMethods.elite_kinematics_get_position_ik(
            _handle.DangerousGetHandle(),
            pose,
            near,
            outSolution,
            out var nativeResult,
            out var success);
        ThrowIfError(status, _handle.DangerousGetHandle());
        result = FromNative(nativeResult);
        if (success == 0)
        {
            return false;
        }
        Array.Copy(outSolution, solution, 6);
        return true;
    }

    public bool getPositionIK(double[] pose, double[] near, List<double[]> solutions, out KinematicsResult result)
    {
        return getPositionIK(pose, near, solutions, 32, out result);
    }

    public bool getPositionIK(double[] pose, double[] near, List<double[]> solutions, int max_solutions, out KinematicsResult result)
    {
        ValidateVector6(pose, nameof(pose));
        ValidateVector6(near, nameof(near));
        ArgumentNullException.ThrowIfNull(solutions);
        if (max_solutions < 0)
        {
            throw new ArgumentException("max_solutions must be >= 0", nameof(max_solutions));
        }

        var outSolutions = new double[max_solutions * 6];
        var status = NativeMethods.elite_kinematics_get_position_ik_all(
            _handle.DangerousGetHandle(),
            pose,
            near,
            outSolutions,
            max_solutions,
            out var solutionCount,
            out var nativeResult,
            out var success);
        ThrowIfError(status, _handle.DangerousGetHandle());
        result = FromNative(nativeResult);
        if (success == 0)
        {
            return false;
        }

        solutions.Clear();
        for (var i = 0; i < solutionCount; i++)
        {
            var solution = new double[6];
            Array.Copy(outSolutions, i * 6, solution, 0, 6);
            solutions.Add(solution);
        }
        return true;
    }

    public void setDefaultTimeout(double timeout)
    {
        var status = NativeMethods.elite_kinematics_set_default_timeout(_handle.DangerousGetHandle(), timeout);
        ThrowIfError(status, _handle.DangerousGetHandle());
    }

    public double getDefaultTimeout()
    {
        var status = NativeMethods.elite_kinematics_get_default_timeout(_handle.DangerousGetHandle(), out var timeout);
        ThrowIfError(status, _handle.DangerousGetHandle());
        return timeout;
    }

    public void Dispose()
    {
        _handle.Dispose();
    }

    private static KinematicsResult FromNative(NativeMethods.EliteKinematicsResultNative result) =>
        new() { kinematic_error = (KinematicError)result.kinematic_error };

    private static void ValidateVector6(double[] values, string name)
    {
        ArgumentNullException.ThrowIfNull(values, name);
        if (values.Length != 6)
        {
            throw new ArgumentException("Array length must be 6", name);
        }
    }

    private static void ThrowIfError(NativeMethods.EliteStatus status, nint handle)
    {
        if (status == NativeMethods.EliteStatus.Ok)
        {
            return;
        }

        var msgPtr = handle != nint.Zero
            ? NativeMethods.elite_kinematics_last_error_message(handle)
            : NativeMethods.elite_kinematics_global_last_error_message();
        var message = Marshal.PtrToStringUTF8(msgPtr);
        throw new EliteSdkException(string.IsNullOrWhiteSpace(message) ? "native call failed" : message, (int)status);
    }
}
