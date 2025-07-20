using GalacticLib.Math.Numerics.Numbers.Quantity;
using GalacticLib.Math.Numerics.Numbers.Quantity.Units.Impl;
using GalacticLib.Math.Numerics.Numbers.Quantity.Units.UnitSystems;

namespace GalacticLib._Test.Quantities;

[TestFixture]
public class QuantitiesTest {

    [Test]
    public void Test_Quantities() {
        var q = new Quantity(5000, [MetricSystem.Kilo, Units.Bit]);
        Assert.That(q.ToString(), Is.EqualTo("5b"));
    }

}
