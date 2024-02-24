using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.DataStructures.NodeFactory
{
	public static class NodeFactoryImpl
	{
		private static readonly Dictionary<Type, object> factories = new Dictionary<Type, object>
		{
			{ typeof(TaskData), new TaskNodeFactory() },
			{ typeof(InstructionData), new InstructionNodeFactory() },
		};

		public static INodeFactory<T> GetFactory<T>() where T : NodeData
		{
			if (factories.TryGetValue(typeof(T), out var factory))
			{
				return (INodeFactory<T>)factory;
			}

			throw new InvalidOperationException($"No factory registered for type {typeof(T).Name}");
		}
	}

}


