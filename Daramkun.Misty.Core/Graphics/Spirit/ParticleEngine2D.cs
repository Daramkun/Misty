using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Log;
using Daramkun.Misty.Mathematics;
using Daramkun.Misty.Nodes;

namespace Daramkun.Misty.Graphics.Spirit
{
	public class ParticleEngine2D : Node
	{
		Random random;
		ITexture2D [] textures;
		TimeSpan lastTimeSpan;

		public Vector2 EmitterLocation { get; set; }
		public TimeSpan GeneratePeriod { get; set; }
		public int GenerateCountInOneTime { get; set; }
		public TimeSpan BaseTTL { get; set; }

		public ParticleEngine2D ( Vector2 position, int generateCountInOneTime, TimeSpan? generatePeriod = null, params ITexture2D [] textures )
		{
			random = new Random ();
			EmitterLocation = position;
			this.textures = textures.Clone () as ITexture2D [];
			GeneratePeriod = generatePeriod == null ? TimeSpan.FromMilliseconds ( 20 ) : generatePeriod.Value;
			GenerateCountInOneTime = generateCountInOneTime;
			BaseTTL = TimeSpan.FromMilliseconds ( 100 );
		}

		protected virtual Particle2D GenerateParticle ()
		{
			ITexture2D particleTexture = textures [ random.Next () % textures.Length ];
			Vector2 particleVelocity = new Vector2 ( 1f * ( float ) ( random.NextDouble () * 2 - 1 ),
				1f * ( float ) ( random.NextDouble () * 2 - 1 ) );
			float particleAngle = 0;
			float particleAngularVelocity = 0.1f * ( float ) ( random.NextDouble () * 2 - 1 );
			Color color = new Color (
				( float ) random.NextDouble (),
				( float ) random.NextDouble (),
				( float ) random.NextDouble ()
			);
			float size = ( float ) random.NextDouble ();
			return new Particle2D ( particleTexture, EmitterLocation, particleVelocity, particleAngle, particleAngularVelocity,
				color, size, TimeSpan.FromMilliseconds ( BaseTTL.TotalMilliseconds + random.Next ( 200 ) ) );
		}

		public override void Draw ( GameTime gameTime )
		{
			double times = ( lastTimeSpan - GeneratePeriod ).TotalMilliseconds / GeneratePeriod.TotalMilliseconds;
			if ( ( ( int ) times ) != 0 )
			{
				for ( int i = 0; i < ( int ) ( times * GenerateCountInOneTime ); ++i )
					Add ( GenerateParticle () );
				lastTimeSpan = gameTime.ElapsedGameTime;
			}
			else lastTimeSpan += gameTime.ElapsedGameTime;
			base.Draw ( gameTime );
		}
	}
}
