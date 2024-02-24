using Assets.Scripts.DataStructures;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Assets.Scripts.DataStructures
{
	[CustomEditor(typeof(InstructionNode))]
	public class InstructionNodeInspector : NodeInspector
	{
		VisualElement _root;
		VisualElement interactionItem;
		EnumField interactionTypeField;


		[SerializeField]
		VisualTreeAsset _taskListAsset;
		[SerializeField]
		StyleSheet styleSheet;

		InstructionNode instructionNode;

		public override VisualElement CreateInspectorGUI()
		{
			interactionItem = new VisualElement();
			instructionNode = target as InstructionNode;
			_root = base.CreateInspectorGUI();

			_taskListAsset.CloneTree(_root);
			_root.styleSheets.Add(styleSheet);

			instructionNode = target as InstructionNode;
			interactionTypeField = _root.Q<EnumField>("InteractionTypeField");
			interactionTypeField.label
				= "Interaction Type";
			
			interactionTypeField.Init(instructionNode.Data.Interaction.data.interactionType);

			interactionTypeField.RegisterValueChangedCallback(evt =>
			{
				instructionNode.Data.Interaction.data.interactionType = (Interaction.InteractionType)evt.newValue;
				SaveProgress();
				DrawInteraction((Interaction.InteractionType)interactionTypeField.value);
			});
			DrawInteraction((Interaction.InteractionType)interactionTypeField.value);

			return _root;
		}

		private void DrawInteraction(Interaction.InteractionType type)
		{
			interactionItem.Clear();
			switch (type)
			{
				case Interaction.InteractionType.MoveToDestination:
					DrawMoveToDestination();
					break;
				case Interaction.InteractionType.Rotate:
					//DrawRotate();
					break;
				case Interaction.InteractionType.RotateToTarget:
					//DrawRotateToTarget();
					break;
			}

		}

		private void DrawMoveToDestination()
		{

			interactionItem.Add(CreateObjectField("Target", instructionNode.Data.Interaction.data.interactableRef, instructionNode.Data.Interaction.SetInteractableReference));


			interactionItem.Add(CreateObjectField("Destination", instructionNode.Data.Interaction.data.destinationRef, instructionNode.Data.Interaction.SetDestinationRef));

			interactionItem.Add(CreateObjectField("Tool", instructionNode.Data.Interaction.data.toolRef, instructionNode.Data.Interaction.SetToolRef));
			_root.Add(interactionItem);

		}

		private VisualElement CreateObjectField<T>(string labelText, ExposedReference<T> exposedReference, Action<ExposedReference<T>?> setRef) where T : Object
		{
			VisualElement interactionItem = new VisualElement();
			Label label = new(labelText);
			ObjectField interactionObjectField = new ObjectField();
			interactionObjectField.objectType = typeof(T);
			interactionObjectField.value = ReferenceTable.Instance.GetReferenceValue(exposedReference.exposedName, out var idValid);
			//bind
			//RuntimeBindingExtensions.BindProperty(interactionObjectField, exposedReference);

			interactionObjectField.RegisterValueChangedCallback(evt =>
			{
				interactionObjectField.value = evt.newValue;
				ExposedReference<T>? x = ReferenceTable.Instance.SetReferenceValue<T>(evt.newValue as T);
				setRef(x);
				SaveProgress();
			});
			interactionItem.Add(label);
			interactionItem.Add(interactionObjectField);
			return interactionItem;
		}

		public void SaveProgress()
		{
			EditorUtility.SetDirty(instructionNode);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

		}
	}
}
