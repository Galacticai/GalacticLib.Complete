namespace GalacticLib.Math.Numerics.Numbers.Units;

/// <summary> Base value unit where the multiplier is 1 </summary>
/// <param name="name"> Unit name (if <see langword="null"/> then <see cref="NameShortLong.Empty"/> will be used) </param>
public class BaseUnit(NameShortLong? name = null)
        : SingleUnit(name ?? NameShortLong.Empty, 1) { }
