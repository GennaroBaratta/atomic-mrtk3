using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Scripts.DataStructures.Interaction;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using Assets.Scripts.DataStructures;
using Unity.VisualScripting;

namespace Assets.Scripts
{
	[System.Serializable]
	public class InteractionData : DataStructures.AbstractInteractionData
	{
		[SerializeField] public new ExposedReference<GameObject> interactableRef;
		public GameObject interactable;
		[SerializeField] public new InteractionType interactionType { get; set; }
		[SerializeField] private bool local;
		[SerializeField] private bool snapToDestination;
		[SerializeField] private bool matchOrientation;
		[SerializeField] private bool remove;

		[SerializeField] public ExposedReference<MRTKSocketInteractor> socketRef;
		private MRTKSocketInteractor socket;

		[SerializeField] public new ExposedReference<Collider> destinationRef;
		private Collider destination;
		[SerializeField] private Vector3 rotation;
		public Action OnInteraction;

		[SerializeField] public new ExposedReference<Collider> toolRef;
		private Collider tool;


		public override void Initialize(Action onInteraction)
		{
			switch (interactionType)
			{
				case InteractionType.MoveToDestination:
					interactable = interactableRef.Resolve(ReferenceTable.Instance);
					//var objm = interactable.GetOrAddComponent<ObjectManipulator>();
					if (interactable == null)
					{
						return;
					}

					destination = destinationRef.Resolve(ReferenceTable.Instance);
					if (!snapToDestination)
					{
						var triggerHandler = interactable.GetComponent<TriggerHandler>() ?? interactable.AddComponent<TriggerHandler>();
						triggerHandler.OnCustomTriggerEnter += OnTriggerEnter;
						triggerHandler.OnCustomTriggerExit += OnTriggerExit;
					}
					else
					{
						socket = destination.GetComponentInParent<MRTKSocketInteractor>();

						socket.selectEntered.AddListener(OnSocketEntered);
						socket.selectExited.AddListener(OnSocketExited);
					}

					tool = toolRef.Resolve(ReferenceTable.Instance);

					OnInteraction = onInteraction;
					break;
				case InteractionType.Rotate:
					break;
				case InteractionType.RotateToTarget:
					break;
			}

			//(destination.Resolve(ExposedPropertyTable)).TryGetComponent<Destination>(out destinationComponent);
		}

		private void OnSocketExited(SelectExitEventArgs arg0)
		{
			if (remove)
			{
				GameObject otherInteractable = arg0.interactableObject.GetAttachTransform(socket).gameObject;
				if (otherInteractable == interactable)
				{
					OnInteraction();
				}
			}
		}

		private void OnSocketEntered(SelectEnterEventArgs arg0)
		{
			GameObject otherInteractable = arg0.interactableObject.GetAttachTransform(socket).gameObject;
			if (otherInteractable == interactable)
			{
				OnInteraction();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			Debug.Log("TriggerEnter");
			if (!remove)
			{
				//if(other.gameObject.GetComponent<Interactable>().id == this.id)
				if (other.Equals(destination))
				{
					Debug.Log("TriggerEnterDestination");
					//if (snapToDestination)
					//{
					//	// calcolo la metà della dimensione del cubo
					//	float cubeHeight = interactable.transform.localScale.y / 2;

					//	// calcolo la metà della dimensione della piattaforma
					//	float platformHeight = other.transform.localScale.y / 2;

					//	// calcolo la posizione y della piattaforma
					//	float platformPosY = other.transform.position.y;

					//	// calcolo la posizione y del cubo: la posizione della piattaforma più la metà della sua altezza più la metà dell'altezza del cubo
					//	float cubePosY = platformPosY + platformHeight + cubeHeight;

					//	// posiziono il cubo sopra la piattaforma
					//	interactable.transform.position = new Vector3(other.transform.position.x, cubePosY, other.transform.position.z);
					//	//transform.position += offset;
					//	// imposta la rotazione del cubo per corrispondere alla rotazione della piattaforma
					//	interactable.transform.rotation = other.transform.rotation;

					//	//interactable.ObjectManipulator.interactionManager.CancelInteractableSelection(interactable.ObjectManipulator as IXRSelectInteractable);
					//}
					OnInteraction();
				}

				if (other.Equals(tool))
				{
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (remove)
			{
				if (other.Equals(destination))
				{
					OnInteraction();
				}
			}
		}

		
	}
}
