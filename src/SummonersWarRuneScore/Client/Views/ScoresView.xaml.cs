using Microsoft.Win32;
using SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Domain;
using SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Events;
using SummonersWarRuneScore.Client.ViewModels;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using SummonersWarRuneScore.Components.ProfileImport;
using System;
using System.Collections.Generic;
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
		
		private List<Rune> mFilteredRunes;

		private ScoresViewModel ViewModel => DataContext as ScoresViewModel;

		public ScoresView()
		{
			InitializeComponent();

			Loaded += ScoresView_Loaded;
		}

		private void ScoresView_Loaded(object sender, RoutedEventArgs e)
		{
			ViewModel.ProfileImported += ProfileImported;

			ViewModel.UpdateRunes();

			CbxSetFilter.ItemsSource = Enum.GetValues(typeof(RuneSet));
			CbxSetFilter.SelectedIndex = 0;

			mAllItem = "<All>";
			CbxSlotFilter.ItemsSource = new List<string> { mAllItem, "1", "2", "3", "4", "5", "6" };
			CbxSlotFilter.SelectAll();

			CbxLocationFilter.ItemsSource = new List<string> { "Inventory", "EquippedOnMonster" };
			CbxLocationFilter.SelectedItems.Add("Inventory");

			ViewModel.SelectedRune = null;

			FilterAndScoreRunes();
			var gridData = new RuneScoringGridData
			{
				MonsterRoles = GetCurrentMonsterRoles(),
				Runes = mFilteredRunes
			};
			RuneScoringGrid.Initialise(ViewModel.RuneScoreCache, ViewModel.ScoreRankCache, gridData);
		}

		private void UpdateGrid()
		{
			FilterAndScoreRunes();

			RuneScoringGrid.Update(new RuneScoringGridData
			{
				MonsterRoles = GetCurrentMonsterRoles(),
				Runes = mFilteredRunes
			});
		}

		private void FilterAndScoreRunes()
		{
			mFilteredRunes = ViewModel.RuneFilteringService.FilterRunes(ViewModel.Runes, BuildRuneFilter());
			List<RuneScoringResult> scores = ViewModel.RuneScoringService.CalculateScores(mFilteredRunes, GetCurrentMonsterRoles());
			ViewModel.RuneScoreCache.SetScores(scores);
			ViewModel.ScoreRankCache.SetRanks(ViewModel.ScoreRankingService.CalculateRanks(scores));
		}

		private List<MonsterRole> GetCurrentMonsterRoles()
		{
			return ViewModel.MonsterRoleRepository.GetByRuneSet((RuneSet)CbxSetFilter.SelectedValue);
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
			
			UpdateGrid();
		}

		private void CbxLocationFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateGrid();
		}

		private void RuneScoringGrid_SelectionChanged(object sender, RuneScoringGridSelectionChangedEventArgs e)
		{
			ViewModel.SelectedRune = e.SelectedRune;
			RuneVisualiser.Rune = e.SelectedRune;
		}

		private void CbxSetFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateGrid();
		}
	}
}
