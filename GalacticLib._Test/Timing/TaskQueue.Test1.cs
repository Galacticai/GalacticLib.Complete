using GalacticLib.Objects;
using GalacticLib.Timing;

namespace GalacticLib._Test.Timing;
[TestFixture]
public class TaskQueueTests {
    TaskQueue<string, int>? Queue { get; }

    private const int ShortDuration = 1000; // milliseconds
    private const int LongDuration = 2500; // milliseconds

    [Test]
    public async Task TestTaskQueue() {
        var taskQueue = new TaskQueue<string, int>(SimulatedTask, LongDuration);

        var task1Key = "Task1";
        var task2Key = "Task2";

        // Test1: Initialize with a task and max duration
        Assert.That(taskQueue is not null);

        if (taskQueue is null) return;
        // Test2: Add tasks with different simulated times using Sleep
        Assert.That(taskQueue.AddWithoutRun(task1Key));
        Assert.That(taskQueue.AddWithoutRun(task2Key));

        // Test3: Check if the tasks are running simultaneously
        Assert.That(taskQueue[task1Key] is FutureValue<int>.Pending);
        Assert.That(taskQueue[task2Key] is FutureValue<int>.Pending);


        _ = taskQueue.RunTask(task1Key).ConfigureAwait(false);
        _ = taskQueue.RunTask(task2Key).ConfigureAwait(false);

        await Task.Delay(200);

        Assert.That(taskQueue[task1Key] is FutureValue<int>.Running);
        Assert.That(taskQueue[task2Key] is FutureValue<int>.Running);

        await Task.Delay(800);

        Console.WriteLine(taskQueue[task1Key]?.GetType().Name);
        Console.WriteLine(taskQueue[task2Key]?.GetType().Name);

        Assert.That(taskQueue[task1Key] is FutureValue<int>.Finished);
        Assert.That(taskQueue[task2Key] is FutureValue<int>.Running);

        await Task.Delay(LongDuration * 2);

        Console.WriteLine(taskQueue[task1Key]?.GetType().Name);
        Console.WriteLine(taskQueue[task2Key]?.GetType().Name);


        Assert.That(taskQueue[task1Key] is null);
        Assert.That(taskQueue[task2Key] is null);

        // Test5: Check if TaskDone event is triggered in order
        Assert.That(taskQueue.Count, Is.EqualTo(0)); // Both tasks are completed
        Assert.That(taskDoneCount, Is.EqualTo(2));


        // Test6: Check if TaskTimedOut event is triggered
        var timeoutTaskKey = "TimeoutTask";
        Assert.That(taskQueue.AddRun(timeoutTaskKey));
        await Task.Delay(LongDuration * 2); // Allow timeout to occur
        Assert.Multiple(() => {
            Assert.That(taskQueue[timeoutTaskKey] is FutureValue<int>.Failed);
            Assert.That(taskTimedOutCount, Is.EqualTo(1));
        });

        // Test7: Check if TaskError event is triggered
        var errorTaskKey = "ErrorTask";
        Assert.That(taskQueue.AddRun(errorTaskKey));
        // Force an error by adding a task that throws an exception
        _ = Task.Run(() => taskQueue.RunTask(errorTaskKey))
            .ConfigureAwait(false);
        await Task.Delay(ShortDuration); // Allow time for the error to be triggered
        Assert.Multiple(() => {
            Assert.That(taskQueue[errorTaskKey] is FutureValue<int>.Failed);
            Assert.That(taskErrorCount, Is.EqualTo(1));
        });
    }


    private void TaskAdded(string key) {
        Console.WriteLine($" >> TaskAdded: {key}");
    }
    private void TaskStarted(string key) {
        Console.WriteLine($" >> TaskStarted: {key}");
    }
    private void TaskDone(string key, int value) {
        taskDoneCount++;
        Console.WriteLine($" >> TaskDone: {key} => Value={value}");
    }
    private void TaskTimedOut(string key, int duration) {
        taskTimedOutCount++;
        Console.WriteLine($" >> TaskTimedOut: {key} -- Duration={duration}");
    }
    private void TaskError(string key, Exception exception) {
        taskErrorCount++;
        Console.WriteLine($" >> TaskError: {key} -- Exception={exception.GetType().Name}");
    }


    private static int SimulatedTask(string key) {
        // Simulate different task durations
        int duration = key switch {
            "Task1" => ShortDuration,
            "Task2" => (int)(LongDuration * .9),
            "TimeoutTask" => LongDuration * 2,
            "ErrorTask" => throw new Exception("Simulated error"),
            _ => throw new ArgumentOutOfRangeException(nameof(key))
        };

        Thread.Sleep(duration);
        return duration;
    }

    private int taskDoneCount = 0;
    private int taskTimedOutCount = 0;
    private int taskErrorCount = 0;

    [SetUp]
    public void Setup() {
        taskDoneCount = 0;
        taskTimedOutCount = 0;
        taskErrorCount = 0;

        if (Queue is null) return;
        Queue.TaskAdded += TaskAdded;
        Queue.TaskStarted += TaskStarted;
        Queue.TaskDone += TaskDone;
        Queue.TaskTimedOut += TaskTimedOut;
        Queue.TaskError += TaskError;
    }

    [TearDown]
    public void TearDown() {
        taskDoneCount = 0;
        taskTimedOutCount = 0;
        taskErrorCount = 0;
        if (Queue is null) return;
        Queue.TaskAdded -= TaskAdded;
        Queue.TaskStarted -= TaskStarted;
        Queue.TaskDone -= TaskDone;
        Queue.TaskTimedOut -= TaskTimedOut;
        Queue.TaskError -= TaskError;
    }
}