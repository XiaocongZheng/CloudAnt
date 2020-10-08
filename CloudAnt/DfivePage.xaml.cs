using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
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
    /// Interaction logic for DfivePage.xaml
    /// </summary>
    public partial class DfivePage : Window
    {
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        public DfivePage()
        {
            InitializeComponent();
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
                        System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btn1_Click_1(object sender, RoutedEventArgs e)
        {
            FourPage fo = new FourPage();
            fo.Show();
            this.Hide();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(cl_id.Text) & !String.IsNullOrWhiteSpace(cl_sect.Text) & !String.IsNullOrWhiteSpace(cl_ten.Text) & !String.IsNullOrWhiteSpace(usr_name.Text) & !String.IsNullOrWhiteSpace(cl_pass.Password)) 
            {
                string ppwd = cl_pass.Password.ToString();
                string CLIENT_ID = cl_id.Text;
                string CLIENT_SECERET = cl_sect.Text;
                string ten_id = cl_ten.Text;
                string u_namre = usr_name.Text;
                GetAccessToken(ppwd, CLIENT_ID, CLIENT_SECERET, ten_id, u_namre);
                DsixPage dx = new DsixPage();
                dx.Show();
                this.Hide();
            } 
            else 
            {
                System.Windows.Forms.MessageBox.Show("Please complete destination informations for migration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        System.Windows.Forms.MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private string GetAccessToken(string ppwd, string clt_id, string clt_cs, string clt_ten, string pusr)
        {
            string token = "";
            string CLIENT_ID = "";
            string CLIENT_SECERET = "";
            string TENAT_ID = "";
            string TOKEN_ENDPOINT = "";
            string MS_GRAPH_SCOPE = "";
            string GRANT_TYPE = "";
            string USERNAME = "";
            string PASSWORD = "";
            try
            {



                /*
                string CLIENT_ID = "db9ad5c4-86be-4266-aacb-0552f5bf1cc4";
                string CLIENT_SECERET = "VOY6w]I/00Je/7tBuaatcHyjoajr_/.s";
                string TENAT_ID = "4dd770a9-6973-4a56-b6a1-89897496fa2d";
                string TOKEN_ENDPOINT = "https://login.microsoftonline.com/" + TENAT_ID + "/oauth2/v2.0/token";
                string MS_GRAPH_SCOPE = "https://graph.microsoft.com/.default";
                string GRANT_TYPE = "password";
                string USERNAME = "kevin@walvishosting.com";
                string PASSWORD = "Zxc15101116269!";
                */
                CLIENT_ID = clt_id;
                CLIENT_SECERET = clt_cs;
                TENAT_ID = clt_ten;
                TOKEN_ENDPOINT = "https://login.microsoftonline.com/" + TENAT_ID + "/oauth2/v2.0/token";
                MS_GRAPH_SCOPE = "https://graph.microsoft.com/.default";
                GRANT_TYPE = "password";
                USERNAME = pusr;
                PASSWORD = ppwd;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A error happened while search config file " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            try
            {
                HttpWebRequest request = WebRequest.Create(TOKEN_ENDPOINT) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                StringBuilder data = new StringBuilder();
                data.Append("client_id=" + HttpUtility.UrlEncode(CLIENT_ID));
                data.Append("&scope=" + HttpUtility.UrlEncode(MS_GRAPH_SCOPE));
                data.Append("&client_secret=" + HttpUtility.UrlEncode(CLIENT_SECERET));
                data.Append("&GRANT_TYPE=" + HttpUtility.UrlEncode(GRANT_TYPE));
                data.Append("&userName=" + HttpUtility.UrlEncode(USERNAME));
                data.Append("&password=" + HttpUtility.UrlEncode(PASSWORD));
                //string postData = "client_id=" + CLIENT_ID+ "&scope="+ MS_GRAPH_SCOPE+ "&client_secret"+ CLIENT_SECERET+ "&grant_type"+ GRANT_TYPE+ "&userName"+ USERNAME+ "&password"+PASSWORD;
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
                request.ContentLength = byteData.Length;
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // Get response

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string sta_cd = "2";
                        System.Web.Script.Serialization.JavaScriptSerializer js = new JavaScriptSerializer();
                        var objText = reader.ReadToEnd();
                        LgObject myojb = (LgObject)js.Deserialize(objText, typeof(LgObject));
                        token = myojb.access_token;
                        
                        
                        con.Open();
                        SqlCommand cmd = new SqlCommand("addUser", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@email", pusr);
                        cmd.Parameters.AddWithValue("@p_pwd", ppwd);
                        cmd.Parameters.AddWithValue("@c_id", CLIENT_ID);
                        cmd.Parameters.AddWithValue("@c_cs", CLIENT_SECERET);
                        cmd.Parameters.AddWithValue("@t_id", clt_ten);
                        cmd.Parameters.AddWithValue("@access_token", token);
                        cmd.Parameters.AddWithValue("@sta_cd", sta_cd);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        
                    }

                }
                return token;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("A error happened while connect to server please check config file " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "error";
            }
        }
    }
}
