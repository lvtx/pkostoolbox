using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;
using System.Net;
using System.Collections.Specialized;
using System.Web;
using System.IO;
using Facebook.Session;
using Facebook.Rest;

namespace FbTracker.Facebook
{


    public enum Method
    {
        GET,
        POST,
        DELETE
    }

    public class FacebookApi
    {
        string appKey;
        string appSecret;
        string sessionKey;
        string sessionSecret;
        DateTime expires;
        long userId;
        string responseString;
        string token = string.Empty;
        string page_token = string.Empty;
        string callback_url = string.Empty;
        IFrameCanvasSession session;

        #region .Ctor

        protected FacebookApi(string appKey, string appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
        }

        #endregion


        #region properties

        public string Page_token
        {
            get { return page_token; }
            set { page_token = value; }
        }

        public string Callback_url
        {
            get { return callback_url; }
            set { callback_url = value; }
        }

        public string Token
        {
            get { return this.token; }
            set { this.token = value; }
        }

        public Api API { get; set; }
        
        #endregion


        private static readonly ILog logger = LogManager.GetLogger(typeof(FacebookApi));
        private static FacebookApi currentInstance;


        public void Connect()
        {
            session = new IFrameCanvasSession(appKey, appSecret);
            session.Login();
            API = new Api(session);
            this.Token = API.Auth.CreateToken();
            API.AuthToken = this.token;

            this.Token = API.Fql.Query("SELECT name FROM user WHERE uid = me() ");
 
            //string accessTokenUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&client_secret={3}", page_token, this.appKey, callback_url, this.appSecret);
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(accessTokenUrl);
            //request.Method = "GET";
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //string results = sr.ReadToEnd();
            //sr.Close();
            //NameValueCollection qs = HttpUtility.ParseQueryString(results);
            //this.Token = results;
            //return string.Empty;
            //if (qs["auth_token"] == null)
            //{
            //    string other = results.Substring(results.IndexOf("href ="));
            //    other = other.Substring(other.IndexOf("\"")+1);
            //    other = other.Substring(0, other.IndexOf("\"") );
            //    request = (HttpWebRequest)HttpWebRequest.Create(other);
            //    response = (HttpWebResponse)request.GetResponse();


            //    sr = new StreamReader(response.GetResponseStream());
            //    results = sr.ReadToEnd();
            //    sr.Close();
            //    qs = HttpUtility.ParseQueryString(results);
            //    return results;
            //}
            //else
            //{
            //    this.token = qs["auth_token"];
            //    return string.Empty;
            //}

        }

        public static FacebookApi Instance
        {
            get
            {
                if (currentInstance == null)
                {

                    if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["appKey"]) && string.IsNullOrEmpty(ConfigurationManager.AppSettings["appSecret"]))
                    {
                        throw new Exception("appKey and appSecret settions is not set");
                    }
                    string appKey = ConfigurationManager.AppSettings["appKey"];
                    string appSecret = ConfigurationManager.AppSettings["appSecret"];
                    string accessToken = ConfigurationManager.AppSettings["PAGE_TOKEN"];
                    string callbackUrl = ConfigurationManager.AppSettings["CALLBACK_URL"];
                    currentInstance = new FacebookApi(appKey, appSecret);
                    currentInstance.page_token = accessToken;
                    currentInstance.Callback_url = callbackUrl;
                }
                return currentInstance;
            }
        }


    }
}
