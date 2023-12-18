namespace GalacticLib.Objects.DataStructure;

/// <summary> Tree structure with <paramref name="left"/>/<paramref name="right"/> branches</summary>
/// <typeparam name="TValue"> Value type </typeparam>
/// <param name="value"> This node value </param>
/// <param name="left"> Left branch </param>
/// <param name="right"> Right branch </param>
public class BinaryTreeNode<TValue>(
        TValue value,
        BinaryTreeNode<TValue>? left = null,
        BinaryTreeNode<TValue>? right = null) {
    public TValue Value = value;
    public BinaryTreeNode<TValue>? Left = left;
    public BinaryTreeNode<TValue>? Right = right;

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
}