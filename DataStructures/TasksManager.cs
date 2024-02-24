using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
	public class TasksManager : MonoBehaviour
	{
		public TaskNode rootTask;
		//freemode all'interno del task, currentTask scelto per priorità
		public bool freeMode;

		public Tree<TaskData, TaskNode> tasks = new();

		public List<TaskNode> currentTasks;
		public List<InstructionNode> currentInstructionNodes = new();

		public TaskNode currentTask;
		public InstructionNode currentInstructionNode;

		private void Start()
		{
			EventsMediator.Instance.InstructionEvents += OnInteraction;
			Initialize();
		}

		//singleton approach
		static TasksManager m_Instance;
		public static TasksManager Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = FindObjectOfType<TasksManager>();
				if (m_Instance == null)
				{
					var go = new GameObject("TasksManager");
					m_Instance = go.AddComponent<TasksManager>();
				}
				return m_Instance;
			}
			private set { }
		}



		public void Initialize()
		{
			tasks.Root = rootTask;
			currentTasks = tasks.GetLeafNodes();
			currentTasks.ForEach(t => t.Data.Initialize());

			if (currentTasks.Any())
			{
				currentTask = currentTasks.Min();
				//popolo currentInstructionNodes con le istruzioni soddisfacibili
				//del tak corrente
				//gestire task vuoti
				currentTask.Data.instructions.ForEach(it =>
				{
					if (it.Children.Count == 0)
					{
						currentInstructionNodes.Add(it);
						it.Initialize();
					}

				});

				//ne seleziono quella con priorità più alta
				if(currentInstructionNodes.Any())
					currentInstructionNodes.Min();

			}
		}

		private void OnInstruction(object sender, InstructionNode instruction)
		{
			switch (freeMode)
			{
				case true:
					ResolveInstruction(instruction);
					break;
				case false:
					if (currentInstructionNode.Data.Equals(instruction.Data))
					{
						ResolveInstruction(currentInstructionNode);
					}
					break;
			}
		}

		public void ResolveInstruction(InstructionNode instructionNode)
		{
			instructionNode.Data.IsResolved = true;
			currentInstructionNodes.Remove(instructionNode);
			print($"{instructionNode.Data.Title} is resolved");
			var parent = instructionNode.Parent as InstructionNode;
			//one instruction tree finished, we arrived at a root
			if (parent == null)
			{
				//there are other instruction trees in current task
				if (currentInstructionNodes.Count > 0)
				{
					currentInstructionNode = currentInstructionNodes.Min();
					return;
				}
				currentTask.Data.IsResolved = true;
				currentTasks.Remove(currentTask);
				print($"{currentTask.Data.Title} is resolved");
				var parentTask = currentTask.Parent as TaskNode;

				if (parentTask != null && parentTask.Children.All(t => t.Data.IsResolved))
				{
					parentTask.Data.Initialize();
					currentTasks.Add(parentTask);
				}

				if (currentTasks.Count > 0)
				{
					currentTask = currentTasks.Min();
					print($"Current task is {currentTask.Data.Title} ");
					currentTask.Data.instructions.ForEach(it =>
					{
						if (it.Children.Count == 0)
							currentInstructionNodes.Add(it);
					});
					currentInstructionNode = currentInstructionNodes.Min();
				}
				else
				{
					print($"Tasks completed");
				}

			}
			else
			{
				if (parent.Children.All(c => c.Data.IsResolved))
				{
					currentInstructionNodes.Add(parent);
				}
				currentInstructionNode = currentInstructionNodes.Min();
			}
		}

		public void OnInteraction(object sender, InstructionNode interaction)
		{
			if (interaction != null && currentInstructionNodes.Contains(interaction))
			{
				print($"Interaction linked to: {interaction.Data.Title} is resolved");
				OnInstruction(this, interaction);
			}
		}
	}
}
