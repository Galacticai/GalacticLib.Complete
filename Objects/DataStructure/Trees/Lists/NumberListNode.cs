using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.Lists;

public class NumberListNode<TNumber>(
        TNumber value,
        ListNode<TNumber>? next = null

) : ListNode<TNumber>(
        value,
        next

), IJsonable<NumberListNode<TNumber>>
where TNumber : notnull {

    public override string ToString()
        => $"[{string.Join(',', this)}]";
    public override JsonNode ToJson() {
        JsonArray array = [];
        foreach (var node in this) array.Add(JsonValue.Create(node.Value));
        return array;
    }
    public static NumberListNode<TNumber>? FromJson(JsonNode json) {
        if (json is not JsonArray numbers)
            throw new ArgumentException($"The provided json is not a {nameof(JsonArray)}");
        if (numbers.Count == 0) return null;

        NumberListNode<TNumber> root = FromValueJ(numbers[0]);
        ListNode<TNumber> node = root;
        for (int i = 1; i < numbers.Count; i++) {
            NumberListNode<TNumber>? child = FromValueJ(numbers[i]);
            node.Next = child;
            node = child;
        }
        return root;
    }

    private static NumberListNode<TNumber> FromValueJ(JsonNode? valueNode) {
        PropertyNotFoundException.ThrowIfNull(valueNode, nameof(Value));

        TNumber? value = valueNode!.GetValue<TNumber>();
        ArgumentNullException.ThrowIfNull(value, nameof(Value));

        return new(value);
    }
}