namespace GalacticLib.Math.Numerics.Numbers.Quantity.Units;
public record Affix(
    string Shorter,
    string Longer
) {
    public string ToString(bool shorter)
        => shorter ? Shorter : Longer;
    public override string ToString()
        => ToString(true);

    public readonly static Affix Empty
        = new(string.Empty, string.Empty);
}

public record AffixWrap(
    Affix Prefix,
    Affix Suffix
) {
    /// <returns> <code><see cref="Prefix"/><paramref name="value"/><see cref="Suffix"/></code> </returns>
    public string ToString(string value, bool shorter = true)
        => Prefix.ToString(shorter)
        + value
        + Suffix.ToString(shorter);

    /// <summary> For debugging </summary>
    /// <returns> <code><see cref="Prefix"/>...<see cref="Suffix"/></code> </returns>
    public override string ToString()
    => ToString("...", true);

    public readonly static AffixWrap Empty
        = new(Affix.Empty, Affix.Empty);
}