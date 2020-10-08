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
    /// Interaction logic for Sixpage.xaml
    /// </summary>
    public partial class Sixpage : Window
    {
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        public List<GetTeamItem> list1 = new List<GetTeamItem>();
        public string token1 = "";
        public Sixpage()
        {
            InitializeComponent();
            con.Open();
            SqlCommand cmd = new SqlCommand("select_team", con);
            cmd.CommandType = CommandType.StoredProcedure;
            using (IDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    GetTeamItem at = new GetTeamItem();
                    at.id = dr[1].ToString();
                    at.displayName = dr[2].ToString();
                    list1.Add(at);
                }
            }
            con.Close();
            foreach (GetTeamItem gt in list1)
            {
                ComboboxItem it = new ComboboxItem();
                it.Text = gt.displayName;
                it.Value = gt.id;
                comboBox1.Items.Add(it);
                comboBox4.Items.Add(it);
            }
        }

        private void remap_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd1 = new SqlCommand("map_del", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.ExecuteNonQuery();
            con.Close();
            dataGridView1.ItemsSource = null;
        }

        private void addmap_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(comboBox1.Text) || String.IsNullOrEmpty(comboBox2.Text) || String.IsNullOrEmpty(comboBox3.Text) || String.IsNullOrEmpty(comboBox4.Text))
            {
                System.Windows.Forms.MessageBox.Show("Please select all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string cunt = "";
                string f_t_id = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
                string f_c_id = (comboBox2.SelectedItem as ComboboxItem).Value.ToString();
                string t_t_id = (comboBox4.SelectedItem as ComboboxItem).Value.ToString();
                string t_c_id = (comboBox3.SelectedItem as ComboboxItem).Value.ToString();
                string f_t_nm = (comboBox1.SelectedItem as ComboboxItem).Text.ToString();
                string f_c_nm = (comboBox2.SelectedItem as ComboboxItem).Text.ToString();
                string t_t_nm = (comboBox4.SelectedItem as ComboboxItem).Text.ToString();
                string t_c_nm = (comboBox3.SelectedItem as ComboboxItem).Text.ToString();
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select count(*) as ct from map_tb where f_cha_id='" + f_c_id + "' and t_cha_id='" + t_c_id + "'", con);
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    if (reader.Read()) { cunt = reader["ct"].ToString(); }
                }
                con.Close();
                if (String.IsNullOrEmpty(f_t_id) || String.IsNullOrEmpty(f_c_id) || String.IsNullOrEmpty(t_t_id) || String.IsNullOrEmpty(t_c_id))
                {
                    System.Windows.Forms.MessageBox.Show("Please select all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (int.Parse(cunt) != 0)
                {
                    System.Windows.Forms.MessageBox.Show("You already select this channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (f_c_id == t_c_id)
                {
                    System.Windows.Forms.MessageBox.Show("Can not move to same channel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string sta_cd = "1";
                    con.Open();
                    SqlCommand cmd2 = new SqlCommand("map_st", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@f_tm_id", f_t_id);
                    cmd2.Parameters.AddWithValue("@f_cha_id", f_c_id);
                    cmd2.Parameters.AddWithValue("@t_tm_id", t_t_id);
                    cmd2.Parameters.AddWithValue("@t_cha_id", t_c_id);
                    cmd2.Parameters.AddWithValue("@FromTeam", f_t_nm);
                    cmd2.Parameters.AddWithValue("@FromChannel", f_c_nm);
                    cmd2.Parameters.AddWithValue("@ToTeam", t_t_nm);
                    cmd2.Parameters.AddWithValue("@ToChannel", t_c_nm);
                    cmd2.Parameters.AddWithValue("@sta_cd", sta_cd);
                    cmd2.Parameters.AddWithValue("@cnt_mes", "");
                    cmd2.ExecuteNonQuery();
                    SqlCommand cmd3 = new SqlCommand("map_select", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd3);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.ItemsSource = dt.DefaultView;
                    con.Close();

                }
            }
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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
            }
        }

        private void btn1_Click_1(object sender, RoutedEventArgs e)
        {
            try 
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("delteamcha", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();
                con.Close();
                FourPage th = new FourPage();
                th.Show();
                this.Hide();
            } 
            catch (Exception em) 
            {
                System.Windows.Forms.MessageBox.Show(em.Message);
            }
            
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            string cunt = "";
            con.Open();
            SqlCommand cmd1 = new SqlCommand("select count(*) as ct from map_tb", con);
            using (SqlDataReader reader = cmd1.ExecuteReader())
            {
                if (reader.Read()) { cunt = reader["ct"].ToString(); }
            }
            con.Close();
            if (int.Parse(cunt) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Cannot proceed without an item in Migration Job List", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                SevPage sv = new SevPage();
                sv.Show();
                this.Hide();
            }
        }

        private async void btn3_Click(object sender, RoutedEventArgs e)
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
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox2.Items.Clear();
            List<GetTeamItem> list2 = new List<GetTeamItem>();
            string tid = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand("sel_chabid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@t_id",tid);
            using (IDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    GetTeamItem it = new GetTeamItem();
                    it.id = dr[1].ToString();
                    it.displayName = dr[2].ToString();
                    list2.Add(it);
                }
            }
            con.Close();
            foreach (GetTeamItem gt in list2)
            {
                ComboboxItem it = new ComboboxItem();
                it.Text = gt.displayName;
                it.Value = gt.id;
                comboBox2.Items.Add(it);
            }
        }

        private void comboBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox3.Items.Clear();
            List<GetTeamItem> list3 = new List<GetTeamItem>();
            string tid = (comboBox4.SelectedItem as ComboboxItem).Value.ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand("sel_chabid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@t_id", tid);
            using (IDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    GetTeamItem it = new GetTeamItem();
                    it.id = dr[1].ToString();
                    it.displayName = dr[2].ToString();
                    list3.Add(it);
                }
            }
            con.Close();
            foreach (GetTeamItem gt in list3)
            {
                ComboboxItem it = new ComboboxItem();
                it.Text = gt.displayName;
                it.Value = gt.id;
                comboBox3.Items.Add(it);
            }
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

}
