using System.ComponentModel;

namespace SummonersWarRuneScore.Client.UserControls.RuneScoringGrid.Domain
{
	public class GridSortInfo
	{
		public string ItemsSourceSort { get; }
		public string SortMemberPath { get; }
		public ListSortDirection SortDirection { get; }

		public GridSortInfo(string itemsSourceSort, string sortMemberPath, ListSortDirection sortDirection)
		{
			ItemsSourceSort = itemsSourceSort;
			SortMemberPath = sortMemberPath;
			SortDirection = sortDirection;
		}
	}
}
