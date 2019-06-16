using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SummonersWarRuneScore.Components.DataAccess;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;

namespace SummonersWarRuneScore.Client.Dialogs
{
	/// <summary>
	/// Interaction logic for NewMonsterRoleDialog.xaml
	/// </summary>
	public partial class EditMonsterRoleDialog : Window
	{
		private EditMonsterRoleDataContext mDataContext;
		private IReadOnlyList<MonsterRole> mAllMonsterRoles;

		public string RoleName => txtRoleName.Text;

		public List<RuneSet> RuneSets
		{
			get
			{
				return mDataContext.RuneSetItems.Where(runeSetItem => runeSetItem.IsChecked).Select(runeSetItem => runeSetItem.RuneSet).ToList();
			}
		}

		public bool CloneExistingWeights => chkCloneWeights.IsChecked ?? false;

		public MonsterRole RoleToClone => (MonsterRole)cbxRoleToClone.SelectedValue;

		public EditMonsterRoleDialog()
		{
			InitializeComponent();

			Loaded += EditMonsterRoleDialog_Loaded;
		}

		private async void EditMonsterRoleDialog_Loaded(object sender, RoutedEventArgs e)
		{
			Task<List<MonsterRole>> getMonsterRolesTask = new MonsterRoleRepository().GetAllAsync();

			mDataContext = new EditMonsterRoleDataContext();
			((FrameworkElement)Content).DataContext = mDataContext;

			var runeSetItems = new List<RuneSetCheckListItem>();
			foreach (RuneSet runeSet in Enum.GetValues(typeof(RuneSet)))
			{
				runeSetItems.Add(new RuneSetCheckListItem(runeSet, false));
			}
			mDataContext.RuneSetItems = runeSetItems;

			mAllMonsterRoles = await getMonsterRolesTask;
			cbxRoleToClone.ItemsSource = mAllMonsterRoles;
		}

		private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
		{
			if (!ValidateRole())
			{
				e.Handled = true;
				return;
			}

			DialogResult = true;
		}

		private bool ValidateRole()
		{
			if (string.IsNullOrEmpty(RoleName))
			{
				MessageBox.Show(this, "Role name cannot be empty", "Invalid Role", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (RuneSets.Count == 0)
			{
				MessageBox.Show(this, "Please select at least one rune set", "Invalid Role", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (mAllMonsterRoles.Any(monsterRole => monsterRole.Name == RoleName))
			{
				MessageBox.Show(this, "That role name already exists", "Invalid Role", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			return true;
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			txtRoleName.SelectAll();
			txtRoleName.Focus();
		}
	}

	public class EditMonsterRoleDataContext : INotifyPropertyChanged
	{
		private List<RuneSetCheckListItem> mRuneSetItems;
		public List<RuneSetCheckListItem> RuneSetItems
		{
			get => mRuneSetItems;
			set
			{
				mRuneSetItems = value;
				NotifyPropertyChanged("RuneSetItems");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}

	public class RuneSetCheckListItem
	{
		public RuneSet RuneSet { get; set; }
		public bool IsChecked { get; set; }

		public RuneSetCheckListItem(RuneSet runeSet, bool initialCheckState)
		{
			RuneSet = runeSet;
			IsChecked = initialCheckState;
		}
	}
}
