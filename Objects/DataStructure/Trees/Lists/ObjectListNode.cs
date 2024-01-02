using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.Lists;

public class ObjectListNode<TValue>(
        TValue value,
        ListNode<TValue>? next = null

) : ListNode<TValue>(
        value,
        next

), IJsonable<ObjectListNode<TValue>>
where TValue : notnull, IJsonable<TValue> {
    public override JsonNode ToJson() {
        JsonArray array = [];
        foreach (var node in this) array.Add(node.Value.ToJson());
        return array;
    }
    public static ObjectListNode<TValue>? FromJson(JsonNode json) {
        if (json is not JsonArray numbers)
            throw new ArgumentException($"The provided json is not a {nameof(JsonArray)}");
        if (numbers.Count == 0) return null;

        ObjectListNode<TValue> root = FromValueJ(numbers[0]);
        ListNode<TValue> node = root;
        for (int i = 1; i < numbers.Count; i++) {
            ObjectListNode<TValue>? child = FromValueJ(numbers[i]);
            node.Next = child;
            node = child;
        }
        return root;
    }

    private static ObjectListNode<TValue> FromValueJ(JsonNode? valueNode) {
        PropertyNotFoundException.ThrowIfNull(valueNode, nameof(Value));

        TValue? value = TValue.FromJson(valueNode!);
        ArgumentNullException.ThrowIfNull(value, nameof(Value));

        return new(value);
    }
}