using Microsoft.Win32;
using SummonersWarRuneScore.DataAccess;
using SummonersWarRuneScore.Dialogs;
using SummonersWarRuneScore.Domain;
using SummonersWarRuneScore.Domain.Enumerations;
using SummonersWarRuneScore.Filtering;
using SummonersWarRuneScore.ProfileImport;
using SummonersWarRuneScore.RuneScoring;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
		private ObservableCollection<MonsterRole> mMonsterRoles;
		IRuneScoringService mRuneScoringService;
		IScoreRankingService mScoreRankingService;
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
			mMonsterRoles = new ObservableCollection<MonsterRole>();

			mRuneRepository = new RuneRepository();
			mRunes = mRuneRepository.GetAll();

			mRuneScoringService = new RuneScoringService();
			mRuneScoreCache = new RuneScoreCache();
			mScoreRankingService = new ScoreRankingService();
			mScoreRankCache = new ScoreRankCache();

			mRuneFilteringService = new RuneFilteringService();

			mDataContext = new MainWindowDataContext();
			DataContext = mDataContext;

			mMonsterRoles.CollectionChanged += mMonsterRoles_CollectionChanged;

			cbxRuneSet.ItemsSource = Enum.GetValues(typeof(RuneSet));
			cbxRuneSet.SelectedIndex = 0;

			mAllItem = "<All>";
			cbxSlotFilter.ItemsSource = new List<string> { mAllItem, "1", "2", "3", "4", "5", "6" };
			cbxSlotFilter.SelectAll();


			cbxLocationFilter.ItemsSource = new List<string> { "Inventory", "EquippedOnMonster" };
			cbxLocationFilter.SelectedItems.Add("Inventory");
		}

		private void mMonsterRoles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			lvMonsterRoles.ItemsSource = mMonsterRoles.Select(monsterRole => monsterRole.Name);
		}

		private void cbxRuneSet_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			mMonsterRoles.Clear();
			var monsterRoles = mMonsterRoleRepository.GetByRuneSet((RuneSet)cbxRuneSet.SelectedValue);
			foreach (MonsterRole monsterRole in monsterRoles)
			{
				mMonsterRoles.Add(monsterRole);
			}
			lvMonsterRoles.SelectedIndex = 0;
			PopulateGrid();
		}

		private void lvMonsterRoles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (mMonsterRoles.Count > 0 && lvMonsterRoles.SelectedIndex >= 0)
			{
				mDataContext.SelectedMonsterRole = mMonsterRoles[lvMonsterRoles.SelectedIndex];
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			NewMonsterRoleDialog inputDialog = new NewMonsterRoleDialog();
			if (inputDialog.ShowDialog() == true)
			{
				mMonsterRoles.Add(new MonsterRole(inputDialog.Answer, (RuneSet)cbxRuneSet.SelectedValue));
				lvMonsterRoles.SelectedItem = inputDialog.Answer;
			}
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			if (lvMonsterRoles.SelectedIndex < 0) return;

			MonsterRole updatedRole;
			if (mDataContext.SelectedMonsterRole.IsNew())
			{
				updatedRole = mMonsterRoleRepository.Add(mDataContext.SelectedMonsterRole);
			}
			else
			{
				updatedRole = mMonsterRoleRepository.Update(mDataContext.SelectedMonsterRole);
			}

			mMonsterRoles[lvMonsterRoles.SelectedIndex] = updatedRole;
			mDataContext.SelectedMonsterRole = updatedRole;

			List<RuneScoringResult> updatedRoleScores = mRuneScoringService.CalculateScores(mRunes, new List<MonsterRole> { updatedRole });
			mRuneScoreCache.AddOrUpdateScores(updatedRoleScores);
			List<ScoreRankingResult> updatedRanks = mScoreRankingService.CalculateRanks(updatedRoleScores);
			mScoreRankCache.AddOrUpdateRanks(updatedRanks);

			PopulateGrid();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			mMonsterRoleRepository.Delete(mDataContext.SelectedMonsterRole.Id);
			mMonsterRoles.Remove(mDataContext.SelectedMonsterRole);
			PopulateGrid();
		}

		private void btnImport_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
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

		private void ReScoreAllRunes(List<Rune> filteredRunes)
		{
			List<RuneScoringResult> scores = mRuneScoringService.CalculateScores(filteredRunes, mMonsterRoleRepository.GetAll());
			mRuneScoreCache.SetScores(scores);
			mScoreRankCache.SetRanks(mScoreRankingService.CalculateRanks(scores));
		}

		private void PopulateGrid()
		{
			string currentSort = "";
			Tuple<string, ListSortDirection> sortedColumn = null;
			if (dtGrdRunes.ItemsSource != null)
			{
				currentSort = (dtGrdRunes.ItemsSource as DataView).Sort;
				foreach (DataGridColumn column in dtGrdRunes.Columns)
				{
					if (column.SortDirection != null)
					{
						sortedColumn = new Tuple<string, ListSortDirection>(column.SortMemberPath, column.SortDirection.Value);
					}
				}
			}

			DataTable table = new DataTable();

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

			foreach (MonsterRole role in mMonsterRoles)
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

				foreach (MonsterRole role in mMonsterRoles)
				{
					RuneScoringResult runeScore = mRuneScoreCache.GetScore(role.Id, rune.Id);
					ScoreRankingResult scoreRank = mScoreRankCache.GetRank(role.Id, rune.Id, ScoreType.Current);

					decimal score = Decimal.Round(runeScore.GetScore(ScoreType.Current), 2);
					int rank = scoreRank.Rank;

					row[role.Name] = new RankedScore(score, rank);
				}

				table.Rows.Add(row);
			}

			dtGrdRunes.ItemsSource = table.AsDataView();

			if (!String.IsNullOrEmpty(currentSort) && sortedColumn != null)
			{
				DataGridColumn sortColumn = dtGrdRunes.Columns.FirstOrDefault(column => column.SortMemberPath == sortedColumn.Item1);
				if (sortColumn != null)
				{
					(dtGrdRunes.ItemsSource as DataView).Sort = currentSort;
					sortColumn.SortDirection = sortedColumn.Item2;
				}
			}

			// Hide ID column
			dtGrdRunes.Columns[0].Visibility = Visibility.Hidden;
		}

		private Filter BuildRuneFilter()
		{
			List<IFilter> filters = new List<IFilter>();

			FilterItem setFilter = new FilterItem(RuneFilterProperty.Set, OperatorType.Equal, (RuneSet)cbxRuneSet.SelectedValue);
			filters.Add(setFilter);

			List<IFilter> slotFilter = new List<IFilter>();
			foreach (string item in cbxSlotFilter.SelectedItems)
			{
				bool isSlotNumber = int.TryParse(item, out int slot);
				if (isSlotNumber)
				{
					slotFilter.Add(new FilterItem(RuneFilterProperty.Slot, OperatorType.Equal, slot));
				}
			}
			filters.Add(new Filter(slotFilter, FilterLogic.Or));

			List<IFilter> locationFilter = new List<IFilter>();
			foreach (string item in cbxLocationFilter.SelectedItems)
			{
				locationFilter.Add(new FilterItem(RuneFilterProperty.Location, OperatorType.Equal, Enum.Parse(typeof(RuneLocation), item)));
			}
			filters.Add(new Filter(locationFilter, FilterLogic.Or));

			return new Filter(filters, FilterLogic.And);
		}

		private void cbxSlotFilter_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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

		private void cbxLocationFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PopulateGrid();
		}

		private void dtGrdRunes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataRow row = (dtGrdRunes.SelectedItem as DataRowView)?.Row;
			if (row == null)
			{
				mDataContext.SelectedRune = null;
			}
			else
			{
				Rune selectedRune = mRunes.Single(rune => rune.Id == (long)row["Rune ID"]);
				mDataContext.SelectedRune = selectedRune;
			}

			runeVisualiser.Rune = mDataContext.SelectedRune;
		}
	}

	public class MainWindowDataContext : INotifyPropertyChanged
	{
		private MonsterRole mSelectedMonsterRole;
		public MonsterRole SelectedMonsterRole
		{
			get { return mSelectedMonsterRole; }
			set
			{
				mSelectedMonsterRole = value;
				NotifyPropertyChanged("SelectedMonsterRole");
			}
		}

		private Rune mSelectedRune;
		public Rune SelectedRune
		{
			get { return mSelectedRune; }
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

	public class RankedScore : IComparable
	{
		public decimal Score { get; set; }
		public int Rank { get; set; }

		public RankedScore(decimal score, int rank)
		{
			Score = score;
			Rank = rank;
		}

		public int CompareTo(object other)
		{
			RankedScore otherScore = other as RankedScore;
			if (otherScore == null)
			{
				return 0;
			}

			return Score.CompareTo(otherScore.Score);
		}

		public override string ToString()
		{
			return $"{Score} ({Rank})";
		}
	}

	public class NullToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? Visibility.Hidden : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
