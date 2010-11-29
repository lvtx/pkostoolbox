using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Facebook.Session;

namespace FbTracker.Web
{
    [Serializable()]
    public class FbAccess 
    {
        public FacebookSession Session { get; set; }        
    }
}