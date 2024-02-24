using Assets.Scripts.DataStructures.NodeFactory;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace Assets.Scripts.DataStructures
{
	public class TaskEditor
	{
		public TreeNode<TaskData> task;

		private readonly string pathToGlobalTask = "Assets/GlobalTasks/GlobalTask.asset";
		public TaskEditor()
		{
			CheckOrCreateGlobalTask();
		}

		public void CheckOrCreateGlobalTask()
		{
			// Controlla se il task esiste.
			task = AssetDatabase.LoadAssetAtPath<TreeNode<TaskData>>(pathToGlobalTask);
			if (task == null)
			{
				// Crea un nuovo task globale se non esiste.
				task = CreateNode<TaskData, TaskNode>();
				task.name = "GlobalTask";

				// Crea la directory se non esiste
				string directoryPath = Path.GetDirectoryName(pathToGlobalTask);
				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}

				// Salva il nuovo task globale.
				AssetDatabase.CreateAsset(task, pathToGlobalTask);
				AssetDatabase.SaveAssets();
			}
		}



		public K CreateNode<T, K>() where T : NodeData, new() where K : TreeNode<T>, new()
		{
			//TreeNode<T> node = ScriptableObject.CreateInstance<TreeNode<T>>();
			var factory = NodeFactoryImpl.GetFactory<T>();
			K node = factory.CreateNode<K>();


			node.Data = new T();
			node.name = node.Data.GetType().Name.Replace("Data", "");
			node.Data.guid = GUID.Generate().ToString();

			TaskNode dependence = node as TaskNode;
			if (dependence && task)
			{
				task.AddChild(dependence);
			}


			if (task)
			{
				AssetDatabase.AddObjectToAsset(node, task);
			}
			InstructionNode instruction = node as InstructionNode;
			if (instruction)
			{
				instruction.Data.Interaction.data = ScriptableObject.CreateInstance<InteractionData>();
				AssetDatabase.AddObjectToAsset(instruction.Data.Interaction.data, instruction);
				task.Data.instructions.Add(instruction);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return node;
		}

		public void DeleteNode<T>(TreeNode<T> node) where T : NodeData
		{

			TaskNode dependence = node as TaskNode;
			if (dependence)
			{
				task.RemoveChild(dependence);
			}

			InstructionNode instruction = node as InstructionNode;
			if (instruction)
			{
				foreach (var c in instruction.Children)
				{
					c.Parent = null;
				}
				instruction.Parent?.RemoveChild(instruction);

				task.Data.instructions.Remove(instruction);


				AssetDatabase.RemoveObjectFromAsset(instruction.Data.Interaction.data);
			}

			AssetDatabase.RemoveObjectFromAsset(node);
			AssetDatabase.SaveAssets();
		}

		public InstructionNode CreateInstructionNode()
		{
			return CreateNode<InstructionData, InstructionNode>();
		}

		public TaskNode CreateTaskNode()
		{
			return CreateNode<TaskData, TaskNode>(); 
		}
	}
}
#endif
