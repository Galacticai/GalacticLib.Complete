namespace GalacticLib.Objects.DataStructure.Trees.NaryTrees.Weight;

/// <summary> The way <see cref="Weight"/> is incremented </summary>
public enum WeightType {
    /// <summary> No auto processing </summary>
    Manual = 0,
    /// <summary> Increment each time <see cref="Value"/> get accessor is called</summary>
    ReadCount = 1,
    /// <summary> Increment each time <see cref="Value"/> set accessor is called</summary>
    WriteCount = 2,
    /// <summary> <see cref="ReadCount"/> or <see cref="WriteCount"/> </summary>
    ReadWriteCount = ReadCount | WriteCount,
}
