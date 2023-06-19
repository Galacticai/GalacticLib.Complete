namespace GalacticLib.Drawing.Colors;

public class Color10bit {
    #region this object

    public ushort Red { get; set; }
    public ushort Green { get; set; }
    public ushort Blue { get; set; }
    public ushort Alpha { get; set; }

    public Color10bit() : this(0, 0, 0, 0) { }
    public Color10bit(long value)
        : this(ExtractRed(value), ExtractGreen(value), ExtractBlue(value), ExtractAlpha(value)) { }
    public Color10bit(ushort red, ushort green, ushort blue)
        : this(red, green, blue, 0xFFFF) { }
    public Color10bit(ushort red, ushort green, ushort blue, ushort alpha) {
        Red = red;
        Green = green;
        Blue = blue;
        Alpha = alpha;
    }

    #endregion
    #region Shortcuts

    public long Value
        => (Red << RedShift) | (Green << GreenShift) | (Blue << BlueShift) | (Alpha << AlphaShift);
    public long ValueARGB
        => (Alpha << AlphaShift) | (Red << RedShift) | (Green << GreenShift) | (Blue << BlueShift);

    public const ushort RedShift = 48, GreenShift = 32, BlueShift = 16, AlphaShift = 0;
    public const ushort AlphaShiftARGB = 48, RedShiftARGB = 32, GreenShiftARGB = 16, BlueShiftARGB = 0;

    #endregion
    #region Methods

    public static ushort ExtractAlpha(long value) => (ushort)((value >> AlphaShift) & 0xFFFF);
    public static ushort ExtractRed(long value) => (ushort)((value >> RedShift) & 0xFFFF);
    public static ushort ExtractGreen(long value) => (ushort)((value >> GreenShift) & 0xFFFF);
    public static ushort ExtractBlue(long value) => (ushort)((value >> BlueShift) & 0xFFFF);

    #region Overrides

    public override string ToString()
        => $"{nameof(Color10bit)} (R={Red:00000}, G={Green:00000}, B={Blue:00000}, A={Alpha:00000})";

    #endregion
    #endregion
    #region Operators
    #region Conversion

    public static implicit operator Color10bit(long value) => new(value);
    /// <summary> RGBA <see cref="long"/> color value </summary>
    public static implicit operator long(Color10bit color) => color.Value;
    /// <summary> RGBA <see cref="ushort"/> values as <see cref="string"/>
    /// <br/> Example: "Color10bit (R=00000, G=00000, B=00000, A=00000)" </summary>
    public static implicit operator string(Color10bit color) => color.ToString();
    public static implicit operator ushort[](Color10bit color) => new[] { color.Red, color.Green, color.Blue, color.Alpha };

    #endregion
    #endregion
}
