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
using Facebook.Session;
using Facebook.Rest;
using FbTracker.DTO;
using System.ComponentModel;
using Facebook.Schema;
using System.Collections.Generic;
using Facebook.Utility;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace FbTracker
{
    public class FbAccess
    {
        
        private BrowserSession _browserSession;        
        private const string ApplicationKey = "9a703a2552981903b1c9a431804af826";
        private const string ApplicationSecret = "989ae6e3f64e32d97fb7f40f6e33c7d9";
        private static Api _fb;
        private string _token;

        public static Api FbAPI
        {
            get { return _fb; }
        }
        public static FbUser CurrentUser { get; set; }
        public FbAccess(string sessionKey, string sessionSecret, int expires, int userId)
        {

            _browserSession = new BrowserSession(ApplicationKey);
            //_browserSession.SessionKey = sessionKey;
            //_browserSession.SessionSecret = sessionSecret;
            //_browserSession.ApplicationSecret = ApplicationSecret;
            _browserSession.LoggedIn(sessionKey, sessionSecret, expires, userId);
            _browserSession.LoginCompleted += new EventHandler<AsyncCompletedEventArgs>(browserSession_LoginCompleted);
            _browserSession.Login();
            CurrentUser = FbUser.CreateUser(_browserSession.UserId);
            
           
        }

        void browserSession_LoginCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _fb = new Api(_browserSession);
            _fb.Users.GetInfoAsync(new Users.GetInfoCallback(GetUserInfoCompleted), null);
        }

        void GetUserInfoCompleted(IList<user> users, Object state, FacebookException e)
        {
            if (e == null)
            {
                user u = users.First();
                if (u.pic != null)
                {
                    Uri uri = new Uri(u.pic);
                    Dispatcher.BeginInvoke(() =>
                    {
                        ProfilePhoto.Source = new BitmapImage(uri);
                        ProfileStatus.Text = u.status.message;
                        ProfileName.Text = u.first_name + " " + u.last_name;
                    });
                }
            }
            else
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e));
            }
        }
            
        

        //private void OnCreateTokenCompleted(string token, Object state, FacebookException e)
        //{
        //    if (e == null)
        //    {
        //        Dispatcher.BeginInvoke(() =>
        //                                   {
        //                                       MessageBox.Show(string.Format("Token created as {0}.", token));
        //                                       _token = token;                               
        //                                   });
        //    }
        //    else
        //    {
        //        Dispatcher.BeginInvoke(() => MessageBox.Show("Error: " + e.Message));
        //    }        
        //}



    }
}
