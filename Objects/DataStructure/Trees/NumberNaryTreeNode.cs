using System.Numerics;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees;

/// <summary> Data structure representing n-ary tree node with support for <see cref="INumber{TValue}"/> data type </summary>
/// <typeparam name="TNumber"> Number type of <see cref="Value"/> that inherits <see cref="INumber{TValue}"/> </typeparam>
/// <param name="value"> This node value </param>
/// <param name="isSequenceEnd"> This node is the end of a sequence </param>
/// <param name="children"> This node children </param>
public class NumberNaryTreeNode<TNumber>(
        TNumber value,
        bool isSequenceEnd = false,
        Dictionary<TNumber, INaryTreeNode<TNumber>>? children = null

) : NaryTreeNode<TNumber>(
        value,
        isSequenceEnd,
        children

), IJsonable<NumberNaryTreeNode<TNumber>>
where TNumber : notnull, INumber<TNumber> {

    public override JsonNode ToJson() => new JsonObject() {
        { nameof(Value), Value.ToString() },
        { nameof(IsSequenceEnd), IsSequenceEnd.ToString() },
    };

    public static NumberNaryTreeNode<TNumber> FromJson(JsonNode json) {
        var value = json["Value"]!.GetValue<TNumber>();
        var isSequenceEnd = json[nameof(IsSequenceEnd)]!.GetValue<bool>();
        return new(value, isSequenceEnd);
    }
}