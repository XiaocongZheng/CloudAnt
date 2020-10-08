using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CloudAnt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer tmr;
        TimeSpan tim;
        public MainWindow()
        {
            InitializeComponent();
            tim = TimeSpan.FromSeconds(4);
            tmr = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                if (tim == TimeSpan.Zero) 
                {
                    tmr.Stop();
                    SeccPage se = new SeccPage();
                    se.Show();
                    this.Hide();
                };
                tim = tim.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Duration duration = new Duration(TimeSpan.FromSeconds(5));
            DoubleAnimation doubleanimation = new DoubleAnimation(100.0, duration);
            bar1.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
