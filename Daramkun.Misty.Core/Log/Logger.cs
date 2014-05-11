using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Daramkun.Misty.Log.Writers;

namespace Daramkun.Misty.Log
{
	[Flags]
	public enum MessageFormat
	{
		Message = 0,

		LogLevel = 1 << 0,
		DateTime = 1 << 1,
		Thread = 1 << 2,
		CultureName = 1 << 3,
	}

	public enum LogLevel
	{
		None,

		Level1,
		Level2,
		Level3,
		Level4,
		Level5,
	}

	public static class Logger
	{
		public static MessageFormat MessageFormat { get; set; }
		public static IEnumerable<ILogWriter> LogWriters { get; private set; }

		public static bool IsParallelLoggingMode { get; set; }

		public static LogLevel LogLevel { get; set; }

		static Logger ()
		{
			LogWriters = new List<ILogWriter> ();
			LogLevel = LogLevel.Level5;
		}

		public static void AddDefaultLogWriter ()
		{
			AddLogWriter ( new DebugLogWriter () );
		}

		public static void AddLogWriter ( ILogWriter logWriter )
		{
			( LogWriters as List<ILogWriter> ).Add ( logWriter );
		}

		private static bool HasFlag ( MessageFormat messageFormat )
		{
			return ( ( MessageFormat & messageFormat ) != 0 );
		}

		public static void Write ( LogLevel level, string message, params object [] args )
		{
			if ( level == LogLevel.None ) throw new ArgumentException ();
			if ( level > LogLevel ) return;

			StringBuilder builder = new StringBuilder ();

			if ( HasFlag ( MessageFormat.LogLevel ) )
				builder.Append ( String.Format ( "[{0}]", level ) );
			if ( HasFlag ( MessageFormat.CultureName ) )
				builder.Append ( String.Format ( "[{0}]", Core.CurrentCulture.Name ) );
			if ( HasFlag ( MessageFormat.Thread ) )
				builder.Append ( String.Format ( "[{0}]", new IntPtr ( Thread.CurrentThread.ManagedThreadId ) ) );
			if ( HasFlag ( MessageFormat.DateTime ) )
				builder.Append ( String.Format ( "[{0}]", DateTime.UtcNow.ToString ( CultureInfo.InvariantCulture.DateTimeFormat ) ) );
			builder.Append ( String.Format ( message, args ) );

			string tempString = builder.ToString ();
			if ( IsParallelLoggingMode )
				ThreadPool.QueueUserWorkItem ( Logging, tempString );
			else Logging ( tempString );
		}

		private static void Logging ( object tempString )
		{
			foreach ( ILogWriter logWriter in LogWriters )
				logWriter.WriteLog ( tempString as string );
		}
	}
}
