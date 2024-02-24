using Assets.Scripts.DataStructures;
using UnityEngine;


//some code here that uses something from the UnityEditor namespace
namespace Assets.Scripts.DataStructures
{
	public class InstructionNode : TreeNode<InstructionData>
	{
		[SerializeField]
		private InstructionNode _Parent = null;
		[SerializeField]
		private InstructionData m_data = new();
		public override InstructionData Data { get => m_data; set => m_data = value; }
		public override TreeNode<InstructionData> Parent { get => _Parent; set => _Parent = (InstructionNode)value; }

		public string prova { get; set; }

		public void Initialize()
		{
			m_data.Initialize();
			EventsMediator.Instance.InteractionEvents += OnInteraction;
		}

		private void OnEnable()
		{
#if UNITY_EDITOR
			m_data.guid = UnityEditor.GUID.Generate().ToString();
#endif
		}

		private void OnInteraction(object sender, Interaction interaction)
		{
			if (Data.Interaction.Equals(interaction))
			{
				EventsMediator.Instance.Fire(this);
			}
		}
	}
}
