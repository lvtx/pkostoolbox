using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Facebook.Entity;

namespace Facebook.Components
{
	public sealed class AsyncFacebookService : FacebookService
	{
		#region Asynchronous GetPhotoAlbums

		public IAsyncResult BeginGetPhotoAlbums(AsyncCallback callBack, Object state)
		{
			GetPhotoAlbumsDelegate1 d = GetPhotoAlbums;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public IAsyncResult BeginGetPhotoAlbums(string userId, AsyncCallback callBack, Object state)
		{
			GetPhotoAlbumsDelegate2 d = GetPhotoAlbums;
			IAsyncResult result = d.BeginInvoke(userId, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetPhotoAlbums(Collection<string> albumList, AsyncCallback callBack, Object state)
		{
			GetPhotoAlbumsDelegate3 d = GetPhotoAlbums;
			IAsyncResult result = d.BeginInvoke(albumList, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetPhotoAlbums(string userId, Collection<string> albumList, AsyncCallback callBack,
		                                        Object state)
		{
			GetPhotoAlbumsDelegate4 d = GetPhotoAlbums;
			IAsyncResult result = d.BeginInvoke(userId, albumList, callBack, state);
			return result;
		}

		public Collection<Album> EndGetPhotoAlbums(IAsyncResult result, Object state)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();

			if (delegateType == typeof (GetPhotoAlbumsDelegate1))
			{
				var d = (GetPhotoAlbumsDelegate1) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetPhotoAlbumsDelegate2))
			{
				var d = (GetPhotoAlbumsDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetPhotoAlbumsDelegate3))
			{
				var d = (GetPhotoAlbumsDelegate3) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (GetPhotoAlbumsDelegate4) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion Asynchronous GetPhotoAlbums

		#region Asynchronous GetPhotos

		public IAsyncResult BeginGetPhotos(string albumId, AsyncCallback callBack, Object state)
		{
			GetPhotosDelegate1 d = GetPhotos;
			IAsyncResult result = d.BeginInvoke(albumId, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetPhotos(Collection<string> photoList, AsyncCallback callBack, Object state)
		{
			GetPhotosDelegate2 d = GetPhotos;
			IAsyncResult result = d.BeginInvoke(photoList, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetPhotos(User user, AsyncCallback callBack, Object state)
		{
			GetPhotosDelegate3 d = GetPhotos;
			IAsyncResult result = d.BeginInvoke(user, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetPhotos(string albumId, Collection<string> photoList, AsyncCallback callBack, Object state)
		{
			GetPhotosDelegate4 d = GetPhotos;
			IAsyncResult result = d.BeginInvoke(albumId, photoList, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetPhotos(string albumId, Collection<string> photoList, User user, AsyncCallback callBack,
		                                   Object state)
		{
			GetPhotosDelegate5 d = GetPhotos;
			IAsyncResult result = d.BeginInvoke(albumId, photoList, user, callBack, state);
			return result;
		}

		public Collection<Photo> EndGetPhotos(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();

			if (delegateType == typeof (GetPhotosDelegate1))
			{
				var d = (GetPhotosDelegate1) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetPhotosDelegate2))
			{
				var d = (GetPhotosDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetPhotosDelegate3))
			{
				var d = (GetPhotosDelegate3) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetPhotosDelegate4))
			{
				var d = (GetPhotosDelegate4) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (GetPhotosDelegate5) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion  Asynchronous GetPhotos

		#region Asynchronous GetUserInfo

		public IAsyncResult BeginGetUserInfo(AsyncCallback callBack, Object state)
		{
			GetUserInfoDelegate1 d = GetUserInfo;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public IAsyncResult BeginGetUserInfo(string userIds, AsyncCallback callBack, Object state)
		{
			GetUserInfoDelegate2 d = GetUserInfo;
			IAsyncResult result = d.BeginInvoke(userIds, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetUserInfo(Collection<string> userIds, AsyncCallback callBack, Object state)
		{
			GetUserInfoDelegate3 d = GetUserInfo;
			IAsyncResult result = d.BeginInvoke(userIds, callBack, state);
			return result;
		}

		public User EndGetUserInfo(IAsyncResult result)
		{
			var d = (GetUserInfoDelegate1) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		/// <summary>
		/// this was necessary because GetUserInfo() returns only User information, instead of Collection<User>, so
		/// I had to name it as the following EndGetUserInfoList instead of EndGetUserInfo
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		public Collection<User> EndGetUserInfoList(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();

			if (delegateType == typeof (GetUserInfoDelegate2))
			{
				var d = (GetUserInfoDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (GetUserInfoDelegate3) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion Asynchronous GetUserInfo

		#region Asynchronous GetEvents

		public IAsyncResult BeginGetEvents(AsyncCallback callBack, Object state)
		{
			GetEventsDelegate1 d = GetEvents;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public IAsyncResult BeginGetEvents(string userId, AsyncCallback callBack, Object state)
		{
			GetEventsDelegate2 d = GetEvents;
			IAsyncResult result = d.BeginInvoke(userId, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetEvents(Collection<string> eventList, AsyncCallback callBack, Object state)
		{
			GetEventsDelegate3 d = GetEvents;
			IAsyncResult result = d.BeginInvoke(eventList, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetEvents(Collection<string> eventList, string userId, AsyncCallback callBack, Object state)
		{
			GetEventsDelegate4 d = GetEvents;
			IAsyncResult result = d.BeginInvoke(eventList, userId, callBack, state);
			return result;
		}

		public Collection<FacebookEvent> EndGetEvents(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();

			if (delegateType == typeof (GetEventsDelegate1))
			{
				var d = (GetEventsDelegate1) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetEventsDelegate2))
			{
				var d = (GetEventsDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetEventsDelegate3))
			{
				var d = (GetEventsDelegate3) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (GetEventsDelegate4) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion Asynchronous GetPhotoAlbums

		#region Asynchronous GetGroups

		public IAsyncResult BeginGetGroups(AsyncCallback callBack, Object state)
		{
			GetGroupsDelegate1 d = GetGroups;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public IAsyncResult BeginGetGroups(string userId, AsyncCallback callBack, Object state)
		{
			GetGroupsDelegate2 d = GetGroups;
			IAsyncResult result = d.BeginInvoke(userId, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetGroups(Collection<string> groupsList, AsyncCallback callBack, Object state)
		{
			GetGroupsDelegate3 d = GetGroups;
			IAsyncResult result = d.BeginInvoke(groupsList, callBack, state);
			return result;
		}

		public IAsyncResult BeginGetGroups(string userId, Collection<string> groupsList, AsyncCallback callBack, Object state)
		{
			GetGroupsDelegate4 d = GetGroups;
			IAsyncResult result = d.BeginInvoke(userId, groupsList, callBack, state);
			return result;
		}

		public Collection<Group> EndGetGroups(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();

			if (delegateType == typeof (GetGroupsDelegate1))
			{
				var d = (GetGroupsDelegate1) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetGroupsDelegate2))
			{
				var d = (GetGroupsDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else if (delegateType == typeof (GetGroupsDelegate3))
			{
				var d = (GetGroupsDelegate3) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (GetGroupsDelegate4) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion Asynchronous GetGroups

		#region Asynchronous AddTag

		public IAsyncResult BeginAddTag(string photoId, string tagText, string tagUserId, int xCoord, int yCoord,
		                                AsyncCallback callBack, Object state)
		{
			AddTagDelegate d = AddTag;
			IAsyncResult result = d.BeginInvoke(photoId, tagText, tagUserId, xCoord, yCoord, callBack, state);
			return result;
		}

		public void EndAddTag(IAsyncResult result)
		{
			var d = (AddTagDelegate) ((AsyncResult) result).AsyncDelegate;
			d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous AreFriends

		public IAsyncResult BeginAreFriends(string userId1, string userId2, AsyncCallback callBack, Object state)
		{
			AreFriendsDelegate1 d = AreFriends;
			IAsyncResult result = d.BeginInvoke(userId1, userId2, callBack, state);
			return result;
		}

		public IAsyncResult BeginAreFriends(User userId1, User userId2, AsyncCallback callBack, Object state)
		{
			AreFriendsDelegate2 d = AreFriends;
			IAsyncResult result = d.BeginInvoke(userId1, userId2, callBack, state);
			return result;
		}

		public bool EndAreFriends(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();
			if (delegateType == typeof (AreFriendsDelegate1))
			{
				var d = (AreFriendsDelegate1) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (AreFriendsDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion

		#region Asynchronous ConnectToFacebook

		public IAsyncResult BeginConnectToFacebook(AsyncCallback callBack, Object state)
		{
			ConnectToFacebookDelegate d = ConnectToFacebook;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public void EndConnectToFacebook(IAsyncResult result)
		{
			var d = (ConnectToFacebookDelegate) ((AsyncResult) result).AsyncDelegate;
			d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous GetEventMembers

		public IAsyncResult BeginGetEventMembers(string eventId, AsyncCallback callBack, Object state)
		{
			GetEventMembersDelegate d = GetEventMembers;
			IAsyncResult result = d.BeginInvoke(eventId, callBack, state);
			return result;
		}

		public Collection<EventUser> EndGetEventMembers(IAsyncResult result)
		{
			var d = (GetEventMembersDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous GetFriends

		public IAsyncResult BeginGetFriends(AsyncCallback callBack, Object state)
		{
			GetFriendsDelegate d = GetFriends;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public Collection<User> EndGetFriends(IAsyncResult result)
		{
			var d = (GetFriendsDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous GetFriendsAppUsers

		public IAsyncResult BeginGetFriendsAppUsers(AsyncCallback callBack, Object state)
		{
			GetFriendsAppUsersDelegate d = GetFriendsAppUsers;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public Collection<User> EndGetFriendsAppUsers(IAsyncResult result)
		{
			var d = (GetFriendsAppUsersDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous GetFriendsNonAppUsers

		public IAsyncResult BeginGetFriendsNonAppUsers(AsyncCallback callBack, Object state)
		{
			GetFriendsNonAppUsersDelegate d = GetFriendsNonAppUsers;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public Collection<User> EndGetFriendsNonAppUsers(IAsyncResult result)
		{
			var d = (GetFriendsNonAppUsersDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous GetGroupMembers

		public IAsyncResult BeginGetGroupMembers(string groupId, AsyncCallback callBack, Object state)
		{
			GetGroupMembersDelegate d = GetGroupMembers;
			IAsyncResult result = d.BeginInvoke(groupId, callBack, state);
			return result;
		}

		public Collection<GroupUser> EndGetGroupMembers(IAsyncResult result)
		{
			var d = (GetGroupMembersDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous GetLoggedInUser

		public IAsyncResult BeginGetLoggedInUser(AsyncCallback callBack, Object state)
		{
			GetLoggedInUserDelegate d = GetLoggedInUser;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public string EndGetLoggedInUser(IAsyncResult result)
		{
			var d = (GetLoggedInUserDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous GetNotifications

		public IAsyncResult BeginGetNotifications(AsyncCallback callBack, Object state)
		{
			GetNotificationsDelegate d = GetNotifications;
			IAsyncResult result = d.BeginInvoke(callBack, state);
			return result;
		}

		public Notifications EndGetNotifications(IAsyncResult result)
		{
			var d = (GetNotificationsDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous PublishAction

		public IAsyncResult BeginPublishAction(string title, string body, Collection<PublishImage> images,
		                                       AsyncCallback callBack, Object state)
		{
			PublishActionDelegate1 d = PublishAction;
			IAsyncResult result = d.BeginInvoke(title, body, images, callBack, state);
			return result;
		}

		public IAsyncResult BeginPublishAction(string title, string body, Collection<PublishImage> images, int priority,
		                                       AsyncCallback callBack, Object state)
		{
			PublishActionDelegate2 d = PublishAction;
			IAsyncResult result = d.BeginInvoke(title, body, images, priority, callBack, state);
			return result;
		}

		public string EndPublishAction(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();
			if (delegateType == typeof (PublishActionDelegate1))
			{
				var d = (PublishActionDelegate1) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (PublishActionDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion

		#region Asynchronous PublishTemplatizedAction

		public IAsyncResult BeginPublishTemplatizedAction(string titleTemplate, Dictionary<string, string> titleData,
		                                                  string bodyTemplate, Dictionary<string, string> bodyData,
		                                                  Collection<PublishImage> images, AsyncCallback callBack,
		                                                  Object state)
		{
			PublishTemplatizedActionDelegate1 d = PublishTemplatizedAction;
			return d.BeginInvoke(titleTemplate, titleData, bodyTemplate, bodyData, images, callBack, state);
		}

		public IAsyncResult BeginPublishTemplatizedAction(string titleTemplate, Dictionary<string, string> titleData,
		                                                  string bodyTemplate, Dictionary<string, string> bodyData,
		                                                  string bodyGeneral, Collection<string> targetIds,
		                                                  Collection<PublishImage> images, AsyncCallback callBack,
		                                                  Object state)
		{
			PublishTemplatizedActionDelegate2 d = PublishTemplatizedAction;
			return d.BeginInvoke(titleTemplate, titleData, bodyTemplate, bodyData, bodyGeneral, targetIds, images, callBack,
			                     state);
		}

		public string EndPublishTemplatizedAction(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();
			if (delegateType == typeof (PublishTemplatizedActionDelegate1))
			{
				return ((PublishTemplatizedActionDelegate1) ((AsyncResult) result).AsyncDelegate).EndInvoke(result);
			}
			else
			{
				return ((PublishTemplatizedActionDelegate2) ((AsyncResult) result).AsyncDelegate).EndInvoke(result);
			}
		}

		#endregion

		#region Asynchronous PublishStory

		public IAsyncResult BeginPublishStory(string title, string body, Collection<PublishImage> images,
		                                      AsyncCallback callBack, Object state)
		{
			PublishStoryDelegate1 d = PublishStory;
			IAsyncResult result = d.BeginInvoke(title, body, images, callBack, state);
			return result;
		}

		public IAsyncResult BeginPublishStory(string title, string body, Collection<PublishImage> images, int priority,
		                                      AsyncCallback callBack, Object state)
		{
			PublishStoryDelegate2 d = PublishStory;
			IAsyncResult result = d.BeginInvoke(title, body, images, priority, callBack, state);
			return result;
		}

		public string EndPublishStory(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();
			if (delegateType == typeof (PublishStoryDelegate1))
			{
				var d = (PublishStoryDelegate1) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (PublishStoryDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion

		#region Asynchronous GetTags

		public IAsyncResult BeginGetTags(Collection<string> photoList, AsyncCallback callBack, Object state)
		{
			GetTagsDelegate d = GetTags;
			IAsyncResult result = d.BeginInvoke(photoList, callBack, state);
			return result;
		}

		public Collection<PhotoTag> EndGetTags(IAsyncResult result)
		{
			var d = (GetTagsDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous SendNotification

		public IAsyncResult BeginSendNotification(string markup, string toList, string email, AsyncCallback callBack,
		                                          Object state)
		{
			SendNotificationDelegate d = SendNotification;
			IAsyncResult result = d.BeginInvoke(markup, toList, email, callBack, state);
			return result;
		}

		public string EndSendNotification(IAsyncResult result)
		{
			var d = (SendNotificationDelegate) ((AsyncResult) result).AsyncDelegate;
			return d.EndInvoke(result);
		}

		#endregion

		#region Asynchronous UploadPhoto

		public IAsyncResult BeginUploadPhoto(string albumId, FileInfo uploadFile, AsyncCallback callBack, Object state)
		{
			UploadPhotoDelegate1 d = UploadPhoto;
			IAsyncResult result = d.BeginInvoke(albumId, uploadFile, callBack, state);
			return result;
		}

		public IAsyncResult BeginUploadPhoto(string albumId, FileInfo uploadFile, string caption, AsyncCallback callBack,
		                                     Object state)
		{
			UploadPhotoDelegate2 d = UploadPhoto;
			IAsyncResult result = d.BeginInvoke(albumId, uploadFile, caption, callBack, state);
			return result;
		}

		public UploadPhotoResult EndUploadPhoto(IAsyncResult result)
		{
			Type delegateType = (((AsyncResult) result).AsyncDelegate).GetType();
			if (delegateType == typeof (UploadPhotoDelegate1))
			{
				var d = (UploadPhotoDelegate1) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
			else
			{
				var d = (UploadPhotoDelegate2) ((AsyncResult) result).AsyncDelegate;
				return d.EndInvoke(result);
			}
		}

		#endregion

		#region Asynchronous CreateAlbum

		public IAsyncResult BeginCreateAlbum(string name, string location, string description, AsyncCallback callBack,
		                                     Object state)
		{
			CreateAlbumDelegate d = CreateAlbum;
			IAsyncResult result = d.BeginInvoke(name, location, description, callBack, state);
			return result;
		}

		public void EndCreateAlbum(IAsyncResult result)
		{
			var d = (CreateAlbumDelegate) ((AsyncResult) result).AsyncDelegate;
			d.EndInvoke(result);
		}

		#endregion

		#region Nested type: AddTagDelegate

		private delegate void AddTagDelegate(string photoId, string tagText, string tagUserId, int xCoord, int yCoord);

		#endregion

		#region Nested type: AreFriendsDelegate1

		private delegate bool AreFriendsDelegate1(string userId1, string userId2);

		#endregion

		#region Nested type: AreFriendsDelegate2

		private delegate bool AreFriendsDelegate2(User userId1, User userId2);

		#endregion

		#region Nested type: ConnectToFacebookDelegate

		private delegate void ConnectToFacebookDelegate();

		#endregion

		#region Nested type: CreateAlbumDelegate

		private delegate Album CreateAlbumDelegate(string name, string location, string description);

		#endregion

		#region Nested type: GetEventMembersDelegate

		private delegate Collection<EventUser> GetEventMembersDelegate(string eventId);

		#endregion

		#region Nested type: GetEventsDelegate1

		private delegate Collection<FacebookEvent> GetEventsDelegate1();

		#endregion

		#region Nested type: GetEventsDelegate2

		private delegate Collection<FacebookEvent> GetEventsDelegate2(string userId);

		#endregion

		#region Nested type: GetEventsDelegate3

		private delegate Collection<FacebookEvent> GetEventsDelegate3(Collection<string> eventList);

		#endregion

		#region Nested type: GetEventsDelegate4

		private delegate Collection<FacebookEvent> GetEventsDelegate4(Collection<string> eventList, string userId);

		#endregion

		#region Nested type: GetFriendsAppUsersDelegate

		private delegate Collection<User> GetFriendsAppUsersDelegate();

		#endregion

		#region Nested type: GetFriendsDelegate

		private delegate Collection<User> GetFriendsDelegate();

		#endregion

		#region Nested type: GetFriendsNonAppUsersDelegate

		private delegate Collection<User> GetFriendsNonAppUsersDelegate();

		#endregion

		#region Nested type: GetGroupMembersDelegate

		private delegate Collection<GroupUser> GetGroupMembersDelegate(string groupId);

		#endregion

		#region Nested type: GetGroupsDelegate1

		private delegate Collection<Group> GetGroupsDelegate1();

		#endregion

		#region Nested type: GetGroupsDelegate2

		private delegate Collection<Group> GetGroupsDelegate2(string userId);

		#endregion

		#region Nested type: GetGroupsDelegate3

		private delegate Collection<Group> GetGroupsDelegate3(Collection<string> groupsList);

		#endregion

		#region Nested type: GetGroupsDelegate4

		private delegate Collection<Group> GetGroupsDelegate4(string userId, Collection<string> groupsList);

		#endregion

		#region Nested type: GetLoggedInUserDelegate

		private delegate string GetLoggedInUserDelegate();

		#endregion

		#region Nested type: GetNotificationsDelegate

		private delegate Notifications GetNotificationsDelegate();

		#endregion

		#region Nested type: GetPhotoAlbumsDelegate1

		private delegate Collection<Album> GetPhotoAlbumsDelegate1();

		#endregion

		#region Nested type: GetPhotoAlbumsDelegate2

		private delegate Collection<Album> GetPhotoAlbumsDelegate2(string userId);

		#endregion

		#region Nested type: GetPhotoAlbumsDelegate3

		private delegate Collection<Album> GetPhotoAlbumsDelegate3(Collection<string> albumList);

		#endregion

		#region Nested type: GetPhotoAlbumsDelegate4

		private delegate Collection<Album> GetPhotoAlbumsDelegate4(string userId, Collection<string> albumList);

		#endregion

		#region Nested type: GetPhotosDelegate1

		private delegate Collection<Photo> GetPhotosDelegate1(string albumId);

		#endregion

		#region Nested type: GetPhotosDelegate2

		private delegate Collection<Photo> GetPhotosDelegate2(Collection<string> photoList);

		#endregion

		#region Nested type: GetPhotosDelegate3

		private delegate Collection<Photo> GetPhotosDelegate3(User user);

		#endregion

		#region Nested type: GetPhotosDelegate4

		private delegate Collection<Photo> GetPhotosDelegate4(string albumId, Collection<string> photoList);

		#endregion

		#region Nested type: GetPhotosDelegate5

		private delegate Collection<Photo> GetPhotosDelegate5(string albumId, Collection<string> photoList, User user);

		#endregion

		#region Nested type: GetTagsDelegate

		private delegate Collection<PhotoTag> GetTagsDelegate(Collection<string> photoList);

		#endregion

		#region Nested type: GetUserInfoDelegate1

		private delegate User GetUserInfoDelegate1();

		#endregion

		#region Nested type: GetUserInfoDelegate2

		private delegate Collection<User> GetUserInfoDelegate2(string userIds);

		#endregion

		#region Nested type: GetUserInfoDelegate3

		private delegate Collection<User> GetUserInfoDelegate3(Collection<string> userIds);

		#endregion

		#region Nested type: PublishActionDelegate1

		private delegate string PublishActionDelegate1(string title, string body, Collection<PublishImage> images);

		#endregion

		#region Nested type: PublishActionDelegate2

		private delegate string PublishActionDelegate2(
			string title, string body, Collection<PublishImage> images, int priority);

		#endregion

		#region Nested type: PublishStoryDelegate1

		private delegate string PublishStoryDelegate1(string title, string body, Collection<PublishImage> images);

		#endregion

		#region Nested type: PublishStoryDelegate2

		private delegate string PublishStoryDelegate2(string title, string body, Collection<PublishImage> images, int priority
			);

		#endregion

		#region Nested type: PublishTemplatizedActionDelegate1

		private delegate string PublishTemplatizedActionDelegate1(string titleTemplate, Dictionary<string, string> titleData,
		                                                          string bodyTemplate, Dictionary<string, string> bodyData,
		                                                          Collection<PublishImage> images);

		#endregion

		#region Nested type: PublishTemplatizedActionDelegate2

		private delegate string PublishTemplatizedActionDelegate2(string titleTemplate, Dictionary<string, string> titleData,
		                                                          string bodyTemplate, Dictionary<string, string> bodyData,
		                                                          string bodyGeneral, Collection<string> targetIds,
		                                                          Collection<PublishImage> images);

		#endregion

		#region Nested type: SendNotificationDelegate

		private delegate string SendNotificationDelegate(string markup, string toList, string email);

		#endregion

		#region Nested type: UploadPhotoDelegate1

		private delegate UploadPhotoResult UploadPhotoDelegate1(string albumId, FileInfo uploadFile);

		#endregion

		#region Nested type: UploadPhotoDelegate2

		private delegate UploadPhotoResult UploadPhotoDelegate2(string albumId, FileInfo uploadFile, string caption);

		#endregion
	}
}