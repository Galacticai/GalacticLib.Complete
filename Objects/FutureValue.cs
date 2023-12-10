
namespace GalacticLib.Objects;

public record FutureValue<TValue> {
    public sealed record Finished(TValue Value) : FutureValue<TValue>;
    public sealed record Pending : FutureValue<TValue>;
    public sealed record TimedOut(int Duration) : FutureValue<TValue>;
    public sealed record NotFound : FutureValue<TValue>;
}
