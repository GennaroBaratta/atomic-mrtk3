using MixedReality.Toolkit.SpatialManipulation;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts
{

	// Ensure the GameObject this adapter is attached to also has ObjectManipulator.
	[RequireComponent(typeof(ObjectManipulator))]
	public class ObjectManipulatorAdapter : XRGrabInteractable
	{
		private ObjectManipulator objectManipulator;

		protected override void Awake()
		{
			base.Awake();
			// Get the attached ObjectManipulator component.
			objectManipulator = GetComponent<ObjectManipulator>();
		}

		// We override OnSelectEntered to handle the interaction logic specific to when
		// an object is first grabbed/interacted with.
		protected override void OnSelectEntered(SelectEnterEventArgs args)
		{
			base.OnSelectEntered(args);
			// Call a method on the ObjectManipulator to handle the interaction.
			// For example, if the ObjectManipulator has a method to start manipulation, call it here.
			// objectManipulator.StartManipulation();
		}

		// Similarly, override OnSelectExited to handle the logic for when
		// the object is released or the interaction ends.
		protected override void OnSelectExited(SelectExitEventArgs args)
		{
			base.OnSelectExited(args);
			// Call the corresponding method on the ObjectManipulator to end manipulation.
			// objectManipulator.EndManipulation();
		}

		// If there are additional manipulation aspects we need to handle, such as updating
		// the transform during manipulation, they could be implemented here, potentially
		// by overriding the Update method or other relevant methods.
	}
}
