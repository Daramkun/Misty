using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents.FileSystems;
using Daramkun.Misty.Contents.Tables;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Graphics.Spirit;
using Daramkun.Misty.Inputs.States;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;

namespace Test.Game.Particle2D
{
	[MainNode]
	public class Container : Node
	{
		ResourceTable contentManager;

		public override void Intro ( params object [] args )
		{
			contentManager = new ResourceTable ( FileSystemManager.GetFileSystem ( "ManifestFileSystem" ) );

			Add ( new ParticleEngine2D ( new Vector2 (), 20, null,
				contentManager.Load<ITexture2D> ( "Resources/circle.png" ),
				contentManager.Load<ITexture2D> ( "Resources/diamond.png" ),
				contentManager.Load<ITexture2D> ( "Resources/star.png" )
			) );

			base.Intro ( args );
		}

		public override void Outro ()
		{
			contentManager.Dispose ();
			base.Outro ();
		}

		public override void Update ( GameTime gameTime )
		{
			( this [ 0 ] as ParticleEngine2D ).EmitterLocation = Core.Inputs.GetDevice<MouseState> ().GetState ().Position;
			base.Update ( gameTime );
		}

		public override void Draw ( GameTime gameTime )
		{
			Core.GraphicsDevice.BeginScene ();
			Core.GraphicsDevice.Clear ( ClearBuffer.AllBuffer, Color.Black );

			base.Draw ( gameTime );
	
			Core.GraphicsDevice.EndScene ();
			Core.GraphicsDevice.SwapBuffer ();
		}

		public override string ToString () { return "Particle Engine 2D Tester"; }
	}
}
