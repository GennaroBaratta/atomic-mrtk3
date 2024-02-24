using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.DataStructures
{
	[Serializable]
	public abstract class TreeNode<T> : ScriptableObject, IComparable<TreeNode<T>>, INotifyPropertyChanged where T : NodeData
	{
		public abstract T Data { get; set; }
		public abstract TreeNode<T> Parent { get; set; }
		[SerializeField]
		public List<TreeNode<T>> Children = new();
		public event PropertyChangedEventHandler PropertyChanged;

		// This method is called by the Set accessor of each property.  
		// The CallerMemberName attribute that is applied to the optional propertyName  
		// parameter causes the property name of the caller to be substituted as an argument.  
		private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public int GetHeight()
		{
			int height = 1;
			TreeNode<T> current = this;
			while (current.Parent != null)
			{
				height++;
				current = current.Parent;
			}
			return height;
		}

		public void AddChildren(List<TreeNode<T>> children)
		{
			Children.AddRange(children);

			children.ForEach(c => c.Parent = this);
			NotifyPropertyChanged(nameof(Children));
		}

		public void AddChild(TreeNode<T> child)
		{
			Children.Add(child);
			child.Parent = this;
			NotifyPropertyChanged(nameof(Children));
		}

		public int CompareTo(TreeNode<T> other)
		{
			return Data.CompareTo(other.Data);
		}

		public void RemoveChild(TreeNode<T> child)
		{
			Children.Remove(child);
			child.Parent = null;
			NotifyPropertyChanged(nameof(Children));
		}
	}
}
