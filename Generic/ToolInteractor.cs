using MixedReality.Toolkit;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static MixedReality.Toolkit.IPokeInteractor;

namespace Assets.Scripts
{
	public class ToolInteractor : XRBaseInteractor, IPokeInteractor
	{
		public float PokeRadius => 0.1f;

		// The last and current poke points, forming a
		// continuous poking trajectory.
		private PokePath pokeTrajectory;

		public PokePath PokeTrajectory => pokeTrajectory;

		public override bool isSelectActive => true;

		// Collection of hover targets.
		private HashSet<IXRInteractable> hoveredTargets = new HashSet<IXRInteractable>();

		/// <inheritdoc />
		public override void GetValidTargets(List<IXRInteractable> targets)
		{
			targets.Clear();
			targets.AddRange(hoveredTargets);
		}

		/// <inheritdoc />
		public override bool CanSelect(IXRSelectInteractable interactable)
		{
			// Can only select if we've hovered.
			return hoveredTargets.Contains(interactable);
		}

		/// <inheritdoc />
		public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
		{
			base.ProcessInteractor(updatePhase);

			if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
			{
				// Update the trajectory.
				// The PokeInteractor we use for hands does advanced
				// sphere casting to ensure reliable pokes; as a demonstration,
				// this simple interactor only performs trigger intersections.
				pokeTrajectory.Start = pokeTrajectory.End;
				pokeTrajectory.End = attachTransform.position;
			}
		}
		void OnTriggerEnter(Collider c)
		{
			if (interactionManager.TryGetInteractableForCollider(c, out var associatedInteractable))
			{
				hoveredTargets.Add(associatedInteractable);
			}
		}

		void OnTriggerStay(Collider c)
		{
			if (interactionManager.TryGetInteractableForCollider(c, out var associatedInteractable))
			{
				hoveredTargets.Add(associatedInteractable);
			}
		}

		/// <summary>
		/// A Unity event function that is called at an framerate independent frequency, and is only called if this object is enabled.
		/// </summary>
		private void FixedUpdate()
		{
			hoveredTargets.Clear();
		}
	}
}
