using System;
using System.ComponentModel;

namespace Facebook.Entity
{
	public enum RSVPStatus
	{
#if !NETCF
		[Description("Not Replied")] not_replied,
#endif
		Attending,
		Unsure,
		Declined
	}

	[Serializable]
	public class EventUser
	{
		#region Private Data

		#endregion Private Data

		#region Properties

		/// <summary>
		/// The facebook unique identifier of the event
		/// </summary>
		public string EventId { get; set; }

		/// <summary>
		/// The facebook unique identifier of the user who was invited to the event
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// The user profile object of the user invited to the event
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// Represents the person's commitment to attend the event or not
		/// </summary>
		public RSVPStatus Attending { get; set; }

		#endregion Properties
	}
}