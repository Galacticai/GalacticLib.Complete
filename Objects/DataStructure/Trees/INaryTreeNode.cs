using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GalacticLib.Objects.DataStructure.Trees;

public interface INaryTreeNode<TValue> : IEnumerable<TValue> where TValue : notnull {

    public TValue Value { get; set; }
    public IDictionary<TValue, INaryTreeNode<TValue>> Children { get; set; }

    /// <summary> End of sequence (could have children that are for other sequences) 
    /// <br/> Example: ABC ... ABCDEF </summary>
    public bool IsSequenceEnd { get; set; }

    /// <summary> End of tree (no children) </summary>
    public bool IsEnd { get; }

    /// <summary> Add a child tree </summary>
    /// <returns> true if added </returns>
    public bool Add(INaryTreeNode<TValue> childTree, bool force = false);
    /// <summary> Add a new tree with the given <paramref name="value"/> </summary> 
    /// <returns> true if it did not exist and was added </returns>
    public bool Add(TValue value);
    /// <summary> Add a given <paramref name="sequence"/> </summary> 
    /// <returns> true if the tree changed </returns>
    public bool Add([MinLength(1)] IEnumerable<TValue> sequence);


    public bool TryGetValue(
            TValue value,
            [MaybeNullWhen(false)] out INaryTreeNode<TValue>? node
    );
    public INaryTreeNode<TValue>? TryGetChild(TValue value);


    /// <summary> Check if a direct child has the given <paramref name="value"/> (Only checks 1 level of children) </summary>
    /// <returns> true if <paramref name="value"/> was found </returns>
    public bool Contains(TValue value);
    /// <summary> Check if this node has the given <paramref name="childTree"/> (Only checks 1 level of children) </summary>
    /// <returns> true if <paramref name="childTree"/> was found </returns>
    public bool Contains(INaryTreeNode<TValue> childTree);

    /// <summary> Checks if a <paramref name="sequence"/> exists under this node (Goes deeper in the tree equal to the sequence length) 
    /// <br/> ⚠️ The <paramref name="sequence"/> starts from the children, not from this node </summary>
    /// <param name="sequence"> Sequence of <typeparamref name="TValue"/> </param>
    /// <param name="nodes"> List of nodes if they were found, otherwise null </param>
    /// <returns> true if the <paramref name="sequence"/> was found </returns>
    public bool Contains(
            [MinLength(1)] IEnumerable<TValue> sequence,
            [MaybeNullWhen(false)] out List<INaryTreeNode<TValue>>? nodes
    );

    /// <summary> Remove a child that has the given <paramref name="value"/> </summary>
    /// <returns> true if <paramref name="value"/> was found and removed </returns>
    public bool Remove(TValue value);
    /// <summary> Remove a the given <paramref name="childTree"/> </summary>
    /// <returns> true if the <paramref name="childTree"/> was found, then removed </returns>
    public bool Remove(INaryTreeNode<TValue> childTree);
    /// <summary> Remove the given <paramref name="sequence"/> only if it exists </summary> 
    /// <returns> true if the whole <paramref name="sequence"/> was found, then removed </returns>
    public bool Remove([MinLength(1)] IEnumerable<TValue> sequence);

    /// <summary> Clear children (<see cref="INaryTreeNode{TValue}"/>s under this node) </summary>
    public void ClearChildren();


    /// <summary> Calls <see cref="Contains(IEnumerable{TValue}, out List{INaryTreeNode{TValue}})"/>
    /// <br/> (and discards the list of nodes that were found) </summary> 
    public bool this[IEnumerable<TValue> sequence] { get; }
    /// <summary> Calls <see cref="Contains(INaryTreeNode{TValue})"/> </summary> 
    public bool this[INaryTreeNode<TValue> tree] { get; }
    /// <summary> Calls <see cref="TryGetChild(TValue)"/> </summary>
    public INaryTreeNode<TValue>? this[TValue value] { get; }
}
