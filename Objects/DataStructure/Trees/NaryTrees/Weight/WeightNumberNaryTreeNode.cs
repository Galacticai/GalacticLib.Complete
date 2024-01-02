using System.Numerics;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees.Weight;

public class WeightNumberNaryTreeNode<TValue>(
        TValue value,
        bool isSequenceEnd = false,
        long weight = 0,
        WeightType weightType = WeightType.ReadCount,
        Dictionary<TValue, INaryTreeNode<TValue>>? children = null

) : WeightNaryTreeNode<TValue>(
    value,
    isSequenceEnd,
    weight,
    weightType,
    children

), IJsonable<WeightNumberNaryTreeNode<TValue>>
where TValue : notnull, INumber<TValue> {

    public override JsonNode ToJson() => new JsonObject() {
        //! IMPORTANT: Do NOT touch Value, only use _Value, otherwise Weight will change
        { "Value", JsonValue.Create(_Value) },
        { nameof(IsSequenceEnd), IsSequenceEnd },
        { nameof(Weight), Weight },
        { nameof(WeightType), WeightType.ToString() },
    };

    public static WeightNumberNaryTreeNode<TValue> FromJson(JsonNode json) {
        var value = json["Value"]!.GetValue<TValue>();
        var isSequenceEnd = json[nameof(IsSequenceEnd)]!.GetValue<bool>();
        var weight = json[nameof(Weight)]!.GetValue<long>();
        var weightType = json[nameof(Weight)]!.GetValue<WeightType>();
        return new(value, isSequenceEnd, weight, weightType);
    }
}