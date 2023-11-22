// —————————————————————————————————————————————
//?
//!? 📜 Mask.cs
//!? 🖋️ Galacticai 📅 2022
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————

using System.Reflection;

namespace GalacticLib.Objects;

/// <summary> Masks the original value with a different one,
/// <br/> while keeping the original value safely as <see cref="BaseValue"/>
/// </summary>
/// <typeparam name="TMaskKey"> Key type of <see cref="Maskers"/> dictionary </typeparam>
/// <typeparam name="TValue"> Type of the <see cref="Value"/> </typeparam>
public class Mask<TMaskKey, TValue>
        where TMaskKey : notnull, IEquatable<TMaskKey> {

    /// <summary> Dictionary of <see cref="Masker"/> functions that are used successively to generate <see cref="Value"/> 
    /// <br/> with <typeparamref name="TMaskKey"/> as the key (to make it easier to access the <see cref="Masker"/>s </summary>
    public SortedDictionary<TMaskKey, Masker> Maskers { get; }
    /// <summary> Original value without the effect of <see cref="Maskers"/> </summary>
    public TValue BaseValue { get; }

    public Mask(TValue baseValue, SortedDictionary<TMaskKey, Masker> maskers) {
        if (baseValue is null)
            throw new ArgumentNullException(nameof(baseValue));
        BaseValue = baseValue;
        Maskers = maskers;
    }
    public Mask(TValue originalValue) : this(originalValue, []) { }


    /// <summary> Value after the effect of <see cref="Maskers"/> </summary>
    public TValue Value => ValueAfter(Maskers.Count);
    public TValue ValueAfter(int maskersCount) {
        if (maskersCount < 0)
            throw new ArgumentOutOfRangeException(nameof(maskersCount));
        else if (maskersCount == 0) return BaseValue;

        TValue value = BaseValue;
        //? Finish either if requested count is <0 or if all maskers have been used
        foreach (Masker masker in Maskers.Values) {
            if (maskersCount == 0) return value;
            value = masker(value);
            maskersCount--;
        }
        return value;
    }

    /// <summary> Clear <see cref="Maskers"/> So <see cref="Value"/> will return <see cref="BaseValue"/> </summary>
    public void Reset() => Maskers.Clear();

    /// <summary> Masking function used in a chain.
    /// <br/> Each <see cref="Masker"/> gets the value of the previous <see cref="Masker"/> into the <paramref name="value"/> parameter,
    /// <br/> then passes the result to the next <see cref="Masker"/> in <see cref="Maskers"/> </summary>
    public delegate TValue Masker(TValue value);

    /// <summary> Convert <typeparamref name="TValue"/> to <see cref="Mask{TMaskKey,TValue}"/> </summary>
    /// <returns> A new instance of <see cref="Mask{TMaskKey,TValue}"/>
    /// <br/> with <paramref name="value"/> as the <see cref="BaseValue"/> of the mask </returns>
    public static explicit operator Mask<TMaskKey, TValue>(TValue value)
        => new(value);
    /// <summary> Convert <see cref="Mask{TMaskKey,TValue}"/> to <typeparamref name="TValue"/> </summary>
    /// <returns> The final <see cref="Value"/> which is affected by <see cref="Maskers"/> 
    /// <br/> ⚠️ This means <see cref="Maskers"/> will no longer affect it because it's no longer inside a <see cref="Mask{TMaskKey,TValue}"/>
    /// </returns>
    public static explicit operator TValue(Mask<TMaskKey, TValue> mask)
        => mask.Value;
}
