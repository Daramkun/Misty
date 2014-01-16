using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Daramkun.Misty.Graphics;

namespace Daramkun.Misty.Contents
{
	public class ImageInfo
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int FrameCount { get; private set; }
		public Stream ImageStream { get; private set; }

		object RawPixels;
		Func<ImageInfo, object, int, Color?, Color []> GetPixelsFunc;

		public ImageInfo ( int width, int height, int frameCount, Stream imageStream,
			object rawPixels, Func<ImageInfo, object, int, Color?, Color []> func )
		{
			Width = width; Height = height;
			FrameCount = frameCount;
			RawPixels = rawPixels;
			ImageStream = imageStream;
			GetPixelsFunc = func;
		}

		public Color [] GetPixels ( Color? colorKey = null, int frameCount = 0 )
		{
			return GetPixelsFunc ( this, RawPixels, frameCount, colorKey );
		}
		
		public override string ToString ()
		{
			return string.Format ( "{{Width:{0}, Height:{1}}}", Width, Height );
		}
	}
}
