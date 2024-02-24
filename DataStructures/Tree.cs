using System.Collections.Generic;


namespace Assets.Scripts.DataStructures
{
	public class Tree<T, K>
		where T : NodeData
		where K : TreeNode<T>
	{
		public K Root { get; set; }

		public List<K> GetLeafNodes()
		{
			return GetLeafNodesRec(Root);
		}
		private List<K> GetLeafNodesRec(K treeNode)
		{
			if (treeNode.Children.Count == 0)
				return new List<K> { treeNode };

			List<K> dataNodes = new List<K>();
			foreach (K child in treeNode.Children)
			{
				dataNodes.AddRange(GetLeafNodesRec(child));
			}
			return dataNodes;
		}
	}
}
