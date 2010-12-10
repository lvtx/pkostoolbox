<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FbTracker.Web.Default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml" >
<head id="Head1" runat="server">
    <title>FbTracker</title>
    <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }
    </style>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
    <%--    <script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script>
    <script type="text/javascript" src="fblogin.js"></script>--%>
    
    
<script src="http://connect.facebook.net/en_US/all.js"></script>
<script language="javascript" type="text/javascript">
    function openImages(pageUrl) {
        //var url = "WFImages.aspx?Instrument_ID=" + instrumentId + "&Instrument_Number=" + instrumentNumber;
        window.open(pageUrl, "Images", "width=650, height=400, scrollbars=yes, resizable=yes, left=200, top=100");
    }
    </script>
</head>
<body>   
    
    <form id="form1" runat="server" style="height:100%">
    
    <div id="fb-root"></div>
    <asp:Panel ID="fbLogin" runat="server">
        <fb:login-button onlogin="window.location.reload()"  >Login with facebook</fb:login-button>
    </asp:Panel>
    <div id="silverlightControlHost"> 
        <asp:Label ID="lblStatus" runat="server"></asp:Label>
        <%--<object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param name="source" value="ClientBin/FbTracker.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="3.0.40818.0" />
		  <param name="autoUpgrade" value="true" />
          <asp:Literal ID="paramInit" runat="server"></asp:Literal>
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40818.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object>--%>
     </div>
    </form>
</body>

<script>
    FB.init({
        appId: '161924267186124',
        status: false, // check login status
        cookie: false, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });

    FB.Event.subscribe('auth.login', function (response) {
        window.location.reload();
        });

//    window.fbAsyncInit = function () {
//        FB.init({ appId: '161924267186124', status: true, cookie: true,
//            xfbml: true
//        });
//    };
//    (function () {
//        var e = document.createElement('script'); e.async = true;
//        e.src = document.location.protocol +
//      '//connect.facebook.net/en_US/all.js';
//        document.getElementById('fb-root').appendChild(e);
//    } ());



</script>

</html>
