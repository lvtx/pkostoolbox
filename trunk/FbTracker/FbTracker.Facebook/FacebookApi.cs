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
using System.Xml;

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
        string appId;
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
        private static ClientType facebookClientType = ClientType.Unkown;
        private Api _facebookApi;

        #region .Ctor

        protected FacebookApi(string client_id, string appKey, string appSecret)
        {
            logger.Debug(string.Format("client_id: {0} \nAppKey: {1} \nAppSecret:{2}", client_id, appKey, appSecret));
            this.appId = client_id;
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

        public Api API
        {
            get { return _facebookApi; }
        }

        public FbUser CurrentUser {  get; private set; }

        public static ClientType FacebookClientType
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
        public bool Connect(HttpCookie facebookCockie = null)
        {
            logger.Debug(string.Format("Client type: {0}", FacebookClientType));
            switch (FacebookClientType)
            {
                case ClientType.InsideIFrame:
                    session = new IFrameCanvasSession(appKey, appSecret);
                    session.Login();
                    _facebookApi = new Api(session);
                    this.Token = API.Auth.CreateToken();
                    _facebookApi.AuthToken = this.token;                    
                    session.Login();
                    break;
                case ClientType.Outside:
                    if (facebookCockie == null)
                    {
                        return false;
                    }
                    else
                    {
                        logger.Debug(string.Format("Coockie value:\n{0}", facebookCockie.Value));
                        session = new ConnectSession(appKey, appSecret);
                        _facebookApi = new Api(session);
                        _facebookApi.Session.SessionSecret = facebookCockie["secret"];
                        _facebookApi.Session.SessionKey = facebookCockie["session_key"];
                        _facebookApi.Session.UserId = Convert.ToInt64(facebookCockie["uid"].Replace('"', ' ').Trim());
                        _facebookApi.AuthToken = facebookCockie["access_token"];
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            CurrentUser = FbUser.UserInfo(session.UserId);
            return true;
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
                    string appId = ConfigurationManager.AppSettings["client_id"];                       
                    string appKey = ConfigurationManager.AppSettings["appKey"];
                    string appSecret = ConfigurationManager.AppSettings["appSecret"];
                    string accessToken = ConfigurationManager.AppSettings["PAGE_TOKEN"];
                    string callbackUrl = ConfigurationManager.AppSettings["CALLBACK_URL"];
                    currentInstance = new FacebookApi(appId, appKey, appSecret);
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

        public XmlDocument FqlQuery(string query)
        {
            try
            {                
                XmlDocument doc = null;
                logger.Debug(string.Format("FQL Query: {0}", query));
                if (!string.IsNullOrEmpty(query.Trim()))
                {
                    string res = _facebookApi.Fql.Query(query);
                    logger.Debug(string.Format("FQL Response: {0}", res));
                    if (!string.IsNullOrEmpty(res))
                    {
                        doc = new XmlDocument();
                        doc.LoadXml(res);
                    }
                }
                return doc;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }
    

    }
}
