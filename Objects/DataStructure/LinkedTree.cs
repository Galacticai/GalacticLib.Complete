namespace GalacticLib.Objects.DataStructure;

public class TreeNode<TValue>(
        TValue value,
        TreeNode<TValue>? left = null,
        TreeNode<TValue>? right = null) {
    public TValue Value = value;
    public TreeNode<TValue>? Left = left;
    public TreeNode<TValue>? Right = right;

    public IEnumerable<TreeNode<TValue>> InOrder() {
        if (Left is not null)
            foreach (var node in Left.InOrder())
                yield return node;

        yield return this;

        if (Right is not null)
            foreach (var node in Right.InOrder())
                yield return node;
    }

    public static string ToString(IEnumerable<TreeNode<TValue>> treeNodes)
        => $"[{string.Join(',', treeNodes)}]";
    public override string ToString() => ToString(InOrder());
}