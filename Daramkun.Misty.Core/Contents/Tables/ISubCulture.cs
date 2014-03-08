using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Daramkun.Misty.Contents.Tables
{
	public interface ISubCulture<T>
	{
		void AddCulture ( CultureInfo cultureInfo, T data );
		void RemoveCulture ( CultureInfo cultureInfo );
	}
}
