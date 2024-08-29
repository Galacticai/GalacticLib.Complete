using System.Text;

namespace GalacticLib.Math.Numerics.Numbers.Units;

/// <summary> Multiple <see cref="ValueUnit"/> in succession </summary>
public class CompoundUnit(IList<SingleUnit> units) : Unit {

    public CompoundUnit(params SingleUnit[] units) : this(new List<SingleUnit>()) {
        AddUnit(units);
    }

    /// <summary> Multiple units ordered by their <see cref="int"/> key (<see cref="SortedDictionary{TKey, TValue}"/>) </summary>
    public IList<SingleUnit> Units { get; } = units;

    /// <summary> Multiplying by this converts a value from this unit into the value corresponding to base unit
    /// <para> Example: 1 KiloByte = (1*1000*8) = 8000 Bits where Bit is the base unit </para> </summary>
    /// <returns> The value corresponding to base unit </returns>
    /// <exception cref="OverflowException" />
    public override double BaseMultiplier {
        get {
            double result = 1d;
            foreach (SingleUnit unit in Units)
                result *= unit.BaseMultiplier;
            return result;
        }
    }

    public override NameShortLong Name
        => NameShortLong.Merge(Units.Select(u => u.Name));

    public void AddUnit(params SingleUnit[] units) {
        foreach (var unit in units) Units.Add(unit);
    }

    public override string ToString() {
        StringBuilder sb = new();
        foreach (SingleUnit unit in Units)
            sb.Append(unit.ToString());
        return sb.ToString();
    }
}