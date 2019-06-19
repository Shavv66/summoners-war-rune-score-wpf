using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SummonersWarRuneScore.Client.UserControls
{
	/// <summary>
	/// Interaction logic for PagingControl.xaml
	/// </summary>
	public partial class PagingControl : UserControl
	{
		private PagingControlDataContext mDataContext;

		public int Page => mDataContext.CurrentPageNumber;
		public int PageSize => mDataContext.PageSize;

		public event EventHandler<EventArgs> PageChanged;
	
		public PagingControl()
		{
			InitializeComponent();

			Loaded += PagingControl_Loaded;
		}

		public void Update(int itemCount)
		{
			mDataContext.ItemCount = itemCount;
			ChangeToPage(1);
		}

		private void PagingControl_Loaded(object sender, RoutedEventArgs e)
		{
			mDataContext = new PagingControlDataContext();
			(Content as FrameworkElement).DataContext = mDataContext;

			ChangeToPage(1);
		}

		private void ChangeToPage(int pageNumber)
		{
			if (pageNumber <= 0 || pageNumber > mDataContext.TotalPages) return;

			int currentButtonNumber = mDataContext.CurrentButtonNumber;
			if (currentButtonNumber >= 0)
			{
				GetPageButtonFromButtonNumber(currentButtonNumber).ClearValue(BackgroundProperty);
			}

			mDataContext.CurrentPageNumber = pageNumber;

			GetPageButtonFromButtonNumber(mDataContext.CurrentButtonNumber).Background = (SolidColorBrush)FindResource("ThemeStandardBrush");

			PageChanged?.Invoke(this, new EventArgs());
		}

		private Button GetPageButtonFromButtonNumber(int buttonNumber)
		{
			switch (buttonNumber)
			{
				case 0: return BtnPage0;
				case 1: return BtnPage1;
				case 2: return BtnPage2;
				case 3: return BtnPage3;
				case 4: return BtnPage4;
				default: throw new ArgumentException($"Button number must be a value from 0 to 4, but was {buttonNumber}");
			}
		}

		private void NumberButton_Click(object sender, RoutedEventArgs e)
		{
			ChangeToPage((int)((Button)sender).Content);
		}

		private void BtnFirst_Click(object sender, RoutedEventArgs e)
		{
			ChangeToPage(1);
		}

		private void BtnPrev_Click(object sender, RoutedEventArgs e)
		{
			ChangeToPage(mDataContext.CurrentPageNumber - 1);
		}

		private void BtnNext_Click(object sender, RoutedEventArgs e)
		{
			ChangeToPage(mDataContext.CurrentPageNumber + 1);
		}

		private void BtnLast_Click(object sender, RoutedEventArgs e)
		{
			ChangeToPage(mDataContext.TotalPages);
		}
	}

	public class PagingControlDataContext : INotifyPropertyChanged
	{
		private const int NUMBER_OF_PAGE_BUTTONS = 5;
	
		public int CurrentButtonNumber => VisiblePageNumbers.IndexOf(CurrentPageNumber);

		public int TotalPages => ItemCount == 0 ? 1 : (int)Math.Ceiling((decimal)ItemCount / PageSize);

		public int PageSize { get; }

		public int ItemCount { get; set; }

		private int mCurrentPageNumber;
		public int CurrentPageNumber
		{
			get => mCurrentPageNumber;
			set
			{
				mCurrentPageNumber = value;
				NotifyPropertyChanged("CurrentPageNumber");

				int firstPageButtonNumber = CurrentPageNumber - 2;
				firstPageButtonNumber -= Math.Max(2 - (TotalPages - CurrentPageNumber), 0);
				firstPageButtonNumber = Math.Max(firstPageButtonNumber, 1);

				List<int?> visiblePageNumbers = new List<int?>();
				for (int index = 0; index < NUMBER_OF_PAGE_BUTTONS; index++)
				{
					int? pageNumber = firstPageButtonNumber + index;
					if (pageNumber > TotalPages)
					{
						pageNumber = null;
					}
					visiblePageNumbers.Add(pageNumber);
				}
				VisiblePageNumbers = visiblePageNumbers;
			}
		}
	
		private List<int?> mVisiblePageNumbers;
		public List<int?> VisiblePageNumbers
		{
			get => mVisiblePageNumbers;
			set
			{
				mVisiblePageNumbers = value;
				NotifyPropertyChanged("VisiblePageNumbers");
			}
		}

		public PagingControlDataContext()
		{
			PageSize = 30;
			ItemCount = 0;
			VisiblePageNumbers = new List<int?>();
			CurrentPageNumber = 1;
		}
	
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
