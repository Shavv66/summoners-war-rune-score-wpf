using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using SummonersWarRuneScore.Components.DataAccess;
using SummonersWarRuneScore.Components.Domain;
using SummonersWarRuneScore.Components.Domain.Enumerations;
using SummonersWarRuneScore.Domain;

namespace SummonersWarRuneScore.Dialogs
{
	/// <summary>
	/// Interaction logic for NewMonsterRoleDialog.xaml
	/// </summary>
	public partial class EditMonsterRoleDialog : Window
	{
		private readonly EditMonsterRoleDataContext mDataContext;
		private readonly IMonsterRoleRepository mMonsterRoleRepository;

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

			mMonsterRoleRepository = new MonsterRoleRepository();

			mDataContext = new EditMonsterRoleDataContext();
			(Content as FrameworkElement).DataContext = mDataContext;

			List<RuneSetCheckListItem> runeSetItems = new List<RuneSetCheckListItem>();
			foreach (RuneSet runeSet in Enum.GetValues(typeof(RuneSet)))
			{
				runeSetItems.Add(new RuneSetCheckListItem(runeSet, false));
			}
			mDataContext.RuneSetItems = runeSetItems;

			cbxRoleToClone.ItemsSource = mMonsterRoleRepository.GetAll();
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
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
			get { return mRuneSetItems; }
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
