using System;
using System.Globalization;
using System.Xml;
using Facebook.Entity;
using Facebook.Types;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class PhotoParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a data object given the xml returned from facebook
		/// </summary>
		internal static Photo ParsePhoto(XmlNode node)
		{
			var photo = new Photo();
			if (node != null)
			{
				photo.PhotoId = XmlHelper.GetNodeText(node, "pid");
				photo.AlbumId = XmlHelper.GetNodeText(node, "aid");
				photo.OwnerUserId = XmlHelper.GetNodeText(node, "owner");
				photo.PictureUrl = new Uri(XmlHelper.GetNodeText(node, "src", Enums.NodeTypes.ImageURL));
				photo.PictureSmallUrl = new Uri(XmlHelper.GetNodeText(node, "src_small", Enums.NodeTypes.ImageURL));
				photo.PictureBigUrl = new Uri(XmlHelper.GetNodeText(node, "src_big", Enums.NodeTypes.ImageURL));
				photo.Link = new Uri(XmlHelper.GetNodeText(node, "link"));
				photo.Caption = XmlHelper.GetNodeText(node, "caption");
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "created")))
				{
					photo.CreateDate =
						DateHelper.ConvertDoubleToDate(double.Parse(XmlHelper.GetNodeText(node, "created"), CultureInfo.InvariantCulture));
				}
			}
			return photo;
		}
	}
}