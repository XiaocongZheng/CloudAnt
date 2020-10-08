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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Identity.Client;

namespace CloudAnt
{
    /// <summary>
    /// Interaction logic for ThirdPage.xaml
    /// </summary>
    public partial class ThirdPage : Window
    {
        public string token1 = "";
        public string ausrn = "";
        //string graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        //Set the scope for API call to user.read
        string[] scopes = new string[] { "user.read" };
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        public ThirdPage()
        {
            InitializeComponent();
            con.Open();
            SqlCommand cmd2 = new SqlCommand("select_atoken", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader reader = cmd2.ExecuteReader())
            {
                if (reader.Read()) { token1 = reader["access_token"].ToString();ausrn = reader["email"].ToString(); }
            }
            con.Close();
            if (token1 != "") 
            {
                this.usr_nm.Text = ausrn;
                usr_nm.Visibility = Visibility.Visible;
                unmlab.Visibility = Visibility.Visible;
                this.btn4.Visibility = Visibility.Visible;
                c_idlab.Visibility = Visibility.Collapsed;
                cl_id.Visibility = Visibility.Collapsed;
                cs_lab.Visibility = Visibility.Collapsed;
                cl_sect.Visibility = Visibility.Collapsed;
                pass_lab.Visibility = Visibility.Collapsed;
                cl_pass.Visibility = Visibility.Collapsed;
            }
        }

        private async void btn2_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult authResult = null;
            var app = App.PublicClientApp;
            

            var accounts = await app.GetAccountsAsync();
            var firstAccount = accounts.FirstOrDefault();

            try
            {
                authResult = await app.AcquireTokenSilent(scopes, firstAccount)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilent. 
                // This indicates you need to call AcquireTokenInteractive to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await app.AcquireTokenInteractive(scopes)
                        .WithAccount(accounts.FirstOrDefault())
                        .WithParentActivityOrWindow(new WindowInteropHelper(this).Handle) // optional, used to center the browser on the window
                        .WithPrompt(Prompt.SelectAccount)
                        .ExecuteAsync();
                }
                catch (MsalException msalex)
                {
                    System.Windows.Forms.MessageBox.Show($"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}");
                return;
            }

            if (authResult != null)
            {
                string aten = "4dd770a9-6973-4a56-b6a1-89897496fa2d";
                if (authResult.TenantId!=aten) 
                {
                    
                    System.Windows.Forms.DialogResult dl;
                    dl = System.Windows.Forms.MessageBox.Show("Account don't have permission to do migration, please check account and try again.", "Permission required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dl == System.Windows.Forms.DialogResult.OK)
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
                                usr_nm.Visibility = Visibility.Collapsed;
                                unmlab.Visibility = Visibility.Collapsed;
                                this.btn4.Visibility = Visibility.Collapsed;
                                c_idlab.Visibility = Visibility.Visible;
                                cl_id.Visibility = Visibility.Visible;
                                cs_lab.Visibility = Visibility.Visible;
                                cl_sect.Visibility = Visibility.Visible;
                                pass_lab.Visibility = Visibility.Visible;
                                cl_pass.Visibility = Visibility.Visible;

                            }
                            catch (MsalException ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.Message);
                            }
                        }
                    }
                } 
                else 
                { 
                string ppwd = cl_pass.Password.ToString();
                string CLIENT_ID = cl_id.Text;
                string CLIENT_SECERET = cl_sect.Text;
                string sta_cd = "1";
                try 
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("addUser", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@email", authResult.Account.Username);
                    cmd.Parameters.AddWithValue("@p_pwd", ppwd);
                    cmd.Parameters.AddWithValue("@c_id", CLIENT_ID);
                    cmd.Parameters.AddWithValue("@c_cs", CLIENT_SECERET);
                    cmd.Parameters.AddWithValue("@t_id", authResult.TenantId);
                    cmd.Parameters.AddWithValue("@access_token", authResult.AccessToken);
                    cmd.Parameters.AddWithValue("@sta_cd", sta_cd);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    FourPage fg = new FourPage();
                    fg.Show();
                    this.Hide();
                    
                } 
                catch (Exception ex) 
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                }

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
                        usr_nm.Visibility = Visibility.Collapsed;
                        unmlab.Visibility = Visibility.Collapsed;
                        this.btn4.Visibility = Visibility.Collapsed;
                        c_idlab.Visibility = Visibility.Visible;
                        cl_id.Visibility = Visibility.Visible;
                        cs_lab.Visibility = Visibility.Visible;
                        cl_sect.Visibility = Visibility.Visible;
                        pass_lab.Visibility = Visibility.Visible;
                        cl_pass.Visibility = Visibility.Visible;

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
                        usr_nm.Visibility = Visibility.Collapsed;
                        unmlab.Visibility = Visibility.Collapsed;
                        this.btn4.Visibility = Visibility.Collapsed;
                        c_idlab.Visibility = Visibility.Visible;
                        cl_id.Visibility = Visibility.Visible;
                        cs_lab.Visibility = Visibility.Visible;
                        cl_sect.Visibility = Visibility.Visible;
                        pass_lab.Visibility = Visibility.Visible;
                        cl_pass.Visibility = Visibility.Visible;

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

        private void btn1_Click_1(object sender, RoutedEventArgs e)
        {
            SeccPage se = new SeccPage();
            se.Show();
            this.Hide();
        }

        

        private async void btn4_Click(object sender, RoutedEventArgs e)
        {
            var accounts = await App.PublicClientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    await App.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("user_del", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.ExecuteNonQuery();
                    con.Close();
                    usr_nm.Visibility = Visibility.Collapsed;
                    unmlab.Visibility = Visibility.Collapsed;
                    this.btn4.Visibility = Visibility.Collapsed;
                    c_idlab.Visibility = Visibility.Visible;
                    cl_id.Visibility = Visibility.Visible;
                    cs_lab.Visibility = Visibility.Visible;
                    cl_sect.Visibility = Visibility.Visible;
                    pass_lab.Visibility = Visibility.Visible;
                    cl_pass.Visibility = Visibility.Visible;
                    
                }
                catch (MsalException ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void hello(object sender, ToolTipEventArgs e)
        {

        }
    }
}
