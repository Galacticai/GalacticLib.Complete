// —————————————————————————————————————————————
//?
//!? 📜 TypeDictionary.cs
//!? 🖋️ Galacticai 📅 2022 - 2023
//!  ⚖️ GPL-3.0-or-later
//?  🔗 Dependencies: No special dependencies
//?
// —————————————————————————————————————————————


namespace GalacticLib;

/// <summary> Dictionary of <typeparamref name="AbstractTValue" /> with a <see cref=" Type" /> as the key </summary>
public class TypeDictionary<AbstractTValue> {
    #region this

    private Dictionary<string, AbstractTValue> _Dictionary { get; }

    public TypeDictionary(params (Type Type, AbstractTValue Value)[] items) : this() {
        Set(items);
    }

    public TypeDictionary() {
        _Dictionary = new();
    }

    #endregion

    #region Shortcuts

    public int Count => _Dictionary.Count;
    public List<Type> Keys => ToTypeList();
    public List<AbstractTValue> Values => ToList();
    public AbstractTValue this[Type type] => _Dictionary[type.ToString()];

    #endregion

    #region Methods

    public void TrimExcess() => _Dictionary.TrimExcess();
    public virtual void Clear() => _Dictionary.Clear();
    public virtual bool Remove(Type type) => _Dictionary.Remove(type.ToString());

    public virtual bool Remove<ITValue>() => _Dictionary.Remove(typeof(ITValue).ToString());

#nullable enable
    /// <summary> Try get a value using the full name of the type </summary>
    /// <param name="typeFullName"> Example: "System.Drawing.Common.Bitmap" </param>
    /// <returns></returns>
    public virtual AbstractTValue? Get(string typeFullName)
        => _Dictionary.TryGetValue(typeFullName, out AbstractTValue? value)
            ? value
            : default;

    public virtual AbstractTValue? Get(Type type)
        => _Dictionary.TryGetValue(type.ToString(), out AbstractTValue? value)
            ? value
            : default;

    public virtual AbstractTValue? Get<T>()
        => _Dictionary.TryGetValue(typeof(T).ToString(), out AbstractTValue? value)
            ? value
            : default;
#nullable restore

    public virtual bool Set(params (Type Type, AbstractTValue Value)[] items)
        => items.Aggregate(true, (current, item) => current && Set(item.Type, item.Value));

    public virtual bool Set<T>(AbstractTValue value, bool force = false) => Set(typeof(T), value, force);

    public virtual bool Set(Type type, AbstractTValue value, bool force = false) {
        if (value == null) return false;
        if (ContainsKey(type) && !force) return false;
        _Dictionary[type.ToString()] = value;
        return true;
    }

    /// <summary> Check if <paramref name="type" /> exists as a key </summary>
    public bool ContainsKey(Type type) => _Dictionary.ContainsKey(type.ToString());

    /// <summary> Check if <typeparamref name="T" /> type exists as a key </summary>
    public bool ContainsKey<T>() => _Dictionary.ContainsKey(typeof(T).ToString());

    /// <summary> Check if <paramref name="value" /> exists as a value </summary>
    public bool Contains(AbstractTValue value) => _Dictionary.ContainsValue(value);

    #endregion

    #region Conversion

    public virtual Dictionary<Type, AbstractTValue>.Enumerator GetEnumerator() => ToDictionary().GetEnumerator();

    /// <summary> Keys list </summary>
    public virtual List<string> ToRawTypeList() => _Dictionary.Keys.ToList();

    /// <summary> Keys list but the parsed into <see cref="Type" />s </summary>
    public virtual List<Type> ToTypeList()
        => _Dictionary.Keys.Aggregate(
            new List<Type>(),
            (list, typeString) => {
                Type? type = Type.GetType(typeString);
                if (type != null) list.Add(type);
                return list;
            }
        );

    /// <summary>
    ///     <see cref="ToValueList" />
    /// </summary>
    public virtual List<AbstractTValue> ToList() => ToValueList();

    /// <summary> Values list </summary>
    public virtual List<AbstractTValue> ToValueList() => _Dictionary.Values.ToList();

    /// <summary> A copy of the original dictionary </summary>
    public virtual Dictionary<string, AbstractTValue> ToRawDictionary() => new(_Dictionary);

    /// <summary>
    ///     A transformed copy of the original dictionary with the <see cref="Type" />s parsed from the
    ///     <see cref="string" /> keys
    /// </summary>
    public virtual Dictionary<Type, AbstractTValue> ToDictionary()
        => _Dictionary.Aggregate(
            new Dictionary<Type, AbstractTValue>(),
            (dictionary, item) => {
                Type? type = Type.GetType(item.Key);
                if (type != null)
                    dictionary.TryAdd(type, item.Value);
                return dictionary;
            }
        );

    public static implicit operator List<string>(TypeDictionary<AbstractTValue> typeDictionary) => typeDictionary.ToRawTypeList();
    public static implicit operator List<Type>(TypeDictionary<AbstractTValue> typeDictionary) => typeDictionary.ToTypeList();
    public static implicit operator List<AbstractTValue>(TypeDictionary<AbstractTValue> typeDictionary) => typeDictionary.ToValueList();
    public static implicit operator Dictionary<string, AbstractTValue>(TypeDictionary<AbstractTValue> typeDictionary) => typeDictionary.ToRawDictionary();
    public static implicit operator Dictionary<Type, AbstractTValue>(TypeDictionary<AbstractTValue> typeDictionary) => typeDictionary.ToDictionary();

    #endregion
}