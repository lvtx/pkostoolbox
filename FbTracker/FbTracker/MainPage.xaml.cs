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
using FbTracker.Facebook;


namespace FbTracker
{
    public partial class MainPage : UserControl
    {

        //FacebookDataAccess fbda;
        FBAccess fba;
        public MainPage()
        {
            InitializeComponent();
             
        }
        public MainPage(string sessionKey, string sessionSecret, DateTime expires, long userId)
            : this()
        {
            //tbName.Text = string.Format("{0}\n{1}\n{2}\n{3}", sessionKey, sessionSecret, expires, userId);
            //fbda = FacebookDataAccess.CreateLoggedSession("9a703a2552981903b1c9a431804af826", "989ae6e3f64e32d97fb7f40f6e33c7d9", sessionKey, sessionSecret, expires, userId);
            fba = new FBAccess("9a703a2552981903b1c9a431804af826", "989ae6e3f64e32d97fb7f40f6e33c7d9", sessionKey, sessionSecret, expires, userId);
            fba.ConnectFb();
           
        }
    }
}
