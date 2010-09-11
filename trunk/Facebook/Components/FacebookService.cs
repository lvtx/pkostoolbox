using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using Facebook.API;
using Facebook.Entity;
using Facebook.Exceptions;
using Facebook.Forms;
using Facebook.Properties;
using Facebook.Types;
using Group=Facebook.Entity.Group;

namespace Facebook.Components
{
	/// <summary>
	/// Provides various methods to use the Facebook Platform API.
	/// </summary>
#if !NETCF
	[ToolboxItem(true), ToolboxBitmap(typeof (FacebookService)), Designer(typeof (FacebookServiceDesigner))]
#endif
	[Obsolete("Call the FacebookAPI class directly instead.")]
	public partial class FacebookService : Component
	{
		#region Private Data

		private FacebookAPI _facebookAPI;

		private Uri _sendRequestResponseUrl;

		#endregion Private Data

		#region Accessors

		/// <summary>
		/// Access Key required to use the API
		/// </summary>
#if !NETCF
		[Category("Facebook"), Description("Access Key required to use the API")]
#endif
			public string ApplicationKey
		{
			get { return _facebookAPI.ApplicationKey; }
			set { _facebookAPI.ApplicationKey = value; }
		}

		/// <summary>
		/// User Id
		/// </summary>
#if !NETCF
		[Browsable(false)]
#endif
			public string UserId
		{
			get { return _facebookAPI.UserId; }
			set { _facebookAPI.UserId = value; }
		}


#if !NETCF
		[Category("Facebook"), Description("Secret Word")]
#endif
			public string Secret
		{
			get { return _facebookAPI.Secret; }
			set { _facebookAPI.Secret = value; }
		}

		/// <summary>
		/// Session key
		/// </summary>
#if !NETCF
		[Browsable(false)]
#endif
			public string SessionKey
		{
			get { return _facebookAPI.SessionKey; }
			set { _facebookAPI.SessionKey = value; }
		}

		/// <summary>
		/// Whether or not the session expires
		/// </summary>
#if !NETCF
		[Browsable(false)]
#endif
			public bool SessionExpires
		{
			get { return _facebookAPI.SessionExpires; }
		}

		/// <summary>
		/// Login Url
		/// </summary>
#if !NETCF
		[Browsable(false)]
#endif
			private string LoginUrl
		{
			get
			{
				var args = new object[2];
				args[0] = _facebookAPI.ApplicationKey;
				args[1] = _facebookAPI.AuthToken;

				return String.Format(CultureInfo.InvariantCulture, Resources.FacebookLoginUrl, args);
			}
		}



		/// <summary>
		/// LogOff Url
		/// </summary>
#if !NETCF
		[Browsable(false)]
#endif
			private string LogOffUrl
		{
			get
			{
				var args = new object[2];
				args[0] = _facebookAPI.ApplicationKey;
				args[1] = _facebookAPI.AuthToken;

				return String.Format(CultureInfo.InvariantCulture, Resources.FacebookLogoutUrl, args);
			}
		}

		/// <summary>
		/// Whether or not this component is being used in a desktop application
		/// </summary>
#if !NETCF
		///<summary>
		///</summary>
		[Browsable(false)]
#endif
			public bool IsDesktopApplication
		{
			get { return _facebookAPI.IsDesktopApplication; }
			set { _facebookAPI.IsDesktopApplication = value; }
		}

		/// <summary>
		/// ExtendedPermissionUrl
		/// </summary>
#if !NETCF
		[Browsable(false)]
#endif
		private string ExtendedPermissionUrl(Enums.Extended_Permissions permission)
		{
			var args = new object[2];
			args[0] = _facebookAPI.ApplicationKey;
			args[1] = permission;

			return
				String.Format(CultureInfo.InvariantCulture, Resources.FacebookRequestExtendedPermissionUrl, args);
		}

		#endregion

		#region Constructors

		[Obsolete("Call the FacebookAPI class directly instead.")]
		public FacebookService()
		{
			_facebookAPI = new FacebookAPI();
			InitializeComponent();
		}

