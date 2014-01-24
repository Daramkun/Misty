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
		SpinLock spinlock = new SpinLock ();
		List<Node> children;
		Node [] childrenArray;

		public Node Parent { get; internal set; }
		public IEnumerable<Node> Children { get { return children; } }
		public int ChildrenCount { get { return children.Count; } }
		public bool IsManuallyChildrenCacheMode { get; set; }

		public virtual uint ZOrder { get; set; }

		public bool IsEnabled { get; set; }
		public bool IsVisible { get; set; }

		public virtual bool IsTailEndNode { get { return false; } }

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
			foreach ( Node node in from a in childrenArray where a.IsEnabled select a )
				node.Update ( gameTime );
		}

		public virtual void Draw ( GameTime gameTime )
		{
			foreach ( Node node in from a in childrenArray where a.IsVisible orderby a.ZOrder select a )
				node.Draw ( gameTime );
		}
	}
}
