using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
	[System.Serializable]
	public struct Interaction
	{
		public enum InteractionType
		{
			MoveToDestination,
			Rotate,
			RotateToTarget
		}
			

		[SerializeField]
		public AbstractInteractionData data;


		public void OnInteraction()
		{
			EventsMediator.Instance.FireInteraction(this);
		}

		public void Initialize()
		{
			data.Initialize(OnInteraction);
		}

		public void SetInteractableReference(ExposedReference<GameObject>? id)
		{
			data.interactableRef = id.HasValue ? id.Value : new() { };
		}
		public void SetDestinationRef(ExposedReference<Collider>? id)
		{
			data.destinationRef = id ?? new() { };
		}
		public void SetToolRef(ExposedReference<Collider>? id)
		{
			data.toolRef = id ?? new() { };
		}
	}

	public abstract class AbstractInteractionData: ScriptableObject
	{
		public ExposedReference<GameObject> interactableRef { get; set; }
		public ExposedReference<Collider> destinationRef { get; set; }
		public ExposedReference<Collider> toolRef { get; set; }
		public Interaction.InteractionType interactionType { get; set; }

		public abstract void Initialize(Action onInteraction);
	}
}
