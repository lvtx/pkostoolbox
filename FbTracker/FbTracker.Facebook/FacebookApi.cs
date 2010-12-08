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
using Facebook.Schema;
using Facebook.Utility;
using System.Reflection;

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
        IList<user> friends;

        private static readonly ILog logger = LogManager.GetLogger(typeof(FacebookApi));
        private static FacebookApi currentInstance;

        #region .Ctor

        protected FacebookApi(string appKey, string appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, "log4net.config");
            
                
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

        public user CurrentUser {  get; private set; }

        
        
        #endregion



        private void GetUserInfoCallback(IList<user> users, object state, FacebookException e)
        {
            if (users != null)
            {
                friends.Union(users.AsEnumerable());
            }
        }

        public void Connect()
        {
            session = new IFrameCanvasSession(appKey, appSecret);
            session.Login();
            API = new Api(session);
            this.Token = API.Auth.CreateToken();
            API.AuthToken = this.token;

            CurrentUser = API.Users.GetInfo(this.session.UserId);
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

        public void LoadFriends()
        {
            if (friends == null)
            {
                friends = new List<user>();
            }
            // API.Friends.GetAppUsersObjectsAsync(GetUserInfoCallback, null);
            friends = API.Friends.GetAppUsersObjects();
        }
        public IList<user> UserFriends
        {
            get
            {
                if (friends == null)
                {
                    friends = new List<user>();
                }

                return friends;
            }
        }
    }
}
