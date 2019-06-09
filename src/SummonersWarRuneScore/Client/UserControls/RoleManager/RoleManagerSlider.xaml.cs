using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SummonersWarRuneScore.Client.UserControls.RoleManager
{
    /// <summary>
    /// Interaction logic for RoleManagerSlider.xaml
    /// </summary>
    public partial class RoleManagerSlider : UserControl
    {
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(RoleManagerSlider));
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(RoleManagerSlider));
		public string LabelText
		{
			get { return (string)GetValue(LabelTextProperty); }
			set { SetValue(LabelTextProperty, value); }
		}

		//private RoleManagerSliderDataContext mDataContext;

		//public double Value
		//{
		//	get => mDataContext.Value;
		//	set => mDataContext.Value = value;
		//}

		//public string LabelText
		//{
		//	get => mDataContext.LabelText;
		//	set => mDataContext.LabelText = value;
		//}

		public RoleManagerSlider()
        {
            InitializeComponent();

			//mDataContext = new RoleManagerSliderDataContext();
			//DataContext = mDataContext;
        }
    }

	public class RoleManagerSliderDataContext: INotifyPropertyChanged
	{
		private double mValue;
		public double Value
		{
			get => mValue;
			set
			{
				mValue = value;
				NotifyPropertyChanged("Value");
			}
		}

		private string mLabelText;
		public string LabelText
		{
			get => mLabelText;
			set
			{
				mLabelText = value;
				NotifyPropertyChanged("LabelText");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string info)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
		}
	}
}
