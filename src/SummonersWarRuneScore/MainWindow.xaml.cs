using Microsoft.Win32;
using SummonersWarRuneScore.Client.UserControls.RoleManager.Events;
using SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Domain;
using SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Events;
using SummonersWarRuneScore.Components.DataAccess;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using SummonersWarRuneScore.Components.Filtering;
using SummonersWarRuneScore.Components.ProfileImport;
using SummonersWarRuneScore.Components.RuneScoring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SummonersWarRuneScore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowDataContext mDataContext;

		private IRuneRepository mRuneRepository;
		private IMonsterRoleRepository mMonsterRoleRepository;

		private IRuneScoringService mRuneScoringService;
		private IScoreRankingService mScoreRankingService;
		private IRuneFilteringService mRuneFilteringService;
		
		private IRuneScoreCache mRuneScoreCache;
		private IScoreRankCache mScoreRankCache;

		private string mAllItem;
		private bool mChangingListBoxSelection;

		private List<Rune> mRunes;
		private List<Rune> mFilteredRunes;
		
		public MainWindow()
		{
			InitializeComponent();
			Style = (Style)FindResource(typeof(Window));
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			mMonsterRoleRepository = new MonsterRoleRepository();

			mRuneRepository = new RuneRepository();
			mRunes = mRuneRepository.GetAll();

			mRuneScoringService = new RuneScoringService();
			mRuneScoreCache = new RuneScoreCache();
			mScoreRankingService = new ScoreRankingService();
			mScoreRankCache = new ScoreRankCache();

			mRuneFilteringService = new RuneFilteringService();

			mDataContext = new MainWindowDataContext();
			DataContext = mDataContext;

			mAllItem = "<All>";
			CbxSlotFilter.ItemsSource = new List<string> { mAllItem, "1", "2", "3", "4", "5", "6" };
			CbxSlotFilter.SelectAll();

			CbxLocationFilter.ItemsSource = new List<string> { "Inventory", "EquippedOnMonster" };
			CbxLocationFilter.SelectedItems.Add("Inventory");
			
			FilterAndScoreRunes();
			var gridData = new RuneScoringGridData
			{
				MonsterRoles = GetCurrentMonsterRoles(),
				Runes = mFilteredRunes
			};
			RuneScoringGrid.Initialise(mRuneScoreCache, mScoreRankCache, gridData);
		}

		private void BtnImport_Click(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				DefaultExt = ".json",
				Filter = "JSON File|*.json"
			};

			if (openFileDialog.ShowDialog() ?? false)
			{
				IProfileImportService profileImportService = new ProfileImportService();
				profileImportService.ImportFile(openFileDialog.FileName);
				mRunes = mRuneRepository.GetAll();
				
				FilterAndScoreRunes();
				UpdateGrid();
			}
		}

		private void RoleManager_OnSelectedRuneSetChanged(object sender, EventArgs e)
		{
			FilterAndScoreRunes();
			UpdateGrid();
		}

		private void RoleManager_OnRoleChanged(object sender, RoleChangedEventArgs e)
		{
			List<RuneScoringResult> updatedRoleScores = mRuneScoringService.CalculateScores(mRunes, new List<MonsterRole> { e.ChangedRole });
			mRuneScoreCache.AddOrUpdateScores(updatedRoleScores);
			List<ScoreRankingResult> updatedRanks = mScoreRankingService.CalculateRanks(updatedRoleScores);
			mScoreRankCache.AddOrUpdateRanks(updatedRanks);

			UpdateGrid();
		}

		private void RoleManager_OnRoleDeleted(object sender, EventArgs e)
		{
			UpdateGrid();
		}

		private void UpdateGrid()
		{
			RuneScoringGrid.Update(new RuneScoringGridData
			{
				MonsterRoles = GetCurrentMonsterRoles(),
				Runes = mFilteredRunes
			});
		}

		private void FilterAndScoreRunes()
		{
			mFilteredRunes = mRuneFilteringService.FilterRunes(mRunes, BuildRuneFilter());
			List<RuneScoringResult> scores = mRuneScoringService.CalculateScores(mFilteredRunes, GetCurrentMonsterRoles());
			mRuneScoreCache.SetScores(scores);
			mScoreRankCache.SetRanks(mScoreRankingService.CalculateRanks(scores));
		}

		private List<MonsterRole> GetCurrentMonsterRoles()
		{
			return mMonsterRoleRepository.GetByRuneSet(RoleManager.SelectedRuneSet);
		}

		private Filter BuildRuneFilter()
		{
			var filters = new List<IFilter>();

			var setFilter = new FilterItem(RuneFilterProperty.Set, OperatorType.Equal, RoleManager.SelectedRuneSet);
			filters.Add(setFilter);

			var slotFilter = new List<IFilter>();
			foreach (string item in CbxSlotFilter.SelectedItems)
			{
				bool isSlotNumber = int.TryParse(item, out int slot);
				if (isSlotNumber)
				{
					slotFilter.Add(new FilterItem(RuneFilterProperty.Slot, OperatorType.Equal, slot));
				}
			}
			filters.Add(new Filter(slotFilter, FilterLogic.Or));

			var locationFilter = new List<IFilter>();
			foreach (string item in CbxLocationFilter.SelectedItems)
			{
				locationFilter.Add(new FilterItem(RuneFilterProperty.Location, OperatorType.Equal, Enum.Parse(typeof(RuneLocation), item)));
			}
			filters.Add(new Filter(locationFilter, FilterLogic.Or));

			return new Filter(filters, FilterLogic.And);
		}

		private void CbxSlotFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (mChangingListBoxSelection)
			{
				return;
			}

			mChangingListBoxSelection = true;

			ListBox listBox = (ListBox)sender;
			bool added = e.AddedItems.Count > 0;
			string selectedItem = (string)(added ? e.AddedItems[0] : e.RemovedItems[0]);
			if (selectedItem == mAllItem)
			{
				if (added)
				{
					listBox.SelectAll();
				}
				else
				{
					listBox.SelectedIndex = -1;
				}
			}
			else
			{
				if (added)
				{
					if (listBox.SelectedItems.Count == 6)
					{
						listBox.SelectedItems.Add(mAllItem);
					}
				}
				else
				{
					listBox.SelectedItems.Remove(mAllItem);
				}
			}

			mChangingListBoxSelection = false;

			FilterAndScoreRunes();
			UpdateGrid();
		}

		private void CbxLocationFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			FilterAndScoreRunes();
			UpdateGrid();
		}

		private void RuneScoringGrid_SelectionChanged(object sender, RuneScoringGridSelectionChangedEventArgs e)
		{
			mDataContext.SelectedRune = e.SelectedRune;
			RuneVisualiser.Rune = e.SelectedRune;
		}
	}

	public class MainWindowDataContext : INotifyPropertyChanged
	{
		private Rune mSelectedRune;
		public Rune SelectedRune
		{
			get => mSelectedRune;
			set
			{
				mSelectedRune = value;
				NotifyPropertyChanged("SelectedRune");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
