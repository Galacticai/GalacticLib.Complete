namespace GalacticLib.Math.Numerics.Numbers.Units;
public class MeasuringValue(double value, Unit unit) {

    /// <summary> Value in the given <see cref="Unit"/> </summary>
    public double Value { get; private set; } = value;
    public Unit Unit { get; private set; } = unit;

    /// <summary> Value in the base unit (using <see cref="Unit{double}.ValueToBase"/>)</summary>
    public double ValueInBaseUnit => Value * Unit.BaseMultiplier;

    /// <summary> Convert <see cref="Value"/> to the given <paramref name="newUnit"/> </summary>
    /// <returns> A new value (<typeparamref name="double"/>) corresponding to the <paramref name="newUnit"/> </returns>
    public double ValueInUnit(Unit newUnit)
        => ValueInBaseUnit / newUnit.BaseMultiplier;

    /// <summary> Convert this to the given <paramref name="newUnit"/> </summary>
    /// <returns> A new <see cref="ValueWithUnit{double}"/> where the <see cref="Value"/> corresponds to the <paramref name="newUnit"/> </returns>
    public MeasuringValue InUnit(Unit newUnit)
        => new(ValueInUnit(newUnit), newUnit);

    public MeasuringValue InNearestUnit(params Unit[] units) {
        MeasuringValue result = FindNearestUnit(ValueInBaseUnit, units);
        result.Unit.Name.PreferShorter = Unit.Name.PreferShorter;
        return result;
    }

    /// <summary> Change <see cref="Value"/> while keeping <see cref="Unit"/> </summary>
    public void UnsafeSet(double newValue) => Value = newValue;
    /// <summary> Change <see cref="Unit"/> while keeping <see cref="Value"/> </summary>
    public void UnsafeSet(Unit newUnit) => Unit = newUnit;

    public string ToString(string separator, string valueFormat = "#0.##")
        => $"{Value.ToString(valueFormat)}{separator}{Unit}";
    public override string ToString() => ToString(string.Empty);

    /// <summary>
    /// <para> Get <see cref="Value"/> </para>
    /// <para> ⚠️ Data loss warning:
    /// <br/> This will discard <see cref="Unit"/> and will only return <see cref="Value"/> (<typeparamref name="double"/>) </para>
    /// </summary>
    public static explicit operator double(MeasuringValue valueWithUnit)
        => valueWithUnit.Value;


    /// <summary> For a given <paramref name="baseValue"/>, find the nearest <see cref="Unit"/> in the given <paramref name="units"/> </summary>
    /// <param name="baseValue"> Value in base unit 
    /// <br/> Example: <list type="bullet">
    /// <item> Good: 8 Bit (Bit is <see cref="BaseUnit"/>) </item>
    /// <item> Bad: 1 Byte (Byte is not a base unit) </item>
    /// </list></param>
    /// <param name="units"> <see cref="Unit"/>s to choose from </param>
    /// <returns><see cref="MeasuringValue"/> with value converted from <paramref name="baseValue"/> to the chosen unit </returns>
    /// <exception cref="ArgumentException"/>
    public static MeasuringValue FindNearestUnit(double baseValue, params Unit[] units) {
        if (units.Length == 0)
            throw new ArgumentException("You must provide units to choose from");

        Unit nearestUnit = units[0];
        double nearestValue = baseValue / nearestUnit.BaseMultiplier;
        double minDifference = double.MaxValue;

        foreach (Unit unit in units) {
            double unitValue = baseValue / unit.BaseMultiplier;
            double difference = System.Math.Abs(unitValue - 1);

            if (difference >= minDifference) continue;

            nearestUnit = unit;
            nearestValue = unitValue;
            minDifference = difference;
        }

        return new(nearestValue, nearestUnit);
    }
}
