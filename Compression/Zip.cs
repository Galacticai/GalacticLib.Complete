using System.IO.Compression;
using System.Text;

namespace GalacticLib.Compression;

public static class Zip {
    private static void _CopyTo(this Stream src, Stream dest) {
        byte[] bytes = new byte[4096];
        int cnt;
        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            dest.Write(bytes, 0, cnt);
    }
    public static byte[] CompressIfSmaller(this string str, out bool didCompress) {
        byte[] compressed = str.Compress();
        if (str.Length >= compressed.Length) {
            didCompress = true;
            return compressed;
        } else {
            didCompress = false;
            return Encoding.UTF8.GetBytes(str);
        }
    }
    public static byte[] Compress(this string str) {
        var bytes = Encoding.UTF8.GetBytes(str);
        using var msi = new MemoryStream(bytes);
        using var mso = new MemoryStream();
        using (var gs = new GZipStream(mso, CompressionMode.Compress))
            msi._CopyTo(gs);
        return mso.ToArray();
    }
    public static string Decompress(this byte[] bytes) {
        using var msi = new MemoryStream(bytes);
        using var mso = new MemoryStream();
        using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            gs._CopyTo(mso);
        return Encoding.UTF8.GetString(mso.ToArray());
    }
}
