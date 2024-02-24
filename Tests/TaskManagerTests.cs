using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Assets.Scripts.DataStructures
{
	public class TasksManagerTests
	{

		// A Test behaves as an ordinary method
		[Test]
		public void TaskManagerSimplePasses()
		{
			// Use the Assert class to test conditions
		}

		// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
		// `yield return null;` to skip a frame.
		[UnityTest]
		public IEnumerator TaskManagerWithEnumeratorPasses()
		{
			// Use the Assert class to test conditions.
			// Use yield to skip a frame.
			yield return null;
		}

		private TasksManager tasksManager;
		private EventsMediator eventsMediator;

		[SetUp]
		public void Setup()
		{
			tasksManager = new GameObject().AddComponent<TasksManager>();
			eventsMediator = new GameObject().AddComponent<EventsMediator>();
		}

		[Test]
		public void Initialize_ShouldSetCurrentTaskAndCurrentInstructionNode()
		{
			// Arrange
			TaskNode rootTask = new TaskNode();
			tasksManager.rootTask = rootTask;

			TaskNode task1 = new TaskNode();
			TaskNode task2 = new TaskNode();
			rootTask.AddChild(task1);
			rootTask.AddChild(task2);

			InstructionNode instruction1 = new InstructionNode();
			InstructionNode instruction2 = new InstructionNode();
			task1.Data.instructions.Add(instruction1);
			task1.Data.instructions.Add(instruction2);

			instruction1.Data = new InstructionData { Title = "Instruction 1" };
			instruction1.Data.Interaction.data = new Scripts.InteractionData();
			instruction2.Data = new InstructionData { Title = "Instruction 2" };
			instruction2.Data.Interaction.data = new Scripts.InteractionData();

			// Act
			tasksManager.Initialize();

			// Assert
			Assert.AreEqual(task1, tasksManager.currentTask);
			Assert.AreEqual(instruction1, tasksManager.currentInstructionNode);
		}

		[Test]
		public void ResolveInstruction_ShouldResolveInstructionAndSetNextInstruction()
		{
			// Arrange
			TaskNode rootTask = new TaskNode();
			tasksManager.rootTask = rootTask;

			TaskNode task1 = new TaskNode();
			rootTask.AddChild(task1);

			InstructionNode instruction1 = new InstructionNode();
			InstructionNode instruction2 = new InstructionNode();
			task1.Data.instructions.Add(instruction1);
			task1.Data.instructions.Add(instruction2);

			instruction1.Data = new InstructionData { Title = "Instruction 1" };
			instruction1.Data.Interaction.data = new Scripts.InteractionData();
			instruction2.Data = new InstructionData { Title = "Instruction 2" };
			instruction2.Data.Interaction.data = new Scripts.InteractionData();

			tasksManager.Initialize();

			// Act
			tasksManager.ResolveInstruction(instruction1);

			// Assert
			Assert.IsTrue(instruction1.Data.IsResolved);
			Assert.IsFalse(tasksManager.currentInstructionNodes.Contains(instruction1));
			Assert.AreEqual(instruction2, tasksManager.currentInstructionNode);
		}

		[Test]
		public void ResolveInstruction_ShouldResolveTaskAndSetNextTask()
		{
			// Arrange
			TaskNode rootTask = new TaskNode();
			tasksManager.rootTask = rootTask;

			TaskNode task1 = new TaskNode();
			TaskNode task2 = new TaskNode();
			rootTask.AddChild(task1);
			rootTask.AddChild(task2);

			InstructionNode instruction1 = new InstructionNode();
			InstructionNode instruction2 = new InstructionNode();
			task1.Data.instructions.Add(instruction1);
			task1.Data.instructions.Add(instruction2);

			instruction1.Data = new InstructionData { Title = "Instruction 1" };
			instruction1.Data.Interaction.data = new Scripts.InteractionData();
			instruction2.Data = new InstructionData { Title = "Instruction 2" };
			instruction2.Data.Interaction.data = new Scripts.InteractionData();

			tasksManager.Initialize();

			// Act
			tasksManager.ResolveInstruction(instruction1);
			tasksManager.ResolveInstruction(instruction2);

			// Assert
			Assert.IsTrue(task1.Data.IsResolved);
			Assert.IsFalse(tasksManager.currentTasks.Contains(task1));
			Assert.AreEqual(task2, tasksManager.currentTask);
		}

		[Test]
		public void ResolveInstruction_ShouldSetNextInstructionNodeInSameTask()
		{
			// Arrange
			TaskNode rootTask = new TaskNode();
			tasksManager.rootTask = rootTask;

			TaskNode task1 = new TaskNode();
			rootTask.AddChild(task1);

			InstructionNode instruction1 = new InstructionNode();
			InstructionNode instruction2 = new InstructionNode();
			task1.Data.instructions.Add(instruction1);
			task1.Data.instructions.Add(instruction2);

			instruction1.Data = new InstructionData { Title = "Instruction 1" };
			instruction1.Data.Interaction.data = new Scripts.InteractionData();
			instruction2.Data = new InstructionData { Title = "Instruction 2" };
			instruction2.Data.Interaction.data = new Scripts.InteractionData();

			tasksManager.Initialize();

			// Act
			tasksManager.ResolveInstruction(instruction1);

			// Assert
			Assert.IsFalse(tasksManager.currentInstructionNodes.Contains(instruction1));
			Assert.AreEqual(instruction2, tasksManager.currentInstructionNode);
		}

		[Test]
		public void ResolveInstruction_ShouldSetNextInstructionNodeInParentTask()
		{
			// Arrange
			TaskNode rootTask = new TaskNode();
			tasksManager.rootTask = rootTask;

			TaskNode task1 = new TaskNode();
			TaskNode task2 = new TaskNode();
			rootTask.AddChild(task1);
			rootTask.AddChild(task2);

			InstructionNode instruction1 = new InstructionNode();
			InstructionNode instruction2 = new InstructionNode();
			task1.Data.instructions.Add(instruction1);
			task2.Data.instructions.Add(instruction2);

			instruction1.Data = new InstructionData { Title = "Instruction 1" };
			instruction1.Data.Interaction.data = new Scripts.InteractionData();
			instruction2.Data = new InstructionData { Title = "Instruction 2" };
			instruction2.Data.Interaction.data = new Scripts.InteractionData();

			tasksManager.Initialize();

			// Act
			tasksManager.ResolveInstruction(instruction1);

			// Assert
			Assert.IsFalse(tasksManager.currentInstructionNodes.Contains(instruction1));
			Assert.AreEqual(instruction2, tasksManager.currentInstructionNode);
		}

		[Test]
		public void OnInteraction_ShouldResolveInstruction()
		{
			// Arrange
			TaskNode rootTask = new TaskNode();
			tasksManager.rootTask = rootTask;

			TaskNode task1 = new TaskNode();
			rootTask.AddChild(task1);

			InstructionNode instruction1 = new InstructionNode();
			InstructionNode instruction2 = new InstructionNode();
			task1.Data.instructions.Add(instruction1);
			task1.Data.instructions.Add(instruction2);

			instruction1.Data = new InstructionData { Title = "Instruction 1" };
			instruction1.Data.Interaction.data = new Scripts.InteractionData();
			instruction2.Data = new InstructionData { Title = "Instruction 2" };
			instruction2.Data.Interaction.data = new Scripts.InteractionData();

			tasksManager.Initialize();

			// Act
			tasksManager.OnInteraction(this, instruction1);

			// Assert
			Assert.IsTrue(instruction1.Data.IsResolved);
			Assert.IsFalse(tasksManager.currentInstructionNodes.Contains(instruction1));
		}
	}
}