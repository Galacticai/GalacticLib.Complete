using GalacticLib.Objects;

namespace GalacticLib.Objects.DataStructure.Trees;

public interface ITreeNode<TValue>
        : IEnumerable<ITreeNode<TValue>>, IJsonable
        where TValue : notnull {
    public TValue Value { get; set; }
}