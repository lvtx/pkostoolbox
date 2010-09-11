using System;
using System.Collections.ObjectModel;

namespace Facebook.Entity
{
	[Serializable]
	public class Notifications
	{
		#region Private Data

		private readonly Collection<string> _eventInvites = new Collection<string>();
		private readonly Collection<string> _friendRequests = new Collection<string>();
		private readonly Collection<string> _groupInvites = new Collection<string>();

		#endregion Private Data

		#region Properties

		/// <summary>
		/// </summary>
		public int UnreadMessageCount { get; set; }

		/// <summary>
		/// </summary>
		public string MostRecentMessageId { get; set; }

		/// <summary>
		/// </summary>
		public int UnreadPokeCount { get; set; }

		/// <summary>
		/// </summary>
		public string MostRecentPokeId { get; set; }

		/// <summary>
		/// </summary>
		public int UnreadShareCount { get; set; }

		/// <summary>
		/// </summary>
		public string MostRecentShareId { get; set; }

		/// <summary>
		/// </summary>
		public Collection<string> FriendRequests
		{
			get { return _friendRequests; }
		}

		/// <summary>
		/// </summary>
		public Collection<string> GroupInvites
		{
			get { return _groupInvites; }
		}

		/// <summary>
		/// </summary>
		public Collection<string> EventInvites
		{
			get { return _eventInvites; }
		}

		#endregion Properties
	}
}