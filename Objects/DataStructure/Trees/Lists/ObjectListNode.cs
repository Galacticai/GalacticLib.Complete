using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.Lists;

public class ObjectListNode<TObject>(
        TObject value,
        ListNode<TObject>? next = null

) : ListNode<TObject>(
        value,
        next

), IJsonable<ObjectListNode<TObject>>
where TObject : notnull, IJsonable<TObject> {
    public override JsonNode ToJson() {
        JsonArray array = [];
        foreach (var node in this) array.Add(node.Value.ToJson());
        return array;
    }
    public static ObjectListNode<TObject>? FromJson(JsonNode json) {
        if (json is not JsonArray numbers)
            throw new ArgumentException($"The provided json is not a {nameof(JsonArray)}");
        if (numbers.Count == 0) return null;

        ObjectListNode<TObject> root = FromValueJ(numbers[0]);
        ListNode<TObject> node = root;
        for (int i = 1; i < numbers.Count; i++) {
            ObjectListNode<TObject>? child = FromValueJ(numbers[i]);
            node.Next = child;
            node = child;
        }
        return root;
    }

    private static ObjectListNode<TObject> FromValueJ(JsonNode? valueNode) {
        PropertyNotFoundException.ThrowIfNull(valueNode, nameof(Value));

        TObject? value = TObject.FromJson(valueNode!);
        ArgumentNullException.ThrowIfNull(value, nameof(Value));

        return new(value);
    }
}