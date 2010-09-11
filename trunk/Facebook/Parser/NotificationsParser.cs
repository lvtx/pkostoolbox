using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class NotificationsParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a collection of work objects given the xml returned from facebook
		/// </summary>
		internal static Notifications ParseNotifications(XmlNode node)
		{
			var notifications = new Notifications();

			if (node != null)
			{
				XmlNode messagesNode = ((XmlElement) node).GetElementsByTagName("messages")[0];
				notifications.UnreadMessageCount = int.Parse(XmlHelper.GetNodeText(messagesNode, "unread"));
				notifications.MostRecentMessageId = XmlHelper.GetNodeText(messagesNode, "most_recent");

				XmlNode pokesNode = ((XmlElement) node).GetElementsByTagName("pokes")[0];
				notifications.UnreadPokeCount = int.Parse(XmlHelper.GetNodeText(pokesNode, "unread"));
				notifications.MostRecentPokeId = XmlHelper.GetNodeText(pokesNode, "most_recent");

				XmlNode sharesNode = ((XmlElement) node).GetElementsByTagName("shares")[0];
				notifications.UnreadShareCount = int.Parse(XmlHelper.GetNodeText(sharesNode, "unread"));
				notifications.MostRecentShareId = XmlHelper.GetNodeText(sharesNode, "most_recent");

				XmlNode friendRequestsNode = ((XmlElement) node).GetElementsByTagName("friend_requests")[0];
				foreach (XmlNode friendNode in ((XmlElement) friendRequestsNode).GetElementsByTagName("uid"))
				{
					notifications.FriendRequests.Add((friendNode).InnerText);
				}

				XmlNode groupInvitesNode = ((XmlElement) node).GetElementsByTagName("group_invites")[0];
				foreach (XmlNode groupNode in ((XmlElement) groupInvitesNode).GetElementsByTagName("gid"))
				{
					notifications.GroupInvites.Add((groupNode).InnerText);
				}

				XmlNode eventInvitesNode = ((XmlElement) node).GetElementsByTagName("event_invites")[0];
				foreach (XmlNode eventNode in ((XmlElement) eventInvitesNode).GetElementsByTagName("eid"))
				{
					notifications.EventInvites.Add((eventNode).InnerText);
				}
			}
			return notifications;
		}
	}
}