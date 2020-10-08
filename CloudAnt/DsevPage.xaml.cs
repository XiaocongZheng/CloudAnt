using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
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
using Microsoft.VisualBasic.FileIO;

namespace CloudAnt
{
    /// <summary>
    /// Interaction logic for DsevPage.xaml
    /// </summary>
    public partial class DsevPage : Window
    {
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        public List<GetTeamItem> list1 = new List<GetTeamItem>();
        public List<GetTeamItem> list2 = new List<GetTeamItem>();
        public string token1 = "";
        public string token2 = "";
        public DsevPage()
        {
            InitializeComponent();
            con.Open();
            SqlCommand cmd = new SqlCommand("select_team", con);
            SqlCommand cmd1 = new SqlCommand("select_tteam", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd1.CommandType = CommandType.StoredProcedure;
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
            using (IDataReader dr = cmd1.ExecuteReader())
            {
                while (dr.Read())
                {
                    GetTeamItem at = new GetTeamItem();
                    at.id = dr[1].ToString();
                    at.displayName = dr[2].ToString();
                    list2.Add(at);
                }
            }
            con.Close();
            foreach (GetTeamItem gt in list1)
            {
                ComboboxItem1 it = new ComboboxItem1();
                it.Text = gt.displayName;
                it.Value = gt.id;
                comboBox4.Items.Add(it);
            }
            foreach (GetTeamItem gt in list2)
            {
                ComboboxItem1 it = new ComboboxItem1();
                it.Text = gt.displayName;
                it.Value = gt.id;
                comboBox1.Items.Add(it);
            }
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

        private void lab2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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
            string cunt1 = "";
            con.Open();
            SqlCommand cmd1 = new SqlCommand("select count(*) as ct from map_tb", con);
            SqlCommand cmd2 = new SqlCommand("select count(*) as uct from map_user", con);
            using (SqlDataReader reader = cmd1.ExecuteReader())
            {
                if (reader.Read()) { cunt = reader["ct"].ToString(); }
            }
            using (SqlDataReader reader = cmd2.ExecuteReader())
            {
                if (reader.Read()) { cunt1 = reader["uct"].ToString(); }
            }
            con.Close();
            if (int.Parse(cunt) == 0 || int.Parse(cunt1) == 0)
            {
                System.Windows.Forms.MessageBox.Show("Cannot proceed without an item in Migration Job List and without an item in Migration User List", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DeitPage de = new DeitPage();
                de.Show();
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

        private void addmap_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(comboBox1.Text) || String.IsNullOrEmpty(comboBox2.Text) || String.IsNullOrEmpty(comboBox3.Text) || String.IsNullOrEmpty(comboBox4.Text))
            {
                System.Windows.Forms.MessageBox.Show("Please select all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string cunt = "";
                string f_t_id = (comboBox1.SelectedItem as ComboboxItem1).Value.ToString();
                string f_c_id = (comboBox2.SelectedItem as ComboboxItem1).Value.ToString();
                string t_t_id = (comboBox4.SelectedItem as ComboboxItem1).Value.ToString();
                string t_c_id = (comboBox3.SelectedItem as ComboboxItem1).Value.ToString();
                string f_t_nm = (comboBox1.SelectedItem as ComboboxItem1).Text.ToString();
                string f_c_nm = (comboBox2.SelectedItem as ComboboxItem1).Text.ToString();
                string t_t_nm = (comboBox4.SelectedItem as ComboboxItem1).Text.ToString();
                string t_c_nm = (comboBox3.SelectedItem as ComboboxItem1).Text.ToString();
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

        private void remap_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd1 = new SqlCommand("map_del", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.ExecuteNonQuery();
            con.Close();
            dataGridView1.ItemsSource = null;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox2.Items.Clear();
            List<GetTeamItem> list4 = new List<GetTeamItem>();
            string tid = (comboBox1.SelectedItem as ComboboxItem1).Value.ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand("selt_chabid", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@t_id", tid);
            using (IDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    GetTeamItem it = new GetTeamItem();
                    it.id = dr[1].ToString();
                    it.displayName = dr[2].ToString();
                    list4.Add(it);
                }
            }
            con.Close();
            foreach (GetTeamItem gt in list4)
            {
                ComboboxItem1 it = new ComboboxItem1();
                it.Text = gt.displayName;
                it.Value = gt.id;
                comboBox2.Items.Add(it);
            }
        }

        private void comboBox4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox3.Items.Clear();
            List<GetTeamItem> list3 = new List<GetTeamItem>();
            string tid = (comboBox4.SelectedItem as ComboboxItem1).Value.ToString();
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
                ComboboxItem1 it = new ComboboxItem1();
                it.Text = gt.displayName;
                it.Value = gt.id;
                comboBox3.Items.Add(it);
            }
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

        private void addumap_Click(object sender, RoutedEventArgs e)
        {
            string sfile = "";
            System.Windows.Forms.OpenFileDialog browseDialog = new OpenFileDialog();
            
            browseDialog.Filter = "CSV files (*.csv)|*.csv";
            if (browseDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) 
            {
                sfile = browseDialog.InitialDirectory + browseDialog.FileName;
            }
            if (sfile != null) 
            {
                DataTable dr = new DataTable();
                dr = GetDataTabletFromCSVFile(sfile);
                if (dr.Rows.Count > 0 & dr.Rows!=null) 
                {
                    string sta = CSVFileSave(dr);
                    if (sta == "good") 
                    {
                        con.Open();
                        SqlCommand cmd1 = new SqlCommand("map_uselect", con);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        DataTable dp = new DataTable();
                        dp.Columns.Add("From Email");
                        dp.Columns.Add("To Email");


                        using (IDataReader dm = cmd1.ExecuteReader())
                        {
                            while (dm.Read())
                            {
                                DataRow row = dp.NewRow();

                                row["From Email"] = dm[0].ToString();
                                row["To Email"] = dm[1].ToString();

                                dp.Rows.Add(row);

                            }
                        }

                        dataGridView2.ItemsSource = dp.DefaultView;
                        con.Close();
                    }
                    
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upload user csv error, please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Upload user csv error, please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void reusr_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd1 = new SqlCommand("map_udel", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.ExecuteNonQuery();
            con.Close();
            dataGridView2.ItemsSource = null;
        }

        private string CSVFileSave(DataTable csv) 
        {
            string stacd = "good";
            con.Open();
            foreach (DataRow row in csv.Rows) 
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("map_usrst", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@f_email", row.ItemArray[0]);
                    cmd.Parameters.AddWithValue("@t_email", row.ItemArray[1]);
                    cmd.Parameters.AddWithValue("@sta_cd", 1);
                    cmd.ExecuteNonQuery();
                } catch (Exception m)
                {
                    System.Windows.Forms.MessageBox.Show(m.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    stacd = "nook";
                    break;
                }
            }


            con.Close();
            return stacd;
        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            return csvData;
        }
    }

    public class ComboboxItem1
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
