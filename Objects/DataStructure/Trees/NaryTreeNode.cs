using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GalacticLib.Objects.DataStructure.Trees;

public class NaryTreeNode<TValue>(
        TValue value,
        bool isSequenceEnd = false,
        Dictionary<TValue, INaryTreeNode<TValue>>? children = null

) : INaryTreeNode<TValue>
where TValue : notnull {

    public TValue Value { get; set; } = value;
    public IDictionary<TValue, INaryTreeNode<TValue>> Children { get; set; }
        = children ?? [];

    public bool IsSequenceEnd { get; set; } = isSequenceEnd;

    public bool IsEnd => Children.Count == 0;

    public bool Add(INaryTreeNode<TValue> childTree, bool force = false) {
        if (force) {
            Children[childTree.Value] = childTree;
            return true;
        }
        return Children.TryAdd(childTree.Value, childTree);
    }
    public bool Add(TValue value)
        => Add(new NaryTreeNode<TValue>(value), false);
    public bool Add([MinLength(1)] IEnumerable<TValue> sequence) {
        ArgumentNullException.ThrowIfNull(sequence);

        NaryTreeNode<TValue> currentNode = this;
        bool changed = false;
        foreach (TValue value in sequence) {
            NaryTreeNode<TValue> childNode = new(value);
            bool added = currentNode.Add(childNode);
            changed = changed || added;
            currentNode = childNode;
        }
        currentNode.IsSequenceEnd = true;
        return changed;
    }


    public bool TryGetValue(TValue value, [MaybeNullWhen(false)] out INaryTreeNode<TValue>? node)
        => Children.TryGetValue(value, out node);
    public INaryTreeNode<TValue>? TryGetChild(TValue value) {
        _ = TryGetValue(value, out INaryTreeNode<TValue>? node);
        return node;
    }


    public bool Contains(TValue value) => Children.ContainsKey(value);
    public bool Contains(INaryTreeNode<TValue> childTree)
        => Children.Contains(new(childTree.Value, childTree));
    public bool Contains(
            [MinLength(1)]
                IEnumerable<TValue> sequence,
            [MaybeNullWhen(false)]
                out List<INaryTreeNode<TValue>>? nodes
    ) {
        ArgumentNullException.ThrowIfNull(nameof(sequence));

        nodes = [];
        NaryTreeNode<TValue> node = this;

        foreach (TValue value in sequence) {
            NaryTreeNode<TValue>? childNode = (NaryTreeNode<TValue>?)node.TryGetChild(value);
            if (childNode is null) goto Fail;
            nodes.Add(childNode);
            node = childNode;
        }
        if (nodes.Count > 0 && node.IsSequenceEnd)
            return true;

        Fail:
        nodes = null;
        return false;
    }

    public bool Remove(TValue value) => Children.Remove(value);
    public bool Remove(INaryTreeNode<TValue> childTree) => Children.Remove(childTree.Value);
    public bool Remove([MinLength(1)] IEnumerable<TValue> sequence) {
        ArgumentNullException.ThrowIfNull(sequence);

        bool found = Contains(sequence, out List<INaryTreeNode<TValue>>? nodes);
        if (!found) return false;

        for (int i = nodes!.Count - 1; i > 0; i--) {
            var parent = nodes[i - 1];
            var child = nodes[i];
            parent.Remove(child);
        }
        Remove(nodes[0]);

        return true;
    }

    public void ClearChildren() => Children.Clear();


    public bool this[IEnumerable<TValue> sequence] => Contains(sequence, out _);
    public bool this[INaryTreeNode<TValue> tree] => Contains(tree);
    public INaryTreeNode<TValue>? this[TValue value] => TryGetChild(value);


    /// <summary> Calls <see cref="Add(TValue)"/> </summary> 
    public static NaryTreeNode<TValue> operator +(NaryTreeNode<TValue> tree, TValue value) {
        tree.Add(value);
        return tree;
    }
    /// <summary> Calls <see cref="Add(INaryTreeNode{TValue}, bool)"/> </summary> 
    public static NaryTreeNode<TValue> operator +(NaryTreeNode<TValue> tree, NaryTreeNode<TValue> childTree) {
        tree.Add(childTree);
        return tree;
    }
    /// <summary> Calls <see cref="Add(IEnumerable{TValue})"/> </summary> 
    public static NaryTreeNode<TValue> operator +(NaryTreeNode<TValue> tree, IEnumerable<TValue> sequence) {
        tree.Add(sequence);
        return tree;
    }

    /// <summary> Calls <see cref="Remove(TValue)"/> </summary> 
    public static NaryTreeNode<TValue> operator -(NaryTreeNode<TValue> tree, TValue value) {
        tree.Remove(value);
        return tree;
    }
    /// <summary> Calls <see cref="Remove(INaryTreeNode{TValue})"/> </summary> 
    public static NaryTreeNode<TValue> operator -(NaryTreeNode<TValue> tree, NaryTreeNode<TValue> childTree) {
        tree.Remove(childTree);
        return tree;
    }
    /// <summary> Calls <see cref="Remove(IEnumerable{TValue})"/> </summary> 
    public static NaryTreeNode<TValue> operator -(NaryTreeNode<TValue> tree, IEnumerable<TValue> sequence) {
        tree.Remove(sequence);
        return tree;
    }

    /// <summary> Get the value of a given <paramref name="tree"/> 
    /// <para> ⚠️ Data loss warning: you will get <see cref="Value"/> and lose the <paramref name="tree"/> along with its branches! </para>
    /// </summary>
    public static explicit operator TValue(NaryTreeNode<TValue> tree)
        => tree.Value;
    /// <summary> Generate a new <see cref="NaryTreeNode{TValue}"/> with the provided <paramref name="value"/> as the root value </summary>
    public static explicit operator NaryTreeNode<TValue>(TValue value)
        => new(value);
}
