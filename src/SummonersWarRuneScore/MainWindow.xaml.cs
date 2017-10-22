using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using SummonersWarRuneScore.DataAccess;
using SummonersWarRuneScore.Dialogs;
using SummonersWarRuneScore.Domain;

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
		IMonsterRolesRepository mMonsterRoleRepository;
		ObservableCollection<string> mMonsterRoles;
		
		public MainWindow()
		{
			InitializeComponent();
			mMonsterRoleRepository = new MonsterRolesRepository();
			mMonsterRoles = new ObservableCollection<string>();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			cbxRuneSet.ItemsSource = Enum.GetValues(typeof(RuneSet));
			cbxRuneSet.SelectedIndex = 0;

			lvMonsterRoles.ItemsSource = mMonsterRoles;
			
			dtGrdRunes.ItemsSource = new List<ScoredRune>
			{
				new ScoredRune(1, 1348573, 1),
				new ScoredRune(2, 5843762, 5)
			};
		}

		private void cbxRuneSet_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			mMonsterRoles.Clear();
			var monsterRoles = mMonsterRoleRepository.GetByRuneSet((RuneSet)cbxRuneSet.SelectedValue);
			foreach (MonsterRole monsterRole in monsterRoles)
			{
				mMonsterRoles.Add(monsterRole.Name);
			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			NewMonsterRoleDialog inputDialog = new NewMonsterRoleDialog();
			if (inputDialog.ShowDialog() == true)
			{
				mMonsterRoles.Add(inputDialog.Answer);
				lvMonsterRoles.SelectedItem = inputDialog.Answer;
			}
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			mMonsterRoleRepository.Add(new MonsterRole((string)lvMonsterRoles.SelectedValue, (RuneSet)cbxRuneSet.SelectedValue));
		}
	}
}
