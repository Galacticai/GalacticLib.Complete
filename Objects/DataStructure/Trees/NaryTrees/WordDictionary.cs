namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees;

public class WordDictionary() : NumberNaryTreeNode<char>('\x02') {
    /// <summary> Check the existence of a sentence</summary>
    /// <returns> true if the whole sentence was found </returns>
    public bool Contains(IEnumerable<IEnumerable<char>> sentence) {
        foreach (var word in sentence)
            if (!base.Contains(word, out _))
                return false;
        return true;
    }
    public bool Contains(params string[] sentence)
        => Contains(sentence as IEnumerable<IEnumerable<char>>);
}