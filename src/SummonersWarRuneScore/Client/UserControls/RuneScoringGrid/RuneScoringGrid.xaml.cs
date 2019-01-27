using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using SummonersWarRuneScore.Components.RuneScoring;
using SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Domain;

namespace SummonersWarRuneScore.Client.UserControls.RuneScoringGrid
{
	/// <summary>
	/// Interaction logic for RuneScoringGrid.xaml
	/// </summary>
	public partial class RuneScoringGrid : UserControl
	{
		private DataTable mTable;
		private bool mInitialised;
		private GridSortInfo mSavedSortInfo;

		private IRuneScoreCache mRuneScoreCache;
		private IScoreRankCache mScoreRankCache;
		private IReadOnlyCollection<Rune> mRunes;
		private IReadOnlyCollection<MonsterRole> mMonsterRoles;

		public event EventHandler<RuneScoringGridSelectionChangedEventArgs> SelectionChanged;

		public RuneScoringGrid()
		{
			InitializeComponent();
			mInitialised = false;
		}

		public void Initialise(IRuneScoreCache runeScoreCache, IScoreRankCache scoreRankCache, RuneScoringGridData data)
		{
			mRuneScoreCache = runeScoreCache;
			mScoreRankCache = scoreRankCache;
			
			mInitialised = true;
			Update(data);
		}

		public void Update(RuneScoringGridData data)
		{
			if (!mInitialised) return;

			SaveSort();

			mMonsterRoles = data.MonsterRoles;
			mRunes = data.Runes;

			mTable = new DataTable();

			AddStaticColumns();
			AddDynamicColumns();
			PopulateData();
			RefreshView();
		}

		private void AddStaticColumns()
		{
			mTable.Columns.Add("Rune ID", typeof(long));
			mTable.Columns.Add("Location", typeof(string));
			mTable.Columns.Add("Slot", typeof(int));
			mTable.Columns.Add("Grade", typeof(int));
			mTable.Columns.Add("Set", typeof(string));
			mTable.Columns.Add("Level", typeof(int));
			mTable.Columns.Add("Primary Stat", typeof(RuneStat));
			//mTable.Columns.Add("In-built Stat", typeof(RuneStat));
			//mTable.Columns.Add("HP%", typeof(int));
			//mTable.Columns.Add("HP", typeof(int));
			//mTable.Columns.Add("ATK%", typeof(int));
			//mTable.Columns.Add("ATK", typeof(int));
			//mTable.Columns.Add("DEF%", typeof(int));
			//mTable.Columns.Add("DEF", typeof(int));
			//mTable.Columns.Add("SPD", typeof(int));
			//mTable.Columns.Add("CRate", typeof(int));
			//mTable.Columns.Add("CDmg", typeof(int));
			//mTable.Columns.Add("RES", typeof(int));
			//mTable.Columns.Add("ACC", typeof(int));
		}

		private void AddDynamicColumns()
		{
			foreach (MonsterRole role in mMonsterRoles)
			{
				mTable.Columns.Add(new DataColumn(role.Name, typeof(RankedScore)));
			}
		}

		private void PopulateData()
		{
			mTable.BeginLoadData();

			foreach (Rune rune in mRunes)
			{
				DataRow row = mTable.NewRow();
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

					decimal score = decimal.Round(runeScore.GetScore(ScoreType.Current), 2);
					int rank = scoreRank.Rank;

					row[role.Name] = new RankedScore(score, rank);
				}

				mTable.Rows.Add(row);
			}

			mTable.EndLoadData();
		}

		private void RefreshView()
		{
			DtGrdRunes.ItemsSource = mTable.AsDataView();

			ApplySavedSort();

			// Hide ID column
			DtGrdRunes.Columns[0].Visibility = Visibility.Hidden;
		}

		private void SaveSort()
		{
			mSavedSortInfo = null;
			var currentSort = "";
			if (DtGrdRunes.ItemsSource != null)
			{
				currentSort = ((DataView)DtGrdRunes.ItemsSource).Sort;
				if (!string.IsNullOrEmpty(currentSort))
				{
					foreach (DataGridColumn column in DtGrdRunes.Columns)
					{
						if (column.SortDirection != null)
						{
							mSavedSortInfo = new GridSortInfo(currentSort, column.SortMemberPath, column.SortDirection.Value);
							break;
						}
					}
				}
			}
		}

		private void ApplySavedSort()
		{
			if (mSavedSortInfo == null) return;

			DataGridColumn sortColumn = DtGrdRunes.Columns.FirstOrDefault(column => column.SortMemberPath == mSavedSortInfo.SortMemberPath);
			if (sortColumn != null)
			{
				((DataView)DtGrdRunes.ItemsSource).Sort = mSavedSortInfo.ItemsSourceSort;
				sortColumn.SortDirection = mSavedSortInfo.SortDirection;
			}
		}

        private void DtGrdRunes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			Rune selectedRune = null;

			DataRow row = (DtGrdRunes.SelectedItem as DataRowView)?.Row;
            if (row != null)
            {
                selectedRune = mRunes.Single(rune => rune.Id == (long)row["Rune ID"]);
            }
			
			SelectionChanged?.Invoke(this, new RuneScoringGridSelectionChangedEventArgs(selectedRune));
		}
    }
}
