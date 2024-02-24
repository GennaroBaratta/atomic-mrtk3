using Assets.Scripts.DataStructures;
using System;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
	[Serializable]
	public class InstructionData : NodeData
	{
		[SerializeField] 
		public Interaction Interaction;

		public new void Initialize()
		{
			base.Initialize();

			Interaction.Initialize();
		}
		public override string ToString()
		{
			return $"{this.Title}:{this.Description}";
		}
	}
}
