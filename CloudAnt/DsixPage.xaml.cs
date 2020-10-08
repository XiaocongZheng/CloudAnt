using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for DsixPage.xaml
    /// </summary>
    public partial class DsixPage : Window
    {
        private delegate void SetprogressBarHandle(int vaule, int count);
        private delegate void progressBarHandle(int vaule, int count);
        public List<TeamItem> list1;
        public List<TeamItem> list2;
        public string token1 = "";
        public string token2 = "";
        public int m, n = 0;
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        private void progressBar(int vaule, int count)
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.Invoke(new progressBarHandle(this.progressBar), vaule, count);
            }
            else
            {
                if (vaule == count)
                {
                    m = vaule;
                }
                else 
                {
                    n = count;
                }
                if ((m != 0 & m == 101) & (n != 0 & n == 102)) 
                {
                    DsevPage dp = new DsevPage();
                    dp.Show();
                    this.Hide();
                }
            }
        }

        private void SetprogressBar1(int vaule, int count)
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.Invoke(new SetprogressBarHandle(this.SetprogressBar1), vaule, count);
            }
            else
            {
                if (vaule == count) 
                {
                    double percent = (vaule * 100) / count;

                    tbar.Value = Math.Round(percent, 0);
                    StatusTextBox.Text = "Completed";
                    progressBar(101, 101);
                } 
                else
                {
                    double percent = (vaule * 100) / count;

                    tbar.Value = Math.Round(percent, 0);

                    StatusTextBox.Text = Math.Round(percent, 0) + "% completed";
                }
                
            }
        }

        private void SetprogressBar2(int vaule, int count)
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.Invoke(new SetprogressBarHandle(this.SetprogressBar2), vaule, count);
            }
            else
            {

                if (vaule == count)
                {
                    double percent = (vaule * 100) / count;

                    pbar.Value = Math.Round(percent, 0);
                    StatusTextBox1.Text = "Completed";
                    progressBar(103, 102);
                }
                else
                {
                    double percent = (vaule * 100) / count;

                    pbar.Value = Math.Round(percent, 0);

                    StatusTextBox1.Text = Math.Round(percent, 0) + "% completed";
                }
            }
        }
        public DsixPage()
        {
            InitializeComponent();
            string sta_cd = "1";
            string sta_cd1 = "2";
            token1 = GetToken(sta_cd);
            token2 = GetToken(sta_cd1);
            Thread mThread = new Thread(ThreadProcess);
            mThread.Start();
            StatusTextBox.Text = "Starting...";
            Thread mtThread = new Thread(ThreadProcesses);
            mtThread.Start();
            StatusTextBox1.Text = "Starting...";
        }

        
        private async void ThreadProcess(object obj)
        {
            
            if (token1 == "")
            {
                DialogResult d;
                d = System.Windows.Forms.MessageBox.Show("Please login your account ", "Login required", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (d == System.Windows.Forms.DialogResult.Yes)
                {
                    Thread.Sleep(50);
                    FourPage fo = new FourPage();
                    fo.Show();
                    this.Hide();
                }
                if (d == System.Windows.Forms.DialogResult.No)
                {
                    Close();
                    Environment.Exit(0);
                }
            }
            else
            {
                FunctionStep1 step1 = new FunctionStep1();
                list1 = step1.GetMeJoinedTeam(token1);
                for (int i=0;i<list1.Count;i++)
                {
                    await testFuctions(token1, list1[i].id, 1);
                    int c = i + 1;
                    SetprogressBar1(c, list1.Count);
                    Thread.Sleep(5);
                }
            }
        }

        private async void ThreadProcesses(object obj)
        {
            if (token2 == "")
            {
                DialogResult d;
                d = System.Windows.Forms.MessageBox.Show("Please login your account ", "Login required", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (d == System.Windows.Forms.DialogResult.Yes)
                {
                    Thread.Sleep(50);
                    FourPage fo = new FourPage();
                    fo.Show();
                    this.Hide();
                }
                if (d == System.Windows.Forms.DialogResult.No)
                {
                    Close();
                    Environment.Exit(0);
                }
            }
            else
            {
                FunctionStep1 step4 = new FunctionStep1();
                list2 = step4.GetMeJoinedTeamTo(token2);
                for (int i = 0; i < list2.Count; i++)
                {
                    await testFuction(token2, list2[i].id, 2);
                    int c = i + 1;
                    SetprogressBar2(c, list2.Count);
                    Thread.Sleep(5);
                }
            }
        }

        private Task testFuctions(string AccessToken, string TeamID, int typ) 
        {
            return Task.Run(()=>
            {
                FunctionStep1 step2 = new FunctionStep1();
                step2.GetMeJoinedChanel(AccessToken, TeamID, typ);
            });
        }

        private Task testFuction(string AccessToken, string TeamID, int typ)
        {
            return Task.Run(() =>
            {
                FunctionStep1 step3 = new FunctionStep1();
                step3.GetMeJoinedChanel(AccessToken, TeamID, typ);
            });
        }

        private void lab2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void tbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        public string GetToken(string sta_cd)
        {
            string token5 = "";
            con.Open();
            SqlCommand cmd2 = new SqlCommand("select_token", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@sta_cd", sta_cd);
            using (SqlDataReader reader = cmd2.ExecuteReader())
            {
                if (reader.Read())
                {
                    token5 = reader["access_token"].ToString();
                }
            }
            con.Close();
            return token5;
        }
    }
}
