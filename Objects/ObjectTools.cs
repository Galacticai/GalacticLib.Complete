// —————————————————————————————————————————————
//?
//!? 📜 Objects.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————

using System.Dynamic;
using System.Reflection;
using System.Runtime.Serialization;

namespace GalacticLib.Objects;

/// <summary> Various tools for <see cref="object" />s </summary>
public static class ObjectTools {
    public static IEnumerable<Type> FindSubClassesOf<T>()
        => typeof(T).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(T)));

    public static ExpandoObject Combine_ExpandoObjects(ExpandoObject obj1, ExpandoObject obj2) {
        IDictionary<string, object?> dict1 = obj1;
        IDictionary<string, object?> dict2 = obj2;
        IDictionary<string, object?> result = new ExpandoObject();
        foreach ((string key, object? value) in dict1.Concat(dict2))
            result[key] = value;
        return (ExpandoObject)result;
    }

    public static IDictionary<string, object?> AsDictionary<T>()
        => typeof(T).AsDictionary();

    public static IDictionary<string, object?> AsDictionary(this Type type)
        => FormatterServices
            .GetUninitializedObject(type)
            .AsDictionary();

    public static IDictionary<string, object?> AsDictionary(this object obj) {
        Type t = obj.GetType();
        Dictionary<string, object?> dictionary = [];
        foreach (var property in t.GetProperties())
            dictionary[property.Name] = property.GetValue(obj);
        foreach (var field in t.GetFields())
            dictionary[field.Name] = field.GetValue(obj);
        return dictionary;
    }

    public static T AsObject<T>(this IDictionary<string, object> dictionary)
        where T : class, new() {
        T obj = new();
        Type type = obj.GetType();
        foreach (KeyValuePair<string, object> item in dictionary)
            type.GetProperty(item.Key)?.SetValue(obj, item.Value);
        return obj;
    }

    /// <summary> Check if <paramref name="en" /> is matching one of <paramref name="enums" /> </summary>
    public static bool Is<TEnum>(this TEnum en, params TEnum[] enums)
            where TEnum : Enum
        => enums.Any(item => item.Equals(en));


    /// <summary> Parse <paramref name="fieldName" /> <see cref="string" /> into <typeparamref name="TEnum" /> </summary>
    /// <exception cref="ArgumentNullException"><typeparamref name="TEnum" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     <paramref name="fieldName" /> is either an empty string or only contains white
    ///     space.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     <paramref name="fieldName" /> is a name, but not one of the named constants defined
    ///     for the enumeration.
    /// </exception>
    /// <exception cref="OverflowException">
    ///     <paramref name="fieldName" /> is outside the range of the underlying type of
    ///     <typeparamref name="TEnum" />
    /// </exception>
    public static TEnum Parse<TEnum>(this string fieldName, bool ignoreCase = false)
            where TEnum : Enum
        => (TEnum)Enum.Parse(typeof(TEnum), fieldName, ignoreCase);

    public static FieldInfo GetFieldInfo<TEnum>(this TEnum value)
            where TEnum : Enum
        //!? Cannot be null. Getting field using field value name from same enum type
        => typeof(TEnum).GetField(value.ToString())!;

}