		[Obsolete("Call the FacebookAPI class directly instead.")]
		public FacebookService(IContainer container)
		{
			if (container != null)
				container.Add(this);

			_facebookAPI = new FacebookAPI();
			InitializeComponent();
		}

		#endregion Constuctors

		/// <summary>
		/// Displays an integrated browser to allow the user to log on to the
		/// Facebook web page.
		/// </summary>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public void ConnectToFacebook()
		{
			if (IsDesktopApplication)
			{
				DialogResult result;
				_facebookAPI.SetAuthenticationToken();

				using (var formLogin = new FacebookAuthentication(LoginUrl))
				{
					result = formLogin.ShowDialog();
				}
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					_facebookAPI.CreateSession();
				}
				else
				{
					throw new FacebookInvalidUserException("Login attempt failed");
				}
			}
		}

		[Obsolete("Call the FacebookAPI class directly instead.")]
		public void ConnectToFacebook(FacebookAPI facebookAPI)
		{
			_facebookAPI = facebookAPI;
			ConnectToFacebook();
		}

		/// <summary>
		/// Displays an integrated browser to allow the user to log on to the
		/// Facebook web page.
		/// </summary>
		public void GetExtendedPermission(Enums.Extended_Permissions permission)
		{
			DialogResult result;

			using (var formLogin = new FacebookExtendedPermission(ExtendedPermissionUrl(permission), permission, this))
			{
				result = formLogin.ShowDialog();
			}

			if (result != DialogResult.OK)
			{
				throw new FacebookInvalidUserExtendedPermission("Extended Permission Denied");
			}
		}

		/// <summary>
		/// Forgets all connection information so that this object may be used for another connection.
		/// </summary>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public void LogOff()
		{
			_facebookAPI.LogOff();
		}

		/// <summary>
		/// Creates a new session with Facebook.
		/// </summary>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public void CreateSession(string authToken)
		{
			_facebookAPI.AuthToken = authToken;
			_facebookAPI.CreateSession(authToken);
		}

		/// <summary>
		/// Sends a direct FQL query to Facebook.
		/// </summary>
		/// <param name="query">An FQL Query.</param>
		/// <returns>The result of the FQL query as an XML string.</returns> 
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public string DirectFQLQuery(string query)
		{
			string results = string.Empty;
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}

			try
			{
				results = _facebookAPI.DirectFQLQuery(query);
			}
			catch (FacebookTimeoutException)
			{
				// Reconnect because of timed out session
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					DirectFQLQuery(query);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Gets all event ids for the logged in user.
		/// </summary>
		/// <returns>A list of event ids.</returns>
		public Collection<string> GetEventIds()
		{
			EnsureConnected();
			return _facebookAPI.GetEventIds();
		}

		/// <summary>
		/// Gets all events for the logged in user.
		/// </summary>
		/// <returns>A list of events.</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public Collection<FacebookEvent> GetEvents()
		{
			return GetEvents(null, _facebookAPI.UserId, null, null);
		}

		/// <summary>
		/// Gets all events for the specified user.
		/// </summary>
		/// <param name="userId">User to return events for.</param>
		/// <returns>A list of events.</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public Collection<FacebookEvent> GetEvents(string userId)
		{
			return GetEvents(null, userId, null, null);
		}

		/// <summary>
		/// Gets all events corresponding to the specified event ids.
		/// </summary>
		/// <param name="eventList">A list of event ids.</param>
		/// <returns>A list of events.</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public Collection<FacebookEvent> GetEvents(Collection<string> eventList)
		{
			return GetEvents(eventList, null, null, null);
		}

		/// <summary>
		/// Gets all events for the specified user corresponding to the specified event ids.
		/// </summary>
		/// <param name="eventList">A list of event ids.</param>
		/// <param name="userId">The user to return events for.</param>
		/// <returns>A list of events.</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public Collection<FacebookEvent> GetEvents(Collection<string> eventList, string userId)
		{
			return GetEvents(eventList, userId, null, null);
		}

		/// <summary>
		/// Gets all events for the specified user and event ids that overlap the
		/// window of time specified by the start and end dates.
		/// </summary> 
		/// <param name="eventList">A list of event ids.</param>
		/// <param name="userId">The user to return events for.</param>
		/// <param name="startDate">The lower bound of the time window.</param>
		/// <param name="endDate">The upper bound of the time window.</param>
		/// <returns>A list of events.</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public Collection<FacebookEvent> GetEvents(Collection<string> eventList, string userId, DateTime? startDate,
		                                           DateTime? endDate)
		{
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetEvents(eventList, userId, startDate, endDate);
		}

