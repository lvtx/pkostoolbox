using System;
using System.Collections.ObjectModel;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class GroupUserParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a GroupUser data object given the xml returned from facebook
		/// does not populate the User portion of the object.  This is done separately since it is not part of this xml node.
		/// </summary>
		internal static GroupUser ParseGroupUser(XmlNode node)
		{
			var groupUser = new GroupUser();
			if (node != null)
			{
				groupUser.GroupId = XmlHelper.GetNodeText(node, "gid");
				groupUser.UserId = XmlHelper.GetNodeText(node, "uid");

				var positionNodeList = ((XmlElement) node).GetElementsByTagName("positions");

				if (positionNodeList.Count > 0)
				{
					ParsePositions(positionNodeList[0], groupUser.Positions);
				}
				else
				{
					groupUser.Positions.Add(GroupPosition.Member);
				}
			}
			return groupUser;
		}

		/// <summary>
		/// Uses DOM parsing to construct a collection of positions
		/// </summary>
		public static void ParsePositions(XmlNode node, Collection<GroupPosition> positions)
		{
			positions.Add(GroupPosition.Member);
			if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "member_type")))
			{
				positions.Add((GroupPosition) Enum.Parse(typeof (GroupPosition), XmlHelper.GetNodeText(node, "member_type"), true));
			}
		}
	}
}