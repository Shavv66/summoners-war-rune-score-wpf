using Microsoft.Win32;
using SummonersWarRuneScore.Client.Domain.Enumerations;
using SummonersWarRuneScore.Client.UserControls.NavigationMenuBar.Events;
using SummonersWarRuneScore.Client.ViewModels;
using SummonersWarRuneScore.Client.ViewModels.Domain;
using SummonersWarRuneScore.Components.DataAccess;
using SummonersWarRuneScore.Components.Filtering;
using SummonersWarRuneScore.Components.ProfileImport;
using SummonersWarRuneScore.Components.RuneScoring;
using System.ComponentModel;
using System.Windows;

namespace SummonersWarRuneScore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowDataContext MainDataContext => (MainWindowDataContext)DataContext;
	
		public MainWindow()
		{
			InitializeComponent();
			Style = (Style)FindResource(typeof(Window));
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			DataContext = new MainWindowDataContext();

			UpdateNavigationBarInfoText();
		}

		private void UpdateNavigationBarInfoText()
		{
			navigationBar.SummonerName = MainDataContext.SummonerRepository.Get()?.Name;
			navigationBar.RuneCount = MainDataContext.RuneRepository.GetAll().Count;
		}

		private void NavigationMenuBar_NavigationItemClicked(object sender, NavigationItemClickedEventArgs e)
		{
			switch (e.SelectedView)
			{
				case View.Scores: MainDataContext.CurrentView = MainDataContext.ScoresViewModel; break;
				case View.Roles: MainDataContext.CurrentView = MainDataContext.RolesViewModel; break;
				case View.About: MainDataContext.CurrentView = MainDataContext.AboutViewModel; break;
			}
		}

		private void navigationBar_ImportClicked(object sender, System.EventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				DefaultExt = ".json",
				Filter = "JSON File|*.json"
			};

			if (openFileDialog.ShowDialog() ?? false)
			{
				MainDataContext.ProfileImportService.ImportFile(openFileDialog.FileName);

				UpdateNavigationBarInfoText();

				if (MainDataContext.CurrentView is IProfileImportListener view)
				{
					view.HandleProfileImport();
				}
			}
		}
	}

	public class MainWindowDataContext : INotifyPropertyChanged
	{
		public IRuneRepository RuneRepository { get; }
		public ISummonerRepository SummonerRepository { get; }
		private readonly IMonsterRoleRepository mMonsterRoleRepository;
		public IProfileImportService ProfileImportService { get; set; }
		private readonly IRuneScoringService mRuneScoringService;
		private readonly IScoreRankingService mScoreRankingService;
		private readonly IRuneFilteringService mRuneFilteringService;
		private readonly IRuneScoreCache mRuneScoreCache;
		private readonly IScoreRankCache mScoreRankCache;

		private object mCurrentView;
		public object CurrentView
		{
			get => mCurrentView;
			set
			{
				mCurrentView = value;
				NotifyPropertyChanged("CurrentView");
			}
		}

		private ScoresViewModel mScoresViewModel;
		public ScoresViewModel ScoresViewModel
		{
			get => mScoresViewModel;
			set
			{
				mScoresViewModel = value;
				NotifyPropertyChanged("ScoresViewModel");
			}
		}

		private RolesViewModel mRolesViewModel;
		public RolesViewModel RolesViewModel
		{
			get => mRolesViewModel;
			set
			{
				mRolesViewModel = value;
				NotifyPropertyChanged("RolesViewModel");
			}
		}

		private AboutViewModel mAboutViewModel;
		public AboutViewModel AboutViewModel
		{
			get => mAboutViewModel;
			set
			{
				mAboutViewModel = value;
				NotifyPropertyChanged("AboutViewModel");
			}
		}

		public MainWindowDataContext()
		{
			RuneRepository = new RuneRepository();
			SummonerRepository = new SummonerRepository();
			mMonsterRoleRepository = new MonsterRoleRepository();
			ProfileImportService = new ProfileImportService();
			mRuneScoringService = new RuneScoringService();
			mScoreRankingService = new ScoreRankingService();
			mRuneFilteringService = new RuneFilteringService();
			mRuneScoreCache = new RuneScoreCache();
			mScoreRankCache = new ScoreRankCache();

			ScoresViewModel = new ScoresViewModel(
				RuneRepository,
				mMonsterRoleRepository,
				mRuneScoringService,
				mScoreRankingService,
				mRuneFilteringService,
				mRuneScoreCache,
				mScoreRankCache);
			RolesViewModel = new RolesViewModel();
			AboutViewModel = new AboutViewModel();
			CurrentView = ScoresViewModel;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
