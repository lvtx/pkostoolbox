using System;
using System.Globalization;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class FacebookEventParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute an Event data object given the xml returned from facebook
		/// </summary>
		internal static FacebookEvent ParseEvent(XmlNode node)
		{
			var ev = new FacebookEvent();
			if (node != null)
			{
				ev.EventId = XmlHelper.GetNodeText(node, "eid");
				ev.Name = XmlHelper.GetNodeText(node, "name");
				ev.TagLine = XmlHelper.GetNodeText(node, "tagline");
				ev.NetworkId = XmlHelper.GetNodeText(node, "nid");
				ev.Host = XmlHelper.GetNodeText(node, "host");
				ev.Description = XmlHelper.GetNodeText(node, "description");
				ev.Type = XmlHelper.GetNodeText(node, "event_type");
				ev.SubType = XmlHelper.GetNodeText(node, "event_subtype");
				ev.Location = XmlHelper.GetNodeText(node, "location");
				ev.Creator = XmlHelper.GetNodeText(node, "creator");
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "pic")))
				{
					ev.PictureUrl = new Uri(XmlHelper.GetNodeText(node, "pic"));
				}
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "update_time")))
				{
					ev.UpdateDate =
						DateHelper.ConvertDoubleToDate(double.Parse(XmlHelper.GetNodeText(node, "update_time"),
						                                            CultureInfo.InvariantCulture));
				}
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "start_time")))
				{
					ev.StartDate =
						DateHelper.ConvertDoubleToEventDate(double.Parse(XmlHelper.GetNodeText(node, "start_time"),
						                                                 CultureInfo.InvariantCulture));
				}
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "end_time")))
				{
					ev.EndDate =
						DateHelper.ConvertDoubleToEventDate(double.Parse(XmlHelper.GetNodeText(node, "end_time"),
						                                                 CultureInfo.InvariantCulture));
				}
				ev.Venue = LocationParser.ParseLocation(((XmlElement) node).GetElementsByTagName("venue")[0]);
			}
			return ev;
		}
	}
}