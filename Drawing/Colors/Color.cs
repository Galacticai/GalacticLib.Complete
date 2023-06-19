using System.Text;

namespace GalacticLib.Drawing.Colors;

/// <summary> Color represented by RGBA <see cref="byte"/> values </summary>
public class Color {
    #region this object
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
    public byte Alpha { get; set; }

    public Color() : this(0, 0, 0, 0) { }
    public Color(uint value, bool argb = false)
        : this(ExtractRed(value, argb), ExtractGreen(value, argb), ExtractBlue(value, argb), ExtractAlpha(value, argb)) { }
    public Color(byte red, byte green, byte blue) : this(red, green, blue, 0xFF) { }
    public Color(byte red, byte green, byte blue, byte alpha) {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    #endregion
    #region Shortcuts

    public const int RedShift = 24, GreenShift = 16, BlueShift = 8, AlphaShift = 0;
    public const int AlphaShiftARGB = 24, RedShiftARGB = 16, GreenShiftARGB = 8, BlueShiftARGB = 0;
    /// <summary> RGBA Color value in the form of unsigned integer (<see cref="uint"/>) </summary>
    public uint Value
        => (uint)((Red << RedShift) | (Green << GreenShift) | (Blue << BlueShift) | (Alpha << AlphaShift));
    /// <summary> ARGB Color value in the form of unsigned integer (<see cref="uint"/>) </summary>
    public uint ValueARGB
        => (uint)((Alpha << AlphaShiftARGB) | (Red << RedShiftARGB) | (Green << GreenShiftARGB) | (Blue << BlueShiftARGB));


    #endregion
    #region Methods

    public static byte ExtractAlpha(uint value, bool argb = false) => (byte)((value >> (argb ? AlphaShift : AlphaShiftARGB)) & 0xFF);
    public static byte ExtractRed(uint value, bool argb = false) => (byte)((value >> (argb ? RedShift : RedShiftARGB)) & 0xFF);
    public static byte ExtractGreen(uint value, bool argb = false) => (byte)((value >> (argb ? GreenShift : GreenShiftARGB)) & 0xFF);
    public static byte ExtractBlue(uint value, bool argb = false) => (byte)((value >> (argb ? BlueShift : BlueShiftARGB)) & 0xFF);
    public static Color Monochrome(byte black) => new(black, black, black);
    public static Color FromCMYK(ColorCMYK color)
        => FromCMYK(color.Cyan, color.Magenta, color.Yellow, color.Alpha);
    public static Color FromCMYK(byte cyan, byte magenta, byte yellow, byte black, byte alpha = 0xFF)
        => new(
            (byte)(0xFF * (1 - (cyan / 100)) * (1 - (black / 100))),
            (byte)(0xFF * (1 - (magenta / 100)) * (1 - (black / 100))),
            (byte)(0xFF * (1 - (yellow / 100)) * (1 - (black / 100))),
            alpha
        );

    #region Overrides

    public override string ToString() => ToString(hex: true);
    public string ToString(string valueFormat, params Channel[] channels) {
        StringBuilder sb = new();
        foreach (Channel channel in channels) {
            sb.Append(channel switch {
                Channel.Red => Red.ToString(valueFormat),
                Channel.Green => Green.ToString(valueFormat),
                Channel.Blue => Blue.ToString(valueFormat),
                Channel.Alpha => Alpha.ToString(valueFormat),
                _ => ""
            });
        }
        return sb.ToString();
    }
    public string ToString(bool hex, bool withHashtag = false, bool argb = false) {
        StringBuilder sb = new();
        if (hex) {
            if (withHashtag) sb.Append('#');
            if (argb)
                sb.Append(ToString("X2", Channel.Red, Channel.Green, Channel.Blue, Channel.Alpha));
            else sb.Append(ToString("X2", Channel.Alpha, Channel.Red, Channel.Green, Channel.Blue));
        } else {
            sb.Append(nameof(Color))
                .Append(" (R=")
                .Append(ToString("000", Channel.Red))
                .Append(", G=")
                .Append(ToString("000", Channel.Green))
                .Append(", B=")
                .Append(ToString("000", Channel.Blue))
                .Append(", A=")
                .Append(ToString("000", Channel.Alpha))
                .Append(')');
        }
        return sb.ToString();
    }

    #endregion
    #endregion
    #region Operators
    #region Conversion

    public static implicit operator Color(System.Drawing.Color sysColor) => new(sysColor.R, sysColor.G, sysColor.B, sysColor.A);
    public static implicit operator System.Drawing.Color(Color color) => System.Drawing.Color.FromArgb((int)color.ValueARGB);

    public static implicit operator Color(uint value) => new(value);
    /// <summary> RGBA <see cref="uint"/> color value </summary>
    public static implicit operator uint(Color color) => color.Value;
    /// <summary> RGBA color hex <see cref="string"/> (Example: "#RRGGBBAA")</summary>
    public static implicit operator string(Color color) => color.ToString(hex: true, withHashtag: true, argb: false);
    public static implicit operator byte[](Color color) => new[] { color.Red, color.Green, color.Blue, color.Alpha };

    #endregion
    #endregion

    public static class Preset {
        public static Color Transparent => new(0, 0, 0, 0);
        public static Color TransparentWhite => new(0xFF, 0xFF, 0xFF, 0);
        public static Color White => new(0xFF, 0xFF, 0xFF);
        public static Color Black => new(0, 0, 0);
        public static Color Red => new(0xFF, 0, 0);
        public static Color Green => new(0, 0xFF, 0);
        public static Color Blue => new(0, 0, 0xFF);
        public static Color Cyan => new(0, 0xFF, 0xFF);
        public static Color Magenta => new(0xFF, 0, 0xFF);
        public static Color Yellow => new(0xFF, 0xFF, 0);

        public static Color AliceBlue => new(0xF0, 0xF8, 0xFF);
        public static Color AntiqueWhite => new(0xFA, 0xEB, 0xD7);
        public static Color Aqua => new(0x00, 0xFF, 0xFF);
        public static Color Aquamarine => new(0x7F, 0xFF, 0xD4);
        public static Color Azure => new(0xF0, 0xFF, 0xFF);
        public static Color Beige => new(0xF5, 0xF5, 0xDC);
        public static Color Bisque => new(0xFF, 0xE4, 0xC4);
        public static Color BlanchedAlmond => new(0xFF, 0xEB, 0xCD);
        public static Color BlueViolet => new(0x8A, 0x2B, 0xE2);
        public static Color Brown => new(0xA5, 0x2A, 0x2A);
        public static Color BurlyWood => new(0xDE, 0xB8, 0x87);
        public static Color CadetBlue => new(0x5F, 0x9E, 0xA0);
        public static Color Chartreuse => new(0x7F, 0xFF, 0x00);
        public static Color Chocolate => new(0xD2, 0x69, 0x1E);
        public static Color Coral => new(0xFF, 0x7F, 0x50);
        public static Color CornflowerBlue => new(0x64, 0x95, 0xED);
        public static Color Cornsilk => new(0xFF, 0xF8, 0xDC);
        public static Color Crimson => new(0xDC, 0x14, 0x3C);
        public static Color DarkBlue => new(0x00, 0x00, 0x8B);
        public static Color DarkCyan => new(0x00, 0x8B, 0x8B);
        public static Color DarkGoldenrod => new(0xB8, 0x86, 0x0B);
        public static Color DarkGray => new(0xA9, 0xA9, 0xA9);
        public static Color DarkGreen => new(0x00, 0x64, 0x00);
        public static Color DarkKhaki => new(0xBD, 0xB7, 0x6B);
        public static Color DarkMagenta => new(0x8B, 0x00, 0x8B);
        public static Color DarkOliveGreen => new(0x55, 0x6B, 0x2F);
        public static Color DarkOrange => new(0xFF, 0x8C, 0x00);
        public static Color DarkOrchid => new(0x99, 0x32, 0xCC);
        public static Color DarkRed => new(0x8B, 0x00, 0x00);
        public static Color DarkSalmon => new(0xE9, 0x96, 0x7A);
        public static Color DarkSeaGreen => new(0x8F, 0xBC, 0x8B);
        public static Color DarkSlateBlue => new(0x48, 0x3D, 0x8B);
        public static Color DarkSlateGray => new(0x2F, 0x4F, 0x4F);
        public static Color DarkTurquoise => new(0x00, 0xCE, 0xD1);
        public static Color DarkViolet => new(0x94, 0x00, 0xD3);
        public static Color DeepPink => new(0xFF, 0x14, 0x93);
        public static Color DeepSkyBlue => new(0x00, 0xBF, 0xFF);
        public static Color DimGray => new(0x69, 0x69, 0x69);
        public static Color DodgerBlue => new(0x1E, 0x90, 0xFF);
        public static Color Firebrick => new(0xB2, 0x22, 0x22);
        public static Color FloralWhite => new(0xFF, 0xFA, 0xF0);
        public static Color ForestGreen => new(0x22, 0x8B, 0x22);
        public static Color Fuchsia => new(0xFF, 0x00, 0xFF);
        public static Color Gainsboro => new(0xDC, 0xDC, 0xDC);
        public static Color GhostWhite => new(0xF8, 0xF8, 0xFF);
        public static Color Gold => new(0xFF, 0xD7, 0x00);
        public static Color Goldenrod => new(0xDA, 0xA5, 0x20);
        public static Color Gray => new(0x80, 0x80, 0x80);
        public static Color GreenYellow => new(0xAD, 0xFF, 0x2F);
        public static Color Honeydew => new(0xF0, 0xFF, 0xF0);
        public static Color HotPink => new(0xFF, 0x69, 0xB4);
        public static Color IndianRed => new(0xCD, 0x5C, 0x5C);
        public static Color Indigo => new(0x4B, 0x00, 0x82);
        public static Color Ivory => new(0xFF, 0xFF, 0xF0);
        public static Color Khaki => new(0xF0, 0xE6, 0x8C);
        public static Color Lavender => new(0xE6, 0xE6, 0xFA);
        public static Color LavenderBlush => new(0xFF, 0xF0, 0xF5);
        public static Color LawnGreen => new(0x7C, 0xFC, 0x00);
        public static Color LemonChiffon => new(0xFF, 0xFA, 0xCD);
        public static Color LightBlue => new(0xAD, 0xD8, 0xE6);
        public static Color LightCoral => new(0xF0, 0x80, 0x80);
        public static Color LightCyan => new(0xE0, 0xFF, 0xFF);
        public static Color LightGoldenrodYellow => new(0xFA, 0xFA, 0xD2);
        public static Color LightGray => new(0xD3, 0xD3, 0xD3);
        public static Color LightGreen => new(0x90, 0xEE, 0x90);
        public static Color LightPink => new(0xFF, 0xB6, 0xC1);
        public static Color LightSalmon => new(0xFF, 0xA0, 0x7A);
        public static Color LightSeaGreen => new(0x20, 0xB2, 0xAA);
        public static Color LightSkyBlue => new(0x87, 0xCE, 0xFA);
        public static Color LightSlateGray => new(0x77, 0x88, 0x99);
        public static Color LightSteelBlue => new(0xB0, 0xC4, 0xDE);
        public static Color LightYellow => new(0xFF, 0xFF, 0xE0);
        public static Color Lime => new(0x00, 0xFF, 0x00);
        public static Color LimeGreen => new(0x32, 0xCD, 0x32);
        public static Color Linen => new(0xFA, 0xF0, 0xE6);
        public static Color Maroon => new(0x80, 0x00, 0x00);
        public static Color MediumAquamarine => new(0x66, 0xCD, 0xAA);
        public static Color MediumBlue => new(0x00, 0x00, 0xCD);
        public static Color MediumOrchid => new(0xBA, 0x55, 0xD3);
        public static Color MediumPurple => new(0x93, 0x70, 0xDB);
        public static Color MediumSeaGreen => new(0x3C, 0xB3, 0x71);
        public static Color MediumSlateBlue => new(0x7B, 0x68, 0xEE);
        public static Color MediumSpringGreen => new(0x00, 0xFA, 0x9A);
        public static Color MediumTurquoise => new(0x48, 0xD1, 0xCC);
        public static Color MediumVioletRed => new(0xC7, 0x15, 0x85);
        public static Color MidnightBlue => new(0x19, 0x19, 0x70);
        public static Color MintCream => new(0xF5, 0xFF, 0xFA);
        public static Color MistyRose => new(0xFF, 0xE4, 0xE1);
        public static Color Moccasin => new(0xFF, 0xE4, 0xB5);
        public static Color NavajoWhite => new(0xFF, 0xDE, 0xAD);
        public static Color Navy => new(0x00, 0x00, 0x80);
        public static Color OldLace => new(0xFD, 0xF5, 0xE6);
        public static Color Olive => new(0x80, 0x80, 0x00);
        public static Color OliveDrab => new(0x6B, 0x8E, 0x23);
        public static Color Orange => new(0xFF, 0xA5, 0x00);
        public static Color OrangeRed => new(0xFF, 0x45, 0x00);
        public static Color Orchid => new(0xDA, 0x70, 0xD6);
        public static Color PaleGoldenrod => new(0xEE, 0xE8, 0xAA);
        public static Color PaleGreen => new(0x98, 0xFB, 0x98);
        public static Color PaleTurquoise => new(0xAF, 0xEE, 0xEE);
        public static Color PaleVioletRed => new(0xDB, 0x70, 0x93);
        public static Color PapayaWhip => new(0xFF, 0xEF, 0xD5);
        public static Color PeachPuff => new(0xFF, 0xDA, 0xB9);
        public static Color Peru => new(0xCD, 0x85, 0x3F);
        public static Color Pink => new(0xFF, 0xC0, 0xCB);
        public static Color Plum => new(0xDD, 0xA0, 0xDD);
        public static Color PowderBlue => new(0xB0, 0xE0, 0xE6);
        public static Color Purple => new(0x80, 0x00, 0x80);
        public static Color RebeccaPurple => new(0x66, 0x33, 0x99);
        public static Color RosyBrown => new(0xBC, 0x8F, 0x8F);
        public static Color RoyalBlue => new(0x41, 0x69, 0xE1);
        public static Color SaddleBrown => new(0x8B, 0x45, 0x13);
        public static Color Salmon => new(0xFA, 0x80, 0x72);
        public static Color SandyBrown => new(0xF4, 0xA4, 0x60);
        public static Color SeaGreen => new(0x2E, 0x8B, 0x57);
        public static Color SeaShell => new(0xFF, 0xF5, 0xEE);
        public static Color Sienna => new(0xA0, 0x52, 0x2D);
        public static Color Silver => new(0xC0, 0xC0, 0xC0);
        public static Color SkyBlue => new(0x87, 0xCE, 0xEB);
        public static Color SlateBlue => new(0x6A, 0x5A, 0xCD);
        public static Color SlateGray => new(0x70, 0x80, 0x90);
        public static Color Snow => new(0xFF, 0xFA, 0xFA);
        public static Color SpringGreen => new(0x00, 0xFF, 0x7F);
        public static Color SteelBlue => new(0x46, 0x82, 0xB4);
        public static Color Tan => new(0xD2, 0xB4, 0x8C);
        public static Color Teal => new(0x00, 0x80, 0x80);
        public static Color Thistle => new(0xD8, 0xBF, 0xD8);
        public static Color Tomato => new(0xFF, 0x63, 0x47);
        public static Color Turquoise => new(0x40, 0xE0, 0xD0);
        public static Color Violet => new(0xEE, 0x82, 0xEE);
        public static Color Wheat => new(0xF5, 0xDE, 0xB3);
        public static Color WhiteSmoke => new(0xF5, 0xF5, 0xF5);
        public static Color YellowGreen => new(0x9A, 0xCD, 0x32);
    }
}
