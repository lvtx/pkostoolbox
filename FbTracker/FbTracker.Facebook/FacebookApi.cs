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
using FbTracker.Facebook.DTOs;

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
        FacebookSession session;
        IList<user> friends;

        private static readonly ILog logger = LogManager.GetLogger(typeof(FacebookApi));
        private static FacebookApi currentInstance;
        private ClientType facebookClientType = ClientType.Unkown;

        #region .Ctor

        protected FacebookApi(string appKey, string appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            CommonUtils.ConfigureLogger();
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

        public FbUser CurrentUser {  get; private set; }

        public ClientType FacebookClientType
        {
            get
            {
                if (facebookClientType == ClientType.Unkown)
                {
                    try
                    {
                        facebookClientType = (ClientType)Convert.ToInt32(ConfigurationManager.AppSettings["ClientType"]);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
                return facebookClientType;
            }
        }
        
        #endregion

        
        private void GetUserInfoCallback(IList<user> users, object state, FacebookException e)
        {
            if (users != null)
            {
                friends.Union(users.AsEnumerable());
            }
        }

        /// <summary>
        /// Connect to Facebook
        /// </summary>
        public void Connect()
        {            
            logger.Debug(string.Format("Client type: {0}", FacebookClientType));
            switch (FacebookClientType)
            {
                case ClientType.InsideIFrame:
                    session = new IFrameCanvasSession(appKey, appSecret); 
            session.Login();                   
                    break;
                case ClientType.Outside:
                    session = new ConnectSession(appKey, appSecret); //DesktopSession(appKey, false);    
                    session.ApplicationSecret = this.sessionSecret;
                    if ((session as ConnectSession).IsConnected())
                    {
                        logger.Info("Is connected");
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
           // DesktopSession session2 = new DesktopSession(appKey, false);
            
            session.Login();
            API = new Api(session);
            API.Session.UserId = 726270083;
            
            //this.Token = API.Auth.CreateToken();
            //API.AuthToken = this.token;
            

            CurrentUser = FbUser.UserInfo(this.session.UserId);//API.Users.GetInfo(this.session.UserId);
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
            //friends = API.Friends.GetAppUsersObjects();
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
