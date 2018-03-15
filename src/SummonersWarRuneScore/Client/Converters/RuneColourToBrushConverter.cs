using SummonersWarRuneScore.Components.Domain.Enumerations;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SummonersWarRuneScore.Client.Converters
{
	public class RuneColourToBrushConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(values[0] is FrameworkElement control))
			{
				throw new ArgumentException("Rune colour to brush converter must be given a reference to the control as the first binding");
			}

			RuneColour runeColour;
			try
			{
				runeColour = (RuneColour)values[1];
			}
			catch (InvalidCastException)
			{
				return control.FindResource("WhiteRune");
			}

			switch (runeColour)
			{
				case RuneColour.Green: return control.FindResource("GreenRune");
				case RuneColour.Blue: return control.FindResource("BlueRune");
				case RuneColour.Purple: return control.FindResource("PurpleRune");
				case RuneColour.Orange: return control.FindResource("OrangeRune");
				default: return control.FindResource("WhiteRune");
			}
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
