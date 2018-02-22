using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using SummonersWarRuneScore.DataAccess;
using SummonersWarRuneScore.Dialogs;
using SummonersWarRuneScore.Domain;
using SummonersWarRuneScore.Domain.Enumerations;
using Microsoft.Win32;
using SummonersWarRuneScore.ProfileImport;
using System.Data;
using SummonersWarRuneScore.RuneScoring;

namespace SummonersWarRuneScore
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowDataContext mDataContext;
		private IMonsterRoleRepository mMonsterRoleRepository;
		private ObservableCollection<MonsterRole> mMonsterRoles;
		IRuneScoringService mRuneScoringService;
		private List<Rune> mRunes;
		private RuneScoreCache mRuneScoreCache;
		
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			mMonsterRoleRepository = new MonsterRoleRepository();
			mMonsterRoles = new ObservableCollection<MonsterRole>();

			IRuneRepository runeRepository = new RuneRepository();
			mRunes = runeRepository.GetAll();

			mRuneScoringService = new RuneScoringService();
			mRuneScoreCache = new RuneScoreCache();
			mRuneScoreCache.SetScores(mRuneScoringService.CalculateScores(mRunes, mMonsterRoleRepository.GetAll()));

			mDataContext = new MainWindowDataContext();
			DataContext = mDataContext;

			mMonsterRoles.CollectionChanged += mMonsterRoles_CollectionChanged;

			cbxRuneSet.ItemsSource = Enum.GetValues(typeof(RuneSet));
			cbxRuneSet.SelectedIndex = 0;

			cbxSlotFilter.ItemsSource = new List<string> { "<All>", "1", "2", "3", "4", "5", "6" };
			cbxSlotFilter.SelectedIndex = 0;
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
				PopulateGrid();
			}
		}

		private void PopulateGrid()
		{
			DataTable table = new DataTable();

			table.Columns.Add("Rune ID", typeof(long));
			table.Columns.Add("Location", typeof(string));
			table.Columns.Add("Slot", typeof(int));
			table.Columns.Add("Grade", typeof(int));
			table.Columns.Add("Set", typeof(string));
			table.Columns.Add("Level", typeof(int));
			table.Columns.Add("Primary Stat", typeof(string));
			table.Columns.Add("Primary Stat Amount", typeof(int));
			table.Columns.Add("In-built Stat", typeof(string));
			table.Columns.Add("In-built Stat Amount", typeof(int));
			table.Columns.Add("HP%", typeof(int));
			table.Columns.Add("HP", typeof(int));
			table.Columns.Add("ATK%", typeof(int));
			table.Columns.Add("ATK", typeof(int));
			table.Columns.Add("DEF%", typeof(int));
			table.Columns.Add("DEF", typeof(int));
			table.Columns.Add("SPD", typeof(int));
			table.Columns.Add("CRate", typeof(int));
			table.Columns.Add("CDmg", typeof(int));
			table.Columns.Add("RES", typeof(int));
			table.Columns.Add("ACC", typeof(int));

			foreach (MonsterRole role in mMonsterRoles)
			{
				table.Columns.Add(role.Name, typeof(decimal));
			}

			foreach (Rune rune in mRunes.Where(rune => rune.Set == (RuneSet)cbxRuneSet.SelectedValue))
			{
				DataRow row = table.NewRow();
				row["Rune ID"] = rune.Id;
				row["Location"] = rune.Location;
				row["Slot"] = rune.Slot;
				row["Grade"] = rune.Stars;
				row["Set"] = rune.Set;
				row["Level"] = rune.Level;
				row["Primary Stat"] = rune.PrimaryStat.Type;
				row["Primary Stat Amount"] = rune.PrimaryStat.Amount;
				row["In-built Stat"] = rune.PrefixStat.Type == 0 ? "" : rune.PrefixStat.Type.ToString();
				row["In-built Stat Amount"] = rune.PrefixStat.Amount;

				foreach (RuneStat stat in rune.Substats)
				{
					switch (stat.Type)
					{
						case RuneStatType.HpPercent: row["HP%"] = stat.Amount; break;
						case RuneStatType.HpFlat: row["HP"] = stat.Amount; break;
						case RuneStatType.AtkPercent: row["ATK%"] = stat.Amount; break;
						case RuneStatType.AtkFlat: row["ATK"] = stat.Amount; break;
						case RuneStatType.DefPercent: row["DEF%"] = stat.Amount; break;
						case RuneStatType.DefFlat: row["DEF"] = stat.Amount; break;
						case RuneStatType.Spd: row["SPD"] = stat.Amount; break;
						case RuneStatType.CriRate: row["CRate"] = stat.Amount; break;
						case RuneStatType.CriDmg: row["CDmg"] = stat.Amount; break;
						case RuneStatType.Resistance: row["RES"] = stat.Amount; break;
						case RuneStatType.Accuracy: row["ACC"] = stat.Amount; break;
					}
				}

				foreach (MonsterRole role in mMonsterRoles)
				{
					RuneScoringResult runeScore = mRuneScoreCache.GetScore(rune.Set, role.Name, rune.Id);
					row[role.Name] = Decimal.Round(runeScore.CurrentScore, 2);
				}

				table.Rows.Add(row);
			}

			dtGrdRunes.ItemsSource = table.DefaultView;
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

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