		/// <summary>
		/// Gets the XML representation of all events for the specified user and
		/// event ids that overlap the window of time specified by the start and
		/// end dates.
		/// </summary>
		/// <param name="eventList">list of event ids.</param>
		/// <param name="userId">User to return events for.</param>
		/// <param name="startDate">events occuring after this date.</param>
		/// <param name="endDate">events occuring before this date.</param>
		/// <returns>A list of events as raw XML.</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public string GetEventsXML(Collection<string> eventList, string userId, DateTime? startDate, DateTime? endDate)
		{
			string results = string.Empty;
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.GetEventsXML(eventList, userId, startDate, endDate);
			}
			catch (FacebookTimeoutException)
			{
				// Reconnect because of timed out session
				_facebookAPI.SessionKey = null;
				if (this.IsDesktopApplication)
				{
					ConnectToFacebook();
					GetEventsXML(eventList, userId, startDate, endDate);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Gets all event members for the specified event.
		/// </summary>
		/// <param name="eventId">The event to return users for.</param>
		/// <returns>A list of event members with rsvp status.</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public Collection<EventUser> GetEventMembers(string eventId)
		{
			return _facebookAPI.GetEventMembers(eventId);
		}

		/// <summary>
		/// Gets the XML representation of all event members for the specified event.
		/// </summary>
		/// <param name="eventId">The event to return users for.</param>
		/// <returns>A list of event members with rsvp status as raw XML.</returns>
		public string GetEventMembersXML(string eventId)
		{
			string results = string.Empty;
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.GetEventMembersXML(eventId);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (this.IsDesktopApplication)
				{
					ConnectToFacebook();
					GetEventMembersXML(eventId);
				}
				else
				{
					throw;
				}
			}
			return results;
		}

		/// <summary>
		/// Get all the friends for the logged in user
		/// </summary>
		/// <returns>user profile of each friend</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public Collection<User> GetFriends()
		{
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetFriends();
		}

		/// <summary>
		/// Get all the friends for the logged in user
		/// </summary>
		/// <returns>user profile of each friend</returns>
		[Obsolete("Call the FacebookAPI class directly instead.")]
		public Collection<string> GetFriendIds()
		{
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetFriendIds();
		}

		/// <summary>
		/// Get all the friends for the logged in user and returns the results as raw XML
		/// </summary>
		/// <returns>The XMl representation of the user profile of each friend</returns>
		public string GetFriendsXML()
		{
			string results = string.Empty;
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}

			try
			{
				results = _facebookAPI.GetFriendsXML();
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (this.IsDesktopApplication)
				{
					ConnectToFacebook();
					GetFriendsXML();
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Get all the friends for the logged in user that use the current application 
		/// </summary>
		/// <returns>user profile of each friend</returns>
		public Collection<User> GetFriendsNonAppUsers()
		{
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetFriendsNonAppUsers();
		}

		/// <summary>
		/// Get all the friends for the logged in user that use the current application 
		/// </summary>
		/// <returns>user profile of each friend</returns>
		public Collection<User> GetFriendsAppUsers()
		{
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetFriendsAppUsers();
		}

		/// <summary>
		/// Get all the friends for the logged in user that use the current application 
		/// </summary>
		/// <returns>user profile of each friend</returns>
		public string GetFriendsAppUsersXML()
		{
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			string results = string.Empty;

			try
			{
				results = _facebookAPI.GetFriendsAppUsersXML();
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (this.IsDesktopApplication)
				{
					ConnectToFacebook();
					GetFriendsAppUsersXML();
				}
				else
				{
					throw;
				}
			}

			return results;
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
		public bool AreFriends(string userId1, string userId2)
		{
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}

			try
			{
				return _facebookAPI.AreFriends(userId1, userId2);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (this.IsDesktopApplication)
				{
					ConnectToFacebook();
					AreFriends(userId1, userId2);
				}
				else
				{
					throw;
				}
			}

			return false;
		}

		/// <summary>
		/// Build the user profile for the logged in user
		/// </summary>
		/// <returns>user profile</returns>
		[Obsolete]
		public User GetUserInfo()
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetUserInfo();
		}

		/// <summary>
		/// Build the user profile for the list of users
		/// </summary>
		/// <param name="userIds">Comma separated list of user ids</param>
		/// <returns>user profile list</returns>
		[Obsolete]
		public Collection<User> GetUserInfo(string userIds)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetUserInfo(userIds);
		}


		/// <summary>
		/// Build the user profile for the list of users
		/// </summary>
		/// <param name="userIds">A collection of userId strings</param>
		/// <returns>user profile list</returns>
		[Obsolete]
		public Collection<User> GetUserInfo(Collection<string> userIds)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetUserInfo(userIds);
		}


