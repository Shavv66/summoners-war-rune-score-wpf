using System;
using System.Windows;

namespace SummonersWarRuneScore.Dialogs
{
	/// <summary>
	/// Interaction logic for NewMonsterRoleDialog.xaml
	/// </summary>
	public partial class NewMonsterRoleDialog : Window
	{
		public NewMonsterRoleDialog()
		{
			InitializeComponent();
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			txtAnswer.SelectAll();
			txtAnswer.Focus();
		}

		public string Answer
		{
			get { return txtAnswer.Text; }
		}
	}
}
