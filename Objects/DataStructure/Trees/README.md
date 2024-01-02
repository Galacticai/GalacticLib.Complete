# GalacticLib

<a href="https://github.com/Galacticai/GalacticLib.Complete/tree/master/Objects/DataStructure/Trees">
    <img height=24 src="https://img.shields.io/badge/Namespace%20Overview-Trees-white?color=informational&style=flat-square" />
</a>

`GalacticLib`>`Objects`>`DataStructures`>**(`Trees`)**

---

```mermaid
classDiagram
    class IEnumerable {
        >T
        +GetEnumerator() : IEnumerable
        +GetEnumerator() : : IEnumerable(T)
    }

    class IJsonable {
        +ToJson(): JsonNode
    }
    class IJsonableObject {
        >T : IJsonableObject
        +FromJson(json: JsonNode): T
    }

    IJsonable <|-- IJsonableObject

    IEnumerable <|-- Tree
    IJsonable <|-- Tree
    class Tree {
        >TValue : notnull
    }
    Tree <|-- NumberTree
    IJsonableObject <|-- NumberTree
    class ObjectTree {
        >TObject: IJsonableObject
    }
    Tree <|-- ObjectTree
    IJsonableObject <|-- ObjectTree
    class NumberTree {
        >TNumber: INumber
    }
    notnull <|-- TValue
    TValue <|-- TNumber
    IJsonableObject <|-- TObject
    TValue <|-- TObject
    INumber <|-- TNumber

```
