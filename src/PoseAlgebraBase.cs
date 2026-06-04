using System.Runtime.InteropServices;

namespace EliteRobots.CSharp;

public enum PoseAlgebraError
{
    SUCCESS = 0,
    INVALID_INPUT = 1,
    SINGULAR_MATRIX = 2,
    INVALID_ROTATION_MATRIX = 3,
    NUMERICAL_ERROR = 4,
    UNSUPPORTED_OPERATION = 5,
    INTERNAL_ERROR = 6,
}

public struct PoseAlgebraResult
{
    public PoseAlgebraError error;
    public string message;
}

public struct PoseMatrix
{
    public double[] data;

    public PoseMatrix(double[] data)
    {
        ArgumentNullException.ThrowIfNull(data);
        if (data.Length != 16)
        {
            throw new ArgumentException("Array length must be 16", nameof(data));
        }
        this.data = data;
    }
}

public struct PoseDistance
{
    public double linear_distance;
    public double angular_distance;
}

public sealed class PoseAlgebraBase : IDisposable
{
    private readonly ElitePoseAlgebraSafeHandle _handle;

    public PoseAlgebraBase(string plugin_lib_path, string? plugin_class_name = null)
    {
        ArgumentNullException.ThrowIfNull(plugin_lib_path);
        var status = NativeMethods.elite_pose_algebra_create(plugin_lib_path, plugin_class_name, out var rawHandle);
        ThrowIfError(status, rawHandle);
        _handle = new ElitePoseAlgebraSafeHandle(rawHandle);
    }

    public bool inverse(PoseMatrix pose, ref PoseMatrix inverse_pose, out PoseAlgebraResult result)
    {
        var inPose = ToNative(pose);
        var outPose = CreateNativeMatrix();
        var status = NativeMethods.elite_pose_algebra_inverse_matrix(
            _handle.DangerousGetHandle(),
            ref inPose,
            ref outPose,
            out var nativeResult,
            out var success);
        return FinishMatrixCall(status, outPose, ref inverse_pose, nativeResult, success, out result);
    }

    public bool inverse(double[] pose, double[] inverse_pose, out PoseAlgebraResult result)
    {
        ValidateVector6(pose, nameof(pose));
        ValidateVector6(inverse_pose, nameof(inverse_pose));
        var outPose = new double[6];
        var status = NativeMethods.elite_pose_algebra_inverse_vector(
            _handle.DangerousGetHandle(),
            pose,
            outPose,
            out var nativeResult,
            out var success);
        return FinishVectorCall(status, outPose, inverse_pose, nativeResult, success, out result);
    }

