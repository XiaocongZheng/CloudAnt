using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Identity.Client;

namespace CloudAnt
{
    /// <summary>
    /// Interaction logic for FourPage.xaml
    /// </summary>
    public partial class FourPage : Window
    {
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        public FourPage()
        {
            InitializeComponent();
        }

        private void btn1_Click_1(object sender, RoutedEventArgs e)
        {
            ThirdPage th = new ThirdPage();
            th.Show();
            this.Hide();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (s_cld.IsChecked == true) 
            {
                FivePage fig = new FivePage();
                fig.Show();
                this.Hide();
            } 
            else 
            {
                DfivePage dg = new DfivePage();
                dg.Show();
                this.Hide();
            }
        }

        private async void btn3_Click(object sender, RoutedEventArgs e)
        {
            DialogResult d;
            d = System.Windows.Forms.MessageBox.Show("Are you sure you want to cancel the migration task?  ", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == System.Windows.Forms.DialogResult.Yes)
            {
                var account = await App.PublicClientApp.GetAccountsAsync();
                if (account.Any())
                {
                    try
                    {
                        await App.PublicClientApp.RemoveAsync(account.FirstOrDefault());
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand("user_del", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.ExecuteNonQuery();
                        con.Close();


                        Close();
                        Environment.Exit(0);

                    }
                    catch (MsalException ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
                else 
                {
                    Close();
                    Environment.Exit(0);
                }
            }
        }

        private void lab2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private async void lab1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.DialogResult d;
            d = System.Windows.Forms.MessageBox.Show("Are you sure you want to cancel the migration task?  ", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == System.Windows.Forms.DialogResult.Yes)
            {
                var account = await App.PublicClientApp.GetAccountsAsync();
                if (account.Any())
                {
                    try
                    {
                        await App.PublicClientApp.RemoveAsync(account.FirstOrDefault());
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand("user_del", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.ExecuteNonQuery();
                        con.Close();


                        Close();
                        Environment.Exit(0);

                    }
                    catch (MsalException ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
                else 
                {
                    Close();
                    Environment.Exit(0);
                }
            }
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
