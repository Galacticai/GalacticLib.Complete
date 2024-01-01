using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees;

/// <summary> Data structure representing n-ary tree node with support for <see cref="object"/>s that inherit <see cref="IJsonable{TValue}"/> </summary>
/// <typeparam name="TObject"> <see cref="object"/> type of <see cref="Value"/> that inherits <see cref="IJsonable{TValue}"/> </typeparam>
/// <param name="value"> This node value </param>
/// <param name="isSequenceEnd"> This node is the end of a sequence </param>
/// <param name="children"> This node children </param>
public class ObjectNaryTreeNode<TObject>(
        TObject value,
        bool isSequenceEnd = false,
        Dictionary<TObject, INaryTreeNode<TObject>>? children = null

) : NaryTreeNode<TObject>(
        value,
        isSequenceEnd,
        children

), IJsonable<ObjectNaryTreeNode<TObject>>
where TObject : notnull, IJsonable<TObject> {

    public override JsonNode ToJson() => new JsonObject() {
        { nameof(Value), Value.ToJson() },
        { nameof(IsSequenceEnd), IsSequenceEnd.ToString() },
    };

    public static ObjectNaryTreeNode<TObject> FromJson(JsonNode json) {
        var valueJ = json[nameof(Value)]!;
        var value = TObject.FromJson((JsonValue)valueJ);
        var isSequenceEnd = json[nameof(IsSequenceEnd)]!.GetValue<bool>();
        return new(value, isSequenceEnd);
    }
}
