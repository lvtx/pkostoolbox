using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using Facebook.API;

namespace FbTracker
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            
             
        }
        public MainPage(string sessionKey, string sessionSecret, int expires, int userId)
            : this()
        {

            var api = new FacebookAPI();
            api.ApplicationKey = "9a703a2552981903b1c9a431804af826";
            api.SessionKey = sessionKey;
            api.Secret = sessionSecret;
            api.IsDesktopApplication = false;
            api.ConnectToFacebook();

        }
    }
}
