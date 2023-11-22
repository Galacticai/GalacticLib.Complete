using System.Reflection;

namespace GalacticLib.Objects;

public class VirtualObject<T>(T instance) {
    private T _Instance { get; } = instance;
    private Dictionary<string, MethodOverride> _MethodOverrides { get; } = [];

    public delegate object MethodOverride(T instance, object[] args);

    public void OverrideMethod(string methodName, MethodOverride methodOverride) {
        _MethodOverrides[methodName] = methodOverride;
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

