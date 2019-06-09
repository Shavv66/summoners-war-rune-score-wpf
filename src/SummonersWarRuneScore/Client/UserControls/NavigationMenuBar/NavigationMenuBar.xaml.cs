using SummonersWarRuneScore.Client.Domain.Enumerations;
using SummonersWarRuneScore.Client.UserControls.NavigationMenuBar.Events;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SummonersWarRuneScore.Client.UserControls.NavigationMenuBar
{
	/// <summary>
	/// Interaction logic for NavigationMenuBar.xaml
	/// </summary>
	public partial class NavigationMenuBar : UserControl
	{
		private readonly NavigationMenuBarDataContext mDataContext;

		public string SummonerName
		{
			get => mDataContext.SummonerName;
			set => mDataContext.SummonerName = value;
		}
		public int RuneCount 
		{
			get => mDataContext.RuneCount;
			set => mDataContext.RuneCount = value;
		}
		public event EventHandler<NavigationItemClickedEventArgs> NavigationItemClicked;
		public event EventHandler<EventArgs> ImportClicked;

		public NavigationMenuBar()
		{
			InitializeComponent();

			mDataContext = new NavigationMenuBarDataContext();
			(Content as FrameworkElement).DataContext = mDataContext;

			BtnScores.IsChecked = true;
		}

		private void ScoresButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationItemClicked?.Invoke(sender, new NavigationItemClickedEventArgs(View.Scores));
		}

		private void RolesButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationItemClicked?.Invoke(sender, new NavigationItemClickedEventArgs(View.Roles));
		}

		private void AboutButton_Click(object sender, RoutedEventArgs e)
		{
			NavigationItemClicked?.Invoke(sender, new NavigationItemClickedEventArgs(View.About));
		}

		private void BtnImport_Click(object sender, RoutedEventArgs e)
		{
			ImportClicked?.Invoke(sender, new EventArgs());
		}
	}

	public class NavigationMenuBarDataContext : INotifyPropertyChanged
	{
		private string mSummonerName;
		public string SummonerName
		{
			get { return mSummonerName; }
			set
			{
				mSummonerName = value;
				NotifyPropertyChanged("SummonerName");
			}
		}

		private int mRuneCount;
		public int RuneCount
		{
			get { return mRuneCount; }
			set
			{
				mRuneCount = value;
				NotifyPropertyChanged("RuneCount");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