		/// <summary>
		/// Builds the user profile for the list of users and returns the results as raw xml
		/// </summary>
		/// <param name="userIds">Comma separated list of user ids</param>
		/// <returns>The xml representation of the user profile list</returns>
		[Obsolete]
		public string GetUserInfoXml(string userIds)
		{
			string results = string.Empty;

			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}

			try
			{
				results = _facebookAPI.GetUserInfoXml(userIds);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					GetUserInfoXml(userIds);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Update your status on Facebook, it will not include "IS" prepended.
		/// 
		/// Requires Facebook extended permission of "status_update"
		/// </summary>
		/// <param name="statusMessage">Your status message to be posted</param>
		public string SetStatus(string statusMessage)
		{
			return SetStatus(statusMessage, false);
		}

		/// <summary>
		/// Update your status on Facebook
		/// 
		/// Requires Facebook extended permission of "status_update"
		/// </summary>
		/// <param name="statusMessage">Your status message to be posted</param>
		/// <param name="IncludeIsVerb">Have your message start with the word "is"</param>
		public string SetStatus(string statusMessage, bool IncludeIsVerb)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}

			if (!_facebookAPI.HasPermission(Enums.Extended_Permissions.status_update))
			{
				GetExtendedPermission(Enums.Extended_Permissions.status_update);
			}

