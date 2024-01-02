using System.Numerics;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees;

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

    public static NumberNaryTreeNode<TNumber>? FromJson(JsonNode json) {
        if (json is not JsonObject jsonO)
            throw new ArgumentException($"The provided json is not a {nameof(JsonObject)}");

        var valueProp = json[nameof(Value)];
        PropertyNotFoundException.ThrowIfNull(valueProp, nameof(Value));

        var isSequenceEndProp = json[nameof(IsSequenceEnd)];
        PropertyNotFoundException.ThrowIfNull(isSequenceEndProp, nameof(IsSequenceEnd));

        TNumber value = valueProp!.GetValue<TNumber>();
        bool isSequenceEnd = isSequenceEndProp!.GetValue<bool>();

        return new(value, isSequenceEnd);
    }
}