
using GLib;

namespace GalacticLib.Objects;

public abstract record FutureValue<TValue> {

    /// <summary> Value is pending to be gotten </summary>
    public sealed record Pending : FutureValue<TValue>;

    /// <summary> Running to get the value </summary>
    public sealed record Running : FutureValue<TValue>;

    /// <summary> <see cref="Value"/> was aquired </summary>
    /// <param name="Value"> The value that was aquired (could also be <see langword="default"/>) </param>
    public sealed record Finished(TValue? Value) : FutureValue<TValue>;

    /// <summary> Something went wrong while getting the value </summary>
    public abstract record Failed : FutureValue<TValue> {
        /// <summary> Failed to get the value due to <paramref name="Duration"/> passing and invalidating the process </summary>
        /// <param name="Duration"> Time elapsed while trying to get the value </param>
        public sealed record TimedOut(int Duration) : Failed;

        /// <summary> Failed to get the value due to an <paramref name="Exception"/> </summary>
        /// <param name="Exception"> <see cref="Exception"/> encountered while getting the value </param>
        public sealed record Error(Exception Exception) : Failed;
    }

}
