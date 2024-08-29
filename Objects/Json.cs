using Newtonsoft.Json;

namespace GalacticLib.Objects;
public static class Json {
    private static JsonSerializerSettings _Settings => new() {
        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        TypeNameHandling = TypeNameHandling.Auto
    };
    /// <summary> ⚠️ Warning:
    /// <br/> Anything that is not <see langword="public"/> must have <see cref="JsonObjectAttribute"/> or <see cref="JsonPropertyAttribute"/> for objects or properties respectively
    /// <br/> otherwise the output won't be as expected </summary>
    public static string ToJson(this object obj, bool indentation = true)
        => JsonConvert.SerializeObject(obj, indentation ? Formatting.Indented : Formatting.None, _Settings);
    public static T? ToObject<T>(string json)
        => JsonConvert.DeserializeObject<T>(json, _Settings);


    // public static JsonObject ToJson<T>(this T obj, HashSet<string>? onlyProperties = null) { 
    //     Type type = typeof(T);
    //     foreach (var prop in type.GetProperties()) {
    //         //? Filtering && name is not found
    //         bool skip = onlyProperties?.Contains(prop.Name) == false;
    //         if (skip) continue;

    //         bool nullable = Nullable.GetUnderlyingType(prop.PropertyType) is not null;
    //         var value = prop.GetValue(obj); 
    //     }
    // }
}
