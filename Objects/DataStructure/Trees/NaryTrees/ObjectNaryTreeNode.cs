using System.Text.Json.Nodes;

namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees;

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

    public static ObjectNaryTreeNode<TObject>? FromJson(JsonNode json) {
        if (json is not JsonObject jsonO)
            throw new ArgumentException($"The provided json is not a {nameof(JsonObject)}");

        var valueProp = json[nameof(Value)];
        PropertyNotFoundException.ThrowIfNull(valueProp, nameof(Value));

        if (valueProp is not JsonValue valueJ)
            throw new ArgumentException($"Value type is invalid. Expected {nameof(JsonValue)}, Found {valueProp!.GetType().Name}");

        TObject? value = TObject.FromJson(valueProp);
        ArgumentNullException.ThrowIfNull(value, nameof(Value));

        var isSequenceEndProp = json[nameof(IsSequenceEnd)];
        PropertyNotFoundException.ThrowIfNull(isSequenceEndProp, nameof(IsSequenceEnd));

        bool isSequenceEnd = isSequenceEndProp!.GetValue<bool>();

        return new(value, isSequenceEnd);
    }
}
