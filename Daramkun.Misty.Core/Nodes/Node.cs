using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;

namespace Daramkun.Misty.Nodes
{
	public class Node
	{
		object forLock = new object ();
		uint zOrder;

		SpinLock spinlock = new SpinLock ();
		List<Node> children;
		Node [] childrenArray;

		public Node Parent { get; internal set; }
		public IEnumerable<Node> Children { get { return children; } }
		public int ChildrenCount { get { return children.Count; } }
		public bool IsManuallyChildrenCacheMode { get; set; }

		public virtual uint ZOrder
		{
			get { return zOrder; }
			set { zOrder = value; }
		}

		public bool IsEnabled { get; set; }
		public bool IsVisible { get; set; }

		public T Instantiate<T> ( params object [] args ) where T : Node
		{
			T instantiate = Activator.CreateInstance ( typeof ( T ), args ) as T;
			Add ( instantiate );
			return instantiate;
		}

		public void StartCoroutine ( Func<IEnumerable> coroutine )
		{
			foreach ( IEnumerable e in coroutine () )
				Core.Window.DoEvents ();
		}

		public void StartCoroutine ( Func<object, IEnumerable> coroutine, object argument )
		{
			foreach ( IEnumerable e in coroutine ( argument ) )
				Core.Window.DoEvents ();
		}

		public Node Add ( Node node, params object [] args )
		{
			if ( node == null ) return null;

			node.Parent = this;
			node.Intro ( args );

			spinlock.Enter ();
			children.Add ( node );
			if ( !IsManuallyChildrenCacheMode )
				childrenArray = children.ToArray ();
			spinlock.Exit ();
			return node;
		}

		public void RefreshChildrenCache ()
		{
			spinlock.Enter ();
			childrenArray = children.ToArray ();
			spinlock.Exit ();
		}

		public void Remove ( Node node )
		{
			if ( node == null ) return;

			spinlock.Enter ();
			children.Remove ( node );
			if ( !IsManuallyChildrenCacheMode )
				childrenArray = children.ToArray ();
			spinlock.Exit ();

			node.Outro ();
			node.Parent = null;
		}

		public Node this [ int index ]
		{
			get { return childrenArray [ index ]; }
		}

		public Node ()
		{
			children = new List<Node> ();
			childrenArray = children.ToArray ();
			IsEnabled = IsVisible = true;
		}

		public virtual void Intro ( params object [] args )
		{

		}

		public virtual void Outro ()
		{
			foreach ( Node node in childrenArray )
				Remove ( node );
			children.Clear ();
		}

		public virtual void Update ( GameTime gameTime )
		{
			if ( children.Count > 0 )
			{
				foreach ( Node item in from a in childrenArray where a.IsEnabled select a )
					item.Update ( gameTime );
			}
		}

		public virtual void Draw ( GameTime gameTime )
		{
			if ( children.Count > 0 )
			{
				foreach ( Node node in from a in childrenArray where a.IsVisible orderby a.ZOrder select a )
					node.Draw ( gameTime );
			}
		}
	}
}
