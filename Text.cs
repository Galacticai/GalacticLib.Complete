using System.Text.RegularExpressions;

namespace GalacticLib;
public static partial class Text {
    public static partial class NewLine {
        public const char Unix = '\n';
        public const string Windows = "\r\n";
        public const char OSX9 = '\r';
        public const char OSX = Unix;

        public const string AllRegexString = "[\r\n]";
        [GeneratedRegex(AllRegexString)] public static partial Regex AllRegex();
    }
    public static string ToUnixNewLines(this string text) => NewLine.AllRegex().Replace(text, "\n");
    public static string OneLine(this string text, string separator = "")
        => text.ToUnixNewLines().Replace("\n", separator);
    public static string[] SplitLines(this string text) => text.ToUnixNewLines().Split("\n");


    /// <summary> <see cref="Environment.NewLine"/> </summary>
    public static string n => Environment.NewLine;
    /// <summary> x2 <see cref="Environment.NewLine"/> </summary>
    public static string nn => n + n;

    /// <summary> Ideographic Space (Tab) ( <c>\u3000</c> ) </summary>
    public const char tab = '\u3000';

    /// <summary> Narrow no-break space ( <c>\u202F</c> )
    /// <br/> Example: 64GB -> 64 GB</summary>
    public const char NarrowSpace = ' ';

    /// <summary> Horizontal Ellipses  (3 dots) ( … ) ( <c>\u2026</c> ) </summary>
    public const char Ellipses = '…'; //"\u2026";
    /// <summary> Vertical Ellipses (Vertical 3 dots) ( ︙ ) ( <c>\uFE19</c> ) </summary>
    public const char Ellipses_Vertical = '︙'; //"\uFE19";

    /// <summary> Right single quotation mark ( ’ ) ( <c>\u2019</c> ) </summary>
    public const char Apostrophe = '’'; //"\u2019";

    /// <summary> Curved left double quotation ( “ ) ( <c>\u201C</c> ) </summary>
    public const char DoubleQuotation_Left = '“'; //"\u201C";
    /// <summary> Curved right double quotation ( ” ) ( <c>\u201D</c> ) </summary>
    public const char DoubleQuotation_Right = '”'; //"\u201D";

    /// <summary> Ratio symbol ( ∶ ) ( <c>\u2236</c> ) </summary>
    public const char Ratio = '∶'; //"\u2236";

    /// <summary> Bullet symbol ( • ) ( <c>\u2022</c> )</summary>
    public const char Bullet = '•'; //"\u2022";

    /// <summary> En dash ( – ) ( <c>\u2013</c> ) </summary>
    public const char RangeDash = '–'; //"\u2013";

}
