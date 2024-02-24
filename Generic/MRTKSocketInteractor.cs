using MixedReality.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;
using System.Linq;
using Unity.XR.CoreUtils.Collections;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts
{
	public class MRTKSocketInteractor : XRSocketInteractor, IGrabInteractor
	{
		readonly HashSetList<ObjectManipulator> m_InteractablesWithSocketTransformer = new HashSetList<ObjectManipulator>();


		protected override void OnSelectEntered(SelectEnterEventArgs args)
		{
			base.OnSelectEntered(args);

			if (args.interactableObject is ObjectManipulator grabInteractable)
				StartSocketSnapping(grabInteractable);
		}

		protected override void OnSelectExited(SelectExitEventArgs args)
		{
			base.OnSelectExited(args);

			if (args.interactableObject is ObjectManipulator grabInteractable)
				EndSocketSnapping(grabInteractable);
		}


		protected virtual bool StartSocketSnapping(ObjectManipulator grabInteractable)
		{
			// If we've already started socket snapping this interactable, do nothing
			var interactablesSocketedCount = m_InteractablesWithSocketTransformer.Count;
			if (interactablesSocketedCount >= socketSnappingLimit ||
				m_InteractablesWithSocketTransformer.Contains(grabInteractable))
				return false;

			if (interactablesSocketedCount > 0 && ejectExistingSocketsWhenSnapping)
			{
				m_InteractablesWithSocketTransformer.Clear();
			}

			AddSingleGrabTransformer(grabInteractable);
			m_InteractablesWithSocketTransformer.Add(grabInteractable);
			return true;
		}

		private void AddSingleGrabTransformer(ObjectManipulator grabInteractable)
		{
			var selector = grabInteractable.interactorsSelecting.FirstOrDefault();
			if (selector == null)
				return;

			grabInteractable.transform.position = attachTransform.transform.position;
			grabInteractable.transform.rotation = attachTransform.transform.rotation;
		}

		protected virtual bool EndSocketSnapping(ObjectManipulator grabInteractable)
		{
			return m_InteractablesWithSocketTransformer.Remove(grabInteractable);
		}
	}
}
