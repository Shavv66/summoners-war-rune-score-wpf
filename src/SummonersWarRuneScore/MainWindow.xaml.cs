using Microsoft.Win32;
using SummonersWarRuneScore.Client.UserControls.RoleManager.Events;
using SummonersWarRuneScore.Components.DataAccess;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using SummonersWarRuneScore.Components.Filtering;
using SummonersWarRuneScore.Components.ProfileImport;
using SummonersWarRuneScore.Components.RuneScoring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
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
		private List<Rune> mRunes;
		private IRuneScoreCache mRuneScoreCache;
		private IScoreRankCache mScoreRankCache;
		private IRuneFilteringService mRuneFilteringService;

		private string mAllItem;
		private bool mChangingListBoxSelection;
		
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
				PopulateGrid();
			}
		}

		private void RoleManager_OnSelectedRuneSetChanged(object sender, EventArgs e)
		{
			PopulateGrid();
		}

		private void RoleManager_OnRoleChanged(object sender, RoleChangedEventArgs e)
		{
			List<RuneScoringResult> updatedRoleScores = mRuneScoringService.CalculateScores(mRunes, new List<MonsterRole> { e.ChangedRole });
			mRuneScoreCache.AddOrUpdateScores(updatedRoleScores);
			List<ScoreRankingResult> updatedRanks = mScoreRankingService.CalculateRanks(updatedRoleScores);
			mScoreRankCache.AddOrUpdateRanks(updatedRanks);

			PopulateGrid();
		}

		private void RoleManager_OnRoleDeleted(object sender, EventArgs e)
		{
			PopulateGrid();
		}

		private void ReScoreAllRunes(List<Rune> filteredRunes)
		{
			List<RuneScoringResult> scores = mRuneScoringService.CalculateScores(filteredRunes, mMonsterRoleRepository.GetAll());
			mRuneScoreCache.SetScores(scores);
			mScoreRankCache.SetRanks(mScoreRankingService.CalculateRanks(scores));
		}

		private void PopulateGrid()
		{
			var currentSort = "";
			Tuple<string, ListSortDirection> sortedColumn = null;
			if (DtGrdRunes.ItemsSource != null)
			{
				currentSort = ((DataView)DtGrdRunes.ItemsSource).Sort;
				foreach (DataGridColumn column in DtGrdRunes.Columns)
				{
					if (column.SortDirection != null)
					{
						sortedColumn = new Tuple<string, ListSortDirection>(column.SortMemberPath, column.SortDirection.Value);
					}
				}
			}

			var table = new DataTable();

			table.Columns.Add("Rune ID", typeof(long));
			table.Columns.Add("Location", typeof(string));
			table.Columns.Add("Slot", typeof(int));
			table.Columns.Add("Grade", typeof(int));
			table.Columns.Add("Set", typeof(string));
			table.Columns.Add("Level", typeof(int));
			table.Columns.Add("Primary Stat", typeof(RuneStat));
			//table.Columns.Add("In-built Stat", typeof(RuneStat));
			//table.Columns.Add("HP%", typeof(int));
			//table.Columns.Add("HP", typeof(int));
			//table.Columns.Add("ATK%", typeof(int));
			//table.Columns.Add("ATK", typeof(int));
			//table.Columns.Add("DEF%", typeof(int));
			//table.Columns.Add("DEF", typeof(int));
			//table.Columns.Add("SPD", typeof(int));
			//table.Columns.Add("CRate", typeof(int));
			//table.Columns.Add("CDmg", typeof(int));
			//table.Columns.Add("RES", typeof(int));
			//table.Columns.Add("ACC", typeof(int));

			foreach (MonsterRole role in RoleManager.MonsterRoles)
			{
				table.Columns.Add(role.Name, typeof(RankedScore));
			}

			List<Rune> filteredRunes = mRuneFilteringService.FilterRunes(mRunes, BuildRuneFilter());
			ReScoreAllRunes(filteredRunes);

			foreach (Rune rune in filteredRunes)
			{
				DataRow row = table.NewRow();
				row["Rune ID"] = rune.Id;
				row["Location"] = rune.Location;
				row["Slot"] = rune.Slot;
				row["Grade"] = rune.Stars;
				row["Set"] = rune.Set;
				row["Level"] = rune.Level;
				row["Primary Stat"] = rune.PrimaryStat;
				//row["In-built Stat"] = rune.PrefixStat;

				//foreach (RuneStat stat in rune.Substats)
				//{
				//	switch (stat.Type)
				//	{
				//		case RuneStatType.HpPercent: row["HP%"] = stat.Amount; break;
				//		case RuneStatType.HpFlat: row["HP"] = stat.Amount; break;
				//		case RuneStatType.AtkPercent: row["ATK%"] = stat.Amount; break;
				//		case RuneStatType.AtkFlat: row["ATK"] = stat.Amount; break;
				//		case RuneStatType.DefPercent: row["DEF%"] = stat.Amount; break;
				//		case RuneStatType.DefFlat: row["DEF"] = stat.Amount; break;
				//		case RuneStatType.Spd: row["SPD"] = stat.Amount; break;
				//		case RuneStatType.CriRate: row["CRate"] = stat.Amount; break;
				//		case RuneStatType.CriDmg: row["CDmg"] = stat.Amount; break;
				//		case RuneStatType.Resistance: row["RES"] = stat.Amount; break;
				//		case RuneStatType.Accuracy: row["ACC"] = stat.Amount; break;
				//	}
				//}

				foreach (MonsterRole role in RoleManager.MonsterRoles)
				{
					RuneScoringResult runeScore = mRuneScoreCache.GetScore(role.Id, rune.Id);
					ScoreRankingResult scoreRank = mScoreRankCache.GetRank(role.Id, rune.Id, ScoreType.Current);

					decimal score = Decimal.Round(runeScore.GetScore(ScoreType.Current), 2);
					int rank = scoreRank.Rank;

					row[role.Name] = new RankedScore(score, rank);
				}

				table.Rows.Add(row);
			}

			DtGrdRunes.ItemsSource = table.AsDataView();

			if (!string.IsNullOrEmpty(currentSort) && sortedColumn != null)
			{
				DataGridColumn sortColumn = DtGrdRunes.Columns.FirstOrDefault(column => column.SortMemberPath == sortedColumn.Item1);
				if (sortColumn != null)
				{
					((DataView)DtGrdRunes.ItemsSource).Sort = currentSort;
					sortColumn.SortDirection = sortedColumn.Item2;
				}
			}

			// Hide ID column
			DtGrdRunes.Columns[0].Visibility = Visibility.Hidden;
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

			PopulateGrid();
		}

		private void CbxLocationFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PopulateGrid();
		}

		private void DtGrdRunes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataRow row = (DtGrdRunes.SelectedItem as DataRowView)?.Row;
			if (row == null)
			{
				mDataContext.SelectedRune = null;
			}
			else
			{
				Rune selectedRune = mRunes.Single(rune => rune.Id == (long)row["Rune ID"]);
				mDataContext.SelectedRune = selectedRune;
			}

			RuneVisualiser.Rune = mDataContext.SelectedRune;
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
