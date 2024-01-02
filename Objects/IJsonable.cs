using System.Text;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects;

/// <summary> Indicates that an object can be converted to <see cref="JsonNode"/> </summary>
public interface IJsonable {
    /// <summary> Convert this object into a <see cref="JsonNode"/> </summary>
    /// <returns> <see cref="JsonNode"/> representation of this object </returns>
    public JsonNode ToJson();
}

/// <summary> Indicates that an object can be converted to <see cref="JsonNode"/> and back to <typeparamref name="T"/> </summary>
public interface IJsonable<T> : IJsonable where T : IJsonable<T> {
    /// <summary> Convert <paramref name="json"/> into an instance of <typeparamref name="T"/> </summary>
    /// <param name="json"> Source json (<see cref="JsonNode"/>) </param>
    /// <returns> Instance of <typeparamref name="T"/> constructed from <paramref name="json"/> </returns>
    public abstract static T? FromJson(JsonNode json);
}

/// <summary> Thrown when a cyclic reference is detected </summary> 
public class CyclicReferenceException(string? message = null, Exception? innerException = null)
    : Exception(message, innerException) { }

/// <summary> Thrown when a property was expected but not found in the <see cref="JsonNode"/> </summary> 
public class PropertyNotFoundException(
            string? propName = null,
            string? message = null,
            Exception? innerException = null
    ) : Exception(
            FormatMessage(propName, message),
            innerException
    ) {
    private static string FormatMessage(string? propName, string? message = null) {
        StringBuilder sb = new();
        if (propName is not null) {
            sb.Append($"Property not found: \"")
                .Append(propName)
                .Append("\".");
        }
        if (message is not null) {
            if (sb.Length > 0) sb.Append(Environment.NewLine);
            sb.Append(message);
        }
        return sb.ToString();
    }
    public static void ThrowIfNull<TProp>(
            TProp? obj,
            string? propName = null,
            string? message = null,
            Exception? innerException = null
    ) {
        if (obj is null)
            throw new PropertyNotFoundException(propName, message, innerException);
    }
}