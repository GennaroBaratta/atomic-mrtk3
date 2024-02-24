using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts
{
	internal class ToolInteractable : XRGrabInteractable
	{
		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
		{
			base.ProcessInteractable(updatePhase);
			if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
			{
				foreach (ToolInteractor interactor in interactorsSelecting.Where(w => w is ToolInteractor))
				{
					// attachTransform will be the actual point of the touch interaction (e.g. index tip)
					// Most applications will probably just end up using this local touch position.
					Vector3 localTouchPosition = transform.InverseTransformPoint(interactor.GetAttachTransform(this).position);
				}
			}
		}
	}
}
