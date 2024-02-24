using Assets.Scripts.DataStructures;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class TaskGraphEditor : EditorWindow
{
	TaskGraphView graphView;
	InspectorView inspectorView;
	[MenuItem("TaskGraphEditor/Editor ...")]
	public static void OpenWindow()
	{
		TaskGraphEditor wnd = GetWindow<TaskGraphEditor>();
		wnd.titleContent = new GUIContent("TaskGraphEditor");

	}

	[OnOpenAsset]
	public static bool OnOpenAsset(int instanceId, int line)
	{
		if (Selection.activeObject is TreeNode<TaskData>)
		{
			OpenWindow();
			return true;
		}
		return false;
	}

	public void CreateGUI()
	{
		// Each editor window contains a root VisualElement object
		VisualElement root = rootVisualElement;
		// Import UXML
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/SRC/MyEditor/TaskGraphEditor.uxml");
		visualTree.CloneTree(root);

		// A stylesheet can be added to a VisualElement.
		// The style will be applied to the VisualElement and all of its children.
		var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/SRC/MyEditor/TaskGraphEditor.uss");
		root.styleSheets.Add(styleSheet);

		graphView = root.Q<TaskGraphView>();
		inspectorView = root.Q<InspectorView>();
		inspectorView.taskGraphView = graphView;
		graphView.OnTaskSelection = OnNodeSelectionChanged;
		graphView.OnInstructionSelection = OnNodeSelectionChanged;
		OnSelectionChange();
	}

	private void OnNodeSelectionChanged<T>(NodeView<T> nodeView) where T : NodeData
	{
		inspectorView.UpdateSelection(nodeView);
	}
	private void OnSelectionChange()
	{
		TreeNode<TaskData> task = Selection.activeObject as TreeNode<TaskData>;
		if (task)
		{
			graphView.PopulateView(task);
		}
	}
}