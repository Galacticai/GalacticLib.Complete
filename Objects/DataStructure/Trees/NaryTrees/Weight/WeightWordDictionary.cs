namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees.Weight;

public class WeightWordDictionary() : WeightNumberNaryTreeNode<char>('\x02') {
    /// <summary> Check the existence of a sentence</summary>
    /// <returns> true if the whole sentence was found </returns>
    public bool Contains(IEnumerable<IEnumerable<char>> sentence) {
        foreach (IEnumerable<char> word in sentence)
            if (!Contains(word, out _))
                return false;
        return true;
    }
    public bool Contains(params string[] sentence)
        => Contains(sentence as IEnumerable<IEnumerable<char>>);
}