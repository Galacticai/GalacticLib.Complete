namespace GalacticLib.Drawing.Colors;
public class ColorCMYK {
    public byte Cyan { get; set; }
    public byte Magenta { get; set; }
    public byte Yellow { get; set; }
    public byte Black { get; set; }
    public byte Alpha { get; set; }

    public ColorCMYK() : this(0, 0, 0, 0, 0) { }
    public ColorCMYK(byte cyan, byte magenta, byte yellow, byte black, byte alpha) {
        Cyan = cyan;
        Magenta = magenta;
        Yellow = yellow;
        Black = black;
        Alpha = alpha;
    }

    public static ColorCMYK FromRGB(Color color)
        => FromRGB(color.Red, color.Green, color.Blue, color.Alpha);
    public static ColorCMYK FromRGB(byte red, byte green, byte blue, byte alpha = 0xFF) {
        byte black = (byte)(1 - (byte.Max(byte.Max(red, green), blue) / 0xFF));
        int blackInverse = 1 / black;
        byte cyan = (byte)((1 - (red / 0xFF) - black) / blackInverse);
        byte magenta = (byte)((1 - (green / 0xFF) - black) / blackInverse);
        byte yellow = (byte)((1 - (blue / 0xFF) - black) / blackInverse);
        return new(cyan, magenta, yellow, black, alpha);
    }

    public static implicit operator Color(ColorCMYK color)
        => Color.FromCMYK(color.Cyan, color.Magenta, color.Yellow, color.Black, color.Alpha);
}
