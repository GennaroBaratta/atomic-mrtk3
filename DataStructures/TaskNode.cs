using UnityEngine;

namespace Assets.Scripts.DataStructures
{
	[UnityEngine.CreateAssetMenu(fileName = "Data", menuName = "Task/Node")]
	public class TaskNode : TreeNode<TaskData>
	{
		[SerializeField]
		private TaskNode _Parent = null;
		[SerializeField]
		private TaskData m_data = new();
		public override TaskData Data { get => m_data; set => m_data = value; }
		public override TreeNode<TaskData> Parent { get => _Parent; set => _Parent = (TaskNode)value; }

		private void OnEnable()
		{
#if UNITY_EDITOR
			m_data.guid = UnityEditor.GUID.Generate().ToString();
#endif
		}
	}
}
