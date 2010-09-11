using System;
using System.Xml;
using Facebook.Entity;
using Facebook.Types;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class PhotoTagParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a PhotoTag data object given the xml returned from facebook
		/// </summary>
		internal static PhotoTag ParsePhotoTag(XmlNode node)
		{
			var photoTag = new PhotoTag();
			if (node != null)
			{
				photoTag.PhotoId = XmlHelper.GetNodeText(node, "pid");
				photoTag.SubjectUserId = XmlHelper.GetNodeText(node, "subject");
				photoTag.XCoord = double.Parse(XmlHelper.GetNodeText(node, "xcoord", Enums.NodeTypes.Double));
				photoTag.YCoord = double.Parse(XmlHelper.GetNodeText(node, "ycoord", Enums.NodeTypes.Double));
				photoTag.Created = DateTime.Parse(XmlHelper.GetNodeText(node, "created", Enums.NodeTypes.DateTime));
			}
			return photoTag;
		}
	}
}