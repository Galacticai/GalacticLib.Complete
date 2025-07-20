
using GalacticLib.Math.Numerics.Numbers.Quantity;
using GalacticLib.Math.Numerics.Numbers.Quantity.Units.Impl;
using GalacticLib.Math.Numerics.Numbers.Quantity.Units.UnitSystems;


namespace GalacticLibConsole;

public static class Program {
    public static void Main(string[] _) {
        var q = new Quantity(5, [
            MetricSystem.Kilo, Units.Byte,
            new PerUnit([ Units.Second ])
        ]);
        Console.WriteLine(q.ToString());
        Console.WriteLine(q.ToString(false));
        Console.WriteLine(q.BaseValue);

        Console.WriteLine($"After Kilo: {MetricSystem.Kilo.ToBase(5)}");
        Console.WriteLine($"After Byte: {Units.Byte.ToBase(5000)}");
        //Console.WriteLine($"After PerSecond: {new PerUnit([Units.Second]).ToBase(result)}");

        Console.WriteLine($"BaseValue: {q.BaseValue}");
        var result = q.Units.Aggregate(q.BaseValue, (v, u) => {
            var newV = u.ToUnit(v);
            Console.WriteLine($"{u.GetType().Name}: {v} → {newV}");
            return newV;
        });
    }
}