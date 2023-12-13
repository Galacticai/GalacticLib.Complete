using GalacticLib.Platforms;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GalacticLib.Filesystem;
/// <summary> Various tools for path (filesystem) manipulation </summary>
public static class Paths {
    #region Extra

    public enum PathType {
        None, File, Directory
    }
    public enum PathOS {
        None, Windows, Unix
    }
    public static class PathRegex {
        public const string Windows
            = @"^(?<drive>[a-z]:)?(?<path>(?:[\\]?(?:[\w !#()-]+|[.]{1,2})+)*[\\])?(?<filename>(?:[.]?[\w !#()-]+)+)?[.]?$";
        public const string Unix = "^(/[^/ ]*)+/?$";
    }

    #endregion
    #region Properties

    /// <summary> (ApplicationData) </summary>
    public static string ApplicationData
        => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    /// <summary> (ApplicationData)/(AppName) </summary>
    public static string? ThisApplicationData {
        get {
            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            return assemblyName.Name is null
                ? null
                : Path.Combine(ApplicationData, assemblyName.Name);
        }
    }

    /// <summary> (<see cref="Environment.SpecialFolder.UserProfile"/>) </summary>
    public static string UserProfile
        => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    /// <summary> (<see cref="Environment.SpecialFolder.MyPictures"/>) </summary>
    public static string PicturesDir
        => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

    /// <summary> (<see cref="Environment.SpecialFolder.ApplicationData"/>) </summary>

    #endregion
    #region Methods

    /// <exception cref="IOException" /> 
    public static void Create_ThisApplicationData()
        => new DirectoryInfo(
            ThisApplicationData
            ?? throw new IOException("Unable to get this app's assembly name")
        ).Create();

    /// <summary> Get path slash according to current OS </summary>
    /// <returns> <list type="bullet">
    /// <item> Windows: '\' </item> <item> Linux: '/' </item>
    /// </list> </returns>
    public static char GetPathSlash()
        => Platform.RunningWindows ? '\\' : '/';
    /// <summary> Same as <see cref="GetPathSlash()"/> </summary>
    public static char Slash => GetPathSlash();
    /// <summary> Get path slash according to <paramref name="pathOS"/> </summary>
    public static char GetPathSlash(PathOS pathOS)
        => pathOS == PathOS.Windows ? '\\' : '/';
    /// <summary> Check if <paramref name="c"/> is a path slash </summary>
    public static bool IsPathSlash(this char c)
        => c == '/' || c == '\\';
    /// <summary> Check if <paramref name="c"/> is a path slash according to current OS </summary>
    public static bool IsPathSlash_CurrentOS(this char c)
        => c == GetPathSlash();

    public static PathType GetPathType(this string path) {
        if (File.Exists(path)) return PathType.File;
        else if (Directory.Exists(path)) return PathType.Directory;
        else return PathType.None;
    }
    public static bool PathExists(this string path)
        => path.GetPathType() != PathType.None;
    public static bool PathIsFile(this string path)
        => path.GetPathType() == PathType.File;
    public static bool PathIsDirectory(this string path)
        => path.GetPathType() == PathType.Directory;
    public static PathOS GetPathOS(this string path) {
        if (path.PathIsValid_Unix()) return PathOS.Unix;
        else if (path.PathIsValid_Windows()) return PathOS.Windows;
        else return PathOS.None;
    }

    public static bool PathIsValid_Unix(this string path)
        => Regex.IsMatch(path, PathRegex.Unix);
    public static bool PathIsValid_Windows(this string path)
        => Regex.IsMatch(path, PathRegex.Windows);
    public static bool PathIsValid(this string path)
        => path.PathIsValid_Unix() || path.PathIsValid_Windows();
    public static bool PathIsSymbolic(this FileInfo fileInfo)
        => fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint);
    public static string[] GetPathParts(this string path, PathOS? pathOS = null)
        => (pathOS ?? path.GetPathOS()) switch {
            PathOS.Windows => path.Split('\\'),
            PathOS.Unix => path.Split('/'),
            _ => new[] { path }
        };

    public static bool DeletePath(string path) {
        if (!path.PathExists()) return true;
        //? invalid path: cannot delete
        if (!path.PathIsValid()) return false;

        switch (path.GetPathType()) {
            case PathType.File:
                File.Delete(path);
                return true;
            case PathType.Directory:
                Directory.Delete(path);
                return true;
            default: return false;
        }
    }

    /// <summary> Try to get an unused path where no file or directory is using the path
    /// <br/><br/> Examples:
    /// <list type="bullet">
    /// <item> "path/to/file.ext" => "path/to/file (1).ext" </item>
    /// <item> "path/to/file.ext" => "path/to/file (GUID).ext" </item>
    /// </list>
    /// </summary>
    /// <returns><list type="bullet">
    /// <item> "path/to/file (index).extension"</item>
    /// <item> "path/to/file (GUID).extension" if exceeding <paramref name="maxTries"/> </item>
    /// </list></returns>
    public static string GetUnusedPath(this string path, int maxTries = 20, Guid? guid = null) {
        if (!path.PathExists() || !path.PathIsValid())
            return path;

        //!? directory is not null because if !path.PathExists() => returns path
        string? directory = Path.GetDirectoryName(path);
        if (directory == null) return path;
        string name = Path.GetFileNameWithoutExtension(path);
        string dot_extension = Path.GetExtension(path) ?? string.Empty;
        string newPath = guid == null ? string.Empty : Path.Combine(directory, $"{name} ({guid}){dot_extension}");
        for (int i = 1; i <= maxTries; ++i) {
            if (!newPath.PathExists()) return newPath;
            newPath = Path.Combine(directory, $"{name} ({i}){dot_extension}");
        }
        //? not returned in the loop = failed to get a unique name
        //?     > use GUID instead of numbers
        return Path.Combine(directory, $"{name} ({guid ?? Guid.NewGuid()}){dot_extension}");
    }

    public static FileSystemInfo? GetFileInfo(this string path)
        => GetPathType(path) switch {
            PathType.Directory => new DirectoryInfo(path),
            PathType.File => new FileInfo(path),
            _ => null
        };

    public static bool PathIsRW(this string path) {
        switch (path.GetPathType()) {
            case PathType.File:
                return (File.GetAttributes(path) & FileAttributes.ReadOnly) == 0;
            case PathType.Directory:
                foreach (string entry in Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories)) {
                    FileAttributes entryAttributes = File.GetAttributes(entry);
                    if ((entryAttributes & FileAttributes.ReadOnly) != 0) return false;
                }
                return true;
            default: return false;
        }
    }

    #endregion
}
