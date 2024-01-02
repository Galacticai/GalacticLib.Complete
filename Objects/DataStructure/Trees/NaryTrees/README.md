# GalacticLib

<a href="https://github.com/Galacticai/GalacticLib.Complete/tree/master/Objects/DataStructure/Trees">
    <img height=24 src="https://img.shields.io/badge/Namespace%20Overview-Trees-white?color=informational&style=flat-square" />
</a>

`GalacticLib`>`Objects`>`DataStructures`>`Trees`>**(`NaryTrees`)**

---

```mermaid
classDiagram
    class IEnumerable {
        >T
        +GetEnumerator() : IEnumerable
        +GetEnumerator() : : IEnumerable(T)
    }
    notnull <|-- TValue
    TValue <|-- TNumber
    IJsonableObject <|-- TObject
    TValue <|-- TObject
    INumber <|-- TNumber

    class IJsonable {
        +ToJson(): JsonNode
    }
    class IJsonableObject {
        >T : IJsonableObject
        +FromJson(json: JsonNode): T
    }

    IJsonable <|-- IJsonableObject

    IEnumerable <|-- INaryTreeNode
    IJsonable <|-- INaryTreeNode
    class INaryTreeNode {
        >TValue : notnull
        ____
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
    INaryTreeNode <|-- NaryTreeNode
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
    NaryTreeNode <|-- NumberNaryTreeNode
    IJsonableObject <|-- NumberNaryTreeNode
    class ObjectNaryTreeNode {
        >TObject: IJsonableObject
    }
    NaryTreeNode <|-- ObjectNaryTreeNode
    IJsonableObject <|-- ObjectNaryTreeNode
    class NumberNaryTreeNode {
        >TNumber: INumber
    }
```
