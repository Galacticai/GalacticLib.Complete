using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.BinaryTrees;

class ObjectNaryTreeNode<TValue>(
        TValue value,
        BinaryTreeNode<TValue>? left = null,
        BinaryTreeNode<TValue>? right = null

) : BinaryTreeNode<TValue>(
        value,
        left,
        right

), IJsonable<ObjectNaryTreeNode<TValue>>
where TValue : notnull, IJsonable<TValue> {

    public override JsonNode ToJson() => new JsonObject() {
        { nameof(Value), Value.ToJson() },
        { nameof(Left), Left?.ToJson() },
        { nameof(Right), Right?.ToJson() },
    };

    public static ObjectNaryTreeNode<TValue>? FromJson(JsonNode json) {
        JsonNode? valueJ = json[nameof(Value)];
        PropertyNotFoundException.ThrowIfNull(valueJ, nameof(Value));

        JsonNode? leftJ = json[nameof(Left)];
        JsonNode? rightJ = json[nameof(Right)];

        TValue? value = TValue.FromJson(valueJ!);
        ArgumentNullException.ThrowIfNull(value, nameof(Value));

        ObjectNaryTreeNode<TValue>? left = leftJ is null ? null : ObjectNaryTreeNode<TValue>.FromJson(leftJ);
        ObjectNaryTreeNode<TValue>? right = rightJ is null ? null : ObjectNaryTreeNode<TValue>.FromJson(rightJ);

        return new(value, left, right);
    }
}