using System;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
	[Serializable]
	public abstract class NodeData : IComparable<NodeData>
	{
		public int Id;

		public string guid;
		[SerializeField]
		public string Title;
		[SerializeField]
		public string Description;
		//[NonSerialized]
		public bool IsResolved;
		[SerializeField]
#pragma warning disable CS0414 // Il campo 'NodeData.TimeToComplete' è assegnato, ma il suo valore non viene mai usato
		private double TimeToComplete;
#pragma warning restore CS0414 // Il campo 'NodeData.TimeToComplete' è assegnato, ma il suo valore non viene mai usato
		[SerializeField]
		public int ExecutionOrder;

		//position in graph
		[SerializeField]
		public Vector2 position;

		public virtual void Initialize()
		{
			IsResolved = false;
			TimeToComplete = 0;
		}

		public int CompareTo(NodeData other)
		{
			return ExecutionOrder.CompareTo(other.ExecutionOrder);
		}

		public override bool Equals(object obj)
		{
			if (obj is NodeData node)
			{
				return (guid != null && guid == node.guid) ||
					string.Equals(Title, node.Title) && string.Equals(Description, node.Description);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return int.Parse(guid);
		}
	}
}
