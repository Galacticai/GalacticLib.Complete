using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.BinaryTrees;
public class NumberBinaryTreeNode<TNumber>(
        TNumber value,
        BinaryTreeNode<TNumber>? left = null,
        BinaryTreeNode<TNumber>? right = null

) : BinaryTreeNode<TNumber>(
        value,
        left,
        right

), IJsonable<NumberBinaryTreeNode<TNumber>>
where TNumber : notnull {

    public override JsonNode ToJson() => new JsonObject() {
        { nameof(Value), JsonValue.Create(Value) },
        { nameof(Left), Left?.ToJson() },
        { nameof(Right), Right?.ToJson() },
    };

    public static NumberBinaryTreeNode<TNumber>? FromJson(JsonNode json) {
        JsonNode? valueJ = json[nameof(Value)];
        PropertyNotFoundException.ThrowIfNull(valueJ, nameof(Value));

        JsonNode? leftJ = json[nameof(Left)];
        JsonNode? rightJ = json[nameof(Right)];

        TNumber value = valueJ!.GetValue<TNumber>();
        NumberBinaryTreeNode<TNumber>? left = leftJ is null ? null : NumberBinaryTreeNode<TNumber>.FromJson(leftJ);
        NumberBinaryTreeNode<TNumber>? right = rightJ is null ? null : NumberBinaryTreeNode<TNumber>.FromJson(rightJ);

        return new(value, left, right);
    }
}