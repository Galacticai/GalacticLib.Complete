using GalacticLib.Timing;

[TestFixture]
public class TaskQueueTests2 {
    // Test case 1
    [Test]
    public async Task Initialize_TaskQueueWithTaskAndMaxDuration_SuccessfullyInitialized() {
        // Arrange
        TaskQueue<int, string> taskQueue = new((key) => key.ToString(), 100);

        // Act
        // No explicit action is required for this test case

        // Assert
        Assert.That(taskQueue is not null);
    }

    // Test case 2
    [Test]
    public async Task AddTasksWithSimulatedDelays_TasksRunSimultaneouslyAndEndInOrder() {
        // Arrange
        TaskQueue<int, string> taskQueue = new(SimulatedDelayTask, 100);

        var tasksAdded = new List<int>();
        var tasksStarted = new List<int>();
        var tasksDone = new List<int>();

        taskQueue.TaskAdded += (key) => tasksAdded.Add(key);
        taskQueue.TaskStarted += (key) => tasksStarted.Add(key);
        taskQueue.TaskDone += (key, value) => tasksDone.Add(key);

        // Act
        // Add tasks with simulated delays
        taskQueue.AddRun(1);
        taskQueue.AddRun(2);
        taskQueue.AddRun(3);

        // Allow some time for tasks to complete
        await Task.Delay(500);

        // Assert
        Assert.That(tasksAdded.Count, Is.EqualTo(3), "All tasks should be added");
        Assert.That(tasksStarted.Count, Is.EqualTo(3), "All tasks should be started");
        Assert.That(tasksDone.Count, Is.EqualTo(3), "All tasks should be done");

        // Check if tasks are done in the correct order
        Assert.That(tasksDone[0], Is.EqualTo(1), "First task should be done");
        Assert.That(tasksDone[1], Is.EqualTo(2), "Second task should be done");
        Assert.That(tasksDone[2], Is.EqualTo(3), "Third task should be done");
    }

    private string? SimulatedDelayTask(int key) {
        Thread.Sleep(key * 100);
        return key.ToString();
    }

}
