using SummonersWarRuneScore.Client.Domain.Enumerations;

namespace SummonersWarRuneScore.Client.UserControls.NavigationMenuBar.Events
{
	public class NavigationItemClickedEventArgs
	{
		public View SelectedView { get; }

		public NavigationItemClickedEventArgs(View selectedView)
		{
			SelectedView = selectedView;
		}
	}
}
