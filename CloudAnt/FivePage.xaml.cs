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

namespace CloudAnt
{
    /// <summary>
    /// Interaction logic for FivePage.xaml
    /// </summary>
    public partial class FivePage : Window
    {
        BackgroundWorker worker;
        public List<TeamItem> list1;
        public string token1 = "";
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        public FivePage()
        {
            InitializeComponent();
            con.Open();
            SqlCommand cmd1 = new SqlCommand("select_atoken", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader reader = cmd1.ExecuteReader())
            {
                if (reader.Read()) { token1 = reader["access_token"].ToString(); }
            }
            con.Close();

            if (token1 == "")
            {
                DialogResult d;
                d = System.Windows.Forms.MessageBox.Show("Please login your account ", "Login required", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (d == System.Windows.Forms.DialogResult.Yes)
                {
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
            }

            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = false;

            int maxItems = list1.Count;
            pbar.Minimum = 1;
            pbar.Maximum = 100;

            StatusTextBox.Text = "Starting...";
            worker.RunWorkerAsync(maxItems);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusTextBox.Text = "Completed";
            Sixpage si = new Sixpage();
            si.Show();
            this.Hide();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double percent = (e.ProgressPercentage * 100) / list1.Count;

            pbar.Value = Math.Round(percent, 0);
            StatusTextBox.Text = Math.Round(percent, 0) + "% completed";
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (list1 != null) 
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                int? maxItems = e.Argument as int?;
                for (int i = 0; i < list1.Count; i++)
                {
                    string tid = list1[i].id;
                    FunctionStep1 f = new FunctionStep1();
                    f.GetMeJoinedChanel(token1, tid, 1);
                    Thread.Sleep(50);
                    int c = i + 1;
                    worker.ReportProgress(c);
                }
            }
            
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

        private void pbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
