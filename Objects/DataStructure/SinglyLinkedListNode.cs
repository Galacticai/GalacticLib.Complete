using System.Collections;

namespace GalacticLib.Objects.DataStructure;

public class SinglyLinkedListNode<TValue>(TValue value, SinglyLinkedListNode<TValue>? next = null)
        : IEnumerable<TValue> {
    public TValue Value = value;
    public SinglyLinkedListNode<TValue>? Next = next;

    public override string ToString()
        => $"[{string.Join(',', this)}]";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); //? old... return modern one below
    public IEnumerator<TValue> GetEnumerator() {
        SinglyLinkedListNode<TValue>? node = this;
        while (node is not null) {
            yield return node.Value;
            node = node.Next;
        }
    }

    public static implicit operator List<TValue>(SinglyLinkedListNode<TValue> node)
        => [.. node];
    public static implicit operator TValue(SinglyLinkedListNode<TValue> node)
        => node.Value;
}
