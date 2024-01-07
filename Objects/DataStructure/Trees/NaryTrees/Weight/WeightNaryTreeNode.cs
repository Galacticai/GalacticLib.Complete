
using System.Diagnostics.CodeAnalysis;

namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees.Weight;

public abstract class WeightNaryTreeNode<TValue>(
        TValue value,
        bool isSequenceEnd = false,
        long weight = 0L,
        WeightType weightType = WeightType.ReadCount,
        Dictionary<TValue, NaryTreeNode<TValue>>? children = null

) : NaryTreeNode<TValue>(
        value,
        isSequenceEnd,
        children

) where TValue : notnull {

    public WeightType WeightType { get; set; } = weightType;

    /// <summary> Read count of <see cref="Value"/> 
    /// <br/> Automatically gets incremented each time the value is read</summary>  
    public long Weight { get; set; } = weight;

    protected TValue _Value = value;
    public new TValue Value {
        get {
            if (WeightType.HasFlag(WeightType.WriteCount))
                Weight++;
            return _Value;
        }
        set {
            if (WeightType.HasFlag(WeightType.ReadCount))
                Weight++;
            _Value = value;
        }
    }


    public void ResetWeight() => Weight = 0L;
    public void ResetWeightAll() {
        foreach (INaryTreeNode<TValue> node in this) {
            (node as WeightNaryTreeNode<TValue>)!.ResetWeight();
        }
    }

    private new bool Get(TValue value, [MaybeNullWhen(false)] out INaryTreeNode<TValue>? node)
        => base.Get(value, out node);
    public bool TryGetValue(TValue value, [MaybeNullWhen(false)] out WeightNaryTreeNode<TValue>? node) {
        bool found = Get(value, out INaryTreeNode<TValue>? child);
        node = (WeightNaryTreeNode<TValue>?)child;
        return found;
    }
    public new WeightNaryTreeNode<TValue>? Get(TValue value)
        => (WeightNaryTreeNode<TValue>?)base.Get(value);


    public new IEnumerator<WeightNaryTreeNode<TValue>> GetEnumerator() {
        foreach (var node in Traverse(this))
            yield return (WeightNaryTreeNode<TValue>)node;
    }

    /// <summary> Calls <see cref="TryGetChild(TValue)"/> </summary>
    public new WeightNaryTreeNode<TValue>? this[TValue value] => (WeightNaryTreeNode<TValue>?)base.Get(value);


    /// <summary> Calls <see cref="NaryTreeNode{TValue}.Add(TValue)"/> </summary> 
    public static WeightNaryTreeNode<TValue> operator +(WeightNaryTreeNode<TValue> tree, TValue value) {
        tree.Add(value);
        return tree;
    }
    /// <summary> Calls <see cref="NaryTreeNode{TValue}.Add(INaryTreeNode{TValue}, bool)"/> </summary> 
    public static WeightNaryTreeNode<TValue> operator +(WeightNaryTreeNode<TValue> tree, WeightNaryTreeNode<TValue> childTree) {
        tree.Add(childTree);
        return tree;
    }
    /// <summary> Calls <see cref="INaryTreeNode{TValue}.Add(IEnumerable{TValue})"/> </summary> 
    public static WeightNaryTreeNode<TValue> operator +(WeightNaryTreeNode<TValue> tree, IEnumerable<TValue> sequence) {
        tree.Add(sequence);
        return tree;
    }

    /// <summary> Calls <see cref="INaryTreeNode{TValue}.Remove(TValue)"/> </summary> 
    public static WeightNaryTreeNode<TValue> operator -(WeightNaryTreeNode<TValue> tree, TValue value) {
        tree.Remove(value);
        return tree;
    }
    /// <summary> Calls <see cref="INaryTreeNode{TValue}.Remove(INaryTreeNode{TValue})"/> </summary> 
    public static WeightNaryTreeNode<TValue> operator -(WeightNaryTreeNode<TValue> tree, WeightNaryTreeNode<TValue> childTree) {
        tree.Remove(childTree);
        return tree;
    }
    /// <summary> Calls <see cref="INaryTreeNode{TValue}.Remove(IEnumerable{TValue})"/> </summary> 
    public static WeightNaryTreeNode<TValue> operator -(WeightNaryTreeNode<TValue> tree, IEnumerable<TValue> sequence) {
        tree.Remove(sequence);
        return tree;
    }
}

