using System.Text.Json.Nodes;

namespace GalacticLib.Objects;

/// <summary> Indicates that an object can be converted to <see cref="JsonValue"/> </summary>
public interface IJsonable {
    /// <summary> Convert this object into a <see cref="JsonValue"/> </summary>
    /// <returns> <see cref="JsonValue"/> representation of this object </returns>
    public JsonValue ToJson();

    /// <summary> Thrown when a cyclic reference is detected </summary>
    /// <param name="message"> Exception message </param>
    public class CyclicReferenceException(string? message = null, Exception? innerException = null)
        : Exception(message, innerException) { }
}

/// <summary> Indicates that an object can be converted to <see cref="JsonValue"/> and back to <typeparamref name="T"/> </summary>
public interface IJsonable<T> : IJsonable where T : IJsonable<T> {
    /// <summary> Convert <paramref name="json"/> into an instance of <typeparamref name="T"/> </summary>
    /// <param name="json"> Source json (<see cref="JsonValue"/>) </param>
    /// <returns> Instance of <typeparamref name="T"/> constructed from <paramref name="json"/> </returns>
    public abstract static T FromJson(JsonValue json);
}
