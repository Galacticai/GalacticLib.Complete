using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees.Weight;

public class WeightObjectNaryTreeNode<TObject>(
        TObject value,
        bool isSequenceEnd = false,
        long weight = 0,
        WeightType weightType = WeightType.ReadCount,
        Dictionary<TObject, NaryTreeNode<TObject>>? children = null

) : WeightNaryTreeNode<TObject>(
    value,
    isSequenceEnd,
    weight,
    weightType,
    children

), IJsonable<WeightObjectNaryTreeNode<TObject>>
where TObject : notnull, IJsonable<TObject> {

    protected override NaryTreeNode<TObject> Create(TObject value)
        => new WeightObjectNaryTreeNode<TObject>(value);

    public override JsonNode ToJson() => new JsonObject() {
        //! IMPORTANT: Do NOT touch Value, only use _Value, otherwise Weight will change
        { "Value", _Value.ToJson() },
        { nameof(IsSequenceEnd), IsSequenceEnd },
        { nameof(Weight), Weight },
        { nameof(WeightType), WeightType.ToString() },
    };

    public static WeightObjectNaryTreeNode<TObject> FromJson(JsonNode json) {
        var value = TObject.FromJson(json["Value"]!)!;
        var isSequenceEnd = json[nameof(IsSequenceEnd)]!.GetValue<bool>();
        var weight = json[nameof(Weight)]!.GetValue<long>();
        var weightType = json[nameof(Weight)]!.GetValue<WeightType>();
        return new(value, isSequenceEnd, weight, weightType);
    }
}