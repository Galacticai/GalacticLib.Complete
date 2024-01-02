using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.BinaryTrees;

public class ObjectBinaryTreeNode<TValue>(
        TValue value,
        BinaryTreeNode<TValue>? left = null,
        BinaryTreeNode<TValue>? right = null

) : BinaryTreeNode<TValue>(
        value,
        left,
        right

), IJsonable<ObjectBinaryTreeNode<TValue>>
where TValue : notnull, IJsonable<TValue> {

    public override JsonNode ToJson() => new JsonObject() {
        { nameof(Value), Value.ToJson() },
        { nameof(Left), Left?.ToJson() },
        { nameof(Right), Right?.ToJson() },
    };

    public static ObjectBinaryTreeNode<TValue>? FromJson(JsonNode json) {
        JsonNode? valueJ = json[nameof(Value)];
        PropertyNotFoundException.ThrowIfNull(valueJ, nameof(Value));

        JsonNode? leftJ = json[nameof(Left)];
        JsonNode? rightJ = json[nameof(Right)];

        TValue? value = TValue.FromJson(valueJ!);
        ArgumentNullException.ThrowIfNull(value, nameof(Value));

        ObjectBinaryTreeNode<TValue>? left = leftJ is null ? null : ObjectBinaryTreeNode<TValue>.FromJson(leftJ);
        ObjectBinaryTreeNode<TValue>? right = rightJ is null ? null : ObjectBinaryTreeNode<TValue>.FromJson(rightJ);

        return new(value, left, right);
    }
}