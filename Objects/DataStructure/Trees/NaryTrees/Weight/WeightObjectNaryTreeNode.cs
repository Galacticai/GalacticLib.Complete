using System.Numerics;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees.Weight;

public class WeightObjectNaryTreeNode<TValue>(
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

), IJsonable<WeightObjectNaryTreeNode<TValue>>
where TValue : notnull, IJsonable<TValue> {

    public override JsonNode ToJson() => new JsonObject() {
        //! IMPORTANT: Do NOT touch Value, only use _Value, otherwise Weight will change
        { "Value", _Value.ToJson() },
        { nameof(IsSequenceEnd), IsSequenceEnd },
        { nameof(Weight), Weight },
        { nameof(WeightType), WeightType.ToString() },
    };

    public static WeightObjectNaryTreeNode<TValue> FromJson(JsonNode json) {
        var value = TValue.FromJson(json["Value"]!)!;
        var isSequenceEnd = json[nameof(IsSequenceEnd)]!.GetValue<bool>();
        var weight = json[nameof(Weight)]!.GetValue<long>();
        var weightType = json[nameof(Weight)]!.GetValue<WeightType>();
        return new(value, isSequenceEnd, weight, weightType);
    }
}