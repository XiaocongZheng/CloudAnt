using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace CloudAnt
{
    public class GetChatContent
    {
        public List<MesageItem> GetMessagesReply(string AccessToken, string TeamID, string ChannelID, string mid)
        {
            //string m_id = mid.ToString();

            try
            {

                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/teams/" + TeamID + "/channels/" + ChannelID + "/messages/" + mid + "/replies") as HttpWebRequest;
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

                var myojb = (MyChanMesageObject)js.Deserialize(objText, typeof(MyChanMesageObject));
                List<MesageItem> message = myojb.value;


                responseStream.Close();
                myWebResponse.Close();

                return message;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

        }


        public List<MesageItem> GetChanelMessages(string AccessToken, string TeamID, string ChannelID)
        {

            try
            {

                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/teams/" + TeamID + "/channels/" + ChannelID + "/messages") as HttpWebRequest;
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

                var myojb = (MyChanMesageObject)js.Deserialize(objText, typeof(MyChanMesageObject));
                List<MesageItem> message = myojb.value;


                responseStream.Close();
                myWebResponse.Close();

                return message;
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

        }

        public string GetUserEmail(string AccessToken, MesageItem item)
        {

            try
            {
                string UID = item.From.user.id;
                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/v1.0/users/" + UID) as HttpWebRequest;
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
                string email = myojb.mail;


                responseStream.Close();
                myWebResponse.Close();
                return email;

            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;

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

        public MesageItem SendMessage(string AccessToken, string TeamID, string ChannelID, MesageItem item)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/teams/" + TeamID + "/channels/" + ChannelID + "/messages") as HttpWebRequest;
                var myHttpWebRequest = (HttpWebRequest)request;
                request.Method = "POST";
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                string mention = " \"mentions\": [";
                foreach (var men in item.mentions)
                {
                    mention = mention + "{\"id\":" + men.id + ",\"mentionText\":\"" + men.mentionText + "\"," + "\"mentioned\": { \"user\": {\"displayName\":\"" + men.mentioned.user.displayName + "\",\"id\": \"" + men.mentioned.user.id + "\",\"userIdentityType\": \"" + men.mentioned.user.userIdentityType + "\"}}}";
                }
                mention += "]";

                //MessageBox.Show(item.body.contentType + item.body.content);
                //MessageBox.Show(mention);
                using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                {
                    string json = "{\"body\":{\"contentType\":\"" + item.body.contentType + "\"," +
                                  "\"content\":\"" + item.body.content + "\"}," + mention + "}";
                    //MessageBox.Show(json);
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                var result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JavaScriptSerializer js = new JavaScriptSerializer();

                var myojb = (MesageItem)js.Deserialize(result, typeof(MesageItem));

                return myojb;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }


        public Boolean ReplyMessage(string AccessToken, string TeamID, string ChannelID, long MessageID, MesageItem item)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/teams/" + TeamID + "/channels/" + ChannelID + "/messages/" + MessageID + "/replies") as HttpWebRequest;
                var myHttpWebRequest = (HttpWebRequest)request;
                request.Method = "POST";
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);

                string mention = " \"mentions\": [";
                foreach (var men in item.mentions)
                {
                    mention = mention + "{\"id\":" + men.id + ",\"mentionText\":\"" + men.mentionText + "\"," + "\"mentioned\": { \"user\": {\"displayName\":\"" + men.mentioned.user.displayName + "\",\"id\": \"" + men.mentioned.user.id + "\",\"userIdentityType\": \"" + men.mentioned.user.userIdentityType + "\"}}}";
                }
                mention += "]";

                //MessageBox.Show(item.body.contentType + item.body.content);
                //MessageBox.Show(mention);
                using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                {

                    string json = "{\"body\":{\"contentType\":\"" + item.body.contentType + "\"," +
                                  "\"content\":\"" + item.body.content + "\"}," + mention + "}";
                    /*   
                    string json = "{\"body\":{\"contentType\":\"" + item.body.contentType + "\"," +
                               "\"content\":\"" + item.body.content + "\"}}";
                    */
                    //MessageBox.Show(json);
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        public MesageItem SendMessageDe(string AccessToken, string TeamID, string ChannelID, MesageItem item, List<MapMenUser> ftemail)
        {

            try
            {


                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/teams/" + TeamID + "/channels/" + ChannelID + "/messages") as HttpWebRequest;
                var myHttpWebRequest = (HttpWebRequest)request;
                request.Method = "POST";
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                string mention = " \"mentions\": [";
                string content = item.body.content;
                foreach (var men in item.mentions)
                {
                    var keyValue = ftemail.FirstOrDefault(x => x.fid == men.mentioned.user.id);
                    //search email
                    string uid = "";
                    string udsname = "";
                    if (keyValue != null)
                    {
                        uid = keyValue.tid;
                        udsname = keyValue.tdisname;
                        content = content.Replace(men.mentionText, udsname);
                    }
                    else
                    {
                        MessageBox.Show("No Mapping User Found");
                        return null;
                    }
                    mention = mention + "{\"id\":" + men.id + ",\"mentionText\":\"" + udsname + "\"," + "\"mentioned\": { \"user\": {\"displayName\":\"" + udsname + "\",\"id\": \"" + uid + "\",\"userIdentityType\": \"" + men.mentioned.user.userIdentityType + "\"}}}";
                    //mention = mention + "{\"id\":" + men.id + ",\"mentionText\":\"" + men.mentionText + "\"," + "\"mentioned\": { \"user\": {\"displayName\":\"" + men.mentioned.user.displayName + "\",\"id\": \"" + men.mentioned.user.id+ "\",\"userIdentityType\": \"" + men.mentioned.user.userIdentityType + "\"}}}";

                }
                mention += "]";

                //MessageBox.Show(item.body.contentType + item.body.content);
                //MessageBox.Show(mention);
                //MessageBox.Show(mention1);

                using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                {
                    string json = "{\"body\":{\"contentType\":\"" + item.body.contentType + "\"," +
                                  "\"content\":\"" + content + "\"}," + mention + "}";
                    //MessageBox.Show(json);
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                var result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                JavaScriptSerializer js = new JavaScriptSerializer();

                var myojb = (MesageItem)js.Deserialize(result, typeof(MesageItem));

                return myojb;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        public Boolean ReplyMessageDe(string AccessToken, string TeamID, string ChannelID, long MessageID, MesageItem item, List<MapMenUser> ftemail)
        {

            try
            {
                HttpWebRequest request = WebRequest.Create("https://graph.microsoft.com/beta/teams/" + TeamID + "/channels/" + ChannelID + "/messages/" + MessageID + "/replies") as HttpWebRequest;
                var myHttpWebRequest = (HttpWebRequest)request;
                request.Method = "POST";
                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.PreAuthenticate = true;
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
                string content = item.body.content;
                string mention = " \"mentions\": [";
                foreach (var men in item.mentions)
                {
                    var keyValue = ftemail.FirstOrDefault(x => x.fid == men.mentioned.user.id);
                    //search email
                    string uid = "";
                    string udsname = "";
                    if (keyValue != null)
                    {
                        uid = keyValue.tid;
                        udsname = keyValue.tdisname;
                        content = content.Replace(men.mentionText, udsname);
                    }
                    else
                    {
                        MessageBox.Show("No Mapping User Found");
                        return false;
                    }
                    mention = mention + "{\"id\":" + men.id + ",\"mentionText\":\"" + udsname + "\"," + "\"mentioned\": { \"user\": {\"displayName\":\"" + udsname + "\",\"id\": \"" + uid + "\",\"userIdentityType\": \"" + men.mentioned.user.userIdentityType + "\"}}}";
                }
                mention += "]";

                //MessageBox.Show(item.body.contentType + item.body.content);
                //MessageBox.Show(mention);
                using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                {

                    string json = "{\"body\":{\"contentType\":\"" + item.body.contentType + "\"," +
                                  "\"content\":\"" + content + "\"}," + mention + "}";
                    /*   
                    string json = "{\"body\":{\"contentType\":\"" + item.body.contentType + "\"," +
                               "\"content\":\"" + item.body.content + "\"}}";
                    */
                    //MessageBox.Show(json);
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
