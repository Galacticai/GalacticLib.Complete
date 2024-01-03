using System.Collections;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.BinaryTrees;

/// <summary> Tree structure with <paramref name="left"/>/<paramref name="right"/> branches</summary>
/// <typeparam name="TValue"> Value type </typeparam>
/// <param name="value"> This node value </param>
/// <param name="left"> Left branch </param>
/// <param name="right"> Right branch </param>
public abstract class BinaryTreeNode<TValue>(
        TValue value,
        BinaryTreeNode<TValue>? left = null,
        BinaryTreeNode<TValue>? right = null

) : ITreeNode<TValue>
where TValue : notnull {
    public TValue Value { get; set; } = value;
    public BinaryTreeNode<TValue>? Left { get; set; } = left;
    public BinaryTreeNode<TValue>? Right { get; set; } = right;


    public IEnumerable<BinaryTreeNode<TValue>> InOrder() {
        if (Left is not null)
            foreach (var node in Left.InOrder())
                yield return node;

        yield return this;

        if (Right is not null)
            foreach (var node in Right.InOrder())
                yield return node;
    }

    public static string ToString(IEnumerable<BinaryTreeNode<TValue>> treeNodes)
        => $"[{string.Join(',', treeNodes)}]";
    public override string ToString() => ToString(InOrder());

    public abstract JsonNode ToJson();

    public IEnumerator<ITreeNode<TValue>> GetEnumerator() {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        throw new NotImplementedException();
    }
}