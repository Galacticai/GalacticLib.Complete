//// —————————————————————————————————————————————
////?
////!? 📜 DirectoryTree.cs
////!? 🖋️ Galacticai 📅 2023
////!  ⚖️ GPL-3.0-or-later
////?  🔗 Dependencies:
////      + (Galacticai) Filesystem/Paths.cs
////?
//// —————————————————————————————————————————————


//using static GalacticLib.Filesystem.Paths;
//using io = System.IO;

//namespace GalacticLib.Filesystem;
//public class DirectoryTree {

//    // private object GetInfo(string path) {
//    //     string[] parts = path.Replace('\\', '/').Split('/');
//    //     FileSystemInfo info = path.GetFileInfo();
//    // }

//    private Dictionary<string, object> _Dictionary;
//    public string Path { get; private set; }

//    public DirectoryTree(string path) {
//        Path = path;
//        if (path.GetPathType() == PathType.None) return;
//        _Dictionary = new();
//        string dirName = io.Path.GetFileName(path);
//        _Dictionary[dirName] = new();
//        Dictionary<string, object> contents = (Dictionary<string, object>)_Dictionary[dirName];

//        //? Add files
//        foreach (string filePath in io.Directory.GetFiles(path))
//            contents[io.Path.GetFileName(filePath)] = new io.FileInfo(io.Path.GetFileName(filePath));

//        //? Add directories (deeper tree)
//        foreach (string subPath in io.Directory.GetDirectories(path))
//            contents[io.Path.GetFileName(subPath)] = new DirectoryTree(subPath);
//    }
//}
