using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook.Session;
using System.Data.Objects;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Runtime.Serialization.Json;
namespace FbTracker.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FacebookSession _session = new IFrameCanvasSession("9a703a2552981903b1c9a431804af826", "989ae6e3f64e32d97fb7f40f6e33c7d9");
           
            _session.Login();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("sessionKey={0}, ", _session.SessionKey));
            sb.Append(string.Format("sessionSecret={0}, ", _session.SessionSecret));
            sb.Append(string.Format("expires={0}, ", _session.ExpiryTime.Second));
            sb.Append(string.Format("userId={0}, ", _session.UserId));

            sb.Append("<param name=\"InitParams\" value=\"" + sb.ToString() + "\" > ");            
            paramInit.Text = sb.ToString();
           // sl.Controls.Add(lt);    
           

        }
        
    }
}