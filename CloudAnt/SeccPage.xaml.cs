using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;


namespace CloudAnt
{
    /// <summary>
    /// Interaction logic for SeccPage.xaml
    /// </summary>
    public partial class SeccPage : Window
    {
        
        public SeccPage()
        {
            InitializeComponent();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void  btn2_Click(object sender, RoutedEventArgs e)
        {
            if (ckBox1.IsChecked == false || ckBox2.IsChecked == false || ckBox3.IsChecked == false)
            {
                DialogResult m;
                m = System.Windows.Forms.MessageBox.Show("Please review and accept the following terms before continuing.", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (m == System.Windows.Forms.DialogResult.Yes)
                {
                    ckBox1.IsChecked = true;
                    ckBox2.IsChecked = true;
                    ckBox3.IsChecked = true;
                }
            }
            else if (ckBox1.IsChecked == true & ckBox2.IsChecked == true & ckBox3.IsChecked == true) 
            {
                ThirdPage th = new ThirdPage();
                th.Show();
                this.Hide();
            }
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult d;
            d = System.Windows.Forms.MessageBox.Show("Are you sure you want to cancel the migration task?  ", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == System.Windows.Forms.DialogResult.Yes)
            {
                Close();
                Environment.Exit(0);
            }
        }

        private void lab2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void lab1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.DialogResult d;
            d = System.Windows.Forms.MessageBox.Show("Are you sure you want to cancel the migration task?  ", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == System.Windows.Forms.DialogResult.Yes)
            {
                Close();
                Environment.Exit(0);
            }
        }

        
    }
}
