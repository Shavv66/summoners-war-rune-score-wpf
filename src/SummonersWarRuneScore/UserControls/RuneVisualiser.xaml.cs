using SummonersWarRuneScore.Domain;
using SummonersWarRuneScore.Domain.Enumerations;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SummonersWarRuneScore.UserControls
{
	/// <summary>
	/// Interaction logic for RuneVisualiser.xaml
	/// </summary>
	public partial class RuneVisualiser : UserControl
    {
		private RuneVisualiserDataContext mDataContext;
		public Rune Rune
		{
			get { return mDataContext.Rune; }
			set { mDataContext.Rune = value; }
		}

		public RuneVisualiser()
        {
            InitializeComponent();

			mDataContext = new RuneVisualiserDataContext();
			(this.Content as FrameworkElement).DataContext = mDataContext;
        }
    }

	public class RuneVisualiserDataContext : INotifyPropertyChanged
	{
		private Rune mRune;
		public Rune Rune
		{
			get { return mRune; }
			set
			{
				mRune = value;
				if (mRune != null)
				{
					RunePowerUpText = mRune.Level == 0 ? "" : $"+{mRune.Level}";
				}
				NotifyPropertyChanged("Rune");
			}
		}

		private string mRunePowerUpText;
		public string RunePowerUpText
		{
			get { return mRunePowerUpText; }
			set
			{
				mRunePowerUpText = value;
				NotifyPropertyChanged("RunePowerUpText");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}

	public class RuneColourToBrushConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			UserControl control = values[0] as UserControl;
			RuneColour runeColour = RuneColour.White;
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
