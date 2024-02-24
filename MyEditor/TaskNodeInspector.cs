using UnityEditor;
using UnityEngine.UIElements;

namespace Assets.Scripts.DataStructures
{
	[CustomEditor(typeof(TaskNode))]
	public class TaskNodeInspector : NodeInspector
	{
		public override VisualElement CreateInspectorGUI()
		{
			var m_root = base.CreateInspectorGUI();
			m_root.Insert(0, new Label("This is a custom inspector for taskNode"));

			return m_root;
		}
	}


}
