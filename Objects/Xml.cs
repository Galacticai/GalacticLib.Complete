// —————————————————————————————————————————————
//?
//!? 📜 Range.cs
//!? 🖋️ Galacticai 📅 2022 - 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies.
//?
// —————————————————————————————————————————————

using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace GalacticLib.Objects;
public static class XML {
    /// <summary> Convert <paramref name="obj"/> to xml <see cref="string"/> </summary>
    public static string ToXML<T>(this T obj) {
        var serializer = new DataContractSerializer(typeof(T));
        using MemoryStream stream = new();
        using XmlTextWriter writer = new(stream, null);
        serializer.WriteObject(writer, obj);
        writer.Flush();
        stream.Position = 0;
        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
    /// <summary> Convert xml <see cref="string"/> to an instance of <typeparamref name="T"/> </summary>
    //public static T? ToObject<T>(string xml) {
    //    var serializer = new DataContractSerializer(typeof(T));
    //    using MemoryStream stream = new(Encoding.UTF8.GetBytes(xml));
    //    using XmlDictionaryReader reader
    //        = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
    //    return (T?)serializer.ReadObject(reader);
    //}

    public static T ToObject<T>(string xml) where T : new()
        => ToObject<T>(xml, new T());
    public static T ToObject<T>(string xml, T instance) {
        var serializer = new DataContractSerializer(typeof(T));
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(xml));
        using XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
        try {
            reader.MoveToContent();
            while (reader.Read()) {
                if (reader.NodeType == XmlNodeType.Element) {
                    string propertyName = reader.LocalName;
                    PropertyInfo? property = typeof(T).GetProperty(propertyName);
                    if (property == null || !property.CanWrite) {
                        Console.Error.WriteLine($"Skipping read-only field '{propertyName}'...");
                        continue;
                    }
                    object? value;
                    try {
                        string xmlValue = reader.ReadElementContentAsString();
                        value = Convert.ChangeType(xmlValue, property.PropertyType);
                    } catch {
                        Console.Error.WriteLine($"Parsing failed for field '{propertyName}'. Using default value.");
                        value = default;
                    }
                    property.SetValue(instance, value);
                }
            }
        } catch (Exception ex) {
            Console.WriteLine($"Error converting XML to object: {ex.Message}");
        }
        return instance;
    }
}
