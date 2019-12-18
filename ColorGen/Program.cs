using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace ColorGen
{
	static class Program
	{
		static void Main()
		{
			foreach (var field in typeof(Color).GetFields())
			{
				if (field.FieldType != typeof(Color))
					continue;

				var color = (Color)field.GetValue(null);
				if (color.A == 1)
				{
					Console.WriteLine($"// Color.FromRgb({ToInt(color.R)}, {ToInt(color.G)}, {ToInt(color.B)})");
				}
				else
				{
					Console.WriteLine($"// Color.FromRgba({ToInt(color.R)}, {ToInt(color.G)}, {ToInt(color.B)}, {ToInt(color.A)})");
				}

				foreach (ObsoleteAttribute obsolete in field.GetCustomAttributes (typeof (ObsoleteAttribute), inherit: false))
				{
					Console.WriteLine($"[Obsolete(\"{obsolete.Message}\")]");
				}
				foreach (EditorBrowsableAttribute browsable in field.GetCustomAttributes(typeof(EditorBrowsableAttribute), inherit: false))
				{
					Console.WriteLine($"[EditorBrowsable(EditorBrowsableState.{browsable.State})]");
				}

				// See: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-round-trip-r-format-specifier
				var format = "R";
				Console.WriteLine($"public static readonly Color {field.Name} = new Color ({color.R.ToString (format)}f, {color.G.ToString(format)}f, {color.B.ToString(format)}f, {color.A.ToString(format)}f, Mode.Rgb, {color.Hue.ToString(format)}f, {color.Saturation.ToString(format)}f, {color.Luminosity.ToString(format)}f);");
			}
		}

		static int ToInt (double value)
		{
			return (int)Math.Round(value * byte.MaxValue, 0);
		}
	}
}
