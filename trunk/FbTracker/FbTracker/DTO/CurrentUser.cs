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
using Facebook.Utility;

namespace FbTracker.DTO
{
    public static class CurrentUser
    {
        public static long UserId { get; set; }
    }

    public class FbUser
    {
        public long uid { get; set; }	//	The user ID of the user being queried.
        public string first_name { get; set; }	//	The first name of the user being queried.
        public string middle_name { get; set; }	//	The middle name of the user being queried.
        public string last_name { get; set; }	//	The last name of the user being queried.
        public string name { get; set; }	//	The full name of the user being queried.
        public string pic_small { get; set; }	//	The URL to the small-sized profile picture for the user being queried. The image can have a maximum width of 50px and a maximum height of 150px. This URL may be blank.
        public string pic_big { get; set; }	//	The URL to the largest-sized profile picture for the user being queried. The image can have a maximum width of 200px and a maximum height of 600px. This URL may be blank.
        public string pic_square { get; set; }	//	The URL to the square profile picture for the user being queried. The image can have a maximum width and height of 50px. This URL may be blank.
        public string pic { get; set; }	//	The URL to the medium-sized profile picture for the user being queried. The image can have a maximum width of 100px and a maximum height of 300px. This URL may be blank.
        public object[] affiliations { get; set; }	//	The networks to which the user being queried belongs. The status field within this field will only return results in English.
        public DateTime profile_update_time { get; set; }	//	The time the profile of the user being queried was most recently updated. If the user's profile has not been updated in the past three days, this value will be 0.
        public string timezone { get; set; }	//	The time zone where the user being queried is located.
        public string religion { get; set; }	//	The religion of the user being queried.
        public string birthday { get; set; }	//	The birthday of the user being queried. The format of this date varies based on the user's locale.
        public string birthday_date { get; set; }	//	The birthday of the user being queried, rendered as a machine-readable string. The format of this date never changes.
        public string sex { get; set; }	//	The gender of the user being queried. This field will only return results in English.
        public object[] hometown_location { get; set; }	//	The home town (and state) of the user being queried.
        public object[] meeting_sex { get; set; }	//	A list of the genders the person the user being queried wants to meet.
        public object[] meeting_for { get; set; }	//	A list of the reasons the user being queried wants to meet someone.
        public string relationship_status { get; set; }	//	The type of relationship for the user being queried. This field will only return results in English.
        public int significant_other_id { get; set; }	//	The user ID of the partner (for example, husband, wife, boyfriend, girlfriend) of the user being queried.
        public string political { get; set; }	//	The political views of the user being queried.
        public object[] current_location { get; set; }	//	The current location of the user being queried.
        public string activities { get; set; }	//	The activities of the user being queried.
        public string interests { get; set; }	//	The interests of the user being queried.
        public bool is_app_user { get; set; }	//	Indicates whether the user being queried has logged in to the current application.
        public string music { get; set; }	//	The favorite music of the user being queried.
        public string tv { get; set; }	//	The favorite television shows of the user being queried.
        public string movies { get; set; }	//	The favorite movies of the user being queried.
        public string books { get; set; }	//	The favorite books of the user being queried.
        public string quotes { get; set; }	//	The favorite quotes of the user being queried.
        public string about_me { get; set; }	//	More information about the user being queried.
        public object[] hs_info { get; set; }	//	Information about high school of the user being queried.
        public object[] education_history { get; set; }	//	Post-high school information for the user being queried.
        public object[] work_history { get; set; }	//	The work history of the user being queried.
        public int notes_count { get; set; }	//	The number of notes created by the user being queried.
        public int wall_count { get; set; }	//	The number of Wall posts for the user being queried.
        public string status { get; set; }	//	The current status of the user being queried.
        public bool has_added_app { get; set; }	//	Deprecated. This value is now equivalent to is_app_user.
        public string online_presence { get; set; }	//	The user's Facebook Chat status. Returns a string, one of active, idle, offline, or error (when Facebook can't determine presence information on the server side). The query does not return the user's Facebook Chat status when that information is restricted for privacy reasons.
        public string locale { get; set; }	//	The two-letter language code and the two-letter country code representing the user's locale. Country codes are taken from the ISO 3166 alpha 2 code list.
        public string proxied_email { get; set; }	//	The proxied wrapper for a user's email address. If the user shared a proxied email address instead of his or her primary email address with you, this address also appears in the email field (see above). Facebook recommends you query the email field to get the email address shared with your application.
        public string profile_url { get; set; }	//	The URL to a user's profile. If the user specified a username for his or her profile, profile_url contains the username.
        public object[] email_hashes { get; set; }	//	An object[] containing a set of confirmed email hashes for the user. Emails are registered via the connect.registerUsers API call and are only confirmed when the user adds your application. The format of each email hash is the crc32 and md5 hashes of the email address combined with an underscore (_).
        public string pic_small_with_logo { get; set; }	//	The URL to the small-sized profile picture for the user being queried. The image can have a maximum width of 50px and a maximum height of 150px, and is overlaid with the Facebook favicon. This URL may be blank.
        public string pic_big_with_logo { get; set; }	//	The URL to the largest-sized profile picture for the user being queried. The image can have a maximum width of 200px and a maximum height of 600px, and is overlaid with the Facebook favicon. This URL may be blank.
        public string pic_square_with_logo { get; set; }	//	The URL to the square profile picture for the user being queried. The image can have a maximum width and height of 50px, and is overlaid with the Facebook favicon. This URL may be blank.
        public string pic_with_logo { get; set; }	//	The URL to the medium-sized profile picture for the user being queried. The image can have a maximum width of 100px and a maximum height of 300px, and is overlaid with the Facebook favicon. This URL may be blank.
        public string allowed_restrictions { get; set; }	//	A comma-delimited list of Demographic Restrictions types a user is allowed to access. Currently, alcohol is the only type that can get returned.
        public bool verified { get; set; }	//	Indicates whether or not Facebook has verified the user.
        public string profile_blurb { get; set; }	//	This string contains the contents of the text box under a user's profile picture.
        public object family { get; set; }	//	Note: For family information, you should query the family FQL table instead.
        //	
        //	This object[] contains a series of entries for the immediate relatives of the user being queried. Each entry is also an object[] containing the following fields:
        //	
        //	relationship -- A string describing the type of relationship. Can be one of parent, mother, father, sibling, sister, brother, child, son, daughter.
        //	uid (optional) -- The user ID of the relative, which gets displayed if the account is linked to (confirmed by) another account. If the relative did not confirm the relationship, the name appears instead.
        //	name (optional) -- The name of the relative, which could be text the user entered. If the relative confirmed the relationship, the uid appears instead.
        //	birthday -- If the relative is a child, this is the birthday the user entered.
        //	
        //	Note: At this time, you cannot query for a specific relationship (like SELECT family FROM user WHERE family.relationship = 'daughter' AND uid = '$x'); you'll have to query on the family field and filter the results yourself.
        public string username { get; set; }	//	The username of the user being queried.
        public string website { get; set; }	//	The website of the user being queried.
        public bool is_blocked { get; set; }	//	Returns true if the user is blocked to the viewer/logged in user.
        public string contact_email { get; set; }	//	A string containing the user's primary Facebook email address. If the user shared his or her primary email address with you, this address also appears in the email field (see below). Facebook recommends you query the email field to get the email address shared with your application.
        public string email { get; set; }	//	A string containing the user's primary Facebook email address or the user's proxied email address, whichever address the user granted your application. Facebook recommends you query this field to get the email address shared with your application.
        public string third_party_id { get; set; }	//	A string containing an anonymous, but unique identifier for the user. You can use this identifier with third-parties.

        private FbUser instance;

        private FbUser()
        {
        }

        private void LoadUser()
        {
            string qry = string.Format("SELECT uid, name, pic_square FROM user WHERE uid = {0}", uid);
            FbAccess.FbAPI.Fql.QueryAsync(qry, new Facebook.Rest.Fql.QueryCallback(QueryCallback), null);
        }

        public static FbUser CreateUser(long uid)
        {
            if (FbAccess.FbAPI != null)
            {
                FbUser instance = new FbUser();
                instance.uid = uid;                
                return instance;
            }
            return null;
        }
        void QueryCallback(string result, object state, FacebookException e)
        {
            string results = result;
            name = result;
        }

    }
}
