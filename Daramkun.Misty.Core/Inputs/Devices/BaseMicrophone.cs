using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Daramkun.Misty.Common;
using Daramkun.Misty.Contents;

namespace Daramkun.Misty.Inputs.Devices
{
	public class BaseMicrophone : StandardDispose, IInputDevice<AudioInfo>
	{
		public virtual AudioInfo GetState ( PlayerIndex index = PlayerIndex.Player1 ) { return new AudioInfo ( 0, 0, 0, TimeSpan.MinValue, null, null, null ); }
		
		public virtual bool IsSupport { get { return false; } }
		public virtual bool IsConnected { get { return false; } }
		public virtual bool IsMultiPlayerable { get { return false; } }

		public virtual void Start () { }
		public virtual void Stop () { }
	}
}
