using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CloudAnt
{
    public class MigrationStep
    {
        
        public MesageItem ClearfyContent(MesageItem item)
        {
            try
            {
                string content = item.body.content;
                content = content.Replace("<div>", "");
                content = content.Replace("</div>", "");
                //content = content.Replace("<at id=\"0\">", "<at id=\\\"0\\\">");
                content = content.Replace("\"", "\\\"");
                string date = item.createdDateTime;
                date = Regex.Replace(date, @"\.[0-9]*Z", string.Empty);
                date = date.Replace("T", " ");
                string realdate = TimeZone.CurrentTimeZone.ToLocalTime(Convert.ToDateTime(date)).ToString();

                content = "[Created at: " + realdate + "] " + " " + content;
                // content = "[Created at: " + date + "]" + content;
                item.body.content = content;
                return item;
            }
            catch (Exception e)
            {
                MessageBox.Show("There are some error while modefy the content, " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

        }

        public string GetOneAccessToken(string user, string pwd, string cid, string cs, string tid)
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


                CLIENT_ID = cid;
                CLIENT_SECERET = cs;
                TENAT_ID = tid;

                // CLIENT_ID = "db9ad5c4-86be-4266-aacb-0552f5bf1cc4";
                //CLIENT_SECERET = "VOY6w]I/00Je/7tBuaatcHyjoajr_/.s";
                //TENAT_ID = "4dd770a9-6973-4a56-b6a1-89897496fa2d";
                TOKEN_ENDPOINT = "https://login.microsoftonline.com/" + TENAT_ID + "/oauth2/v2.0/token";
                MS_GRAPH_SCOPE = "https://graph.microsoft.com/.default";
                GRANT_TYPE = "password";



                USERNAME = user;
                PASSWORD = pwd;
            }
            catch (Exception e)
            {
                MessageBox.Show("A error happened while search config file, "+ e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var objText = reader.ReadToEnd();
                        LgObject myojb = (LgObject)js.Deserialize(objText, typeof(LgObject));
                        token = myojb.access_token;
                    }

                }
                return token;
            }
            catch (Exception e)
            {
                MessageBox.Show("A error happened while connect to server please check config file, " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "error";
            }
        }

        public UserInfoItem GetUserByEmail(string AccessToken, string email)
        {
            string user = email;

            try
            {

                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/v1.0/users/" + user) as HttpWebRequest;
                var myHttpWebRequest = (HttpWebRequest)request;
                request.Method = "GET";
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                myHttpWebRequest.Accept = "application/json";

                var myWebResponse = request.GetResponse();

                var responseStream = myWebResponse.GetResponseStream();

                string objText = null;

                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    //while (myStreamReader.Peek()>=0) {

                    objText = myStreamReader.ReadToEnd();
                    //Console.WriteLine(objText);
                    //}

                }


                JavaScriptSerializer js = new JavaScriptSerializer();

                var myojb = (UserInfoItem)js.Deserialize(objText, typeof(UserInfoItem));
                UserInfoItem userobject = myojb;


                responseStream.Close();
                myWebResponse.Close();

                return userobject;

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                return null;

            }
        }
    }
}
