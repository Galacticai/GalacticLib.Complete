// —————————————————————————————————————————————
//?
//!? 📜 TypeDictionary.cs
//!? 🖋️ Galacticai 📅 2022 - 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies:
//      + (Galacticai) GalacticLib/TypeDictionary.cs
//?
// —————————————————————————————————————————————

namespace GalacticLib;

/// <summary> Dictionary of <typeparamref name="AbstractTValue"/> with the <see cref=" Type"/> of <typeparamref name="AbstractTValue"/> as the key
/// <br/> ⚠️ Warning: If you repeat the same <typeparamref name="AbstractTValue"/>, the first one will be applied. </summary>
public class StrictTypeDictionary<AbstractTValue> : TypeDictionary<AbstractTValue> {
    #region Shortcuts

    public bool TypeIsValid<T>() => TypeIsValid(typeof(T));
    public bool TypeIsValid(Type type) => type.IsSubclassOf(typeof(AbstractTValue));

    #endregion
    #region Methods

    public override bool Remove(Type type)
        => TypeIsValid(type)
        && base.Remove(type);

    public new virtual bool Remove<ITValue>() where ITValue : AbstractTValue
        => base.Remove(typeof(ITValue));

#nullable enable
    public override AbstractTValue? Get(Type type)
        => TypeIsValid(type) ? base.Get(type) : default;
    public new ITValue? Get<ITValue>() where ITValue : AbstractTValue
        => (ITValue?)base.Get<ITValue>();
#nullable restore

    //! Hidden
    // private new bool Set(params (Type Type, AbstractTValue Value)[] items)
    //     => base.Set(items);
    private new bool Set<T>(AbstractTValue value, bool force = false)
        => base.Set<T>(value, force);
    private new bool Set(Type type, AbstractTValue value, bool force = false)
        => base.Set(type, value, force);
    //! =====
    public virtual bool Set<ITValue>(params ITValue[] values) where ITValue : AbstractTValue {
        bool success = true;
        foreach (var value in values)
            success = success && Set(value);
        return success;
    }

    public virtual bool Set<ITValue>(ITValue value, bool force = false) where ITValue : AbstractTValue
        => Set(typeof(ITValue), value, force);
    /// <summary> Check if <typeparamref name="ITValue"/> type exists as a key </summary>
    public new virtual bool ContainsKey<ITValue>() where ITValue : AbstractTValue
        => base.ContainsKey(typeof(ITValue));
    /// <summary> Check if <paramref name="value"/> exists as a value </summary>
    private new bool Contains(AbstractTValue value)
        => base.Contains(value);
    /// <summary> Check if <paramref name="value"/> exists as a value </summary>
    public bool Contains<ITValue>(ITValue value) where ITValue : AbstractTValue {
        //? No need to loop through all values
        //? X Dictionary.ContainsValue(value)
        //? > Directly:
        //?     + Check if type exists
        //?     + Compare value of that key to the provided value
        ITValue? value_FromDictionary = Get<ITValue>();
        if (value_FromDictionary == null) return false;
        return Comparer<ITValue>.Default.Compare(value, value_FromDictionary) >= 0;
    }

    #endregion

    /// <summary> ℹ️ Note: If you repeat the same type more than once, the last one will be applied. </summary>
    public StrictTypeDictionary(params AbstractTValue[] values) : this() {
        foreach (AbstractTValue value in values) {
            Set(value);
        }
    }
    public StrictTypeDictionary(TypeDictionary<AbstractTValue> typeDictionary)
                : this(typeDictionary.Values) { }
    public StrictTypeDictionary(List<AbstractTValue> list) : this() {
        foreach (var value in list) Set(value);
    }
    public StrictTypeDictionary() { }

    #region Conversion
    public new virtual List<AbstractTValue>.Enumerator GetEnumerator()
        => ToList().GetEnumerator();

    public static implicit operator StrictTypeDictionary<AbstractTValue>(List<AbstractTValue> list)
        => new(list);
    public static implicit operator StrictTypeDictionary<AbstractTValue>(AbstractTValue[] array)
        => new(array);
    public static implicit operator StrictTypeDictionary<AbstractTValue>(Dictionary<string, AbstractTValue>.ValueCollection typeDictionaryValues)
        => typeDictionaryValues.ToArray();
    public static implicit operator StrictTypeDictionary<AbstractTValue>(Dictionary<string, AbstractTValue> typeDictionary)
        => typeDictionary.Values;
    #endregion
}
