using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Json;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Browser;

namespace FbTracker.Facebook
{
    public enum Method
    {
        GET,
        POST,
        DELETE
    }
    public class FBAccess
    {
        string appKey;
        string appSecret;
        string sessionKey;
        string sessionSecret;
        DateTime expires;
        long userId;
        string responseString;
        string token = string.Empty;

        public FBAccess(string appKey, string appSecret, string sessionKey, string sessionSecret, DateTime expires, long userId)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.sessionKey = sessionKey;
            this.sessionSecret = sessionSecret;
            this.expires = expires;
            this.userId = userId;
        }

        public void ConnectFb()
        {
            string ACCESS_TOKEN = "https://graph.facebook.com/oauth/access_token";
            string CALLBACK_URL = "http://localhost/FbTracker";
            string accessTokenUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&client_secret={3}&code={4}",ACCESS_TOKEN, this.appKey, CALLBACK_URL, this.appSecret, "");
            string response = requestFBData(accessTokenUrl, Method.GET);
//if (response.Length > 0)
//{
//    //Store the returned access_token
//    NameValueCollection qs = HttpUtility.ParseQueryString(response);

//    if (qs["access_token"] != null)
//    {
//    this.token = qs["access_token"];
//    }
//}
          
            string url = "https://graph.facebook.com/me?fields=id,name,email&access_token=" + "11321";
            //JsonObject myProfile = JsonObject.Parse(
            string res = requestFBData(url, Method.GET);
            JsonValue myProfile = JsonObject.Parse(res);

            string myID = myProfile["id"].ToString().Replace("\"", "");
            string myName = myProfile["name"].ToString().Replace("\"", "");
            string email = myProfile["email"].ToString().Replace("\"", "");

            //lblID.Text = myID;
            //lblFullName.Text = myName;
            //lblEmail.Text = email;
            //imgUser.ImageUrl = "https://graph.facebook.com/me/picture?type=large&access_token=" + oAuth.Token;

        }

        private static ManualResetEvent allDone = new ManualResetEvent(false);


        public string requestFBData(string action, Method method)
        {
          
            
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(action);
            req.Method = method.ToString();
                
            if (method == Method.POST)
            {
                req.ContentType = "application/x-www-form-urlencoded";
            }
           // HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            req.BeginGetResponse(new AsyncCallback(GetResponseCallback), req);
            allDone.WaitOne();

            string results = string.Empty;
            if (!string.IsNullOrEmpty(responseString))
            {
                results = responseString;
                responseString = null;
            }

            return results;
            
        }
        
        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            // End the operation
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            responseString = streamRead.ReadToEnd();
            
            streamResponse.Close();
            streamRead.Close();

            // Release the HttpWebResponse
            response.Close();
            allDone.Set();
        }

        //Profile URL




        //#region JSON.Net Friends

        ////Friends URL
        //url = "https://graph.facebook.com/me/friends?access_token=" + oAuth.Token;


        //JObject myFriends = JObject.Parse(requestFBData(url));

        //string id="";
        //string name = "";

        ////Loop through the returned friends
        //foreach (var i in myFriends["data"].Children())
        //{
        //    id = i["id"].ToString().Replace("\"", "");
        //    name = i["name"].ToString().Replace("\"", "");
        //    lblFriends.Text = lblFriends.Text + "<br/> " + "id: " + id + " name: " + name + "<img src=" + "https://graph.facebook.com/" + id + "/picture>";
        //}

        //#endregion






    }
}
