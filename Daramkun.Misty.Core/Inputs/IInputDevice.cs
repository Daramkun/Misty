using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Inputs
{
	public enum PlayerIndex
	{
		Player1,
		Player2,
		Player3,
		Player4,
	}

	public interface IInputDevice : IDisposable
	{
		bool IsSupport { get; }
		bool IsConnected { get; }
		bool IsMultiPlayerable { get; }
	}

	public interface IInputDevice<T> : IInputDevice
	{
		T GetState ( PlayerIndex index = PlayerIndex.Player1 );
	}
}
