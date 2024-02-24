using Assets.Scripts.DataStructures;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeView<T> : Node where T : NodeData
{
	public Action<NodeView<T>> OnSelection;
	public Action<NodeView<T>> OnChange;
	public TreeNode<T> node;
	public Port input;
	public Port output;

	public NodeView(TreeNode<T> node)
	{

		this.node = node;
		this.title = $"{node.name}: {node.Data.Title}";

		this.viewDataKey = node.Data.guid;

		style.left = node.Data.position.x;
		style.top = node.Data.position.y;

		CreateInputPorts();
		CreateOutputPorts();
	}


	private void CreateInputPorts()
	{
		input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(TreeNode<NodeData>));

		if (input != null)
		{
			input.portName = "Next Instructions";
			inputContainer.Add(input);
		}
	}

	private void CreateOutputPorts()
	{
		output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(TreeNode<NodeData>));
		if (output != null)
		{
			output.portName = "Dependencies";
			outputContainer.Add(output);
		}
	}

	public override void SetPosition(Rect newPos)
	{
		base.SetPosition(newPos);
		node.Data.position.x = newPos.xMin;
		node.Data.position.y = newPos.yMin;
	}

	public override void OnSelected()
	{
		base.OnSelected();
		OnSelection?.Invoke(this);
	}

	internal void Update()
	{
		OnChange?.Invoke(this);
	}
}
