using System.Collections;

namespace GalacticLib.Objects.DataStructure;

public class LinkedList<TValue>(TValue value, LinkedList<TValue>? next = null)
        : IEnumerable<TValue> {
    public TValue Value = value;
    public LinkedList<TValue>? Next = next;

    public override string ToString()
        => $"[{string.Join(',', this)}]";

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); //? old... return modern one below
    public IEnumerator<TValue> GetEnumerator() {
        LinkedList<TValue>? node = this;
        while (node is not null) {
            yield return node.Value;
            node = node.Next;
        }
    }

    public static implicit operator List<TValue>(LinkedList<TValue> node)
        => [.. node];
    public static implicit operator TValue(LinkedList<TValue> node)
        => node.Value;
}
