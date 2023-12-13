

using GalacticLib._Test.Timing;

namespace GalacticLib._Test;

public class Program {
    public static async Task Main(string[] args) {
        var tests1 = new TaskQueueTests();
        tests1.Setup();
        await tests1.TestTaskQueue();
        tests1.TearDown();
        Console.WriteLine(tests1);
    }
}