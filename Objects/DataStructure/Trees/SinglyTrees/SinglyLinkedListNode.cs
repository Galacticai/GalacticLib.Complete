using System.Collections;

namespace GalacticLib.Objects.DataStructure.Trees.SinglyTrees;

public class SinglyLinkedListNode<TValue>(
        TValue value,
        SinglyLinkedListNode<TValue>? next = null

) : IEnumerable<SinglyLinkedListNode<TValue>> {

    public TValue Value { get; set; } = value;
    public SinglyLinkedListNode<TValue>? Next { get; set; } = next;

    public override string ToString()
        => $"[{string.Join(',', this)}]";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); //? old... return modern one below
    public IEnumerator<SinglyLinkedListNode<TValue>> GetEnumerator() {
        SinglyLinkedListNode<TValue>? node = this;
        while (node is not null) {
            yield return node;
            node = node.Next;
        }
    }

    public static implicit operator List<TValue>(SinglyLinkedListNode<TValue> node)
        => [.. node];
    public static implicit operator TValue(SinglyLinkedListNode<TValue> node)
        => node.Value;
}
