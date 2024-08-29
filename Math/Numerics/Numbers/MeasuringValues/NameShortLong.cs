using System.Text;

namespace GalacticLib.Math.Numerics.Numbers.Units;
public class NameShortLong(string shorter, string longer, bool preferShorter = true) {
    public string Shorter { get; set; } = shorter;
    public string Longer { get; set; } = longer;
    public bool PreferShorter { get; set; } = preferShorter;

    public override string ToString() => PreferShorter ? Shorter : Longer;

    /// <summary> Empty name (<see cref="string.Empty"/>) (useful for values that don't need a unit postfix) </summary>
    public static NameShortLong Empty => new(string.Empty, string.Empty);

    public static NameShortLong Merge(IEnumerable<NameShortLong> names) {
        StringBuilder shorter = new(), longer = new();
        foreach (var name in names) {
            shorter.Append(name.Shorter);
            longer.Append(name.Longer);
        }
        return new(shorter.ToString(), longer.ToString());
    }

    public static implicit operator NameShortLong((string Shorter, string Longer) tuple)
        => new(tuple.Shorter, tuple.Longer);
    public static implicit operator (string Shorter, string Longer)(NameShortLong name)
        => (name.Shorter, name.Longer);
}
