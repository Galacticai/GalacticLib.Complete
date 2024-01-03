using System.Collections;
using System.Text.Json.Nodes;
using Gtk;

namespace GalacticLib.Objects.DataStructure.Trees.Lists;

public abstract class ListNode<TValue>(
        TValue value,
        ListNode<TValue>? next = null

) : ITreeNode<TValue>
where TValue : notnull {

    public TValue Value { get; set; } = value;
    public ListNode<TValue>? Next { get; set; } = next;

    public int Count => this.Aggregate(0, (count, _) => count + 1);
    public override string ToString()
        => $"[{string.Join(',', this)}]";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<ITreeNode<TValue>> GetEnumerator() {
        ListNode<TValue>? node = this;
        while (node is not null) {
            yield return node;
            node = node.Next;
        }
    }

    public abstract JsonNode ToJson();

    public static implicit operator List<TValue>(ListNode<TValue> node)
        => node.Aggregate(new List<TValue>(), (list, e) => {
            list.Add(e.Value);
            return list;
        });
    public static implicit operator TValue(ListNode<TValue> node)
        => node.Value;
}
