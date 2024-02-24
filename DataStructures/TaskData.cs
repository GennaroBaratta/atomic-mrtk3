using System;
using System.Collections.Generic;

namespace Assets.Scripts.DataStructures
{
	[Serializable]
	public class TaskData : NodeData
	{
		public List<InstructionNode> instructions = new();
	}
}

