using System.Reflection;

namespace GalacticLib.Objects;

public class VirtualObject<T>(T instance) {
    T _Instance { get; }
        = instance;
    Dictionary<string, MethodOverride> _MethodOverrides { get; }
        = [];
    HashSet<string> _MethodNames { get; }
        = typeof(T).GetMethods()
            .Aggregate(new HashSet<string>(),
                (set, method) => {
                    set.Add(method.Name);
                    return set;
                }
            );

    public delegate object MethodOverride(T instance, object[] args);

    public bool OverrideMethod(string methodName, MethodOverride methodOverride) {
        if (!_MethodNames.Contains(methodName)) return false;
        _MethodOverrides[methodName] = methodOverride;
        return true;
    }

    public object? InvokeMethod(string methodName, params object[] args) {
        if (_MethodOverrides.TryGetValue(methodName, out MethodOverride? methodOverride))
            return methodOverride(_Instance, args);

        MethodInfo methodInfo = typeof(T).GetMethod(methodName)
            ?? throw new ArgumentException($"Method {methodName} not found in class {typeof(T).Name}");
        return methodInfo?.Invoke(_Instance, args);
    }

    /// <summary> Convert <typeparamref name="T"/> to <see cref="VirtualObject{T}"/> </summary> 
    public static explicit operator VirtualObject<T>(T instance) => new(instance);
}

