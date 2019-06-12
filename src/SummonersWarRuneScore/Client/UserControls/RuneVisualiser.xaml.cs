using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using SummonersWarRuneScore.Components.Domain;

namespace SummonersWarRuneScore.Client.UserControls
{
	/// <summary>
	/// Interaction logic for RuneVisualiser.xaml
	/// </summary>
	public partial class RuneVisualiser : UserControl
    {
		private readonly RuneVisualiserDataContext mDataContext;

		public Rune Rune
		{
			get { return mDataContext.Rune; }
			set { mDataContext.Rune = value; }
		}

		public RuneVisualiser()
        {
            InitializeComponent();

			mDataContext = new RuneVisualiserDataContext();
			(Content as FrameworkElement).DataContext = mDataContext;
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
}
