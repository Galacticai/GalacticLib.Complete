using System.Numerics;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees.Weight;

public class WeightNumberNaryTreeNode<TNumber>(
        TNumber value,
        bool isSequenceEnd = false,
        long weight = 0,
        WeightType weightType = WeightType.ReadCount,
        Dictionary<TNumber, NaryTreeNode<TNumber>>? children = null

) : WeightNaryTreeNode<TNumber>(
    value,
    isSequenceEnd,
    weight,
    weightType,
    children

), IJsonable<WeightNumberNaryTreeNode<TNumber>>
where TNumber : notnull, INumber<TNumber> {

    protected override NaryTreeNode<TNumber> Create(TNumber value)
        => new WeightNumberNaryTreeNode<TNumber>(value);

    public override JsonNode ToJson() => new JsonObject() {
        //! IMPORTANT: Do NOT touch Value, only use _Value, otherwise Weight will change
        { "Value", JsonValue.Create(_Value) },
        { nameof(IsSequenceEnd), IsSequenceEnd },
        { nameof(Weight), Weight },
        { nameof(WeightType), WeightType.ToString() },
    };

    public static WeightNumberNaryTreeNode<TNumber> FromJson(JsonNode json) {
        var value = json["Value"]!.GetValue<TNumber>();
        var isSequenceEnd = json[nameof(IsSequenceEnd)]!.GetValue<bool>();
        var weight = json[nameof(Weight)]!.GetValue<long>();
        var weightType = json[nameof(Weight)]!.GetValue<WeightType>();
        return new(value, isSequenceEnd, weight, weightType);
    }
}
