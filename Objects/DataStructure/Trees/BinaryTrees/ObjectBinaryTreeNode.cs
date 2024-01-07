using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.BinaryTrees;

public class ObjectBinaryTreeNode<TObject>(
        TObject value,
        BinaryTreeNode<TObject>? left = null,
        BinaryTreeNode<TObject>? right = null

) : BinaryTreeNode<TObject>(
        value,
        left,
        right

), IJsonable<ObjectBinaryTreeNode<TObject>>
where TObject : notnull, IJsonable<TObject> {

    public override JsonNode ToJson() => new JsonObject() {
        { nameof(Value), Value.ToJson() },
        { nameof(Left), Left?.ToJson() },
        { nameof(Right), Right?.ToJson() },
    };

    public static ObjectBinaryTreeNode<TObject>? FromJson(JsonNode json) {
        JsonNode? valueJ = json[nameof(Value)];
        PropertyNotFoundException.ThrowIfNull(valueJ, nameof(Value));

        JsonNode? leftJ = json[nameof(Left)];
        JsonNode? rightJ = json[nameof(Right)];

        TObject? value = TObject.FromJson(valueJ!);
        ArgumentNullException.ThrowIfNull(value, nameof(Value));

        ObjectBinaryTreeNode<TObject>? left = leftJ is null ? null : ObjectBinaryTreeNode<TObject>.FromJson(leftJ);
        ObjectBinaryTreeNode<TObject>? right = rightJ is null ? null : ObjectBinaryTreeNode<TObject>.FromJson(rightJ);

        return new(value, left, right);
    }
}