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
        IDictionary<TValue, NaryTreeNode<TValue>>? children = null

) : INaryTreeNode<TValue>
where TValue : notnull {

    public virtual TValue Value { get; set; } = value;

    public virtual INaryTreeNode<TValue>? Parent => _Parent;
    protected NaryTreeNode<TValue>? _Parent;

    protected virtual IDictionary<TValue, NaryTreeNode<TValue>> Children { get; set; }
        = children ?? new Dictionary<TValue, NaryTreeNode<TValue>>();

    public virtual bool IsSequenceEnd { get; set; } = isSequenceEnd;

    public virtual bool IsEnd => Children.Count == 0;

    /// <summary> Very important to instantiate the correct child type </summary> 
    protected abstract NaryTreeNode<TValue> Create(TValue value);

    public virtual bool Add(INaryTreeNode<TValue> childTree, bool force = false) {
        if (childTree is not NaryTreeNode<TValue> nary)
            throw new ArgumentException($"Child tree is not a {nameof(NaryTreeNode<TValue>)}", nameof(childTree));

        if (force) {
            nary._Parent = this;
            Children[nary.Value] = nary;
            return true;
        }
        return Children.TryAdd(nary.Value, nary);
    }
    public virtual bool Add(TValue value)
        => Add(Create(value), false);
    public virtual bool Add([MinLength(1)] IEnumerable<TValue> sequence) {
        ArgumentNullException.ThrowIfNull(sequence);

        INaryTreeNode<TValue> node = this;
        bool changed = false;
        foreach (TValue value in sequence) {
            if (node.Contains(value)) {
                node = node.Get(value)!;
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


    public virtual bool Get(TValue value, [MaybeNullWhen(false)] out INaryTreeNode<TValue>? node) {
        bool found = Children.TryGetValue(value, out var child);
        node = child; //? Cant "out node" directly, but can implictly cast here "node = child"
        return found;
    }
    public virtual INaryTreeNode<TValue>? Get(TValue value) {
        _ = Get(value, out INaryTreeNode<TValue>? node);
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
            NaryTreeNode<TValue>? childNode = (NaryTreeNode<TValue>?)node.Get(value);
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

    public virtual bool Remove(INaryTreeNode<TValue> childTree) {
        if (childTree.Parent != this)
            throw new ArgumentException("Child tree must be a child of this tree", nameof(childTree));

        var nary = (NaryTreeNode<TValue>)childTree;
        bool removed = Children.Remove(nary.Value);
        if (removed) nary._Parent = null;
        return removed;
    }
    public virtual bool Remove(TValue value) {
        if (Get(value) is not INaryTreeNode<TValue> child)
            return false;
        //? Must use this so it checks parent/child relationship
        return Remove(child);
    }
    public virtual bool Remove([MinLength(1)] IEnumerable<TValue> sequence) {
        ArgumentNullException.ThrowIfNull(sequence);

        bool found = Contains(sequence, out List<INaryTreeNode<TValue>>? nodes);
        if (!found) return false;

        for (int i = nodes!.Count - 1; i > 0; i--) {
            var child = nodes[i];
            if (child.Parent is null)
                throw new NullReferenceException("BUG: Parent should not be null in this context");
            child.Parent.Remove(child);
        }
        Remove(nodes[0]);

        return true;
    }

    public virtual void ClearChildren() {
        foreach (var (_, child) in Children) Remove(child);
    }


    public abstract JsonNode ToJson();
    public virtual JsonObject ToJsonTree() {
        HashSet<NaryTreeNode<TValue>> visitedNodes = [];
        JsonObject treeJ = ToJsonTree(this, visitedNodes);
        visitedNodes.Clear(); //? Explicitly avoid memory leak
        return treeJ;
    }
    protected virtual JsonObject ToJsonTree(NaryTreeNode<TValue> node, HashSet<NaryTreeNode<TValue>> visitedNodes) {
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

    protected static IEnumerable<INaryTreeNode<TValue>> Traverse(NaryTreeNode<TValue>? node) {
        if (node is null) yield break;
        yield return node;
        foreach (var childNode in node.Children.Values)
            foreach (var child in Traverse(childNode))
                yield return child;
    }
    public virtual IEnumerator<ITreeNode<TValue>> GetEnumerator()
        => Traverse(this).GetEnumerator();

    public virtual bool this[IEnumerable<TValue> sequence] => Contains(sequence, out _);
    public virtual bool this[INaryTreeNode<TValue> tree] => Contains(tree);
    public virtual INaryTreeNode<TValue>? this[TValue value] => Get(value);


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
