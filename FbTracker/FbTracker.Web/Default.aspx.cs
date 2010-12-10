using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Net;
using FbTracker.Facebook;
using System.Collections.Specialized;
using FbTracker.Facebook.DTOs;
using log4net;
using System.Configuration;
using Facebook.Session;
using Facebook.Rest;
using Facebook.Schema;
namespace FbTracker.Web
{
    public partial class Default : System.Web.UI.Page
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Default));
        // NOTE: In production app, these keys would be stored in config file
        private const string APPLICATION_KEY = "741d66bac1bbbad52ec40a1fedafe8e1";
        private const string SECRET_KEY = "85c8249e1e741731dbb0f52dc46f582c";

        private Api _facebookAPI;
       // private List<EventUser> _eventUsers;
        private ConnectSession _connectSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonUtils.ConfigureLogger();
            //FacebookSession _session = new IFrameCanvasSession("9a703a2552981903b1c9a431804af826", "989ae6e3f64e32d97fb7f40f6e33c7d9");

            //_session.Login();
            //StringBuilder sb = new StringBuilder();
            //sb.Append(string.Format("sessionKey={0}, ", _session.SessionKey));
            //sb.Append(string.Format("sessionSecret={0}, ", _session.SessionSecret));
            //sb.Append(string.Format("expires={0}, ", _session.ExpiryTime));
            //sb.Append(string.Format("userId={0}, ", _session.UserId));
            //sb.Append("<param name=\"InitParams\" value=\"" + sb.ToString() + "\" > ");
            //paramInit.Text = sb.ToString();
           
            logger.Info("Page loading");
            fbLogin.Visible = false;
            HttpCookie fbCockie = null;
            try
            {
                fbCockie = Request.Cookies[string.Format("fbs_{0}", ConfigurationManager.AppSettings["client_id"].Trim())];
                logger.Info(string.Format("Coockie: {0}", fbCockie.Value));
            }
            catch { }
            FacebookApi fb = FacebookApi.Instance;
            if (!fb.Connect(fbCockie))
            {
                fbLogin.Visible = true & FacebookApi.FacebookClientType == ClientType.Outside;
            }
            else
            {
                lblStatus.Text = FacebookApi.Instance.CurrentUser.name;
            }
          
            //_connectSession = new ConnectSession(APPLICATION_KEY, SECRET_KEY);

            //if (fbCockie == null)
            //{
            //    // Not authenticated, proceed as usual.                

            //    //_connectSession.Login();
            //    lblStatus.Text = "Please sign-in with Facebook.";

            //}
            //else
            //{
            //    // Authenticated, create API instance                

            //    //lblStatus.Text = fbCockie["uid"].Replace('"', ' ').Trim();
            //    _facebookAPI = new Api(_connectSession);
            //    _facebookAPI.Session.SessionSecret = fbCockie["secret"];
            //    _facebookAPI.Session.SessionKey = fbCockie["session_key"];
            //    _facebookAPI.Session.UserId = Convert.ToInt64(fbCockie["uid"].Replace('"', ' ').Trim());
            //    _facebookAPI.AuthToken = fbCockie["access_token"];
                

            //    // Load user
            //    user user = _facebookAPI.Users.GetInfo();

            //    if (user == null)
            //    {
            //        lblStatus.Text += "  user is null";
            //    }
            //    if (!Page.IsPostBack)
            //    {
            //        lblStatus.Text = string.Format("Signed in as {0} {1}", user.first_name, user.last_name);
            //        //NewEventGrantPermission.NavigateUrl =
            //        //    string.Format("http://www.facebook.com/authorize.php?api_key={0}&v=1.0&ext_perm={1}", APPLICATION_KEY, Enums.ExtendedPermissions.create_event);
            //        //NewEventDate.Text = DateTime.Now.AddDays(7).ToString();
            //        //NewEventHost.Text = string.Format("{0} {1}", user.first_name, user.last_name);

            //        //// Load Existing Events
            //        //LoadExistingEvents();
            //    }
            //}

            //if (!Page.IsPostBack)
            //{
            //    // Set section visibility
            //    //ToggleSectionVisibility();
            //}



        }

        protected string getAppId()
        {
            return ConfigurationManager.AppSettings["client_id"].Trim();
        }
        
    }
}