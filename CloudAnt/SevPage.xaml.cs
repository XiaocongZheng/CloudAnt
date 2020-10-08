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
    /// Interaction logic for SevPage.xaml
    /// </summary>
    public partial class SevPage : Window
    {
        public class TeamChanelItem
        {
            public string fTid { get; set; }
            public string fCid { get; set; }
            public string tTid { get; set; }
            public string tCid { get; set; }
            public string map_id { get; set; }
        }
        public SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
        public List<TeamChanelItem> maplist = new List<TeamChanelItem>();

        public string token1 = "";
        //public string token4 = "";
        public string ppwd = "";

        public string cid = "";
        public string cs = "";
        public string tid = "";
        private delegate void SetprogressBarHandle(int vaule, int count);
        
        private void SetprogressBar(int vaule, int count)
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.Invoke(new SetprogressBarHandle(this.SetprogressBar), vaule, count);
            }
            else
            {
                if (vaule == count) 
                {
                    double percent = (vaule * 100) / count;

                    tpbar.Value = Math.Round(percent, 0);

                    StatusTextBox.Text = "Completed";

                    Finwin fw = new Finwin();
                    fw.Show();
                    this.Hide();
                } 
                else 
                {
                    double percent = (vaule * 100) / count;

                    tpbar.Value = Math.Round(percent, 0);

                    StatusTextBox.Text = Math.Round(percent, 0) + "% percent completed";
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

                double percent = (vaule * 100) / count;

                itbar.Value = Math.Round(percent, 0);

                
            }
        }

        
        public SevPage()
        {
            InitializeComponent();
            Thread mThread = new Thread(ThreadProcess);
            mThread.Start();
            StatusTextBox.Text = "Starting...";
        }

        private async void ThreadProcess(object obj)
        {
            string token2 = GetToken();
            con.Open();
            SqlCommand cmd = new SqlCommand("map_id_select", con);
            cmd.CommandType = CommandType.StoredProcedure;
            using (IDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    TeamChanelItem f = new TeamChanelItem();

                    f.fTid = dr[0].ToString();
                    f.fCid = dr[1].ToString();
                    f.tTid = dr[2].ToString();
                    f.tCid = dr[3].ToString();
                    f.map_id = dr[4].ToString();
                    maplist.Add(f);

                }
            }
            con.Close();
            if (!String.IsNullOrEmpty(token2)) 
            {

                for (int i = 0; i < maplist.Count; i++) 
                {
                    string ftid = maplist[i].fTid;
                    string fcid = maplist[i].fCid;
                    string ttid = maplist[i].tTid;
                    string tcid = maplist[i].tCid;
                    string mp_id = maplist[i].map_id;
                    int x = int.Parse(mp_id);
                    await testFuctions(ftid, fcid, ttid, tcid, x);
                    
                    int c = i + 1;
                    SetprogressBar(c, maplist.Count);
                }
            } 
            else 
            {
                System.Windows.Forms.MessageBox.Show("Access token expired, please sign in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Thread.Sleep(200);
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


                        ThirdPage th = new ThirdPage();
                        th.Show();
                        this.Hide();
                    }
                    catch (MsalException ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    ThirdPage th = new ThirdPage();
                    th.Show();
                    this.Hide();
                }
            }
            Thread.Sleep(200);
        }


        private Task testFuctions(string fid, string c_id, string ttid, string tcid, int map_id)
        {
            return Task.Run(async () =>
            {
            string token7 = GetToken();
            if (!String.IsNullOrEmpty(token7))
            {
                GetChatContent step2 = new GetChatContent();
                List<MesageItem> messages = step2.GetChanelMessages(token7, fid, c_id);
                Comparison<MesageItem> comparison = new Comparison<MesageItem>((MesageItem x, MesageItem y) =>
                {
                    if (x.id < y.id)
                        return -1;
                    else if (x.id == y.id)
                        return 0;
                    else
                        return 1;
                });
                messages.Sort(comparison);
                    
                for (int i = 0; i < messages.Count; i++)
                {
                    string user1 = step2.GetUserEmail(token7, messages[i]);
                    string token3 = GetUsrToken(user1);
                    MigrationStep step3 = new MigrationStep();
                    string oneaccess = "";
                    if (String.IsNullOrEmpty(token3))
                    {
                        string sta_cd = "1";
                        con.Open();
                        SqlCommand cmd2 = new SqlCommand("select_token", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@sta_cd", sta_cd);
                        using (SqlDataReader reader = cmd2.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                token1 = reader["access_token"].ToString();
                                ppwd = reader["p_pwd"].ToString();
                                cid = reader["c_id"].ToString();
                                cs = reader["c_cs"].ToString();
                                tid = reader["t_id"].ToString();
                            }
                        }
                        con.Close();
                        oneaccess = step3.GetOneAccessToken(user1, ppwd, cid, cs, tid);


                    }
                    else
                    {
                        oneaccess = token3;
                    }
                    MesageItem mesaft = step3.ClearfyContent(messages[i]);
                        var res = step2.SendMessage(oneaccess, ttid, tcid, mesaft);
                        if (res == null)
                        {
                            System.Windows.Forms.MessageBox.Show("One message throw an error");
                            continue;
                        }

                        long new_m_id = res.id;
                        
                        List<MesageItem> replies = step2.GetMessagesReply(token7, fid, c_id, messages[i].id.ToString());
                    
                        if (replies.Count>0) 
                        {
                            Comparison<MesageItem> comparison1 = new Comparison<MesageItem>((MesageItem x, MesageItem y) =>
                            {
                                if (x.id < y.id)
                                    return -1;
                                else if (x.id == y.id)
                                    return 0;
                                else
                                    return 1;
                            });
                            replies.Sort(comparison1);
                            
                            foreach (MesageItem reply in replies)
                            {
                                MesageItem newreply = step3.ClearfyContent(reply);
                                long mesid = new_m_id;
                                string user2 = step2.GetUserEmail(token7, reply);
                                string token6 = GetUsrToken(user2);
                                
                                string oaccess = "";
                                if (String.IsNullOrEmpty(token6))
                                {
                                    string sta_cd = "1";
                                    con.Open();
                                    SqlCommand cmd2 = new SqlCommand("select_token", con);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@sta_cd", sta_cd);
                                    using (SqlDataReader reader = cmd2.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            token1 = reader["access_token"].ToString();
                                            ppwd = reader["p_pwd"].ToString();
                                            cid = reader["c_id"].ToString();
                                            cs = reader["c_cs"].ToString();
                                            tid = reader["t_id"].ToString();
                                        }
                                    }
                                    con.Close();
                                    oaccess = step3.GetOneAccessToken(user2, ppwd, cid, cs, tid);


                                }
                                else
                                {
                                    oaccess = token6;
                                }
                                if (!step2.ReplyMessage(oaccess, ttid, tcid, mesid, newreply))
                                {
                                    System.Windows.Forms.MessageBox.Show("Send a reply error");
                                    continue;
                                }
                            }
                        }
                        int c = i + 1;
                        SetprogressBar1(c, messages.Count);
                        Thread.Sleep(5);
                    }
                    con.Open();
                    SqlCommand cmd6 = new SqlCommand("upt_map", con);
                    cmd6.CommandType = CommandType.StoredProcedure;
                    cmd6.Parameters.AddWithValue("@map_id", map_id);
                    cmd6.Parameters.AddWithValue("@cnt_mes", "");
                    cmd6.ExecuteNonQuery();
                    con.Close();
                } 
                else 
                {
                    System.Windows.Forms.MessageBox.Show("Access token expired, please sign in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Thread.Sleep(200);
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


                            ThirdPage th = new ThirdPage();
                            th.Show();
                            this.Hide();
                        }
                        catch (MsalException ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        ThirdPage th = new ThirdPage();
                        th.Show();
                        this.Hide();
                    }
                }
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

        public string GetUsrToken(string email) 
        {
            string token4="";
            con.Open();
            SqlCommand cmd2 = new SqlCommand("sel_utoken", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@email", email);
            using (SqlDataReader reader = cmd2.ExecuteReader())
            {
                if (reader.Read())
                {
                    token4 = reader["access_token"].ToString();
                }
            }
            con.Close();
            return token4;
            
        }
    }

}
