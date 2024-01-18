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
    public static byte[] Compress(this string str) {
        var bytes = Encoding.UTF8.GetBytes(str);
        using var msi = new MemoryStream(bytes);
        using var mso = new MemoryStream();
        using (var gs = new GZipStream(mso, CompressionMode.Compress))
            msi._CopyTo(gs);
        return mso.ToArray();
    }
    public static string Decompress(this byte[] bytes) {
        using var inStream = new MemoryStream(bytes);
        using var outStream = new MemoryStream();
        using (var gzStream = new GZipStream(inStream, CompressionMode.Decompress))
            gzStream._CopyTo(outStream);
        return Encoding.UTF8.GetString(outStream.ToArray());
    }
}
