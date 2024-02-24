using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class TriggerHandler : MonoBehaviour
	{
		public event Action<Collider> OnCustomTriggerEnter;
		public event Action<Collider> OnCustomTriggerExit;

		void OnTriggerEnter(Collider other)
		{
			OnCustomTriggerEnter?.Invoke(other);
		}

		void OnTriggerExit(Collider other)
		{
			OnCustomTriggerExit?.Invoke(other);
		}
	}

}
