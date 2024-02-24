using Assets.Scripts.DataStructures;
using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
	public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }
	public TaskGraphView taskGraphView;
	private NodeInspector editor;

	public InspectorView() { }

	internal void UpdateSelection<T>(NodeView<T> nodeView) where T : NodeData
	{
		Clear();
		if (editor != null)
			editor.OnDataChanged -= nodeView.Update;

		UnityEngine.Object.DestroyImmediate(editor);

		editor = (NodeInspector)Editor.CreateEditor(nodeView.node);
		editor.OnDataChanged += nodeView.Update;

		VisualElement container = new VisualElement();
		container.Add(editor.CreateInspectorGUI());
		Add(container);
	}
}
