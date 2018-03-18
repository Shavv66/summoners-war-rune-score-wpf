using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SummonersWarRuneScore.Client.Dialogs;
using SummonersWarRuneScore.Client.UserControls.RoleManager.Events;
using SummonersWarRuneScore.Components.DataAccess;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using Xceed.Wpf.AvalonDock.Controls;

namespace SummonersWarRuneScore.Client.UserControls.RoleManager
{
	/// <summary>
	/// Interaction logic for RoleManager.xaml
	/// </summary>
	public partial class RoleManager : UserControl
	{
		private readonly RoleManagerDataContext mDataContext;
		private readonly IMonsterRoleRepository mMonsterRoleRepository;

		public ObservableCollection<MonsterRole> MonsterRoles { get; }
		public RuneSet SelectedRuneSet => (RuneSet)CbxRuneSet.SelectedValue;

		public event EventHandler SelectedRuneSetChanged;
		public event EventHandler<RoleChangedEventArgs> RoleChanged;
		public event EventHandler RoleDeleted;

		public RoleManager()
		{
			InitializeComponent();

			mDataContext = new RoleManagerDataContext();
			((FrameworkElement)Content).DataContext = mDataContext;

			mMonsterRoleRepository = new MonsterRoleRepository();

			MonsterRoles = new ObservableCollection<MonsterRole>();
			MonsterRoles.CollectionChanged += MonsterRoles_CollectionChanged;

			CbxRuneSet.ItemsSource = Enum.GetValues(typeof(RuneSet));
			CbxRuneSet.SelectedIndex = 0;
		}

		private void MonsterRoles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			LvMonsterRoles.ItemsSource = MonsterRoles.Select(monsterRole => monsterRole.Name);
		}

		private void CbxRuneSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MonsterRoles.Clear();
			List<MonsterRole> monsterRoles = mMonsterRoleRepository.GetByRuneSet((RuneSet)CbxRuneSet.SelectedValue);
			foreach (MonsterRole monsterRole in monsterRoles)
			{
				MonsterRoles.Add(monsterRole);
			}
			LvMonsterRoles.SelectedIndex = 0;
			SelectedRuneSetChanged?.Invoke(this, EventArgs.Empty);
		}

		private void LvMonsterRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (MonsterRoles.Count > 0 && LvMonsterRoles.SelectedIndex >= 0)
			{
				mDataContext.SelectedMonsterRole = MonsterRoles[LvMonsterRoles.SelectedIndex];
			}
		}

		private void BtnAdd_Click(object sender, RoutedEventArgs e)
		{
			var inputDialog = new EditMonsterRoleDialog
			{
				Owner = this.FindLogicalAncestor<Window>()
			};

			if (inputDialog.ShowDialog() == true)
			{
				var newRole = new MonsterRole(inputDialog.RoleName, inputDialog.RuneSets);
				if (inputDialog.CloneExistingWeights && inputDialog.RoleToClone != null)
				{
					newRole.CopyWeightsFrom(inputDialog.RoleToClone);
				}

				MonsterRoles.Add(newRole);
				LvMonsterRoles.SelectedItem = inputDialog.RoleName;
			}
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			if (LvMonsterRoles.SelectedIndex < 0) return;

			MonsterRole updatedRole;
			if (mDataContext.SelectedMonsterRole.IsNew())
			{
				updatedRole = mMonsterRoleRepository.Add(mDataContext.SelectedMonsterRole);
			}
			else
			{
				updatedRole = mMonsterRoleRepository.Update(mDataContext.SelectedMonsterRole);
			}

			MonsterRoles[LvMonsterRoles.SelectedIndex] = updatedRole;
			mDataContext.SelectedMonsterRole = updatedRole;

			RoleChanged?.Invoke(this, new RoleChangedEventArgs(updatedRole));
		}

		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show(this.FindLogicalAncestor<Window>(), $"Are you sure you want to delete role '{mDataContext.SelectedMonsterRole.Name}'?", "Delete Monster Role", MessageBoxButton.YesNo, MessageBoxImage.Warning)
			    == MessageBoxResult.Yes)
			{
				mMonsterRoleRepository.Delete(mDataContext.SelectedMonsterRole.Id);
				MonsterRoles.Remove(mDataContext.SelectedMonsterRole);
				RoleDeleted?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public class RoleManagerDataContext : INotifyPropertyChanged
	{
		private MonsterRole mSelectedMonsterRole;
		public MonsterRole SelectedMonsterRole
		{
			get => mSelectedMonsterRole;
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
