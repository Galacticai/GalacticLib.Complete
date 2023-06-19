namespace GalacticLib.Drawing.Images;
public static class ImageHeaders {
    private static byte[] _BitByte(int i) => BitConverter.GetBytes(i);
    public static class BMP {
        public static List<byte> FileHeader(int rawPixelsLength) {
            List<byte> bytes = new();

            // BMP file header (14 bytes)
            bytes.AddRange(_BitByte('B')); // Signature
            bytes.AddRange(_BitByte('M'));
            bytes.AddRange(_BitByte(54 + rawPixelsLength)); // File size in bytes (including header)
            bytes.AddRange(_BitByte(0)); // Reserved
            bytes.AddRange(_BitByte(54)); // Data offset (start of pixel data)

            return bytes;
        }
        public static List<byte> V5Header(int width, int height, int rawPixelsLength) {
            List<byte> bytes = new();

            // BMP info header (40 bytes)
            bytes.AddRange(_BitByte(40)); // Info header size
            bytes.AddRange(_BitByte(width)); // Image width
            bytes.AddRange(_BitByte(height)); // Image height
            bytes.AddRange(_BitByte(1)); // Number of color planes
            bytes.AddRange(_BitByte(32)); // Bits per pixel (RGBA)
            bytes.AddRange(_BitByte(0)); // Compression method (none)
            bytes.AddRange(_BitByte(rawPixelsLength)); // Image size in bytes (including padding)
            bytes.AddRange(_BitByte(2835)); // Horizontal resolution (pixels per meter)
            bytes.AddRange(_BitByte(2835)); // Vertical resolution (pixels per meter)
            bytes.AddRange(_BitByte(0)); // Number of colors in the palette
            bytes.AddRange(_BitByte(0)); // Number of important colors

            return bytes;
        }

        public static List<byte> Headers(int width, int height, int rawPixelsLength) {
            List<byte> headers = new();
            List<byte> fileHeader = FileHeader(rawPixelsLength);
            List<byte> v5Header = V5Header(width, height, rawPixelsLength);
            headers.AddRange(fileHeader);
            headers.AddRange(v5Header);
            return headers;
        }
    }
}
