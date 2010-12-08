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
namespace FbTracker.Web
{
    public partial class Default : System.Web.UI.Page
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Default));
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
            FacebookApi fb = FacebookApi.Instance;
            fb.Connect();
            if (string.IsNullOrEmpty(fb.Token.Trim()))
            {
                lbl1.Text = "hola world";
            }
            else
            {
                FbUser currentUser = fb.CurrentUser;
                lbl1.Text = currentUser.uid.ToString();
                //Image img = new Image();
                //img.ImageUrl = currentUser.pic_square;
                //form1.Controls.Add(img);
                //fb.LoadFriends();
                //
                //if (fb.UserFriends.Count > 0)
                //{
                //    img.ImageUrl = fb.UserFriends.First().pic_square;
                //}
            }


        }
        
    }
}