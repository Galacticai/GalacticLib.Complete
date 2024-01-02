using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.BinaryTrees;
class NumberNaryTreeNode<TValue>(
        TValue value,
        BinaryTreeNode<TValue>? left = null,
        BinaryTreeNode<TValue>? right = null

) : BinaryTreeNode<TValue>(
        value,
        left,
        right

), IJsonable<NumberNaryTreeNode<TValue>>
where TValue : notnull {

    public override JsonNode ToJson() => new JsonObject() {
        { nameof(Value), JsonValue.Create(Value) },
        { nameof(Left), Left?.ToJson() },
        { nameof(Right), Right?.ToJson() },
    };

    public static NumberNaryTreeNode<TValue>? FromJson(JsonNode json) {
        JsonNode? valueJ = json[nameof(Value)];
        PropertyNotFoundException.ThrowIfNull(valueJ, nameof(Value));

        JsonNode? leftJ = json[nameof(Left)];
        JsonNode? rightJ = json[nameof(Right)];

        TValue value = valueJ!.GetValue<TValue>();
        NumberNaryTreeNode<TValue>? left = leftJ is null ? null : NumberNaryTreeNode<TValue>.FromJson(leftJ);
        NumberNaryTreeNode<TValue>? right = rightJ is null ? null : NumberNaryTreeNode<TValue>.FromJson(rightJ);

        return new(value, left, right);
    }
}