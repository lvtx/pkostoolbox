using System;
using System.Net;
using log4net;
using Facebook.Rest;

namespace FbTracker.Facebook.DTOs
{
    public class FbUser
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(FbUser));
        private Api api;
        #region Properties 
        ///	<sumary>	
        ///	The user ID of the user being queried.	
        ///	</summary>	
        public long uid { get; private set; }
        ///	<sumary>	
        ///	The first name of the user being queried.	
        ///	</summary>	
        public string first_name { get; private set; }
        ///	<sumary>	
        ///	The middle name of the user being queried.	
        ///	</summary>	
        public string middle_name { get; private set; }
        ///	<sumary>	
        ///	The last name of the user being queried.	
        ///	</summary>	
        public string last_name { get; private set; }
        ///	<sumary>	
        ///	The full name of the user being queried.	
        ///	</summary>	
        public string name { get; private set; }
        ///	<sumary>	
        ///	The URL to the small-sized profile picture for the user being queried. The image can have a maximum width of 50px and a maximum height of 150px. This URL may be blank.	
        ///	</summary>	
        public string pic_small { get; private set; }
        ///	<sumary>	
        ///	The URL to the largest-sized profile picture for the user being queried. The image can have a maximum width of 200px and a maximum height of 600px. This URL may be blank.	
        ///	</summary>	
        public string pic_big { get; private set; }
        ///	<sumary>	
        ///	The URL to the square profile picture for the user being queried. The image can have a maximum width and height of 50px. This URL may be blank.	
        ///	</summary>	
        public string pic_square { get; private set; }
        ///	<sumary>	
        ///	The URL to the medium-sized profile picture for the user being queried. The image can have a maximum width of 100px and a maximum height of 300px. This URL may be blank.	
        ///	</summary>	
        public string pic { get; private set; }
        ///	<sumary>	
        ///	The networks to which the user being queried belongs. The status field within this field will only return results in English.	
        ///	</summary>	
        public object affiliations { get; private set; }
        ///	<sumary>	
        ///	The time the profile of the user being queried was most recently updated. If the user's profile has not been updated in the past three days, this value will be 0.	
        ///	</summary>	
        public DateTime profile_update_time { get; private set; }
        ///	<sumary>	
        ///	The time zone where the user being queried is located.	
        ///	</summary>	
        public string timezone { get; private set; }
        ///	<sumary>	
        ///	The religion of the user being queried.	
        ///	</summary>	
        public string religion { get; private set; }
        ///	<sumary>	
        ///	The birthday of the user being queried. The format of this date varies based on the user's locale.	
        ///	</summary>	
        public string birthday { get; private set; }
        ///	<sumary>	
        ///	The birthday of the user being queried, rendered as a machine-readable string. The format of this date never changes.	
        ///	</summary>	
        public string birthday_date { get; private set; }
        ///	<sumary>	
        ///	The gender of the user being queried. This field will only return results in English.	
        ///	</summary>	
        public string sex { get; private set; }
        ///	<sumary>	
        ///	The home town (and state) of the user being queried.	
        ///	</summary>	
        public object hometown_location { get; private set; }
        ///	<sumary>	
        ///	A list of the genders the person the user being queried wants to meet.	
        ///	</summary>	
        public object meeting_sex { get; private set; }
        ///	<sumary>	
        ///	A list of the reasons the user being queried wants to meet someone.	
        ///	</summary>	
        public object meeting_for { get; private set; }
        ///	<sumary>	
        ///	The type of relationship for the user being queried. This field will only return results in English.	
        ///	</summary>	
        public string relationship_status { get; private set; }
        ///	<sumary>	
        ///	The user ID of the partner (for example, husband, wife, boyfriend, girlfriend) of the user being queried.	
        ///	</summary>	
        public int significant_other_id { get; private set; }
        ///	<sumary>	
        ///	The political views of the user being queried.	
        ///	</summary>	
        public string political { get; private set; }
        ///	<sumary>	
        ///	The current location of the user being queried.	
        ///	</summary>	
        public object current_location { get; private set; }
        ///	<sumary>	
        ///	The activities of the user being queried.	
        ///	</summary>	
        public string activities { get; private set; }
        ///	<sumary>	
        ///	The interests of the user being queried.	
        ///	</summary>	
        public string interests { get; private set; }
        ///	<sumary>	
        ///	Indicates whether the user being queried has logged in to the current application.	
        ///	</summary>	
        public bool is_app_user { get; private set; }
        ///	<sumary>	
        ///	The favorite music of the user being queried.	
        ///	</summary>	
        public string music { get; private set; }
        ///	<sumary>	
        ///	The favorite television shows of the user being queried.	
        ///	</summary>	
        public string tv { get; private set; }
        ///	<sumary>	
        ///	The favorite movies of the user being queried.	
        ///	</summary>	
        public string movies { get; private set; }
        ///	<sumary>	
        ///	The favorite books of the user being queried.	
        ///	</summary>	
        public string books { get; private set; }
        ///	<sumary>	
        ///	The favorite quotes of the user being queried.	
        ///	</summary>	
        public string quotes { get; private set; }
        ///	<sumary>	
        ///	More information about the user being queried.	
        ///	</summary>	
        public string about_me { get; private set; }
        ///	<sumary>	
        ///	Information about high school of the user being queried.	
        ///	</summary>	
        public object hs_info { get; private set; }
        ///	<sumary>	
        ///	Post-high school information for the user being queried.	
        ///	</summary>	
        public object education_history { get; private set; }
        ///	<sumary>	
        ///	The work history of the user being queried.	
        ///	</summary>	
        public object work_history { get; private set; }
        ///	<sumary>	
        ///	The number of notes created by the user being queried.	
        ///	</summary>	
        public int notes_count { get; private set; }
        ///	<sumary>	
        ///	The number of Wall posts for the user being queried.	
        ///	</summary>	
        public int wall_count { get; private set; }
        ///	<sumary>	
        ///	The current status of the user being queried.	
        ///	</summary>	
        public string status { get; private set; }
        ///	<sumary>	
        ///	Deprecated. This value is now equivalent to is_app_user.	
        ///	</summary>	
        public bool has_added_app { get; private set; }
        ///	<sumary>	
        ///	The user's Facebook Chat status. Returns a string, one of active, idle, offline, or error (when Facebook can't determine presence information on the server side). The query does not return the user's Facebook Chat status when that information is restricted for privacy reasons.	
        ///	</summary>	
        public string online_presence { get; private set; }
        ///	<sumary>	
        ///	The two-letter language code and the two-letter country code representing the user's locale. Country codes are taken from the ISO 3166 alpha 2 code list.	
        ///	</summary>	
        public string locale { get; private set; }
        ///	<sumary>	
        ///	The proxied wrapper for a user's email address. If the user shared a proxied email address instead of his or her primary email address with you, this address also appears in the email field (see above). Facebook recommends you query the email field to get the email address shared with your application.	
        ///	</summary>	
        public string proxied_email { get; private set; }
        ///	<sumary>	
        ///	The URL to a user's profile. If the user specified a username for his or her profile, profile_url contains the username.	
        ///	</summary>	
        public string profile_url { get; private set; }
        ///	<sumary>	
        ///	An array containing a set of confirmed email hashes for the user. Emails are registered via the connect.registerUsers API call and are only confirmed when the user adds your application. The format of each email hash is the crc32 and md5 hashes of the email address combined with an underscore (_).	
        ///	</summary>	
        public object email_hashes { get; private set; }
        ///	<sumary>	
        ///	The URL to the small-sized profile picture for the user being queried. The image can have a maximum width of 50px and a maximum height of 150px, and is overlaid with the Facebook favicon. This URL may be blank.	
        ///	</summary>	
        public string pic_small_with_logo { get; private set; }
        ///	<sumary>	
        ///	The URL to the largest-sized profile picture for the user being queried. The image can have a maximum width of 200px and a maximum height of 600px, and is overlaid with the Facebook favicon. This URL may be blank.	
        ///	</summary>	
        public string pic_big_with_logo { get; private set; }
        ///	<sumary>	
        ///	The URL to the square profile picture for the user being queried. The image can have a maximum width and height of 50px, and is overlaid with the Facebook favicon. This URL may be blank.	
        ///	</summary>	
        public string pic_square_with_logo { get; private set; }
        ///	<sumary>	
        ///	The URL to the medium-sized profile picture for the user being queried. The image can have a maximum width of 100px and a maximum height of 300px, and is overlaid with the Facebook favicon. This URL may be blank.	
        ///	</summary>	
        public string pic_with_logo { get; private set; }
        ///	<sumary>	
        ///	A comma-delimited list of Demographic Restrictions types a user is allowed to access. Currently, alcohol is the only type that can get returned.	
        ///	</summary>	
        public string allowed_restrictions { get; private set; }
        ///	<sumary>	
        ///	Indicates whether or not Facebook has verified the user.	
        ///	</summary>	
        public bool verified { get; private set; }
        ///	<sumary>	
        ///	This string contains the contents of the text box under a user's profile picture.	
        ///	</summary>	
        public string profile_blurb { get; private set; }
        ///	<sumary>	
        ///	"Note: For family information, you should query the family FQL table instead.
        /// This array contains a series of entries for the immediate relatives of the user being queried. Each entry is also an array containing the following fields:
        ////relationship -- A string describing the type of relationship. Can be one of parent, mother, father, sibling, sister, brother, child, son, daughter.
        /// uid (optional) -- The user ID of the relative, which gets displayed if the account is linked to (confirmed by) another account. If the relative did not confirm the relationship, the name appears instead.
        /// name (optional) -- The name of the relative, which could be text the user entered. If the relative confirmed the relationship, the uid appears instead.
        /// birthday -- If the relative is a child, this is the birthday the user entered.
        /// Note: At this time, you cannot query for a specific relationship (like SELECT family FROM user WHERE family.relationship = 'daughter' AND uid = '$x'); you'll have to query on the family field and filter the results yourself."	
        ///	</summary>	
        public object family { get; private set; }
        ///	<sumary>	
        ///	The username of the user being queried.	
        ///	</summary>	
        public string username { get; private set; }
        ///	<sumary>	
        ///	The website of the user being queried.	
        ///	</summary>	
        public string website { get; private set; }
        ///	<sumary>	
        ///	Returns true if the user is blocked to the viewer/logged in user.	
        ///	</summary>	
        public bool is_blocked { get; private set; }
        ///	<sumary>	
        ///	A string containing the user's primary Facebook email address. If the user shared his or her primary email address with you, this address also appears in the email field (see below). Facebook recommends you query the email field to get the email address shared with your application.	
        ///	</summary>	
        public string contact_email { get; private set; }
        ///	<sumary>	
        ///	A string containing the user's primary Facebook email address or the user's proxied email address, whichever address the user granted your application. Facebook recommends you query this field to get the email address shared with your application.	
        ///	</summary>	
        public string email { get; private set; }
        ///	<sumary>	
        ///	A string containing an anonymous, but unique identifier for the user. You can use this identifier with third-parties.	
        ///	</summary>	
        public string third_party_id { get; private set; }
        
        #endregion

        private FbUser(long uid)
        {
            this.uid = uid;
            api = FacebookApi.Instance.API;
            string res = api.Fql.Query("select * from user where uid = " + uid.ToString());
        }

        public static FbUser UserInfo(long uid)
        {
            try
            {
                logger.Info("entro");
                FbUser fbu = new FbUser(uid);
                return fbu;
            }
            catch(Exception ex) 
            {
                logger.Error(ex);
            }
            return null;
        }
    }
}
