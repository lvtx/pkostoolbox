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
using FbTracker.DTO;
using System.Threading;

namespace FbTracker
{
    public partial class MainPage : UserControl
    {
        private readonly FbAccess _dataAccess;
        public MainPage()
        {
            InitializeComponent();
            
             
        }
        public MainPage(string sessionKey, string sessionSecret, int expires, int userId)
            : this()
        {
            _dataAccess = new FbAccess(sessionKey, sessionSecret, expires, userId);

            tbName.Text = FbAccess.CurrentUser.uid.ToString() + " Name: " + FbAccess.CurrentUser.name;
            
        }
    }
}
