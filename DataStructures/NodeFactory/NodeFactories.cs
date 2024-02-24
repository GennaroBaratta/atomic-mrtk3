using Assets.Scripts.DataStructures;
using UnityEngine;

namespace Assets.Scripts.DataStructures.NodeFactory
{
	public class TaskNodeFactory : INodeFactory<TaskData>
	{

		K INodeFactory<TaskData>.CreateNode<K>()
		{
			return ScriptableObject.CreateInstance<K>();
		}
	}

	public class InstructionNodeFactory : INodeFactory<InstructionData>
	{
		K INodeFactory<InstructionData>.CreateNode<K>()
		{
			return ScriptableObject.CreateInstance<K>();
		}
	}

}