			return _facebookAPI.SetStatus(statusMessage, IncludeIsVerb);
		}

		/// <summary>
		/// Verify user has permission to do action
		/// </summary>
		/// <param name="permission">Extended Permission</param>
		/// <returns>Does user have permission.</returns>
		public bool HasPermission(Enums.Extended_Permissions permission)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}

			return _facebookAPI.HasPermission(permission);
		}

		/// <summary>
		/// Return the notifications
		/// </summary>
		/// <returns>user profile list</returns>
		public Notifications GetNotifications()
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetNotifications();
		}

		/// <summary>
		/// Builds the list of notifications and returns the results as raw xml
		/// </summary>
		/// <returns>The xml representation of the user profile list</returns>
		public string GetNotificationsXml()
		{
			string results = string.Empty;

			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.GetNotificationsXml();
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					GetNotificationsXml();
				}
				else
				{
					throw;
				}
			}

			return results;
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
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetPhotos(albumId, photoList, user);
		}

		/// <summary>
		/// Get the photos for a specified album and list of photos
		/// </summary>
		/// <param name="albumId">The album</param>
		/// <param name="photoList">Collection of photo ids</param>
		/// <returns>photos</returns>
		[Obsolete("We now include a more extensive version which includes a User as well")]
		public string GetPhotosXML(string albumId, Collection<string> photoList)
		{
			// Eventually, this version should be depreciated.
			// Simply call the newer function.
			return GetPhotosXML(albumId, photoList, null);
		}

		/// <summary>
		/// Get the photos for a specified album, list of photos, and User
		/// </summary>
		/// <param name="albumId">The album</param>
		/// <param name="photoList">Collection of photo ids</param>
		/// <param name="user">The user</param>
		/// <returns>photos</returns>
		public string GetPhotosXML(string albumId, Collection<string> photoList, User user)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			string results = string.Empty;
			try
			{
				results = _facebookAPI.GetPhotosXML(albumId, photoList, user);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					GetPhotosXML(albumId, photoList, user);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Get the albums for the logged in user
		/// </summary>
		/// <returns>albums</returns>
		public Collection<Album> GetPhotoAlbums()
		{
			EnsureConnected();
			return GetPhotoAlbums(_facebookAPI.UserId, null);
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
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetPhotoAlbums(userId, albumList);
		}

		/// <summary>
		/// Get the albums for the specified user and list of albums
		/// </summary>
		/// <param name="userId">user to return albums for</param>
		/// <param name="albumList">collection of album ids</param>
		/// <returns>albums</returns>
		public string GetPhotoAlbumsXML(string userId, Collection<string> albumList)
		{
			if (!IsSessionActive() && this.IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			string results = string.Empty;
			try
			{
				results = _facebookAPI.GetPhotoAlbumsXML(userId, albumList);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					GetPhotoAlbumsXML(userId, albumList);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Get the tags for the specifed photos
		/// </summary>
		/// <param name="photoList">collection of photo ids</param>
		/// <returns>photo tags</returns>
		public Collection<PhotoTag> GetTags(Collection<string> photoList)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetTags(photoList);
		}

		/// <summary>
		/// Get the tags for the specifed photos
		/// </summary>
		/// <param name="photoList">collection of photo ids</param>
		/// <returns>photo tags</returns>
		public string GetTagsXML(Collection<string> photoList)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			string results = string.Empty;
			try
			{
				results = _facebookAPI.GetTagsXML(photoList);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					GetTagsXML(photoList);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Gets the FBML on the logged-in user's profile.
		/// </summary>
		/// <returns>The FBML from the user's profile.</returns>
		public string GetFBML()
		{
			EnsureConnected();

			try
			{
				return _facebookAPI.GetFBML();
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;

				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					return GetFBML();
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the FBML on a user's profile
		/// </summary>
		/// <param name="userId">The id of the user to get the FBML from.</param>
		/// <returns>The FBML markup from the user's profile.</returns>
		public string GetFBML(string userId)
		{
			EnsureConnected();

			try
			{
				return _facebookAPI.GetFBML(userId);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;

				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					return GetFBML(userId);
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Set the FBML on a profile
		/// </summary>
		/// <param name="profileFBML">Profile FBML markup</param>
		/// <param name="profileActionFBML">Profile Action FBML markup</param>
		/// <param name="mobileFBML">Mobile Profile FBML markup</param>
		public string SetFBML(string profileFBML, string profileActionFBML, string mobileFBML)
		{
			return SetFBML(profileFBML, profileActionFBML, mobileFBML, null);
		}

		/// <summary>
		/// Set the FBML on a profile
		/// </summary>
		/// <param name="profileFBML">Profile FBML markup</param>
		/// <param name="profileActionFBML">Profile Action FBML markup</param>
		/// <param name="mobileFBML">Mobile Profile FBML markup</param>
		/// <param name="userId">user id</param>
		/// <returns>The result of the method call as a string.</returns>
		public string SetFBML(string profileFBML, string profileActionFBML, string mobileFBML, string userId)
		{
			string results = string.Empty;
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.SetFBML(profileFBML, profileActionFBML, mobileFBML, userId);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					SetFBML(profileFBML, profileActionFBML, mobileFBML, userId);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Associates a specified handle with a piece of FBML markup.
		/// </summary>
		/// <param name="handle">The handle to use as a reference.</param>
		/// <param name="markup">The FBML markup to be referenced.</param>
		/// <returns>The result of the method call as a string.</returns>
		public string SetRefHandle(string handle, string markup)
		{
			string results = string.Empty;
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.SetRefHandle(handle, markup);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					SetRefHandle(handle, markup);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Tells Facebook to fetches and re-cache the content stored at the
		/// given URL for use in a fb:ref FBML tag.
		/// </summary>
		/// <param name="url">The URL of the content to refresh.</param>
		/// <returns>The result of the method call as a string.</returns>
		public string RefreshRefUrl(string url)
		{
			string results = string.Empty;
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.RefreshRefUrl(url);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					RefreshRefUrl(url);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Tells Facebook to fetche and re-cache the image stored at the given
		/// URL for use in non-canvas pages on Facebook.
		/// </summary>
		/// <param name="url">The URL of the image to refresh.</param>
		/// <returns>The result of the method call as a string.</returns>
		public string RefreshImgSrc(string url)
		{
			string results = string.Empty;
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.RefreshImgSrc(url);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					RefreshImgSrc(url);
				}
				else
				{
					throw;
				}
			}

			return results;
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
			string results = string.Empty;
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.SendNotification(markup, toList, email);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					SendNotification(markup, toList, email);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

#if !NETCF
		/// <summary>
		/// Display a page to send a request
		/// </summary>
		/// <param name="markup">FBML markup for the request</param>
		/// <param name="requestType">Type of request</param>
		/// <param name="text">Text to display in the friend selection form</param>
		/// <param name="isInvite">Whether this is an invitation (true) or request (false)</param>
		/// <returns>A collection of the ids of friends to whom requests were sent</returns>
		/// <remarks>This metod only works for desktop applications. For web applications, use
		/// the new fb:multi-friend-selector tag, or one of its relatives.</remarks>
		public Collection<string> SendRequest(string markup, string requestType, string text, bool isInvite)
		{
			EnsureConnected();

			// create the URL from the parameters
			var parameters = new Dictionary<string, string>(7);
			string callbackUrl = Resources.DummyCallbackUrl;
			parameters.Add("api_key", ApplicationKey);
			parameters.Add("content", markup);
			parameters.Add("type", requestType);
			parameters.Add("action", callbackUrl);
			parameters.Add("actiontext", text);
			parameters.Add("invite", isInvite.ToString().ToLower());
			parameters.Add("sig", _facebookAPI.GenerateSignature(parameters));

			var sb = new StringBuilder(Resources.SendRequestUrl);
			sb.Append("?");

			foreach (var kvp in parameters)
			{
				sb.Append(kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value) + "&");
			}

			sb.Remove(sb.Length - 1, 1);

			using (var requestForm = new RequestSelection(sb.ToString(), new Uri(callbackUrl), this))
			{
				requestForm.ShowDialog();
			}

			// process the returned list of ids
			if (_sendRequestResponseUrl == null)
			{
				return new Collection<string>();
			}

			return ParseResponse(_sendRequestResponseUrl.Query);
		}
#endif

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
			string results = string.Empty;
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.PublishStory(title, body, images, priority);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					PublishStory(title, body, images, priority);
				}
				else
				{
					throw;
				}
			}

			return results;
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
			string results = string.Empty;
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.PublishAction(title, body, images);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					PublishAction(title, body, images, priority);
				}
				else
				{
					throw;
				}
			}

			return results;
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
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				return _facebookAPI.UploadPhoto(albumId, uploadFile, caption);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					return UploadPhoto(albumId, uploadFile, caption);
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Add a tag to a photo.  Only allowed on photos in pending state.
		/// </summary>
		/// <param name="photoId">The id of the photo to tag</param>
		/// <param name="tagText">The text of the tag.  Need to specify either this of tagUserId</param>
		/// <param name="tagUserId">The facebook id the person that was tagged</param>
		/// <param name="xCoord">The x position of the tag on the photo</param>
		/// <param name="yCoord">The y position of the tag on the photo</param>
		public void AddTag(string photoId, string tagText, string tagUserId, int xCoord, int yCoord)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				_facebookAPI.AddTag(photoId, tagText, tagUserId, xCoord, yCoord);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					AddTag(photoId, tagText, tagUserId, xCoord, yCoord);
				}
				else
				{
					throw;
				}
			}
		}

		/// <summary>
		/// Create Album.
		/// </summary>
		/// <param name="name">The name of the album</param>
		/// <param name="location">The location of the album.  (Optional)</param>
		/// <param name="description">The description of the album.  (Optional)</param>
		public Album CreateAlbum(string name, string location, string description)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			Album results = null;
			try
			{
				results = _facebookAPI.CreateAlbum(name, location, description);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					CreateAlbum(name, location, description);
				}
				else
				{
					throw;
				}
			}
			return results;
		}

		/// <summary>
		/// Get the groups that the logged in user belongs to
		/// </summary>
		/// <returns>groups</returns>
		public Collection<Group> GetGroups()
		{
			EnsureConnected();
			return GetGroups(_facebookAPI.UserId, null);
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
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetGroups(userId, groupsList);
		}

		/// <summary>
		/// Get the groups for the specified user and group list
		/// </summary>
		/// <param name="userId">The id of the user to return groups for</param>
		/// <param name="groupsList">Collection of group ids</param>
		/// <returns>groups</returns>
		public string GetGroupsXML(string userId, Collection<string> groupsList)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			string results = string.Empty;
			try
			{
				results = _facebookAPI.GetGroupsXML(userId, groupsList);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					GetGroupsXML(userId, groupsList);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Get the members of the specified group
		/// </summary>
		/// <param name="groupId">The id of the group to return members for</param>
		/// <returns>Group members (user profiles, and group roles)</returns>
		public Collection<GroupUser> GetGroupMembers(string groupId)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			return _facebookAPI.GetGroupMembers(groupId);
		}

		/// <summary>
		/// Get the members of the specified group
		/// </summary>
		/// <param name="groupId">The id of the group to return members for</param>
		/// <returns>Group members (user profiles, and group roles)</returns>
		public string GetGroupMembersXML(string groupId)
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			string results = string.Empty;
			try
			{
				results = _facebookAPI.GetGroupMembersXML(groupId);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					GetGroupMembersXML(groupId);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Get the facebook user id of the user associated with the current session
		/// </summary>
		/// <returns>facebook userid</returns>
		public string GetLoggedInUser()
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			string results = string.Empty;
			try
			{
				results = _facebookAPI.GetLoggedInUser();
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					GetLoggedInUser();
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		/// <summary>
		/// Determine if the current user is a user of this application already
		/// </summary>
		/// <returns>facebook userid</returns>
		public bool IsAppAdded()
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			bool results = false;
			try
			{
				results = _facebookAPI.IsAppAdded();
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				_facebookAPI.UserId = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					IsAppAdded();
				}
				else
				{
					throw;
				}
			}

			return results;
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
			EnsureConnected();
			return _facebookAPI.PublishTemplatizedAction(titleTemplate, titleData, bodyTemplate, bodyData, bodyGeneral,
			                                             targetIds, images);
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
			string results = string.Empty;
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
			try
			{
				results = _facebookAPI.SendEmail(recipients, subject, text, fbml);
			}
			catch (FacebookTimeoutException)
			{
				_facebookAPI.SessionKey = null;
				if (IsDesktopApplication)
				{
					ConnectToFacebook();
					SendEmail(recipients, subject, text, fbml);
				}
				else
				{
					throw;
				}
			}

			return results;
		}

		#region Private Methods

		private bool IsSessionActive()
		{
			return _facebookAPI.IsSessionActive();
		}

		private void EnsureConnected()
		{
			if (!IsSessionActive() && IsDesktopApplication)
			{
				ConnectToFacebook();
			}
		}

		private Collection<string> ParseResponse(string response)
		{
			string decodedResponse = response.Replace("%5B", "[").Replace("%5D", "]");
			MatchCollection matches = Regex.Matches(decodedResponse, @"ids\[]=(\d+)");
			var ids = new Collection<string>();

			foreach (Match match in matches)
			{
				string id = match.Groups[1].Value;
				ids.Add(id);
			}

			return ids;
		}

		#endregion

		#region Internal Methods

		internal void ReceiveRequestData(Uri responseUrl)
		{
			_sendRequestResponseUrl = responseUrl;
		}

		#endregion
	}
}