using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CloudAnt
{
    public class FunctionStep1
    {
        public List<TeamItem> GetMeJoinedTeam(string AccessToken)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/me/joinedTeams") as HttpWebRequest;
                var myHttpWebRequest = (HttpWebRequest)request;
                request.Method = "GET";
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = request.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();


                var myStreamReader = new StreamReader(responseStream, Encoding.Default);

                JavaScriptSerializer js = new JavaScriptSerializer();
                var objText = myStreamReader.ReadToEnd();


                MyTeamObject myojb = (MyTeamObject)js.Deserialize(objText, typeof(MyTeamObject));
                List<TeamItem> teamname = myojb.value;
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
                foreach (var t in teamname)
                {
                    string tdsc = checknull(t.description);
                    string ttid = checknull(t.internalId);
                    string tclass = checknull(t.classification);
                    string tspec = checknull(t.specialization);
                    string tvis = checknull(t.visibility);
                    string tweb = checknull(t.webUrl);
                    string tisatch = checknull(t.isArchived);
                    string tmen = checknull(t.memberSettings);
                    string tguest = checknull(t.guestSettings);
                    string tmsg = checknull(t.messagingSettings);
                    string tfun = checknull(t.funSettings);
                    string tdis = checknull(t.discoverySettings);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("team_info", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@t_id", t.id);
                    cmd.Parameters.AddWithValue("@t_dis_nm", t.displayName);
                    cmd.Parameters.AddWithValue("@t_dsc", tdsc);
                    cmd.Parameters.AddWithValue("@t_interid", ttid);
                    cmd.Parameters.AddWithValue("@t_class", tclass);
                    cmd.Parameters.AddWithValue("@t_spec", tspec);
                    cmd.Parameters.AddWithValue("@t_visib", tvis);
                    cmd.Parameters.AddWithValue("@t_web", tweb);
                    cmd.Parameters.AddWithValue("@t_isAtch", tisatch);
                    cmd.Parameters.AddWithValue("@t_mem", tmen);
                    cmd.Parameters.AddWithValue("@t_guest", tguest);
                    cmd.Parameters.AddWithValue("@t_msg", tmsg);
                    cmd.Parameters.AddWithValue("@t_fun", tfun);
                    cmd.Parameters.AddWithValue("@t_dis", tdis);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                responseStream.Close();
                myWebResponse.Close();
                return teamname;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public List<TeamItem> GetMeJoinedTeamTo(string AccessToken)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/me/joinedTeams") as HttpWebRequest;
                var myHttpWebRequest = (HttpWebRequest)request;
                request.Method = "GET";
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = request.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();


                var myStreamReader = new StreamReader(responseStream, Encoding.Default);

                JavaScriptSerializer js = new JavaScriptSerializer();
                var objText = myStreamReader.ReadToEnd();


                MyTeamObject myojb = (MyTeamObject)js.Deserialize(objText, typeof(MyTeamObject));
                List<TeamItem> teamname = myojb.value;
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
                foreach (var t in teamname)
                {
                    string tdsc = checknull(t.description);
                    string ttid = checknull(t.internalId);
                    string tclass = checknull(t.classification);
                    string tspec = checknull(t.specialization);
                    string tvis = checknull(t.visibility);
                    string tweb = checknull(t.webUrl);
                    string tisatch = checknull(t.isArchived);
                    string tmen = checknull(t.memberSettings);
                    string tguest = checknull(t.guestSettings);
                    string tmsg = checknull(t.messagingSettings);
                    string tfun = checknull(t.funSettings);
                    string tdis = checknull(t.discoverySettings);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("tteam_info", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@t_id", t.id);
                    cmd.Parameters.AddWithValue("@t_dis_nm", t.displayName);
                    cmd.Parameters.AddWithValue("@t_dsc", tdsc);
                    cmd.Parameters.AddWithValue("@t_interid", ttid);
                    cmd.Parameters.AddWithValue("@t_class", tclass);
                    cmd.Parameters.AddWithValue("@t_spec", tspec);
                    cmd.Parameters.AddWithValue("@t_visib", tvis);
                    cmd.Parameters.AddWithValue("@t_web", tweb);
                    cmd.Parameters.AddWithValue("@t_isAtch", tisatch);
                    cmd.Parameters.AddWithValue("@t_mem", tmen);
                    cmd.Parameters.AddWithValue("@t_guest", tguest);
                    cmd.Parameters.AddWithValue("@t_msg", tmsg);
                    cmd.Parameters.AddWithValue("@t_fun", tfun);
                    cmd.Parameters.AddWithValue("@t_dis", tdis);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                responseStream.Close();
                myWebResponse.Close();
                return teamname;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public string checknull(string a)
        {
            if (a != null)
            {
                return a;
            }
            else
            {
                return "";
            }
        }

        public List<ChannelItem> GetMeJoinedChanel(string AccessToken, string TeamID, int typ)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/teams/" + TeamID + "/channels") as HttpWebRequest;
                var myHttpWebRequest = (HttpWebRequest)request;
                request.Method = "GET";
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = request.GetResponse();
                var responseStream = myWebResponse.GetResponseStream();


                var myStreamReader = new StreamReader(responseStream, Encoding.Default);

                JavaScriptSerializer js = new JavaScriptSerializer();
                var objText = myStreamReader.ReadToEnd();
                MyChannelObject myojb = (MyChannelObject)js.Deserialize(objText, typeof(MyChannelObject));
                List<ChannelItem> teamname = myojb.value;
                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Clouddata.mdf;Integrated Security=True");
                foreach (var c in teamname)
                {

                    string cdsc = checknull(c.description);
                    string cmem = checknull(c.MemberShipType);
                    string cfav = checknull(c.isFavoriteByDefault);
                    string cweb = checknull(c.webUrl);
                    string cemail = checknull(c.email);
                    string typ1;
                    con.Open();
                    if (typ == 1)
                    {
                        typ1 = "cha_tb";
                    }
                    else
                    {
                        typ1 = "tcha_tb";
                    }
                    SqlCommand cmd = new SqlCommand(typ1, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@c_id", c.id);
                    cmd.Parameters.AddWithValue("@c_dis_nm", c.displayName);
                    cmd.Parameters.AddWithValue("@c_dsc", cdsc);
                    cmd.Parameters.AddWithValue("@c_mem", cmem);
                    cmd.Parameters.AddWithValue("@c_fav", cfav);
                    cmd.Parameters.AddWithValue("@c_web", cweb);
                    cmd.Parameters.AddWithValue("@c_email", cemail);
                    cmd.Parameters.AddWithValue("@t_id", TeamID);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


                responseStream.Close();
                myWebResponse.Close();
                return teamname;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
