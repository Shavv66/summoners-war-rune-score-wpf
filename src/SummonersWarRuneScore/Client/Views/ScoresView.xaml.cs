using SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Domain;
using SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Events;
using SummonersWarRuneScore.Client.ViewModels;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SummonersWarRuneScore.Client.Views
{
	/// <summary>
	/// Interaction logic for ScoresView.xaml
	/// </summary>
	public partial class ScoresView : UserControl
	{
		private string mAllItem;
		private bool mChangingListBoxSelection;
		private bool mInitialised;
		
		private List<Rune> mFilteredRunes;

		private ScoresViewModel mViewModel;

		public ScoresView()
		{
			InitializeComponent();

			Loaded += ScoresView_Loaded;
		}

		private async void ScoresView_Loaded(object sender, RoutedEventArgs e)
		{
			mViewModel = DataContext as ScoresViewModel;

			var updateRunesTask = mViewModel.UpdateRunes();

			CbxSetFilter.ItemsSource = Enum.GetValues(typeof(RuneSet));
			CbxSetFilter.SelectedIndex = 0;

			var getCurrentMonsterRolesTask = GetCurrentMonsterRoles();

			mAllItem = "<All>";
			CbxSlotFilter.ItemsSource = new List<string> { mAllItem, "1", "2", "3", "4", "5", "6" };
			CbxSlotFilter.SelectAll();

			CbxLocationFilter.ItemsSource = new List<string> { "Inventory", "EquippedOnMonster" };
			CbxLocationFilter.SelectedItems.Add("Inventory");

			mViewModel.SelectedRune = null;

			List<MonsterRole> monsterRoles = await getCurrentMonsterRolesTask;
			await updateRunesTask;

			FilterAndScoreRunes(monsterRoles);
			var gridData = new RuneScoringGridData
			{
				MonsterRoles = monsterRoles,
				Runes = mFilteredRunes
			};
			RuneScoringGrid.Initialise(mViewModel.RuneScoreCache, mViewModel.ScoreRankCache, gridData);
			mInitialised = true;

			mViewModel.ProfileImported += ProfileImported;
		}

		private async void UpdateGrid()
		{
			List<MonsterRole> monsterRoles = await GetCurrentMonsterRoles();

			FilterAndScoreRunes(monsterRoles);

			RuneScoringGrid.Update(new RuneScoringGridData
			{
				MonsterRoles = monsterRoles,
				Runes = mFilteredRunes
			});
		}

		private void FilterAndScoreRunes(List<MonsterRole> monsterRoles)
		{
			mFilteredRunes = mViewModel.RuneFilteringService.FilterRunes(mViewModel.Runes, BuildRuneFilter());
			List<RuneScoringResult> scores = mViewModel.RuneScoringService.CalculateScores(mFilteredRunes, monsterRoles);
			mViewModel.RuneScoreCache.SetScores(scores);
			mViewModel.ScoreRankCache.SetRanks(mViewModel.ScoreRankingService.CalculateRanks(scores));
		}

		private async Task<List<MonsterRole>> GetCurrentMonsterRoles()
		{
			return await mViewModel.MonsterRoleRepository.GetByRuneSetAsync((RuneSet)CbxSetFilter.SelectedValue);
		}

		private Filter BuildRuneFilter()
		{
			var filters = new List<IFilter>();

			var setFilter = new FilterItem(RuneFilterProperty.Set, OperatorType.Equal, (RuneSet)CbxSetFilter.SelectedValue);
			filters.Add(setFilter);

			var slotFilter = new List<IFilter>();
			foreach (string item in CbxSlotFilter.SelectedItems)
			{
				if (item != mAllItem)
				{
					slotFilter.Add(new FilterItem(RuneFilterProperty.Slot, OperatorType.Equal, byte.Parse(item)));
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

		private void ProfileImported(object sender, EventArgs e)
		{
			UpdateGrid();
		}

		private void CbxSlotFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!mInitialised || mChangingListBoxSelection) return;

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
			
			UpdateGrid();
		}

		private void CbxLocationFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!mInitialised) return;
			
			UpdateGrid();
		}

		private void RuneScoringGrid_SelectionChanged(object sender, RuneScoringGridSelectionChangedEventArgs e)
		{
			mViewModel.SelectedRune = e.SelectedRune;
			RuneVisualiser.Rune = e.SelectedRune;
		}

		private void CbxSetFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!mInitialised) return;

			UpdateGrid();
		}
	}
}
