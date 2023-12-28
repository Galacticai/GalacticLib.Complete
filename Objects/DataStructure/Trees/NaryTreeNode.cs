using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GalacticLib.Objects.DataStructure.Trees;

public class NaryTreeNode<TValue>(
        TValue value,
        bool isSequenceEnd = false,
        IDictionary<TValue, NaryTreeNode<TValue>>? children = null

) where TValue : notnull {

    public TValue Value { get; set; } = value;
    public IDictionary<TValue, NaryTreeNode<TValue>> Children { get; set; }
        = children ?? new Dictionary<TValue, NaryTreeNode<TValue>>();

    /// <summary> End of sequence (could have children that are for other sequences) 
    /// <br/> Example: ABC ... ABCDEF </summary>
    public bool IsSequenceEnd { get; set; } = isSequenceEnd;

    /// <summary> End of tree (no children) </summary>
    public bool IsEnd => Children.Count == 0;

    /// <summary> Add a child tree </summary>
    /// <returns> true if added </returns>
    public bool Add(NaryTreeNode<TValue> childTree, bool force = false) {
        if (force) {
            Children[childTree.Value] = childTree;
            return true;
        }
        return Children.TryAdd(childTree.Value, childTree);
    }
    /// <summary> Add a new tree with the given <paramref name="value"/> </summary> 
    /// <returns> true if it did not exist and was added </returns>
    public bool Add(TValue value)
        => Add(new NaryTreeNode<TValue>(value), false);
    /// <summary> Add a given <paramref name="sequence"/> </summary> 
    /// <returns> true if the tree changed </returns>
    public bool Add([MinLength(1)] params TValue[] sequence) {
        if (sequence is null || sequence.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(sequence));

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
    /// <summary> Add a given <paramref name="sequence"/> </summary> 
    /// <returns> true if the tree changed </returns>
    public bool Add([MinLength(1)] IEnumerable<TValue> sequence)
        => Add(sequence.ToArray());


    public bool TryGetValue(TValue value, [MaybeNullWhen(false)] out NaryTreeNode<TValue>? node)
        => Children.TryGetValue(value, out node);
    public NaryTreeNode<TValue>? TryGetChild(TValue value) {
        _ = TryGetValue(value, out NaryTreeNode<TValue>? node);
        return node;
    }


    /// <summary> Check if a direct child has the given <paramref name="value"/> (Only checks 1 level of children) </summary>
    /// <returns> true if <paramref name="value"/> was found </returns>
    public bool Contains(TValue value) => Children.ContainsKey(value);
    /// <summary> Check if this node has the given <paramref name="childTree"/> (Only checks 1 level of children) </summary>
    /// <returns> true if <paramref name="childTree"/> was found </returns>
    public bool Contains(NaryTreeNode<TValue> childTree)
        => Children.Contains(new(childTree.Value, childTree));
    /// <summary> Checks if a <paramref name="sequence"/> exists under this node (Goes deeper in the tree equal to the sequence length) 
    /// <br/> ⚠️ The <paramref name="sequence"/> starts from the children, not from this node </summary>
    /// <param name="sequence"> Sequence of <typeparamref name="TValue"/> </param>
    /// <returns> true if the <paramref name="sequence"/> was found </returns>
    public bool Contains([MinLength(1)] params TValue[] sequence) {
        if (sequence is null || sequence.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(sequence));

        NaryTreeNode<TValue> node = this;
        foreach (TValue value in sequence) {
            NaryTreeNode<TValue>? childNode = node.TryGetChild(value);
            if (childNode is null) return false;
            node = childNode;
        }
        return node.IsSequenceEnd;
    }
    /// <summary> Checks if a <paramref name="sequence"/> exists under this node (Goes deeper in the tree equal to the sequence length) 
    /// <br/> ⚠️ The <paramref name="sequence"/> starts from the children, not from this node </summary>
    /// <param name="sequence"> Sequence of <typeparamref name="TValue"/> </param>
    /// <returns> true if the <paramref name="sequence"/> was found </returns>
    public bool Contains([MinLength(1)] IEnumerable<TValue> sequence)
        => Contains(sequence.ToArray());


    /// <summary> Remove a child that has the given <paramref name="value"/> </summary>
    /// <returns> true if <paramref name="value"/> was found and removed </returns>
    public bool Remove(TValue value) => Children.Remove(value);
    /// <summary> Remove a the given <paramref name="childTree"/> </summary>
    /// <returns> true if the <paramref name="childTree"/> was found, then removed </returns>
    public bool Remove(NaryTreeNode<TValue> childTree) => Children.Values.Remove(childTree);
    // /// <summary> Remove the given <paramref name="sequence"/> only if it exists </summary> 
    // /// <returns> true if the whole <paramref name="sequence"/> was found, then removed </returns>
    // public bool Remove([MinLength(1)] params TValue[] sequence) {
    //     if (sequence is null || sequence.Length == 0)
    //         throw new ArgumentOutOfRangeException(nameof(sequence));

    //     NaryTreeNode<TValue> node = this;
    //     Stack<NaryTreeNode<TValue>> stack = new();

    //     foreach (TValue value in sequence) {
    //         NaryTreeNode<TValue>? nodeChild = node.TryGetChild(value);
    //         if (nodeChild is null) return false;
    //         stack.Push(node);
    //         node = nodeChild;
    //     }
    //     if (stack.Count == 0) return false;
    //     var last = stack.Peek();
    //     //! important: the last node must be a sequence end
    //     if (!last.IsSequenceEnd) return false;

    //     // Remove each child in the stack
    //     while (stack.Count > 0) {
    //         NaryTreeNode<TValue> targetNode = stack.Pop();
    //         TValue target = sequence[stack.Count - 1];
    //         bool removed = targetNode.Remove(target);
    //         if (!removed) return false;
    //     }

    //     return true;
    // }
    // /// <summary> Remove the given <paramref name="sequence"/> only if it exists </summary> 
    // /// <returns> true if the whole <paramref name="sequence"/> was found, then removed </returns>
    // public bool Remove(IEnumerable<TValue> sequence)
    //     => Remove(sequence.ToArray());


    /// <summary> Clear children (<see cref="NaryTreeNode{TValue}"/>s under this node) </summary>
    public void ClearChildren() => Children.Clear();


    /// <summary> Calls <see cref="Contains(IEnumerable{TValue})"/> </summary> 
    public bool this[IEnumerable<TValue> sequence] => Contains(sequence);
    /// <summary> Calls <see cref="Contains(NaryTreeNode{TValue})"/> </summary> 
    public bool this[NaryTreeNode<TValue> tree] => Contains(tree);
    /// <summary> Calls <see cref="TryGetChild(TValue)"/> </summary>
    public NaryTreeNode<TValue>? this[TValue value] => TryGetChild(value);


    /// <summary> Calls <see cref="Add(TValue)"/> </summary> 
    public static NaryTreeNode<TValue> operator +(NaryTreeNode<TValue> tree, TValue value) {
        tree.Add(value);
        return tree;
    }
    /// <summary> Calls <see cref="Add(NaryTreeNode{TValue}, bool)"/> </summary> 
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
    /// <summary> Calls <see cref="Remove(NaryTreeNode{TValue})"/> </summary> 
    public static NaryTreeNode<TValue> operator -(NaryTreeNode<TValue> tree, NaryTreeNode<TValue> childTree) {
        tree.Remove(childTree);
        return tree;
    }
    // /// <summary> Calls <see cref="Remove(IEnumerable{TValue})"/> </summary> 
    // public static NaryTreeNode<TValue> operator -(NaryTreeNode<TValue> tree, IEnumerable<TValue> sequence) {
    //     tree.Remove(sequence);
    //     return tree;
    // }

    /// <summary> Get the value of a given <paramref name="tree"/> 
    /// <para> ⚠️ Data loss warning: you will get <see cref="Value"/> and lose the <paramref name="tree"/> along with its branches! </para>
    /// </summary>
    public static explicit operator TValue(NaryTreeNode<TValue> tree)
        => tree.Value;
    /// <summary> Generate a new <see cref="NaryTreeNode{TValue}"/> with the provided <paramref name="value"/> as the root value </summary>
    public static explicit operator NaryTreeNode<TValue>(TValue value)
        => new(value);
}
