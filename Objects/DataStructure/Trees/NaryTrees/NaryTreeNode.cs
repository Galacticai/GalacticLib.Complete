using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees;

/// <summary> Data structure representing n-ary tree node </summary>
/// <typeparam name="TValue"> Type of <see cref="Value"/> </typeparam>
/// <param name="value"> This node value </param>
/// <param name="isSequenceEnd"> This node is the end of a sequence </param>
/// <param name="children"> This node children </param>
public abstract class NaryTreeNode<TValue>(
        TValue value,
        bool isSequenceEnd = false,
        Dictionary<TValue, INaryTreeNode<TValue>>? children = null

) : INaryTreeNode<TValue>
where TValue : notnull {

    public virtual TValue Value { get; set; } = value;
    public virtual IDictionary<TValue, INaryTreeNode<TValue>> Children { get; set; }
        = children ?? [];

    public virtual bool IsSequenceEnd { get; set; } = isSequenceEnd;

    public virtual bool IsEnd => Children.Count == 0;


    public virtual INaryTreeNode<TValue> Create(TValue value)
        => Create(value);

    public virtual bool Add(INaryTreeNode<TValue> childTree, bool force = false) {
        if (force) {
            Children[childTree.Value] = childTree;
            return true;
        }
        return Children.TryAdd(childTree.Value, childTree);
    }
    public virtual bool Add(TValue value)
        => Add(Create(value), false);
    public virtual bool Add([MinLength(1)] IEnumerable<TValue> sequence) {
        ArgumentNullException.ThrowIfNull(sequence);

        INaryTreeNode<TValue> node = this;
        bool changed = false;
        foreach (TValue value in sequence) {
            if (node.Contains(value)) {
                node = node.TryGetChild(value)!;
                continue;
            }
            NaryTreeNode<TValue> childNode = (NaryTreeNode<TValue>)Create(value);
            bool added = node.Add(childNode);
            changed = changed || added;
            node = childNode;
        }
        node.IsSequenceEnd = true;
        return changed;
    }


    public virtual bool TryGetValue(TValue value, [MaybeNullWhen(false)] out INaryTreeNode<TValue>? node)
        => Children.TryGetValue(value, out node);
    public virtual INaryTreeNode<TValue>? TryGetChild(TValue value) {
        _ = TryGetValue(value, out INaryTreeNode<TValue>? node);
        return node;
    }


    public virtual bool Contains(TValue value) => Children.ContainsKey(value);
    public virtual bool Contains(INaryTreeNode<TValue> childTree)
        => Children.Values.Contains(childTree);
    public virtual bool Contains(
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

    public virtual bool Remove(TValue value) => Children.Remove(value);
    public virtual bool Remove(INaryTreeNode<TValue> childTree) => Children.Remove(childTree.Value);
    public virtual bool Remove([MinLength(1)] IEnumerable<TValue> sequence) {
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

    public virtual void ClearChildren() => Children.Clear();


    public abstract JsonNode ToJson();
    public virtual JsonObject ToJsonTree() {
        HashSet<INaryTreeNode<TValue>> visitedNodes = [];
        var treeJ = ToJsonTree(this, visitedNodes);
        visitedNodes.Clear();
        return treeJ;
    }
    protected virtual JsonObject ToJsonTree(INaryTreeNode<TValue> node, HashSet<INaryTreeNode<TValue>> visitedNodes) {
        visitedNodes.Add(node);
        JsonObject treeJ = [.. node.ToJson().AsObject()];

        if (node.IsEnd) return treeJ;

        JsonArray childrenJ = [];

        foreach (var childNode in node.Children.Values) {
            if (visitedNodes.Contains(childNode)) {
                visitedNodes.Clear();
                throw new CyclicReferenceException(
                    $"Unable to convert this tree into {nameof(JsonObject)} since it contains cyclic references (Continuing would cause {nameof(StackOverflowException)})"
                );
            }
            JsonObject childJ = ToJsonTree(childNode, visitedNodes);
            childrenJ.Add(childJ);
        }

        treeJ.Add(nameof(Children), childrenJ);

        return treeJ;
    }

    protected static IEnumerable<INaryTreeNode<TValue>> Traverse(INaryTreeNode<TValue>? node) {
        if (node is null) yield break;
        yield return node;
        foreach (var childNode in node.Children.Values)
            foreach (var child in Traverse(childNode))
                yield return child;
    }
    public virtual IEnumerator<INaryTreeNode<TValue>> GetEnumerator()
        => Traverse(this).GetEnumerator();


    public virtual bool this[IEnumerable<TValue> sequence] => Contains(sequence, out _);
    public virtual bool this[INaryTreeNode<TValue> tree] => Contains(tree);
    public virtual INaryTreeNode<TValue>? this[TValue value] => TryGetChild(value);


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
    // /// <summary> Generate a new <see cref="NaryTreeNode{TValue}"/> with the provided <paramref name="value"/> as the root value </summary>
    // public static explicit operator NaryTreeNode<TValue>(TValue value)
    //     => new(value);
}
