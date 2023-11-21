// —————————————————————————————————————————————
//?
//!? 📜 Mask.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————


namespace GalacticLib;
/// <summary> Masks the original value with a different one,
/// <br/> while keeping the original value safely as <see cref="OriginalValue"/>
/// <br/><br/>
/// _______________
/// <br/>
/// To get the currently active value of this mask:
/// <br/><list type="bullet">
///     <item> (Implicit) `<c> mask </c>`  (Not all types work with this)</item>
///     <item> (Explicit) `<c> (<typeparamref name="TValue"/>)mask </c>` </item>
///     <item>   (Direct) `<c> <see cref="Value"/> </c>` </item>
/// </list></summary>
/// <typeparam name="TMaskKey"> Type of the key of <see cref="Maskers"/> </typeparam>
/// <typeparam name="TValue"> Type of the <see cref="Value"/> </typeparam>
public class Mask<TMaskKey, TValue> where TMaskKey : notnull {
    public TValue OriginalValue { get; }
    public Dictionary<TMaskKey, Masker> Maskers { get; set; }
    public TValue Value
        => Maskers.Values.Aggregate(
            OriginalValue,
            (current, maskFunction) => maskFunction(current)
        );

    public Mask(TValue originalValue, Dictionary<TMaskKey, Masker>? maskFunctions = null) {
        OriginalValue = originalValue;
        Maskers = maskFunctions ?? [];
    }

    /// <summary> Masking function used in a chain.
    /// <br/> Each <see cref="Masker"/> gets the value of the previous <see cref="Masker"/> into the <paramref name="value"/> parameter,
    /// <br/> then passes the result to the next <see cref="Masker"/> in <see cref="Maskers"/> </summary>
    public delegate TValue Masker(TValue value);

    public Mask<TMaskKey, TValue> ResetOriginal(TValue originalValue)
        => new(originalValue, Maskers);
    public TValue Reset() {
        Maskers = [];
        return OriginalValue;
    }

    //!? ! Data loss warning: Don't convert from TValue to Mask implicitly
    // public static implicit operator Mask<TMaskKey, TValue>(TValue value)
    //     => value;
    public static implicit operator TValue(Mask<TMaskKey, TValue> mask)
        => mask.Value;
}
