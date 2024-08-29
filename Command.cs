using System.Diagnostics;
using System.Text;

namespace GalacticLib;

/// <summary>
/// Building blocks for a command
/// <para> Example: "target --arg1 value1 -a2 value2.1 value2.2" </para>
/// </summary>
/// <param name="target"> The target executable </param>
/// <param name="args"> The arguments for the executable </param>
public class Command(
        string target,
        params Command.Argument[] args
) {
    public string Target { get; } = target;
    public List<Argument> Args { get; } = [.. args];

    public Command(string target, params string[] argsWithKeys)
            : this(
                  target,
                  argsWithKeys.Select(arg => new Argument(arg)).ToArray()
             ) { }

    public Command(string full)
            : this(full[..full.IndexOf(' ')], full[(full.IndexOf(' ') + 1)..]) { }

    public string ArgsOnly
        => string.Join(" ", Args.Select(a => a.ToString()));

    public override string ToString() {
        var argsString = ArgsOnly;
        return $"{Target} {(argsString.Length > 0 ? $" {argsString}" : "")}";
    }

    protected virtual void AddArgument(Argument argument) {
        Args.RemoveAll(a => a.Key == argument.Key);
        Args.Add(argument);
    }

    protected virtual void AddArgument(string key, params string[] value)
        => AddArgument(new Argument(key, value));

    public Process Run()
        => Process.Start(ToString());


    public static Command Parse(string command) {
        var firstEnd = command.IndexOf(' ');
        if (firstEnd < 0 || firstEnd == command.Length) return new(command);
        string target = command[..firstEnd];
        string args = target[(firstEnd + 1)..];
        return new(target, args);
    }

    public class Argument(string key, params string[] values) {
        public string Key { get; } = key;
        public List<string> Values { get; } = [.. values];


        public override string ToString() {
            StringBuilder builder = new();
            builder.Append(Key);
            foreach (var value in Values) {
                builder.Append(' ');
                builder.Append(value.Trim());
            }
            return builder.ToString();
        }
    }


    public const string Prefix = "-";
    public const string DoublePrefix = "--";
    public class Key(string name, string prefix = DoublePrefix) {
        public string Name { get; } = name;
        public string Prefix { get; } = prefix;

        public override string ToString()
            => $"{Prefix}{Name}";

        public static Key FromString(string name, string prefix = DoublePrefix)
            => new(name, prefix);
    }
}
