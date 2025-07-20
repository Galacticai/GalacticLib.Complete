using GalacticLib.Math.Numerics.Numbers.Quantity.Units;

namespace GalacticLib.Math.Numerics.Numbers.Quantity;

public record Quantity {
    private double _BaseValue { get; set; }
    public double BaseValue {
        get => _BaseValue;
        set {
            if (_BaseValue == value) return;
            _BaseValue = value;
            UpdateValue();
        }
    }

    private IReadOnlyList<Unit> _Units { get; set; }
    public IReadOnlyList<Unit> Units {
        get => _Units;
        set {
            _Units = value;
            UpdateAll();
        }
    }

    public double Value { get; private set; } = 0;
    public AffixWrap Affix { get; private set; } = AffixWrap.Empty;
    public FormatValueFunction FormatValue { get; set; }

    private void UpdateValue() {
        Value = Units.Aggregate(BaseValue, (v, u) => u.ToUnit(v));
    }
    private void UpdateAffix() {
        Affix = Units.Aggregate(AffixWrap.Empty, (v, u) => u.ApplyAffixWrap(v));
    }

    public void UpdateAll() {
        UpdateValue();
        UpdateAffix();
    }

    public string ToString(bool shorter = true)
        => Affix.ToString(FormatValue(Value), shorter);
    public override string ToString()
        => ToString(true);

    public Quantity(
        double unitValue,
        IReadOnlyList<Unit> units,
        FormatValueFunction? formatter = null
    ) {
        _Units = units;
        _BaseValue = _Units.Aggregate(unitValue, (v, u) => u.ToBase(v));
        FormatValue = formatter ?? DefaultValueFormatter;
        UpdateAll();
    }


    /// <summary> Format <see cref="Value"/> as <see cref="string"/> </summary>
    /// <param name="value"> <see cref="Value"/> </param>
    public delegate string FormatValueFunction(double value);

    public static FormatValueFunction DefaultValueFormatter
        => (v) => v.ToString("0.##");
}
