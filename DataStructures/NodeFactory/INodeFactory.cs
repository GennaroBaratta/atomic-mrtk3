using Assets.Scripts.DataStructures;

namespace Assets.Scripts.DataStructures.NodeFactory
{
	public interface INodeFactory<T> where T : NodeData
	{
		K CreateNode<K>() where K : TreeNode<T>;
	}
}
