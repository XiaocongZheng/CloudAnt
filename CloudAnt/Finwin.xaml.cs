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
    /// Interaction logic for Finwin.xaml
    /// </summary>
    public partial class Finwin : Window
    {
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        
        
        public Finwin()
        {
            InitializeComponent();
            string token8 = GetToken();
            con.Open();
            SqlCommand cmd = new SqlCommand("map_cselect", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("From Team");
            dt.Columns.Add("From Channel");
            dt.Columns.Add("To Team");
            dt.Columns.Add("To Channel");
            dt.Columns.Add("Status");
            
            using (IDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    DataRow row = dt.NewRow();
                    
                        row["ID"] = (int)dr[0];
                        row["From Team"] = dr[1].ToString();
                        row["From Channel"] = dr[2].ToString();
                        row["To Team"] = dr[3].ToString();
                        row["To Channel"] = dr[4].ToString();
                        row["Status"] = "Success";
                        
                        dt.Rows.Add(row);
                    
                }
            }
            
            dataGridView1.ItemsSource = dt.DefaultView;
            con.Close();
            
        }

        

        private async void lab1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.DialogResult d;
            d = System.Windows.Forms.MessageBox.Show("Are you sure you want to exit the migration task?  ", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
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

        private async void btn1_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult d;
            d = System.Windows.Forms.MessageBox.Show("Are you sure you want to exit the migration task?  ", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
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
            }
        }

        public string GetToken()
        {
            string token5 = "";
            string sta_cd = "1";
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

    public class Mcdata
    {
        public int ID { get; set; }
        public string FromTeam { get; set; }
        public string FromChannel { get; set; }
        public string ToTeam { get; set; }
        public string ToChannel { get; set; }
        public string Status { get; set; }
        public string TotalMessages { get; set; }
    }
}