    public bool multiply(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
    {
        var left = ToNative(left_pose);
        var right = ToNative(right_pose);
        var output = CreateNativeMatrix();
        var status = NativeMethods.elite_pose_algebra_multiply_matrix(
            _handle.DangerousGetHandle(),
            ref left,
            ref right,
            ref output,
            out var nativeResult,
            out var success);
        return FinishMatrixCall(status, output, ref out_pose, nativeResult, success, out result);
    }

    public bool multiply(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
    {
        ValidateVector6(left_pose, nameof(left_pose));
        ValidateVector6(right_pose, nameof(right_pose));
        ValidateVector6(out_pose, nameof(out_pose));
        var output = new double[6];
        var status = NativeMethods.elite_pose_algebra_multiply_vector(
            _handle.DangerousGetHandle(),
            left_pose,
            right_pose,
            output,
            out var nativeResult,
            out var success);
        return FinishVectorCall(status, output, out_pose, nativeResult, success, out result);
    }

    public bool add(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
    {
        var left = ToNative(left_pose);
        var right = ToNative(right_pose);
        var output = CreateNativeMatrix();
        var status = NativeMethods.elite_pose_algebra_add_matrix(
            _handle.DangerousGetHandle(),
            ref left,
            ref right,
            ref output,
            out var nativeResult,
            out var success);
        return FinishMatrixCall(status, output, ref out_pose, nativeResult, success, out result);
    }

    public bool add(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
    {
        ValidateVector6(left_pose, nameof(left_pose));
        ValidateVector6(right_pose, nameof(right_pose));
        ValidateVector6(out_pose, nameof(out_pose));
        var output = new double[6];
        var status = NativeMethods.elite_pose_algebra_add_vector(
            _handle.DangerousGetHandle(),
            left_pose,
            right_pose,
            output,
            out var nativeResult,
            out var success);
        return FinishVectorCall(status, output, out_pose, nativeResult, success, out result);
    }

    public bool subtract(PoseMatrix left_pose, PoseMatrix right_pose, ref PoseMatrix out_pose, out PoseAlgebraResult result)
    {
        var left = ToNative(left_pose);
        var right = ToNative(right_pose);
        var output = CreateNativeMatrix();
        var status = NativeMethods.elite_pose_algebra_subtract_matrix(
            _handle.DangerousGetHandle(),
            ref left,
            ref right,
            ref output,
            out var nativeResult,
            out var success);
        return FinishMatrixCall(status, output, ref out_pose, nativeResult, success, out result);
    }

    public bool subtract(double[] left_pose, double[] right_pose, double[] out_pose, out PoseAlgebraResult result)
    {
        ValidateVector6(left_pose, nameof(left_pose));
        ValidateVector6(right_pose, nameof(right_pose));
        ValidateVector6(out_pose, nameof(out_pose));
        var output = new double[6];
        var status = NativeMethods.elite_pose_algebra_subtract_vector(
            _handle.DangerousGetHandle(),
            left_pose,
            right_pose,
            output,
            out var nativeResult,
            out var success);
        return FinishVectorCall(status, output, out_pose, nativeResult, success, out result);
    }

    public bool vectorToMatrix(double[] pose_vector, ref PoseMatrix pose_matrix, out PoseAlgebraResult result)
    {
        ValidateVector6(pose_vector, nameof(pose_vector));
        var output = CreateNativeMatrix();
        var status = NativeMethods.elite_pose_algebra_vector_to_matrix(
            _handle.DangerousGetHandle(),
            pose_vector,
            ref output,
            out var nativeResult,
            out var success);
        return FinishMatrixCall(status, output, ref pose_matrix, nativeResult, success, out result);
    }

    public bool matrixToVector(PoseMatrix pose_matrix, double[] pose_vector, out PoseAlgebraResult result)
    {
        ValidateVector6(pose_vector, nameof(pose_vector));
        var input = ToNative(pose_matrix);
        var output = new double[6];
        var status = NativeMethods.elite_pose_algebra_matrix_to_vector(
            _handle.DangerousGetHandle(),
            ref input,
            output,
            out var nativeResult,
            out var success);
        return FinishVectorCall(status, output, pose_vector, nativeResult, success, out result);
    }

    public bool distance(PoseMatrix pose_a, PoseMatrix pose_b, out PoseDistance out_distance, out PoseAlgebraResult result)
    {
        var a = ToNative(pose_a);
        var b = ToNative(pose_b);
        var status = NativeMethods.elite_pose_algebra_distance_matrix(
            _handle.DangerousGetHandle(),
            ref a,
            ref b,
            out var nativeDistance,
            out var nativeResult,
            out var success);
        return FinishDistanceCall(status, nativeDistance, out out_distance, nativeResult, success, out result);
    }

    public bool distance(double[] pose_a, double[] pose_b, out PoseDistance out_distance, out PoseAlgebraResult result)
    {
        ValidateVector6(pose_a, nameof(pose_a));
        ValidateVector6(pose_b, nameof(pose_b));
        var status = NativeMethods.elite_pose_algebra_distance_vector(
            _handle.DangerousGetHandle(),
            pose_a,
            pose_b,
            out var nativeDistance,
            out var nativeResult,
            out var success);
        return FinishDistanceCall(status, nativeDistance, out out_distance, nativeResult, success, out result);
    }

    public bool worldToLocal(PoseMatrix world_ref_pose, PoseMatrix world_pose, ref PoseMatrix local_pose, out PoseAlgebraResult result)
    {
        var worldRef = ToNative(world_ref_pose);
        var world = ToNative(world_pose);
        var output = CreateNativeMatrix();
        var status = NativeMethods.elite_pose_algebra_world_to_local_matrix(
            _handle.DangerousGetHandle(),
            ref worldRef,
            ref world,
            ref output,
            out var nativeResult,
            out var success);
        return FinishMatrixCall(status, output, ref local_pose, nativeResult, success, out result);
    }

    public bool worldToLocal(double[] world_ref_pose, double[] world_pose, double[] local_pose, out PoseAlgebraResult result)
    {
        ValidateVector6(world_ref_pose, nameof(world_ref_pose));
        ValidateVector6(world_pose, nameof(world_pose));
        ValidateVector6(local_pose, nameof(local_pose));
        var output = new double[6];
        var status = NativeMethods.elite_pose_algebra_world_to_local_vector(
            _handle.DangerousGetHandle(),
            world_ref_pose,
            world_pose,
            output,
            out var nativeResult,
            out var success);
        return FinishVectorCall(status, output, local_pose, nativeResult, success, out result);
    }

    public bool localToWorld(PoseMatrix world_ref_pose, PoseMatrix local_pose, ref PoseMatrix world_pose, out PoseAlgebraResult result)
    {
        var worldRef = ToNative(world_ref_pose);
        var local = ToNative(local_pose);
        var output = CreateNativeMatrix();
        var status = NativeMethods.elite_pose_algebra_local_to_world_matrix(
            _handle.DangerousGetHandle(),
            ref worldRef,
            ref local,
            ref output,
            out var nativeResult,
            out var success);
        return FinishMatrixCall(status, output, ref world_pose, nativeResult, success, out result);
    }

    public bool localToWorld(double[] world_ref_pose, double[] local_pose, double[] world_pose, out PoseAlgebraResult result)
    {
        ValidateVector6(world_ref_pose, nameof(world_ref_pose));
        ValidateVector6(local_pose, nameof(local_pose));
        ValidateVector6(world_pose, nameof(world_pose));
        var output = new double[6];
        var status = NativeMethods.elite_pose_algebra_local_to_world_vector(
            _handle.DangerousGetHandle(),
            world_ref_pose,
            local_pose,
            output,
            out var nativeResult,
            out var success);
        return FinishVectorCall(status, output, world_pose, nativeResult, success, out result);
    }

    public void Dispose()
    {
        _handle.Dispose();
    }

    private bool FinishMatrixCall(
        NativeMethods.EliteStatus status,
        NativeMethods.ElitePoseMatrixNative output,
        ref PoseMatrix destination,
        NativeMethods.ElitePoseAlgebraResultNative nativeResult,
        int success,
        out PoseAlgebraResult result)
    {
        ThrowIfError(status, _handle.DangerousGetHandle());
        result = FromNative(nativeResult);
        if (success == 0)
        {
            return false;
        }
        destination = FromNative(output);
        return true;
    }

    private bool FinishVectorCall(
        NativeMethods.EliteStatus status,
        double[] output,
        double[] destination,
        NativeMethods.ElitePoseAlgebraResultNative nativeResult,
        int success,
        out PoseAlgebraResult result)
    {
        ThrowIfError(status, _handle.DangerousGetHandle());
        result = FromNative(nativeResult);
        if (success == 0)
        {
            return false;
        }
        Array.Copy(output, destination, 6);
        return true;
    }

    private bool FinishDistanceCall(
        NativeMethods.EliteStatus status,
        NativeMethods.ElitePoseDistanceNative distance,
        out PoseDistance out_distance,
        NativeMethods.ElitePoseAlgebraResultNative nativeResult,
        int success,
        out PoseAlgebraResult result)
    {
        ThrowIfError(status, _handle.DangerousGetHandle());
        result = FromNative(nativeResult);
        out_distance = new PoseDistance
        {
            linear_distance = distance.linear_distance,
            angular_distance = distance.angular_distance,
        };
        return success != 0;
    }

    private static NativeMethods.ElitePoseMatrixNative ToNative(PoseMatrix matrix)
    {
        ValidateMatrix(matrix.data, nameof(matrix));
        var data = new double[16];
        Array.Copy(matrix.data, data, 16);
        return new NativeMethods.ElitePoseMatrixNative { data = data };
    }

    private static PoseMatrix FromNative(NativeMethods.ElitePoseMatrixNative matrix)
    {
        var data = new double[16];
        if (matrix.data is not null)
        {
            Array.Copy(matrix.data, data, Math.Min(matrix.data.Length, data.Length));
        }
        return new PoseMatrix(data);
    }

    private static NativeMethods.ElitePoseMatrixNative CreateNativeMatrix() =>
        new() { data = new double[16] };

    private static PoseAlgebraResult FromNative(NativeMethods.ElitePoseAlgebraResultNative result) =>
        new()
        {
            error = (PoseAlgebraError)result.error,
            message = Marshal.PtrToStringUTF8(result.message) ?? string.Empty,
        };

    private static void ValidateVector6(double[] values, string name)
    {
        ArgumentNullException.ThrowIfNull(values, name);
        if (values.Length != 6)
        {
            throw new ArgumentException("Array length must be 6", name);
        }
    }

    private static void ValidateMatrix(double[] values, string name)
    {
        ArgumentNullException.ThrowIfNull(values, name);
        if (values.Length != 16)
        {
            throw new ArgumentException("Array length must be 16", name);
        }
    }

    private static void ThrowIfError(NativeMethods.EliteStatus status, nint handle)
    {
        if (status == NativeMethods.EliteStatus.Ok)
        {
            return;
        }

        var msgPtr = handle != nint.Zero
            ? NativeMethods.elite_pose_algebra_last_error_message(handle)
            : NativeMethods.elite_pose_algebra_global_last_error_message();
        var message = Marshal.PtrToStringUTF8(msgPtr);
        throw new EliteSdkException(string.IsNullOrWhiteSpace(message) ? "native call failed" : message, (int)status);
    }
}
