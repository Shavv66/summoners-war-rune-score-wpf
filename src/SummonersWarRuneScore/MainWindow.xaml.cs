using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using SummonersWarRuneScore.DataAccess;
using SummonersWarRuneScore.Dialogs;
using SummonersWarRuneScore.Domain;
using Microsoft.Win32;
using SummonersWarRuneScore.ProfileImport;

namespace SummonersWarRuneScore
{
	public class ScoredRune
	{
		public int Id { get; private set; }
		public int MonsterId { get; private set; }
		public int Slot { get; private set; }
		
		public ScoredRune(int id, int monsterId, int slot)
		{
			Id = id;
			MonsterId = monsterId;
			Slot = slot;
		}
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainWindowDataContext mDataContext;
		IMonsterRolesRepository mMonsterRoleRepository;
		ObservableCollection<MonsterRole> mMonsterRoles;
		
		public MainWindow()
		{
			InitializeComponent();
			mMonsterRoleRepository = new MonsterRolesRepository();
			mMonsterRoles = new ObservableCollection<MonsterRole>();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			mDataContext = new MainWindowDataContext();
			DataContext = mDataContext;

			mMonsterRoles.CollectionChanged += mMonsterRoles_CollectionChanged;

			cbxRuneSet.ItemsSource = Enum.GetValues(typeof(RuneSet));
			cbxRuneSet.SelectedIndex = 0;
			
			dtGrdRunes.ItemsSource = new List<ScoredRune>
			{
				new ScoredRune(1, 1348573, 1),
				new ScoredRune(2, 5843762, 5)
			};
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
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			mMonsterRoleRepository.Delete(mDataContext.SelectedMonsterRole.Id);
			mMonsterRoles.Remove(mDataContext.SelectedMonsterRole);
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
