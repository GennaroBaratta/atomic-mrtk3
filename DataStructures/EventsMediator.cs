using Assets.Scripts.DataStructures;
using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class EventsMediator : MonoBehaviour
	{
		// Singleton approach
		static EventsMediator m_Instance;


		private void Awake()
		{
			if(m_Instance != null && m_Instance != this)
			{
				Destroy(this.gameObject);
				return;
			}
			m_Instance = this;
			// Opzionale: rendi l'istanza persistente tra le scene
			DontDestroyOnLoad(gameObject);
		}

		public static EventsMediator Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = FindObjectOfType<EventsMediator>();
				if (m_Instance == null)
				{
					var go = new GameObject("ReferenceTable");
					m_Instance = go.AddComponent<EventsMediator>();
				}
				return m_Instance;
			}
			private set { }
		}

		public event EventHandler<InstructionNode> InstructionEvents;
		public void Fire(InstructionNode args)
		{
			InstructionEvents?.Invoke(this, args);
		}

		public event EventHandler<Interaction> InteractionEvents;
		public void FireInteraction(Interaction args)
		{
			InteractionEvents?.Invoke(this, args);
		}
	}
}
