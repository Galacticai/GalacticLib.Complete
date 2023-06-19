using GalacticLib.Objects;

namespace GalacticLib.Configs;

/// <summary> ⚠️ Warning:
/// <br/> Anything that is not <see langword="public"/> must have <see cref="Newtonsoft.Json.JsonObjectAttribute"/>
/// or <see cref="Newtonsoft.Json.JsonPropertyAttribute"/> for objects or properties respectively
/// <br/> > Otherwise <see cref="Save(TConfig, string)"/> will not save <typeparamref name="TConfig"/> as expected </summary>
public static class ConfigTools<TConfig> where TConfig : IConfig, new() {

    // public static string ConfigDirectory => Path.Combine(Paths.ThisApplicationData, "Config");
    public static string ConfigName => $"{typeof(TConfig).Name}.json";
    public static string ConfigPath(string configDirectory) => Path.Combine(configDirectory, ConfigName);

    public static TConfig Get(string configDirectory) => Get(configDirectory, out _);
    public static TConfig Get(string configDirectory, out bool failed) {
        failed = false;
        TConfig? config = default;
        try {
            config = Json.ToObject<TConfig>(File.ReadAllText(ConfigPath(configDirectory)));
        } catch { }
        if (config == null) {
            failed = true;
            config = new();
        }
        return config;
    }

    public static void Save(TConfig obj, string configDirectory) {
        if (!Directory.Exists(configDirectory))
            new DirectoryInfo(configDirectory).Create();
        File.WriteAllText(ConfigPath(configDirectory), obj.ToJson(indentation: true));
    }
}
