using System;
using System.Globalization;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class GroupParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a Group data object given the xml returned from facebook
		/// </summary>
		internal static Group ParseGroup(XmlNode node)
		{
			var group = new Group();
			if (node != null)
			{
				group.GroupId = XmlHelper.GetNodeText(node, "gid");
				group.Name = XmlHelper.GetNodeText(node, "name");
				group.NetworkId = XmlHelper.GetNodeText(node, "nid");
				group.Type = XmlHelper.GetNodeText(node, "group_type");
				group.SubType = XmlHelper.GetNodeText(node, "group_subtype");
				group.RecentNews = XmlHelper.GetNodeText(node, "recent_news");
				group.Description = XmlHelper.GetNodeText(node, "description");

				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "pic")))
				{
					group.PictureUrl = new Uri(XmlHelper.GetNodeText(node, "pic"));
				}
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "pic_big")))
				{
					group.PictureBigUrl = new Uri(XmlHelper.GetNodeText(node, "pic_big"));
				}
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "pic_small")))
				{
					group.PictureSmallUrl = new Uri(XmlHelper.GetNodeText(node, "pic_small"));
				}
				group.Creator = XmlHelper.GetNodeText(node, "creator");
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "update_time")))
				{
					group.UpdateDate =
						DateHelper.ConvertDoubleToDate(double.Parse(XmlHelper.GetNodeText(node, "update_time"),
						                                            CultureInfo.InvariantCulture));
				}
				group.Office = XmlHelper.GetNodeText(node, "office");
				group.WebSite = XmlHelper.GetNodeText(node, "website");
				group.Venue = LocationParser.ParseLocation(((XmlElement) node).GetElementsByTagName("venue")[0]);
			}
			return group;
		}
	}
}