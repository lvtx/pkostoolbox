#if SILVERLIGHT
#else
    using Facebook.Forms;
#endif

namespace Facebook.API
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using System.IO;
	using System.Net;
	using System.Security.Cryptography;
	using System.Text;
	using System.Web;
	using System.Xml;

	using Entity;
	using Exceptions;
	using Parser;
	using Properties;
	using Types;
	using Utility;

	/// <summary>
	/// Provides various methods to use the Facebook Platform API.
	/// </summary>
	public partial class FacebookAPI
	{
		private const string ANDCLAUSE = " AND";
		private const string NEWLINE = "\r\n";
		private const string PREFIX = "--";
		private const string VERSION = "1.0";

		public string ApplicationKey { get; set; }

		/// <summary>
		/// Whether or not the session expires
		/// </summary>
		public bool SessionExpires { get; private set; }

		/// <summary>
		/// Whether or not this component is being used in a desktop application
		/// </summary>
		public bool IsDesktopApplication { get; set; }

		/// <summary>
		/// Secret word
		/// </summary>
		public string Secret { get; set; }

		/// <summary>
		/// Session key
		/// </summary>
		public string SessionKey { get; set; }

		/// <summary>
		/// User Id
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// Authorization token
		/// </summary>
		public string AuthToken { get; set; }

		private string LoginUrl
		{
			get
			{
				var args = new object[2];
				args[0] = ApplicationKey;
				args[1] = AuthToken;

				return String.Format(CultureInfo.InvariantCulture, Resources.FacebookLoginUrl, args);
			}
		}

		/// <summary>
		/// XML Namespace Manager
		/// </summary>
		public XmlNamespaceManager NsManager { get; private set; }

		internal CultureInfo InstalledCulture { get; private set; }

		#region Public Methods

		/// <summary>
		/// Creates and sets a new authentication token.
		/// </summary>
		public void SetAuthenticationToken()
		{
			var parameterList = new Dictionary<string, string>(2) {{"method", "facebook.auth.createToken"}};

			AuthToken = GetSingleNode(ExecuteApiCallString(parameterList, true), "auth_createToken_response");
		}

		/// <summary>
		/// Displays an integrated browser to allow the user to log on to the
		/// Facebook web page.
		/// </summary>
		public void ConnectToFacebook()
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				System.Windows.Forms.DialogResult result;
				SetAuthenticationToken();

				using (var formLogin = new FacebookAuthentication(LoginUrl))
				{
					result = formLogin.ShowDialog();
				}
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					CreateSession();
				}
				else
				{
					throw new FacebookInvalidUserException("Login attempt failed");
				}
			}
		}


		/// <summary>
		/// Creates a new session with Facebook.
		/// </summary>
		/// <param name="authToken">The auth token received from Facebook.</param>
		public void CreateSession(string authToken)
		{
			AuthToken = authToken;
			CreateSession();
		}

		/// <summary>
		/// Sends a direct FQL query to FB
		/// </summary>
		/// <param name="query">FQL Query</param>
		/// <returns>Result of the FQL query as an XML string</returns> 
		public string DirectFQLQuery(string query)
		{
			if (String.IsNullOrEmpty(query))
			{
				throw new FacebookException("Query string is required");
			}

			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}, {"query", query}};
			return ExecuteApiCallString(parameterList, true);
		}


		/// <summary>
		/// Gets all events for the logged in user.
		/// </summary>
		/// <returns>A collection of the events.</returns>
		public Collection<FacebookEvent> GetEvents()
		{
			return GetEvents(null, UserId, null, null);
		}

		/// <summary>
		/// Get all event ids for the logged in user.
		/// </summary>
		/// <returns>A collection of the event IDs.</returns>
		public Collection<string> GetEventIds()
		{
			var eventIds = new Collection<string>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetEventIdsXML(), "fql_query_response", "event_member", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				eventIds.Add(XmlHelper.GetNodeText(node, "eid"));
			}

			return eventIds;
		}

		/// <summary>
		/// Gets the XML representation of the ids of all events for the logged in user.
		/// </summary>
		/// <returns>Event id list as raw XML</returns>
		public string GetEventIdsXML()
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}};

			if (string.IsNullOrEmpty(UserId))
			{
				throw new FacebookException("User Id is required");
			}

			parameterList.Add("query", String.Format(CultureInfo.InvariantCulture,
			                                         "SELECT eid FROM event_member WHERE uid = {0}", UserId));
			return ExecuteApiCallString(parameterList, true);
		}


		/// <summary>
		/// Get all events for the specified user.
		/// </summary>
		/// <param name="userId">User to return events for</param>
		/// <returns>event list</returns>
		public Collection<FacebookEvent> GetEvents(string userId)
		{
			return GetEvents(null, userId, null, null);
		}

		/// <summary>
		/// Get all events in the list of events.
		/// </summary>
		/// <param name="eventList">list of event ids</param>
		/// <returns>event list</returns>
		public Collection<FacebookEvent> GetEvents(Collection<string> eventList)
		{
			return GetEvents(eventList, null, null, null);
		}

		/// <summary>
		/// Get all events for the specified user and list of events
		/// </summary>
		/// <param name="userId">User to return events for</param>
		/// <param name="eventList">list of event ids</param>
		/// <returns>event list</returns>
		public Collection<FacebookEvent> GetEvents(Collection<string> eventList, string userId)
		{
			return GetEvents(eventList, userId, null, null);
		}

		/// <summary>
		/// Get all events for the specified user and list of events between 2 dates
		/// </summary>
		/// <param name="userId">User to return events for</param>
		/// <param name="eventList">list of event ids</param>
		/// <param name="startDate">events occuring after this date</param>
		/// <param name="endDate">events occuring before this date</param>
		/// <returns>event list</returns>
		public Collection<FacebookEvent> GetEvents(Collection<string> eventList, string userId, DateTime? startDate, DateTime? endDate)
		{
			var events = new Collection<FacebookEvent>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetEventsXML(eventList, userId, startDate, endDate), "fql_query_response", "event", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				events.Add(FacebookEventParser.ParseEvent(node));
			}

			return events;
		}

		/// <summary>
		/// Get the XML representation of all events for the specified user and list of events between 2 dates
		/// </summary>
		/// <param name="userId">User to return events for</param>
		/// <param name="eventList">list of event ids</param>
		/// <param name="startDate">events occuring after this date</param>
		/// <param name="endDate">events occuring before this date</param>
		/// <returns>event list as raw XML</returns> 
		public string GetEventsXML(Collection<string> eventList, string userId, DateTime? startDate, DateTime? endDate)
		{
			if (eventList == null || eventList.Count == 0)
			{
				eventList = GetEventIds();
			}

			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}};

			string uidClause = string.Empty;
			string startClause = string.Empty;
			string endClause = string.Empty;

			// Build an FQL query for retrieving events based on search criteria
			// Use FQL instead of direct REST call to insulate from REST changes
			// Similar to events.get in REST api
			string eidClause = String.Concat(" eid IN (", StringHelper.ConvertToCommaSeparated(eventList), ")");

			if (!String.IsNullOrEmpty(userId))
			{
				uidClause = String.Concat(ANDCLAUSE, " eid IN (SELECT eid FROM event_member WHERE uid=", userId, ")");
			}

			if (startDate != null)
			{
				startClause =
					String.Concat(ANDCLAUSE, " end_time >=",
					              DateHelper.ConvertDateToDouble(startDate.Value).ToString(
					              	CultureInfo.InvariantCulture));
			}
			if (endDate != null)
			{
				endClause =
					String.Concat(ANDCLAUSE, " start_time <",
					              DateHelper.ConvertDateToDouble(endDate.Value).ToString(
					              	CultureInfo.InvariantCulture));
			}
			parameterList.Add("query",
			                  String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}",
			                                "SELECT eid, name, tagline, nid, pic, pic_big, pic_small, host, description, event_type, event_subtype, start_time, end_time, creator, update_time, location, venue FROM event WHERE",
			                                eidClause, uidClause, startClause, endClause));

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Get all event members for the specified event
		/// </summary>
		/// <param name="eventId">Event to return users for</param>
		/// <returns>evet user list with rsvp status</returns>
		public Collection<EventUser> GetEventMembers(string eventId)
		{
			var eventUsers = new Collection<EventUser>();
			var xml = GetEventMembersXML(eventId);

			if (!String.IsNullOrEmpty(xml))
			{
				XmlDocument xmlDocument = LoadXMLDocument(xml);

				XmlNodeList nodeList = xmlDocument.GetElementsByTagName("fql_query_response");

				if (nodeList.Count > 0)
				{
					if (nodeList[0].HasChildNodes)
					{
						var results = xmlDocument.GetElementsByTagName("event_member");
						var eventUserDict = new Dictionary<string, EventUser>();
						foreach (XmlNode node in results)
						{
							var eventUser = EventUserParser.ParseEventUser(node);
							eventUserDict.Add(eventUser.UserId, eventUser);
						}

						// Get the profile information for every user invited to this event
						var users = GetUserInfo(StringHelper.ConvertToCommaSeparated(eventUserDict.Keys));
						foreach (var user in users)
						{
							if (!eventUserDict.ContainsKey(user.UserId)) continue;

							eventUserDict[user.UserId].User = user;
							eventUsers.Add(eventUserDict[user.UserId]);
						}
					}
				}
			}
			return eventUsers;
		}

		/// <summary>
		/// Get all event members for the specified event
		/// </summary>
		/// <param name="eventId">Event to return users for</param>
		/// <returns>evet user list with rsvp status</returns>
		public string GetEventMembersXML(string eventId)
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}};

			// Build an FQL query for retrieving members for a particular event
			// Use FQL instead of direct REST call to insulate from REST changes
			// Similar to events.getMembers in REST api
			if (!string.IsNullOrEmpty(eventId))
			{
				parameterList.Add("query",
				                  String.Format(CultureInfo.InvariantCulture, "{0}{1}",
				                                "SELECT uid, eid, rsvp_status FROM event_member WHERE eid=", eventId));
			}
			else
			{
				throw new FacebookException("Event Id is required");
			}

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Get all the friends for the logged in user
		/// </summary>
		/// <returns>user profile of each friend</returns>
		public Collection<User> GetFriends()
		{
			return GetUserInfo(StringHelper.ConvertToCommaSeparated(GetFriendIds()));
		}

		/// <summary>
		/// Get all the friends for the logged in user
		/// </summary>
		/// <returns>user profile of each friend</returns>
		public Collection<string> GetFriendIds()
		{
			var friendList = new Collection<string>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetFriendsXML(), "fql_query_response", "friend_info", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				friendList.Add(XmlHelper.GetNodeText(node, "uid2"));
			}

			return friendList;
		}

		/// <summary>
		/// Get all the friends for the logged in user and returns the results as raw XML
		/// </summary>
		/// <returns>The XMl representation of the user profile of each friend</returns>
		public string GetFriendsXML()
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}};

			if (!string.IsNullOrEmpty(UserId))
			{
				parameterList.Add("query",
				                  String.Format(CultureInfo.InvariantCulture, "{0}{1}",
				                                "SELECT uid2 FROM friend WHERE uid1=", UserId));
			}
			else
			{
				throw new FacebookException("User Id is required");
			}

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Get all the friends for the logged in user that do not use the current application 
		/// </summary>
		/// <returns>user profile of each friend</returns>
		public Collection<User> GetFriendsNonAppUsers()
		{
			var friends = new Collection<User>();
			
			XmlNodeList nodeList;
			GetMultipleNodes(GetFriendsAppUsersXML(), "fql_query_response", "user", out nodeList);

			var friendList = GetFriendIds();
			foreach (XmlNode node in nodeList)
			{
				friendList.Remove(XmlHelper.GetNodeText(node, "uid"));
			}

			if (friendList.Count > 0)
			{
				friends = GetUserInfo(StringHelper.ConvertToCommaSeparated(friendList));
			}

			return friends;
		}

		/// <summary>
		/// Get all the friends for the logged in user that use the current application 
		/// </summary>
		/// <returns>user profile of each friend</returns>
		public Collection<User> GetFriendsAppUsers()
		{
			XmlNodeList nodeList;
			GetMultipleNodes(GetFriendsAppUsersXML(), "fql_query_response", "user", out nodeList);

			var friendList = new Collection<string>();
			foreach (XmlNode node in nodeList)
			{
				friendList.Add(XmlHelper.GetNodeText(node, "uid"));
			}

			return GetUserInfo(StringHelper.ConvertToCommaSeparated(friendList));
		}

		/// <summary>
		/// Get all the friends for the logged in user that use the current application 
		/// </summary>
		/// <returns>user profile of each friend</returns>
		public string GetFriendsAppUsersXML()
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}};

			// Build an FQL query for retrieving friends that are also users of this application
			// Use FQL instead of direct REST call to insulate from REST changes
			// Similar to friends.getAppUsers in REST api
			if (!string.IsNullOrEmpty(UserId))
			{
				parameterList.Add("query",
				                  String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
				                                "SELECT uid FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1=",
				                                UserId, ") AND is_app_user"));
			}
			else
			{
				throw new FacebookException("User Id is required");
			}

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Determine if the two specified users are friends
		/// </summary>
		/// <param name="user1">User to check</param>
		/// <param name="user2">User to check</param>
		/// <returns>whether specified users are friends or not</returns>
		public bool AreFriends(User user1, User user2)
		{
			if (user1 == null)
			{
				throw new ArgumentNullException("user1");
			}

			if (user2 == null)
			{
				throw new ArgumentNullException("user2");
			}

			return AreFriends(user1.UserId, user2.UserId);
		}

		/// <summary>
		/// Determine if the two specified users are friends
		/// </summary>
		/// <param name="userId1">User to check</param>
		/// <param name="userId2">User to check</param>
		/// <returns>whether specified users are friends or not</returns>
		/// <remarks>
		/// Build an FQL query for determining if 2 users are facebook friends
		/// Use FQL instead of direct REST call to insulate from REST changes
		/// Similar to friends.areFriends in REST api
		/// </remarks>
		public bool AreFriends(string userId1, string userId2)
		{
			var parameterList = new Dictionary<string, string> {{"method", "facebook.fql.query"}};

			if (!string.IsNullOrEmpty(userId1) && !string.IsNullOrEmpty(userId2))
			{
				parameterList.Add("query",
				                  String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}",
				                                "SELECT uid1, uid2 FROM friend WHERE uid1=", userId1, " AND uid2=",
				                                userId2));
			}
			else
				throw new FacebookException("userids are empty or null");

			XmlNode node;
			return GetSingleNode(ExecuteApiCallString(parameterList, true), "fql_query_response", out node) ? true : false;
		}

		/// <summary>
		/// returns if the application is added
		/// </summary>
		/// <returns>user profile list</returns>
		public bool IsAppAdded()
		{
			var parameterList = new Dictionary<string, string> {{"method", "users.isAppAdded"}};

			XmlNode node;
			if (GetSingleNode(ExecuteApiCallString(parameterList, true), "users_isAppAdded_response", out node))
			{
				return node.InnerText == "1";
			}

			throw new FacebookException("Unable to determine if the app is added.");
		}


		/// <summary>
		/// Build the user profile for the logged in user
		/// </summary>
		/// <returns>user profile</returns>
		public User GetUserInfo()
		{
			var users = GetUserInfo(UserId);
			return users.Count > 0 ? users[0] : null;
		}

		/// <summary>
		/// Build the user profile for the list of users
		/// </summary>
		/// <param name="userIds">Comma separated list of user ids</param>
		/// <returns>user profile list</returns>
		public Collection<User> GetUserInfo(string userIds)
		{
			var users = new Collection<User>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetUserInfoXml(userIds), "fql_query_response", "user", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				users.Add(UserParser.ParseUser(node));
			}

			return users;
		}


		/// <summary>
		/// Build the user profile for the list of users
		/// </summary>
		/// <param name="userIds">A collection of userId strings</param>
		/// <returns>user profile list</returns>
		public Collection<User> GetUserInfo(Collection<string> userIds)
		{
			var paramBuilder = new StringBuilder();

			for (var i = 0; i < userIds.Count; i++)
			{
				if (i != 0) paramBuilder.Append(',');
				paramBuilder.Append(userIds[i]);
			}

			return GetUserInfo(paramBuilder.ToString());
		}


		/// <summary>
		/// Builds the user profile for the list of users and returns the results as raw xml
		/// </summary>
		/// <param name="userIds">Comma separated list of user ids</param>
		/// <returns>The xml representation of the user profile list</returns>
		public string GetUserInfoXml(string userIds)
		{
			var parameterList = new Dictionary<string, string>(3)
			                    	{
			                    		{"method", "facebook.fql.query"},
			                    		{"query", String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
			                    			"SELECT uid, first_name, last_name, name, pic_small, pic_big, pic_square, pic, affiliations, profile_update_time, timezone, religion, birthday, sex, hometown_location, meeting_sex, meeting_for, relationship_status, significant_other_id, political, current_location, activities, interests, is_app_user, music, tv, movies, books, quotes, about_me, hs_info, education_history, work_history, notes_count, wall_count, status FROM user WHERE uid IN (", userIds, ")")
			                    			}
			                    	};

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Update your status on Facebook, it will not include "IS" prepended.
		/// Requires Facebook extended permission of "status_update"
		/// </summary>
		/// <param name="statusMessage">Your status message to be posted</param>
		public string SetStatus(string statusMessage)
		{
			return SetStatus(statusMessage, false);
		}

		/// <summary>
		/// Update your status on Facebook
		/// Requires Facebook extended permission of "status_update"
		/// </summary>
		/// <param name="statusMessage">Your status message to be posted</param>
		/// <param name="IncludeVerb">Have your message start with the word "is"</param>
		public string SetStatus(string statusMessage, bool IncludeVerb)
		{
			var parameterList = new Dictionary<string, string>(5)
			                    	{
			                    		{"method", "Users.setStatus"},
			                    		{"status", statusMessage},
			                    		{"status_includes_verb", IncludeVerb.ToString()}
			                    	};

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "users_setStatus_response");
		}

		/// <summary>
		/// Do you have permission to do the task needed?
		/// </summary>
		/// <param name="permission">Your status message to be posted</param>
		public bool HasPermission(Enums.Extended_Permissions permission)
		{
			var parameterList = new Dictionary<string, string>(5)
			                    	{
			                    		{"method", "Users.hasAppPermission"},
			                    		{"ext_perm", permission.ToString()}
			                    	};

			XmlNode node;
			return GetSingleNode(ExecuteApiCallString(parameterList, true), "Users_hasAppPermission_response", out node) && Convert.ToBoolean(node.InnerText);
		}



		/// <summary>
		/// Return the notifications
		/// </summary>
		/// <returns>user profile list</returns>
		public Notifications GetNotifications()
		{
			XmlNode node;
			return GetSingleNode(GetNotificationsXml(), "notifications_get_response", out node) ? NotificationsParser.ParseNotifications(node) : new Notifications();
		}

		/// <summary>
		/// Builds the list of notifications and returns the results as raw xml
		/// </summary>
		/// <returns>The xml representation of the user profile list</returns>
		public string GetNotificationsXml()
		{
			var parameterList = new Dictionary<string, string>(2) {{"method", "facebook.notifications.get"}};

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Get the photos for a specified album
		/// </summary>
		/// <param name="albumId">The album</param>
		/// <returns>photos</returns>
		public Collection<Photo> GetPhotos(string albumId)
		{
			return GetPhotos(albumId, null, null);
		}

		/// <summary>
		/// Get the photos for a specified list of photos
		/// </summary>
		/// <param name="photoList">Collection of photo ids</param>
		/// <returns>photos</returns>
		public Collection<Photo> GetPhotos(Collection<string> photoList)
		{
			return GetPhotos(null, photoList, null);
		}

		/// <summary>
		/// Get the photos for a specified User
		/// </summary>
		/// <param name="user">The user</param>
		/// <returns>photos</returns>
		public Collection<Photo> GetPhotos(User user)
		{
			return GetPhotos(null, null, user);
		}

		/// <summary>
		/// Get the photos for a specified album and list of photos
		/// </summary>
		/// <param name="albumId">The album</param>
		/// <param name="photoList">Collection of photo ids</param>
		/// <returns>photos</returns>
		public Collection<Photo> GetPhotos(string albumId, Collection<string> photoList)
		{
			return GetPhotos(albumId, photoList, null);
		}

		/// <summary>
		/// Get the photos for a specified album, photo list, and User
		/// </summary>
		/// <param name="albumId">The album</param>
		/// <param name="photoList">Collection of photo ids</param>
		/// <param name="user">The user</param>
		/// <returns>photos</returns>
		public Collection<Photo> GetPhotos(string albumId, Collection<string> photoList, User user)
		{
			var photos = new Collection<Photo>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetPhotosXML(albumId, photoList, user), "fql_query_response", "photo", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				photos.Add(PhotoParser.ParsePhoto(node));
			}

			return photos;
		}

		/// <summary>
		/// Get the photos for a specified album, list of photos, and User
		/// </summary>
		/// <param name="albumId">The album</param>
		/// <param name="photoList">Collection of photo ids</param>
		/// <param name="user">The user</param>
		/// <returns>photos</returns>
		/// <remarks>
		/// Build an FQL query for determining retrieving the photos for an album or list of photos
		/// Use FQL instead of direct REST call to insulate from REST changes
		/// Similar to photos.get in REST api
		/// </remarks>
		public string GetPhotosXML(string albumId, Collection<string> photoList, User user)
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}};
			string aidClause = string.Empty;
			string pidClause = string.Empty;
			string uidClause = string.Empty;

			if (!String.IsNullOrEmpty(albumId))
			{
				aidClause = String.Concat(" aid =", albumId);
			}
			if (photoList != null)
			{
				if (!String.IsNullOrEmpty(albumId))
				{
					pidClause = ANDCLAUSE;
				}
				pidClause =
					String.Concat(pidClause, " pid IN (", StringHelper.ConvertToCommaSeparated(photoList), ")");
			}
			if ((user != null) && (!String.IsNullOrEmpty(user.UserId)))
			{
				if ((!String.IsNullOrEmpty(albumId)) || (photoList != null))
				{
					uidClause = ANDCLAUSE;
				}
				uidClause =
					String.Concat(uidClause, " pid IN (SELECT pid FROM photo_tag WHERE subject=", user.UserId, ")");
			}
			parameterList.Add("query",
			                  String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}",
			                                "SELECT pid, aid, owner, src, src_big, src_small, link, caption, created FROM photo WHERE",
			                                aidClause, pidClause, uidClause));

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Get the albums for the logged in user
		/// </summary>
		/// <returns>albums</returns>
		public Collection<Album> GetPhotoAlbums()
		{
			return GetPhotoAlbums(UserId, null);
		}

		/// <summary>
		/// Get the albums for the specified user
		/// </summary>
		/// <param name="userId">user to return albums for</param>
		/// <returns>albums</returns>
		public Collection<Album> GetPhotoAlbums(string userId)
		{
			return GetPhotoAlbums(userId, null);
		}

		/// <summary>
		/// Get the albums for the specified list of albums
		/// </summary>
		/// <param name="albumList">collection of album ids</param>
		/// <returns>albums</returns>
		public Collection<Album> GetPhotoAlbums(Collection<string> albumList)
		{
			return GetPhotoAlbums(null, albumList);
		}

		/// <summary>
		/// Get the albums for the specified user and list of albums
		/// </summary>
		/// <param name="userId">user to return albums for</param>
		/// <param name="albumList">collection of album ids</param>
		/// <returns>albums</returns>
		public Collection<Album> GetPhotoAlbums(string userId, Collection<string> albumList)
		{
			var albums = new Collection<Album>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetPhotoAlbumsXML(userId, albumList), "fql_query_response", "album", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				albums.Add(AlbumParser.ParseAlbum(node));
			}

			return albums;
		}

		private bool GetSingleNode(string xml, string rootTag, out XmlNode node)
		{
			node = null;

			if (!String.IsNullOrEmpty(xml))
			{
				var xmlDocument = LoadXMLDocument(xml);
				var nodeList = xmlDocument.GetElementsByTagName(rootTag);
				if (nodeList.Count > 0)
				{
					if (nodeList[0].HasChildNodes)
					{
						node = nodeList[0];
						return true;
					}
				}
			}

			return false;
		}

		private string GetSingleNode(string xml, string rootTag)
		{
			XmlNode node;
			return GetSingleNode(xml, rootTag, out node) ? node.InnerText : string.Empty;
		}

		private bool GetMultipleNodes(string xml, string rootTag, string listTag, out XmlNodeList nodeList)
		{
			//TODO: if bool is never used, change to XmlNodeList
			nodeList = null;

			if (!String.IsNullOrEmpty(xml))
			{
				var xmlDocument = LoadXMLDocument(xml);
				var rootNodeList = xmlDocument.GetElementsByTagName(rootTag); //TODO: Always "fql_query_response" ????
				if (rootNodeList.Count > 0)
				{
					if (rootNodeList[0].HasChildNodes)
					{
						nodeList = xmlDocument.GetElementsByTagName(listTag);
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Get the albums for the specified user and list of albums
		/// </summary>
		/// <param name="userId">user to return albums for</param>
		/// <param name="albumList">collection of album ids</param>
		/// <returns>albums</returns>
		public string GetPhotoAlbumsXML(string userId, Collection<string> albumList)
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}};

			string uidClause = string.Empty;
			string aidClause = string.Empty;

			if (!String.IsNullOrEmpty(userId))
			{
				uidClause = String.Concat(" owner =", userId);
			}
			if (albumList != null)
			{
				if (!String.IsNullOrEmpty(userId))
				{
					aidClause = ANDCLAUSE;
				}
				aidClause =
					String.Concat(aidClause, " aid IN (", StringHelper.ConvertToCommaSeparated(albumList), ")");
			}
			parameterList.Add("query",
			                  String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
			                                "SELECT aid, cover_pid, owner, name, created, modified, description, location, size FROM album WHERE",
			                                uidClause, aidClause));

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Get the tags for the specifed photos
		/// </summary>
		/// <param name="photoList">collection of photo ids</param>
		/// <returns>photo tags</returns>
		public Collection<PhotoTag> GetTags(Collection<string> photoList)
		{
			var photoTags = new Collection<PhotoTag>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetTagsXML(photoList), "fql_query_response", "photo_tag", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				photoTags.Add(PhotoTagParser.ParsePhotoTag(node));
			}

			return photoTags;
		}

		/// <summary>
		/// Get the tags for the specifed photos
		/// </summary>
		/// <param name="photoList">collection of photo ids</param>
		/// <returns>photo tags</returns>
		public string GetTagsXML(Collection<string> photoList)
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.fql.query"}};

			if (photoList != null && photoList.Count > 0)
			{
				parameterList.Add("query",
				                  String.Format(CultureInfo.InvariantCulture, "{0}{1}",
				                                "SELECT pid, subject, xcoord, ycoord FROM photo_tag WHERE pid IN",
				                                StringHelper.ConvertToCommaSeparated(photoList)));
			}
			else
			{
				throw new FacebookException("photo list not specified");
			}

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Gets the FBML on the logged-in user's profile.
		/// </summary>
		/// <returns>The FBML from the user's profile.</returns>
		public string GetFBML()
		{
			return GetFBML(null);
		}

		/// <summary>
		/// Gets the FBML on a user's profile
		/// </summary>
		/// <param name="userId">The id of the user to get the FBML from.</param>
		/// <returns>The FBML markup from the user's profile.</returns>
		public string GetFBML(string userId)
		{
			var parameterList = new Dictionary<string, string>(1) {{"method", "facebook.profile.getFBML"}};

			if (!string.IsNullOrEmpty(userId))
			{
				parameterList.Add("uid", userId);
			}

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "profile_getFBML_response");
		}

		/// <summary>
		/// Set the FBML on a profile
		/// </summary>
		/// <param name="profileFBML">Profile FBML markup</param>
		/// <param name="profileActionFBML">Profile Action FBML markup</param>
		/// <param name="mobileFBML">Mobile Profile FBML markup</param>
		/// <param name="userId">user id</param>
		public string SetFBML(string profileFBML, string profileActionFBML, string mobileFBML, string userId)
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "facebook.profile.setFBML"}};
			AddParameterCultureFBML(parameterList, "profile", profileFBML);
			AddParameterCultureFBML(parameterList, "profile_action", profileActionFBML);
			AddParameterCultureFBML(parameterList, "mobile_fbml", mobileFBML);
			AddParameterCulture(parameterList, "uid", userId);

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "profile_setFBML_response");
		}

		private void AddParameterCultureFBML(IDictionary<string, string> dict, string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				dict.Add(key, string.Format(InstalledCulture, value.Replace("{", "{{").Replace("}", "}}")));
			}
		}

		private void AddParameterCulture(IDictionary<string, string> dict, string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				dict.Add(key, string.Format(InstalledCulture, value));
			}
		}

		private static void AddParameter(IDictionary<string, string> dict, string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				dict.Add(key, value);
			}
		}

		private static void AddParameterWithException(IDictionary<string, string> dict, string key, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				dict.Add(key, value);
			}
			else
			{
				throw new FacebookException(string.Format("Error: Parameter '{0}' is required.", key));
			}
		}

		private void AddParameterJSON(IDictionary<string, string> dict, string key, Dictionary<string, string> value)
		{
			if (value != null && value.Count > 0)
			{
				dict.Add(key, ToJsonAssociativeArray(value));
			}
		}

		/// <summary>
		/// Associates a specified handle with a piece of FBML markup.
		/// </summary>
		/// <param name="handle">The handle to use as a reference.</param>
		/// <param name="markup">The FBML markup to be referenced.</param>
		/// <returns>The result of the method call as a string.</returns>
		public string SetRefHandle(string handle, string markup)
		{
			var parameterList = new Dictionary<string, string>(3)
			                    	{
			                    		{"method", "facebook.fbml.setRefHandle"},
			                    		{"handle", string.Format(InstalledCulture, handle)},
			                    		{"fbml", string.Format(InstalledCulture, markup)}
			                    	};

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "fbml_setRefHandle_response");
		}

		/// <summary>
		/// Tells Facebook to fetches and re-cache the content stored at the
		/// given URL for use in a fb:ref FBML tag.
		/// </summary>
		/// <param name="url">The URL of the content to refresh.</param>
		/// <returns>The result of the method call as a string.</returns>
		public string RefreshRefUrl(string url)
		{
			var parameterList = new Dictionary<string, string>(2)
			                    	{
			                    		{"method", "facebook.fbml.refreshRefUrl"},
			                    		{"url", string.Format(InstalledCulture, url)}
			                    	};

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "fbml_refreshRefUrl_response");
		}

		/// <summary>
		/// Tells Facebook to fetche and re-cache the image stored at the given
		/// URL for use in non-canvas pages on Facebook.
		/// </summary>
		/// <param name="url">The URL of the image to refresh.</param>
		/// <returns>The result of the method call as a string.</returns>
		public string RefreshImgSrc(string url)
		{
			var parameterList = new Dictionary<string, string>(2)
			                    	{
			                    		{"method", "facebook.fbml.refreshImgSrc"},
			                    		{"url", string.Format(InstalledCulture, url)}
			                    	};

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "fbml_refreshImgSrc_response");
		}

		/// <summary>
		/// Send a notification
		/// </summary>
		/// <param name="markup">fbml markup</param>
		/// <param name="toList">list of users to be notified</param>
		public string SendNotification(string markup, string toList)
		{
			return SendNotification(markup, toList, null);
		}

		/// <summary>
		/// Send a notification
		/// </summary>
		/// <param name="markup">fbml markup</param>
		/// <param name="toList">list of users to be notified</param>
		/// <param name="email">fbml of email</param>
		public string SendNotification(string markup, string toList, string email)
		{
			var parameterList = new Dictionary<string, string>(5)
			                    	{
			                    		{"method", "facebook.notifications.send"},
			                    		{"notification", string.Format(InstalledCulture, markup)},
			                    		{"to_ids", toList}
			                    	};
			AddParameterCulture(parameterList, "email", email);

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "notifications_send_response");
		}

		/// <summary>
		/// Publishes a story to the logged-in user's feed.
		/// </summary>
		/// <param name="title">The title of the story.</param>
		/// <param name="body">The body of the story.</param>
		/// <param name="images">A list of images, with associated links, to be
		/// used in the story.</param>
		/// <remarks>The priority is set to 1 by default.</remarks>
		/// <returns>The result of the method call as a string.</returns>
		public string PublishStory(string title, string body, Collection<PublishImage> images)
		{
			return PublishStory(title, body, images, 1);
		}

		/// <summary>
		/// Publishes a story to the logged-in user's feed.
		/// </summary>
		/// <param name="title">The title of the story.</param>
		/// <param name="body">The body of the story.</param>
		/// <param name="images">A list of images, with associated links, to be
		/// used in the story.</param>
		/// <param name="priority">The priority of the story.</param>
		/// <returns>The result of the method call as a string.</returns>
		public string PublishStory(string title, string body, Collection<PublishImage> images, int priority)
		{
			var parameterList = new Dictionary<string, string> {{"method", "facebook.feed.publishStoryToUser"}};
			AddParameterWithException(parameterList, "title", title);
			AddParameter(parameterList, "body", body);
			parameterList.Add("priority", priority.ToString());
			
			if (images != null)
			{
				if (images.Count > 5)
				{
					throw new FacebookException("Maximum 5 images allowed");
				}
				for (var i = 0; i < images.Count; i++)
				{
					if (string.IsNullOrEmpty(images[i].ImageLocation))
					{
						throw new FacebookException("Image location missing of image " + (i + 1));
					}
					parameterList.Add("image_" + (i + 1), images[i].ImageLocation);
					AddParameter(parameterList, "image_" + (i + 1) + "_link", images[i].ImageLink);

				}
			}

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "feed_publishStoryToUser_response");
		}

		/// <summary>
		/// Publishes an action to the logged-in user's mini-feed, and to the
		/// news feeds of the user's friends who have added the application.
		/// </summary>
		/// <param name="title">The title of the action.</param>
		/// <param name="body">The body of the action.</param>
		/// <param name="images">A list of images, with associated links, to be
		/// used in the action.</param>
		/// <remarks>The priority is set to 1 by default.</remarks>
		/// <returns>The result of the method call as a string.</returns>
		public string PublishAction(string title, string body, Collection<PublishImage> images)
		{
			return PublishAction(title, body, images, 1);
		}

		/// <summary>
		/// Publishes an action to the logged-in user's mini-feed, and to the
		/// news feeds of the user's friends who have added the application.
		/// </summary>
		/// <param name="title">The title of the action.</param>
		/// <param name="body">The body of the action.</param>
		/// <param name="images">A list of images, with associated links, to be
		/// used in the action.</param>
		/// <param name="priority">The priority of the action.</param>
		/// <returns>The result of the method call as a string.</returns>
		public string PublishAction(string title, string body, Collection<PublishImage> images, int priority)
		{
			var parameterList = new Dictionary<string, string> {{"method", "facebook.feed.publishActionOfUser"}};
			AddParameterWithException(parameterList, "title", title);
			AddParameter(parameterList, "body", body);
			parameterList.Add("priority", priority.ToString());

			if (images != null)
			{
				if (images.Count > 5)
				{
					throw new FacebookException("Maximum 5 images allowed");
				}
				for (int i = 0; i < images.Count; i++)
				{
					if (string.IsNullOrEmpty(images[i].ImageLocation) || string.IsNullOrEmpty(images[i].ImageLink))
					{
						throw new FacebookException("Image location and link are required.  Missing for image " + (i + 1));
					}
					parameterList.Add("image_" + (i + 1), images[i].ImageLocation);
					parameterList.Add("image_" + (i + 1) + "_link", images[i].ImageLink);
				}
			}

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "feed_publishActionOfUser_response");
		}

		/// <summary>
		/// Uploads the specified photo to the specified album.
		/// </summary>
		/// <param name="albumId">The album to upload to. If not specified, will use default album.</param>
		/// <param name="uploadFile">The .jpg file to upload.</param>
		/// <returns>A result containing the ids for the photo and album.</returns>
		public UploadPhotoResult UploadPhoto(string albumId, FileInfo uploadFile)
		{
			return UploadPhoto(albumId, uploadFile, null);
		}

		/// <summary>
		/// Uploads the specified photo to the specified album.
		/// </summary>
		/// <param name="albumId">The album to upload to. If not specified, will use default album.</param>
		/// <param name="uploadFile">The .jpg file to upload.</param>
		/// <param name="caption">The caption to attach to the photo.</param>
		/// <returns>A result containing the ids for the photo and album.</returns>
		public UploadPhotoResult UploadPhoto(string albumId, FileInfo uploadFile, string caption)
		{
			var parameterList = new Dictionary<string, string>(4) {{"method", "photos.upload"}};
			AddParameter(parameterList, "aid", albumId);
			AddParameter(parameterList, "caption", caption);
			parameterList.Add("session_key", SessionKey);

			XmlNode node;
			return GetSingleNode(ExecuteApiUpload(uploadFile, parameterList), "photos_upload_response", out node) ? new UploadPhotoResult(XmlHelper.GetNodeText(node, "pid"), XmlHelper.GetNodeText(node, "aid")) : null;
		}

		/// <summary>
		/// Add tag to a photo.  Only allowed on photos in pending state.
		/// </summary>
		/// <param name="photoId">The id of the photo to tag</param>
		/// <param name="tagText">The text of the tag.  Need to specify either this of tagUserId</param>
		/// <param name="tagUserId">The facebook id the person that was tagged</param>
		/// <param name="xCoord">The x position of the tag on the photo</param>
		/// <param name="yCoord">The y position of the tag on the photo</param>
		public void AddTag(string photoId, string tagText, string tagUserId, int xCoord, int yCoord)
		{
			var parameterList = new Dictionary<string, string>(7) {{"method", "photos.addTag"}, {"pid", photoId}};
			if (!string.IsNullOrEmpty(tagText))
			{
				parameterList.Add("tag_text", tagText);
			}
			else if (!string.IsNullOrEmpty(tagUserId))
			{
				parameterList.Add("tag_uid", tagUserId);
			}
			else
			{
				throw new FacebookException("either text or user id must be specified");
			}
			parameterList.Add("x", xCoord.ToString(CultureInfo.InvariantCulture));
			parameterList.Add("y", yCoord.ToString(CultureInfo.InvariantCulture));
			ExecuteApiCall(parameterList, true);
		}

		/// <summary>
		/// Create Album.
		/// </summary>
		/// <param name="name">The name of the album</param>
		/// <param name="location">The location of the album.  (Optional)</param>
		/// <param name="description">The description of the album.  (Optional)</param>
		public Album CreateAlbum(string name, string location, string description)
		{
			var parameterList = new Dictionary<string, string>(5) {{"method", "photos.createAlbum"}};
			AddParameter(parameterList, "location", location);
			AddParameter(parameterList, "description", description);
			AddParameterWithException(parameterList, "name", name);

			XmlNode node;
			if (GetSingleNode(ExecuteApiCallString(parameterList, true), "photos_createAlbum_response", out node))
			{
				return AlbumParser.ParseAlbum(node);
			}
			throw new FacebookException("Album creation failed");
		}

		/// <summary>
		/// Get the groups that the logged in user belongs to
		/// </summary>
		/// <returns>groups</returns>
		public Collection<Group> GetGroups()
		{
			return GetGroups(UserId, null);
		}

		/// <summary>
		/// Get the groups that the specified user belongs to
		/// </summary>
		/// <param name="userId">The id of the user to return groups for</param>
		/// <returns>groups</returns>
		public Collection<Group> GetGroups(string userId)
		{
			return GetGroups(userId, null);
		}

		/// <summary>
		/// Get the groups for the group list
		/// </summary>
		/// <param name="groupsList">Collection of group ids</param>
		/// <returns>groups</returns>
		public Collection<Group> GetGroups(Collection<string> groupsList)
		{
			return GetGroups(null, groupsList);
		}

		/// <summary>
		/// Get the groups for the specified user and group list
		/// </summary>
		/// <param name="userId">The id of the user to return groups for</param>
		/// <param name="groupsList">Collection of group ids</param>
		/// <returns>groups</returns>
		public Collection<Group> GetGroups(string userId, Collection<string> groupsList)
		{
			var groups = new Collection<Group>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetGroupsXML(userId, groupsList), "fql_query_response", "group", out nodeList);

			foreach (XmlNode node in nodeList)
			{
				groups.Add(GroupParser.ParseGroup(node));
			}

			return groups;
		}

		/// <summary>
		/// Get the groups for the specified user and group list
		/// </summary>
		/// <param name="userId">The id of the user to return groups for</param>
		/// <param name="groupsList">Collection of group ids</param>
		/// <returns>groups</returns>
		public string GetGroupsXML(string userId, Collection<string> groupsList)
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "fql.query"}};

			string uidClause = string.Empty;
			string gidClause = string.Empty;
			if (!String.IsNullOrEmpty(userId))
			{
				uidClause = String.Concat(" gid IN (SELECT gid FROM group_member WHERE uid=", userId, ")");
			}
			if (groupsList != null)
			{
				if (!String.IsNullOrEmpty(userId))
				{
					gidClause = ANDCLAUSE;
				}
				gidClause =
					String.Concat(gidClause, " gid IN (", StringHelper.ConvertToCommaSeparated(groupsList), ")");
			}
			parameterList.Add("query",
			                  String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
			                                "SELECT gid, name, nid, description, group_type, group_subtype, recent_news, pic, pic_big, pic_small, creator, update_time, office, website, venue FROM group WHERE",
			                                uidClause, gidClause));

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Get the members of the specified group
		/// </summary>
		/// <param name="groupId">The id of the group to return members for</param>
		/// <returns>Group members (user profiles, and group roles)</returns>
		public Collection<GroupUser> GetGroupMembers(string groupId)
		{
			var groupUsers = new Collection<GroupUser>();

			XmlNodeList nodeList;
			GetMultipleNodes(GetGroupMembersXML(groupId), "fql_query_response", "group_member", out nodeList);

			var groupUserDict = new Dictionary<string, GroupUser>();
			foreach (XmlNode node in nodeList)
			{
				var groupUser = GroupUserParser.ParseGroupUser(node);
				groupUserDict.Add(groupUser.UserId, groupUser);
			}
			var users = GetUserInfo(StringHelper.ConvertToCommaSeparated(groupUserDict.Keys));
			foreach (var user in users)
			{
				if (!groupUserDict.ContainsKey(user.UserId)) continue;
				groupUserDict[user.UserId].User = user;
				groupUsers.Add(groupUserDict[user.UserId]);
			}

			return groupUsers;
		}

		/// <summary>
		/// Get the members of the specified group
		/// </summary>
		/// <param name="groupId">The id of the group to return members for</param>
		/// <returns>Group members (user profiles, and group roles)</returns>
		public string GetGroupMembersXML(string groupId)
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "fql.query"}};

			if (!string.IsNullOrEmpty(groupId))
			{
				parameterList.Add("query",
				                  String.Format(CultureInfo.InvariantCulture, "{0}{1}",
				                                "SELECT uid, gid, positions FROM group_member WHERE gid=", groupId));
			}
			else
			{
				throw new FacebookException("Group Id is required");
			}

			return ExecuteApiCallString(parameterList, true);
		}

		/// <summary>
		/// Get the facebook user id of the user associated with the current session
		/// </summary>
		/// <returns>facebook userid</returns>
		public string GetLoggedInUser()
		{
			var parameterList = new Dictionary<string, string> {{"method", "users.getLoggedInUser"}};

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "users_getLoggedInUser_response");
		}

		/// <summary>
		/// Publishes a templatized action to the logged-in user's mini-feed,
		/// and to the news feeds of the user's friends who have added the application.
		/// </summary>
		/// <remarks>More documentation on the template parameters can be found
		/// on the Facebook Developers web site.
		/// </remarks>
		/// <param name="titleTemplate">Templated markup for the title of the action.</param>
		/// <param name="titleData">A dictionary of token values.</param>
		/// <param name="bodyTemplate">Templated markup for the body content.</param>
		/// <param name="bodyData">A dictionary of token values for the body.</param>
		/// <param name="images">A collection of images, and their associated
		/// links, to be shown in the action.</param>
		/// <returns>The string result of the call.</returns>
		public string PublishTemplatizedAction(string titleTemplate, Dictionary<string, string> titleData,
		                                       string bodyTemplate, Dictionary<string, string> bodyData,
		                                       Collection<PublishImage> images)
		{
			return PublishTemplatizedAction(titleTemplate, titleData, bodyTemplate, bodyData, null, null, images);
		}

		/// <summary>
		/// Publishes a templatized action to the logged-in user's mini-feed,
		/// and to the news feeds of the user's friends who have added the application.
		/// </summary>
		/// <remarks>More documentation on the template parameters can be found
		/// on the Facebook Developers web site.
		/// </remarks>
		/// <param name="titleTemplate">Templated markup for the title of the action.</param>
		/// <param name="titleData">A dictionary of token values.</param>
		/// <param name="bodyTemplate">Templated markup for the body content.</param>
		/// <param name="bodyData">A dictionary of token values for the body.</param>
		/// <param name="bodyGeneral">General content (markup) to be added to the action</param>
		/// <param name="targetIds">The ids of other users to "target" with this action</param>        
		/// <param name="images">A collection of images, and their associated
		/// links, to be shown in the action.</param>
		/// <returns>The string result of the call.</returns>
		public string PublishTemplatizedAction(string titleTemplate, Dictionary<string, string> titleData,
		                                       string bodyTemplate, Dictionary<string, string> bodyData, string bodyGeneral,
		                                       Collection<string> targetIds,
		                                       Collection<PublishImage> images)
		{
			var parameterList = new Dictionary<string, string>
			                    	{
			                    		{"method", "feed.publishTemplatizedAction"},
			                    		{"actor_id", UserId},
			                    		{"title_template", titleTemplate}
			                    	};

			AddParameterJSON(parameterList, "title_data", titleData);
			AddParameterJSON(parameterList, "body_data", bodyData);
			AddParameter(parameterList, "body_template", bodyTemplate);
			AddParameter(parameterList, "body_general", bodyGeneral);

			if (targetIds != null && targetIds.Count > 0)
			{
				parameterList.Add("target_ids", string.Join(",", new List<string>(targetIds).ToArray()));
			}

			if (images != null)
			{
				for (int i = 1; i <= images.Count; i++)
				{
					parameterList.Add("image_" + i, images[i - 1].ImageLocation);
					parameterList.Add("image_" + i + "_link", images[i - 1].ImageLink);
				}
			}

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "feed_publishTemplatizedAction_response");
		}

		/// <summary>
		/// Forgets all connection information so that this object may be used for another connection.
		/// </summary>
		public void LogOff()
		{
			AuthToken = null;
			SessionKey = null;
			UserId = null;
		}

		/// <summary>
		/// Send an email
		/// </summary>
		/// <param name="recipients">A comma-separated list of recipient IDs. The recipients must be people who have already added your application. You can email up to 100 people at a time.</param>
		/// <param name="subject">The subject of the email message.</param>
		/// <param name="text">The plain text version of the email content. You must include at least one of either the fbml or text parameters. </param>
		/// <param name="fbml">The FBML version of the email. You must include at least one of either the fbml or text parameters. The fbml parameter is a stripped-down set of FBML that allows only tags that result in text, links and linebreaks.</param>
		public string SendEmail(string recipients, string subject, string text, string fbml)
		{
			var parameterList = new Dictionary<string, string>(5)
			                    	{
			                    		{"method", "facebook.notifications.sendEmail"},
			                    		{"recipients", recipients},
			                    		{"subject", subject},
			                    		{"text", text}
			                    	};

			AddParameterCultureFBML(parameterList, "fbml", fbml);

			return GetSingleNode(ExecuteApiCallString(parameterList, true), "notifications_sendEmail_response");
		}

		#endregion

		#region Private functions

		/// <summary>
		/// Method creates a session
		/// </summary>
		internal void CreateSession()
		{
			var parameterList = new Dictionary<string, string>(3) {{"method", "auth.getSession"}, {"auth_token", AuthToken}};
			
			XmlDocument xmlDocument = ExecuteApiCall(parameterList, false);
			if (xmlDocument.DocumentElement == null) throw new FacebookException("Unable to create session");

			SessionKey = xmlDocument.DocumentElement.SelectSingleNode("Facebook:session_key", NsManager).InnerText;
			UserId = xmlDocument.DocumentElement.SelectSingleNode("Facebook:uid", NsManager).InnerText;

			XmlNode secretNode = xmlDocument.DocumentElement.SelectSingleNode("Facebook:secret", NsManager);
			if (IsDesktopApplication && (secretNode != null && !string.IsNullOrEmpty(secretNode.InnerText)))
			{
				Secret = secretNode.InnerText;
			}

			var expires = double.Parse(xmlDocument.DocumentElement.SelectSingleNode("Facebook:expires", NsManager).InnerText, CultureInfo.InvariantCulture);

			SessionExpires = expires > 0;
		}

		/// <summary>
		/// Get Query Response
		/// </summary>
		/// <param name="requestUrl">Request Url</param>
		/// <param name="postString">posted query</param>
		/// <returns>Response data</returns>
		internal static string GetQueryResponse(string requestUrl, string postString)
		{
			// Create a web request for input path.
			WebRequest webRequest = WebRequest.Create(requestUrl);
			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";

			if (!String.IsNullOrEmpty(postString))
			{
				byte[] parameterString = Encoding.ASCII.GetBytes(postString);
				webRequest.ContentLength = parameterString.Length;

				using (Stream buffer = webRequest.GetRequestStream())
				{
					buffer.Write(parameterString, 0, parameterString.Length);
					buffer.Close();
				}
			}

			WebResponse webResponse = webRequest.GetResponse();

			string responseData;
			using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
			{
				responseData = streamReader.ReadToEnd();
			}
			return responseData;
		}

		/// <summary>
		/// Get File Query Response
		/// </summary>
		/// <param name="parameterDictionary">parameter list</param>
		/// <param name="uploadFile">uploaded file info</param>
		/// <returns>Response data</returns>
		internal static string GetFileQueryResponse(IEnumerable<KeyValuePair<string, string>> parameterDictionary,
		                                            FileSystemInfo uploadFile)
		{
			string responseData;

			string boundary = DateTime.Now.Ticks.ToString("x", CultureInfo.InvariantCulture);
			string sRequestUrl = Resources.FacebookRESTUrl;

			// Build up the post message header
			var sb = new StringBuilder();
			foreach (var kvp in parameterDictionary)
			{
				sb.Append(PREFIX).Append(boundary).Append(NEWLINE);
				sb.Append("Content-Disposition: form-data; name=\"").Append(kvp.Key).Append("\"");
				sb.Append(NEWLINE);
				sb.Append(NEWLINE);
				sb.Append(kvp.Value);
				sb.Append(NEWLINE);
			}
			sb.Append(PREFIX).Append(boundary).Append(NEWLINE);
			sb.Append("Content-Disposition: form-data; filename=\"").Append(uploadFile.Name).Append("\"").Append(NEWLINE);
			sb.Append("Content-Type: image/jpeg").Append(NEWLINE).Append(NEWLINE);

			byte[] postHeaderBytes = Encoding.ASCII.GetBytes(sb.ToString());
#if NETCF
            byte[] fileData = Facebook.Compact.FileHelper.ReadAllBytes(uploadFile.FullName);
#else
			byte[] fileData = File.ReadAllBytes(uploadFile.FullName);
#endif
			byte[] boundaryBytes = Encoding.ASCII.GetBytes(String.Concat(NEWLINE, PREFIX, boundary, PREFIX, NEWLINE));

			var webrequest = (HttpWebRequest) WebRequest.Create(sRequestUrl);
			webrequest.ContentLength = postHeaderBytes.Length + fileData.Length + boundaryBytes.Length;
			webrequest.ContentType = String.Concat("multipart/form-data; boundary=", boundary);
			webrequest.Method = "POST";

			using (Stream requestStream = webrequest.GetRequestStream())
			{
				requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
				requestStream.Write(fileData, 0, fileData.Length);
				requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
			}
			var response = (HttpWebResponse) webrequest.GetResponse();
			using (var streamReader = new StreamReader(response.GetResponseStream()))
			{
				responseData = streamReader.ReadToEnd();
			}

			return responseData;
		}

		internal string ExecuteApiUpload(FileSystemInfo uploadFile, IDictionary<string, string> parameterList)
		{
			parameterList.Add("api_key", ApplicationKey);
			parameterList.Add("v", VERSION);
			parameterList.Add("call_id", DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture));
			parameterList.Add("sig", GenerateSignature(parameterList));

			// Get response
			return GetFileQueryResponse(parameterList, uploadFile);
		}

		internal XmlDocument ExecuteApiCall(IDictionary<string, string> parameterDictionary, bool useSession)
		{
			return LoadXMLDocument(ExecuteApiCallString(parameterDictionary, useSession));
		}

		internal string ExecuteApiCallString(IDictionary<string, string> parameterDictionary, bool useSession)
		{
			if (useSession)
			{
				parameterDictionary.Add("session_key", SessionKey);
			}

			string requestUrl = GetRequestUrl(parameterDictionary["method"] == "auth.getSession");
			string parameters = CreateHTTPParameterList(parameterDictionary);
			return GetQueryResponse(requestUrl, parameters);
		}

		/// <summary>
		/// Parse the data and extract the session details
		/// </summary>
		/// <param name="rawXML">string</param>
		/// <returns>XmlDocument</returns>
		internal XmlDocument LoadXMLDocument(string rawXML)
		{
			var xmlDocument = new XmlDocument();

			xmlDocument.LoadXml(rawXML);

			NsManager = new XmlNamespaceManager(xmlDocument.NameTable);
			NsManager.AddNamespace("Facebook", Resources.FacebookNamespace);

			ErrorCheck(xmlDocument);
			return xmlDocument;
		}

		/// <summary>
		/// Gets the request url
		/// </summary>
		/// <param name="useSSL">True if the request should use SSL, otherwise False</param>
		/// <returns>Request Url</returns>
		internal static string GetRequestUrl(bool useSSL)
		{
			return useSSL ? Resources.FacebookRESTUrl.Replace("http", "https") : Resources.FacebookRESTUrl;
		}

		internal string CreateHTTPParameterList(IDictionary<string, string> parameterList)
		{
			var queryBuilder = new StringBuilder();

			parameterList.Add("api_key", ApplicationKey);
			parameterList.Add("v", VERSION);
			parameterList.Add("call_id", DateTime.Now.Ticks.ToString("x", CultureInfo.InvariantCulture));
			parameterList.Add("sig", GenerateSignature(parameterList));

			// Build the query
			foreach (var kvp in parameterList)
			{
				queryBuilder.Append(kvp.Key);
				queryBuilder.Append("=");
				queryBuilder.Append(HttpUtility.UrlEncode(kvp.Value));
				queryBuilder.Append("&");
			}
			queryBuilder.Remove(queryBuilder.Length - 1, 1);

			return queryBuilder.ToString();
		}

		internal static List<string> ParameterDictionaryToList(IEnumerable<KeyValuePair<string, string>> parameterDictionary)
		{
			var parameters = new List<string>();

			foreach (var kvp in parameterDictionary)
			{
				parameters.Add(String.Format(CultureInfo.InvariantCulture, "{0}", kvp.Key));
			}
			return parameters;
		}

		/// <summary>
		/// This method generates the signature based on parameters supplied
		/// </summary>
		/// <param name="parameters">List of paramenters</param>
		/// <returns>Generated signature</returns>
		internal string GenerateSignature(IDictionary<string, string> parameters)
		{
			var signatureBuilder = new StringBuilder();

			// Sort the keys of the method call in alphabetical order
			List<string> keyList = ParameterDictionaryToList(parameters);
			keyList.Sort();

			// Append all the parameters to the signature input paramaters
			foreach (string key in keyList)
				signatureBuilder.Append(String.Format(CultureInfo.InvariantCulture, "{0}={1}", key, parameters[key]));

			// Append the secret to the signature builder
			signatureBuilder.Append(Secret);

			MD5 md5 = MD5.Create();
			// Compute the MD5 hash of the signature builder
			byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(signatureBuilder.ToString().Trim()));

			// Reinitialize the signature builder to store the actual signature
			signatureBuilder = new StringBuilder();

			// Append the hash to the signature
			foreach (byte hashByte in hash)
				signatureBuilder.Append(hashByte.ToString("x2", CultureInfo.InvariantCulture));

			return signatureBuilder.ToString();
		}

		internal bool IsSessionActive()
		{
			return !String.IsNullOrEmpty(SessionKey);
		}

		/// <summary>
		/// Parse the Facebook result for an error, and throw an exception. 
		/// For some of the different types of exceptions, custom action might be desirable.
		/// </summary>
		/// <param name="doc">The XML result.</param>
		internal static void ErrorCheck(XmlDocument doc)
		{
			XmlNodeList errors = doc.GetElementsByTagName("error_response");

			if (errors.Count > 0)
			{
				var errorCode = int.Parse(XmlHelper.GetNodeText(errors[0], "error_code"), CultureInfo.InvariantCulture);

				// Custom exception for some of the errors
				switch (errorCode)
				{
					case 1:
						throw new FacebookUnknownException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					case 2:
						throw new FacebookServiceUnavailableException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					case 4:
						throw new FacebookRequestLimitException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					case 102:
						throw new FacebookTimeoutException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					case 104:
						throw new FacebookSigningException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					case 110:
						throw new FacebookInvalidUserException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					case 120:
						throw new FacebookInvalidAlbumException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					case 210: // user not visible
					case 220: // album not visible
					case 221: // photo not visible
						throw new FacebookNotVisibleException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					case 601:
						throw new FacebookInvalidFqlSyntaxException(XmlHelper.GetNodeText(errors[0], "error_msg"));
					default:
						throw new FacebookException(XmlHelper.GetNodeText(errors[0], "error_msg"));
				}
			}
		}

		internal static string ToJsonAssociativeArray(Dictionary<string, string> paramDictionary)
		{
			var nameValuePairs = new List<string>();

			foreach (var pair in paramDictionary)
			{
				nameValuePairs.Add("\"" + EscapeJsonString(pair.Key) + "\":\"" + EscapeJsonString(pair.Value) + "\"");
			}

			return "{" + string.Join(",", nameValuePairs.ToArray()) + "}";
		}

		/// <summary>
		/// Escape backslashes and double quotes
		/// </summary>
		/// <param name="originalString">string</param>
		/// <returns>string</returns>
		internal static string EscapeJsonString(string originalString)
		{
			return originalString.Replace("\\", "\\\\").Replace("\"", "\\\"");
		}

		#endregion


		


		public FacebookAPI()
		{
			AuthToken = string.Empty;
			IsDesktopApplication = true;
#if NETCF 
			InstalledCulture = CultureInfo.CurrentUICulture;
#else
			InstalledCulture = CultureInfo.InstalledUICulture;
#endif
		}
	}
}