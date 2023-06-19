
// —————————————————————————————————————————————
//?
//!? 📜 PathDictionary.cs
//!? 🖋️ Galacticai 📅 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) Filesystem/Paths.cs
//?
// —————————————————————————————————————————————

using static GalacticLib.Filesystem.Paths;

namespace GalacticLib.Filesystem;
public class PathDictionary {
    #region Shortcuts
    public bool PathExists => Path.GetPathType() != PathType.None;
    #endregion

    #region Methods
    /// <summary> Reload using the current<see cref="Path"/> </summary>
    public void ReloadInfo() {
        Content = new();
        switch (Path.GetPathType()) {
        case PathType.File:
            Info = new FileInfo(Path);
            break;

        case PathType.Directory:
            Info = new DirectoryInfo(Path);

            foreach (var item in ((DirectoryInfo)Info).GetFileSystemInfos())
                Content[item.Name] = item;

            break;

        default: // case PathType.None:
            Info = null;
            break;
        }
    }
    #endregion

    private string _Path = "";
    /// <summary> Set path and auto reload (<see cref="ReloadInfo"/>) </summary>
    public string Path {
        get => _Path;
        protected set {
            _Path = value;
            ReloadInfo();
        }
    }
#nullable enable

    /// <summary> Current <see cref="Path"/> info as <see cref="FileSystemInfo"/>
    /// <br/> ?????? If this is null then the path was not found </summary>
    public FileSystemInfo? Info { get; private set; }
    /// <summary> The contents of  the current <see cref="Path"/>
    /// <br/> ?????? If this is null then the path was not found </summary>
    public Dictionary<string, FileSystemInfo>? Content { get; private set; }
#nullable restore

    public PathDictionary(string path) {
        Path = path;
    }

    public override string ToString() {
        string count = "not found";
        if (Content != null)
            count = $"contains {(Content.Count > 0 ? Content.Count : "no")} item{(Content.Count != 1 ? 's' : "")}";
        return $"Path {count}: \"{Path}\"";
    }
}
