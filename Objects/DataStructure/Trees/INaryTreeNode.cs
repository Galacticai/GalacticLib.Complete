using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees;

public interface INaryTreeNode<TValue>
        : IEnumerable<INaryTreeNode<TValue>>, IJsonable
        where TValue : notnull {

    /// <summary> Very important to generate the correct child type that will be added into the tree </summary>
    protected INaryTreeNode<TValue> Create(TValue value);

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


    /// <summary> Convert the whole tree to <see cref="JsonObject"/> recursively 
    /// <br/> ⚠️ Cyclic references will cause <see cref="IJsonable.CyclicReferenceException"/> </summary>
    /// <exception cref="IJsonable.CyclicReferenceException" />
    /// <returns> This entire tree as a <see cref="JsonObject"/> </returns>
    public JsonObject ToJsonTree();

    // public  JsonValue ToJson();

    // public  IEnumerator<INaryTreeNode<TValue>> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
/*
classDiagram
    class IEnumerable {
        > T
        +GetEnumerator() : IEnumerable
        +GetEnumerator() : : IEnumerable(T)
    }
    notnull <|-- "is" TValue
    INumber <|-- "inherits" TNumber
    IJsonableObject <|-- "inherits" TObject
    
    class CyclicReferenceException {
        +CyclicReferenceException(message: string?, innerException: Exception?)
    }
    class IJsonable {
        +ToJson(): JsonNode
    } 
    class IJsonableObject {
        >T : IJsonableObject
        +FromJson(json: JsonNode): T
    }
    TValue <|-- "inherits" TNumber
    TValue <|-- "inherits" TObject
    
    IJsonable <|-- "inherits" IJsonableObject
    IJsonable --> CyclicReferenceException

    IEnumerable <|-- "inherits" INaryTreeNode
    IJsonable <|-- "inherits" INaryTreeNode
    class INaryTreeNode {
        >TValue : notnull

        -Create(TValue)

        +Value: TValue: get set
        +Children : : Dictionary(TValue, INaryTreeNode): get set
        +IsSequenceEnd: bool: get set
        +IsEnd: bool: get

        +Add(INaryTreeNode(TValue), bool): bool
        +Add(TValue): bool
        +Add(IEnumerable(TValue)): bool

        +TryGetValue(TValue, out INaryTreeNode(TValue) ?): bool
        +TryGetChild(TValue): INaryTreeNode(TValue)

        +Contains(TValue): bool
        +Contains(INaryTreeNode(TValue)): bool
        +Contains(IEnumerable(TValue), out List(INaryTreeNode(TValue))?): bool
    
        +Remove(INaryTreeNode(TValue), bool): bool
        +Remove(TValue): bool
        +Remove(IEnumerable `TValue): bool

        +ClearChildren()

        +this(IEnumerable(TValue)) : bool: get
        +this(INaryTreeNode(TValue)) : bool: get
        +this(TValue) : : INaryTreeNode(TValue): get

        +ToJsonTree(): JsonObject
    }
    INaryTreeNode <|-- "inherits" NaryTreeNode
    class NaryTreeNode {
        +Traverse : : IEnumerable(INaryTreeNode(TValue))
        +op_Addition(NaryTreeNode(TValue), TValue) : : NaryTreeNode(TValue)
        +op_Addition(NaryTreeNode(TValue), NaryTreeNode(TValue)) : : NaryTreeNode(TValue)
        +op_Addition(NaryTreeNode(TValue), IEnumerable(TValue)) : : NaryTreeNode(TValue)
        +op_Subtraction(NaryTreeNode(TValue), TValue) : : NaryTreeNode(TValue)
        +op_Subtraction(NaryTreeNode(TValue), NaryTreeNode(TValue)) : : NaryTreeNode(TValue)
        +op_Subtraction(NaryTreeNode(TValue), IEnumerable(TValue)) : : NaryTreeNode(TValue)
        +op_Explicit(NaryTreeNode(TValue)): TValue
    }
    NaryTreeNode <|-- "inherits" NumberNaryTreeNode
    IJsonableObject <|-- "inherits" NumberNaryTreeNode
    class ObjectNaryTreeNode {
        >TObject: IJsonableObject
    }
    NaryTreeNode <|-- "inherits" ObjectNaryTreeNode
    IJsonableObject <|-- "inherits" ObjectNaryTreeNode
    class NumberNaryTreeNode {
        >TNumber: INumber
    }
*/