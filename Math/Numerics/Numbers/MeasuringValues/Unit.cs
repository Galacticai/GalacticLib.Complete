namespace GalacticLib.Math.Numerics.Numbers.Units;

public abstract class Unit {

    /// <summary> Multiplying by this converts a value from this unit into the value corresponding to base unit
    /// <para> Example: 1 Byte = (1*8) = 8 Bits where Bit is the base unit </para> </summary>
    /// <returns> The value corresponding to base unit </returns>
    /// <exception cref="OverflowException" />
    public abstract double BaseMultiplier { get; }

    public abstract NameShortLong Name { get; }

    public override string ToString() => Name.ToString();
}
