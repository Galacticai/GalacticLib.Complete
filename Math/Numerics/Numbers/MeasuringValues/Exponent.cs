namespace GalacticLib.Math.Numerics.Numbers.Units;

/// <summary> This is a suffix for <see cref="SingleValueUnit{TNumber}"/> 
/// <para> Example: Kilo, Mega, Giga... </para> 
/// <para> ⚠️ <typeparamref name="TNumber"/> must be a suitable number type 
///     for the <paramref name="valueToBase"/> function
///     otherwise it will cause <typeparamref name="TNumber"/> in checked mode </para>
/// </summary>
/// <exception cref="OverflowException" />
public class Exponent(NameShortLong name, double toBaseMultiplier)
        : SingleUnit(name, toBaseMultiplier, null) { }
