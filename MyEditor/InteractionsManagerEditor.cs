//using UnityEditor.AddressableAssets.BuildReportVisualizer;
namespace Assets.Editor
{
	////[CustomEditor(typeof(InteractionsManager))]
	//public class InteractionsManagerEditor : UnityEditor.Editor
	//{
	//	public override VisualElement CreateInspectorGUI()
	//	{
	//		// Create a new VisualElement to be the root of our inspector UI
	//		VisualElement myInspector = new VisualElement();

	//		// Add a simple label
	//		myInspector.Add(new Label("This is a custom inspector"));
	//		//myInspector.Add(new IMGUIContainer());
	//		//var defaultInspector = myInspector.Q<IMGUIContainer>();
	//		//defaultInspector.onGUIHandler = () => DrawDefaultInspector();

	//		var taskField = new PropertyField(serializedObject.FindProperty("rootTask"));
	//		myInspector.Add(taskField);
	//		var instructionsField = new PropertyField(serializedObject.FindProperty("instructions"));
	//		myInspector.Add(instructionsField);

	//		taskField.RegisterValueChangeCallback((evt) =>
	//		{
	//			var target = serializedObject.targetObject as InteractionsManager;
	//			target.UpdateInstructions();

	//		});

	//		// Return the finished inspector UI
	//		return myInspector;
	//	}

	//}
}
