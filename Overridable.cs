namespace GalacticLib;
public class Overridable<T> {

    protected readonly IDictionary<string, object?> Dictionary;

#nullable enable
    protected TProperty? Get<TProperty>(string name) {
        if (Dictionary.TryGetValue(name, out object? value)) {
            return typeof(TProperty) != value?.GetType()
                ? default
                : (TProperty)value;
        }
        return default;
    }
#nullable restore

    protected TProperty Set<TProperty>(string name, TProperty value) {
        if (Dictionary.TryGetValue(name, out object? valueFromDictionary)
            && typeof(TProperty) != valueFromDictionary?.GetType()) {
            throw new System.AccessViolationException(
                $"The type {typeof(TProperty).Name} is different from the existing type {valueFromDictionary?.GetType().Name}."
            );
        }
        Dictionary[name] = value;
        return value;
    }

    protected virtual Overridable<T> Override(params object[] targets) {
        foreach (var target in targets)
            Set(target.GetType().Name, target);
        //? For continuity...
        return this;
    }

    public Overridable() {
        Dictionary = Objects.ObjectTools.AsDictionary<T>();
    }
}
