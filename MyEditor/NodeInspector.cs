using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Assets.Scripts.DataStructures
{
	public abstract class NodeInspector : UnityEditor.Editor
	{
		public event Action OnDataChanged;
		public VisualTreeAsset m_InspectorXML;


		public override VisualElement CreateInspectorGUI()
		{
			base.CreateInspectorGUI();
			//EditorGUI.BeginChangeCheck();
			var m_root = new VisualElement();

			var foldout = new Foldout();
			m_root.Add(foldout);
			//draw default inspector
			foldout.Add(new IMGUIContainer());
			var defaultInspector = foldout.Q<IMGUIContainer>();
			defaultInspector.onGUIHandler = () => DrawDefaultInspector();
			foldout.value = true;
			
			m_root.Bind(serializedObject);
			m_root.TrackSerializedObjectValue(serializedObject, TrackCallback);
			return m_root;
		}

		public void TrackCallback(SerializedObject so)
		{
			so.ApplyModifiedProperties();
			OnDataChanged?.Invoke();
		}
	}
}
