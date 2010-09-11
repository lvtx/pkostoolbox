using System;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class EventUserParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute an EventUser data object given the xml returned from facebook
		/// this does not include the actual user object as that is populated separately
		/// </summary>
		internal static EventUser ParseEventUser(XmlNode node)
		{
			var eventUser = new EventUser();
			if (node != null)
			{
				eventUser.EventId = XmlHelper.GetNodeText(node, "gid");
				eventUser.UserId = XmlHelper.GetNodeText(node, "uid");

				// if we found an rsvp status, populate the enum with the matching value
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "rsvp_status")))
				{
					eventUser.Attending =
						(RSVPStatus) Enum.Parse(typeof (RSVPStatus), XmlHelper.GetNodeText(node, "rsvp_status"), true);
				}
			}
			return eventUser;
		}
	}
}