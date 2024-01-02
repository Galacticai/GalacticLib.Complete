using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.Lists;

public class NumberListNode<TValue>(
        TValue value,
        ListNode<TValue>? next = null

) : ListNode<TValue>(
        value,
        next

), IJsonable<NumberListNode<TValue>>
where TValue : notnull {
    public override string ToString()
        => $"[{string.Join(',', this)}]";
    public override JsonNode ToJson() {
        JsonArray array = [];
        foreach (var node in this) array.Add(JsonValue.Create(node.Value));
        return array;
    }
    public static NumberListNode<TValue>? FromJson(JsonNode json) {
        if (json is not JsonArray numbers)
            throw new ArgumentException($"The provided json is not a {nameof(JsonArray)}");
        if (numbers.Count == 0) return null;

        NumberListNode<TValue> root = FromValueJ(numbers[0]);
        ListNode<TValue> node = root;
        for (int i = 1; i < numbers.Count; i++) {
            NumberListNode<TValue>? child = FromValueJ(numbers[i]);
            node.Next = child;
            node = child;
        }
        return root;
    }

    private static NumberListNode<TValue> FromValueJ(JsonNode? valueNode) {
        PropertyNotFoundException.ThrowIfNull(valueNode, nameof(Value));

        TValue? value = valueNode!.GetValue<TValue>();
        ArgumentNullException.ThrowIfNull(value, nameof(Value));

        return new(value);
    }
}