using Assets.Scripts;
using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskGraphView : GraphView
{
	public Action<NodeView<InstructionData>> OnInstructionSelection;
	public Action<NodeView<TaskData>> OnTaskSelection;
	//hides UxmlFactory inherited from VisualElement to make this custom controls appear in UI builder
	public new class UxmlFactory : UxmlFactory<TaskGraphView, GraphView.UxmlTraits> { }
	readonly TaskEditor taskEditor;

	private TreeNode<TaskData> lastTask;
	public TaskGraphView()
	{
		taskEditor = new TaskEditor();
		lastTask = taskEditor.task;
		Insert(0, new GridBackground());
		var selectionDragger = new SelectionDragger();
		this.AddManipulator(new ContentDragger());
		this.AddManipulator(new ContentZoomer());
		this.AddManipulator(selectionDragger);
		this.AddManipulator(new RectangleSelector());
		var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/SRC/MyEditor/TaskGraphEditor.uss");
		styleSheets.Add(styleSheet);
	}

	NodeView<T> FindNodeView<T>(TreeNode<T> node) where T : NodeData
	{
		return GetNodeByGuid(node.Data.guid) as NodeView<T>;
	}
	public void UpdateGraph()
	{
		PopulateView(lastTask);
	}
	internal void PopulateView(TreeNode<TaskData> task)
	{
		lastTask = task;
		this.taskEditor.task = task;
		//ignore previous events
		graphViewChanged -= OnGraphViewChanged;
		DeleteElements(graphElements);
		graphViewChanged += OnGraphViewChanged;
		foreach (var node in taskEditor.task.Children)
		{
			CreateNodeView(node);
		}
		task.Data.instructions.ForEach(i => CreateNodeView(i));

		foreach (var t in taskEditor.task.Children)
		{
			var dependencies = t.Children;
			foreach (var d in dependencies)
			{
				if (d != null)
				{
					NodeView<TaskData> dependentView = FindNodeView(t);
					NodeView<TaskData> dependenceView = FindNodeView(d);

					Edge edge = dependentView.output.ConnectTo(dependenceView.input);
					AddElement(edge);
				}

			}
		}
		foreach (var i in taskEditor.task.Data.instructions)
		{
			var dependencies = i.Children;
			foreach (var d in dependencies)
			{
				if (d != null)
				{
					NodeView<InstructionData> dependentView = FindNodeView(i);
					NodeView<InstructionData> dependenceView = FindNodeView(d);

					Edge edge = dependentView.output.ConnectTo(dependenceView.input);
					AddElement(edge);
				}

			}
		}
	}


	public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
	{
		return ports.ToList().Where(endPort =>
		endPort.direction != startPort.direction &&
		endPort.node != startPort.node).ToList();
	}

	private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
	{
		graphViewChange.elementsToRemove?.ForEach(elem =>
			{
				NodeView<TaskData> taskView = elem as NodeView<TaskData>;
				RemoveNodeView(taskView);
				NodeView<InstructionData> instructionView = elem as NodeView<InstructionData>;
				RemoveNodeView(instructionView);

				if (elem is Edge edge)
				{
					NodeView<TaskData> outputTask = edge.output.node as NodeView<TaskData>;
					NodeView<TaskData> inputTask = edge.input.node as NodeView<TaskData>;
					RemoveEdge(outputTask, inputTask);

					NodeView<InstructionData> outputInstruction = edge.output.node as NodeView<InstructionData>;
					NodeView<InstructionData> inputInstruction = edge.input.node as NodeView<InstructionData>;
					RemoveEdge(outputInstruction, inputInstruction);
				}
			});
		List<Edge> toNotCreate = new();
		graphViewChange.edgesToCreate?.ForEach(edge =>
			{
				NodeView<TaskData> dependentView = edge.output.node as NodeView<TaskData>;
				NodeView<TaskData> dependenceView = edge.input.node as NodeView<TaskData>;
				bool inserted = InsertEdge(dependentView, dependenceView);


				NodeView<InstructionData> dependentViewIn = edge.output.node as NodeView<InstructionData>;
				NodeView<InstructionData> dependenceViewIn = edge.input.node as NodeView<InstructionData>;
				inserted = inserted ? inserted : InsertEdge(dependentViewIn, dependenceViewIn);

				if (!inserted)
				{
					toNotCreate.Add(edge);

				}
			});
		graphViewChange.edgesToCreate?.RemoveAll(p => toNotCreate.Contains(p));

		return graphViewChange;
	}
	private bool InsertEdge<T>(NodeView<T> dependentView, NodeView<T> dependenceView) where T : NodeData
	{
		if (dependentView != null && dependenceView != null)
		{
			dependentView.node.AddChild(dependenceView.node);
			if (
				dependenceView.node.GetType() == typeof(TreeNode<InstructionData>) &&
				dependentView.node.GetType() == typeof(TreeNode<InstructionData>)
			)
			{
				dependenceView.node.Parent = (dependentView.node);
			}

			return true;
		}
		return false;
	}

	private void RemoveNodeView<T>(NodeView<T> nodeView) where T : NodeData
	{
		if (nodeView != null)
		{
			taskEditor.DeleteNode(nodeView.node);
		}
	}

	private void RemoveEdge<T>(NodeView<T> output, NodeView<T> input) where T : NodeData
	{
		if (output != null && input != null)
		{
			NodeView<T> dependentView = output;
			NodeView<T> dependenceView = input;

			dependentView.node.RemoveChild(dependenceView.node);
		}
	}

	public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
	{
		//base.BuildContextualMenu(evt);
		{
			evt.menu.AppendAction($"[Instruction]", a =>
			{
				var node = taskEditor.CreateInstructionNode();
				CreateNodeView(node);
			});
			evt.menu.AppendAction($"[Task]", a =>
			{
				var node = taskEditor.CreateTaskNode();
				CreateNodeView(node);
			});
		}
	}

	private void CreateNodeView(TreeNode<TaskData> node)
	{
		NodeView<TaskData> nodeView = new(node)
		{
			OnSelection = OnTaskSelection,
			OnChange = UpdateNode,
			name = "Task"
		};
		AddElement(nodeView);
	}

	private void CreateNodeView(TreeNode<InstructionData> node)
	{
		NodeView<InstructionData> nodeView = new(node)
		{
			OnSelection = OnInstructionSelection,
			OnChange = UpdateNode,
			name = "Instruction"
		};
		AddElement(nodeView);
	}

	private void UpdateNode<T>(NodeView<T> view) where T : NodeData
	{
		view.title = $"{view.node.name}: {view.node.Data.Title}";

		return;
	}
}
