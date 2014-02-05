using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Daramkun.Misty.Graphics;
using Daramkun.Misty.Mathematics;

namespace Daramkun.Misty.Common
{
	public static class Utilities
	{
		public static bool IsSubtypeOf ( Type majorType, Type minorType )
		{
			if ( majorType == minorType || majorType.IsSubclassOf ( minorType ) )
				return true;
			else if ( minorType.IsInterface )
			{
				foreach ( Type type in majorType.GetInterfaces () )
					if ( type == minorType )
						return true;
			}
			return false;
		}

		public static bool FloatEqual ( float a, float b )
		{
			if ( a == b ) return true;
			if ( a >= b - 0.0001f && a <= b + 0.0001f ) return true;
			if ( b >= a - 0.0001f && b <= a + 0.0001f ) return true;
			return false;
		}

		public static VertexElement [] CreateVertexElementArray<T> () where T : struct
		{
			List<VertexElement> elements = new List<VertexElement> ();

			FieldInfo [] fields = typeof ( T ).GetFields ();
			PropertyInfo [] properties = typeof ( T ).GetProperties ();

			foreach ( FieldInfo field in fields )
			{
				VertexElementationAttribute [] attrs = field.GetCustomAttributes ( typeof ( VertexElementationAttribute ),
					true ) as VertexElementationAttribute [];
				if ( attrs == null || attrs.Length == 0 ) continue;
				var attr = attrs [ 0 ];
				VertexElement element = new VertexElement ();
				element.Type = attr.ElementType;
				element.UsageIndex = attr.UsageIndex;
				if ( field.FieldType == typeof ( float ) )
					element.Size = ElementSize.Float1;
				else if ( field.FieldType == typeof ( Vector2 ) )
					element.Size = ElementSize.Float2;
				else if ( field.FieldType == typeof ( Vector3 ) )
					element.Size = ElementSize.Float3;
				else if ( field.FieldType == typeof ( Vector4 ) || 
					field.FieldType == typeof ( Color ) )
					element.Size = ElementSize.Float4;
				elements.Add ( element );
			}

			foreach ( PropertyInfo prop in properties )
			{
				VertexElementationAttribute [] attrs = prop.GetCustomAttributes ( typeof ( VertexElementationAttribute ),
					true ) as VertexElementationAttribute [];
				if ( attrs == null || attrs.Length == 0 ) continue;
				var attr = attrs [ 0 ];
				VertexElement element = new VertexElement ();
				element.Type = attr.ElementType;
				element.UsageIndex = attr.UsageIndex;
				if ( prop.PropertyType == typeof ( float ) )
					element.Size = ElementSize.Float1;
				else if ( prop.PropertyType == typeof ( Vector2 ) )
					element.Size = ElementSize.Float2;
				else if ( prop.PropertyType == typeof ( Vector3 ) )
					element.Size = ElementSize.Float3;
				else if ( prop.PropertyType == typeof ( Vector4 ) || 
					prop.PropertyType == typeof ( Color ) )
					element.Size = ElementSize.Float4;
				elements.Add ( element );
			}

			return elements.ToArray ();
		}
	}
}
