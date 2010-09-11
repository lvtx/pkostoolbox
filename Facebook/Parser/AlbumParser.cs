using System;
using System.Globalization;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class AlbumParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute an Album data object given the xml returned from facebook
		/// representing the album
		/// </summary>
		internal static Album ParseAlbum(XmlNode node)
		{
			var album = new Album();
			if (node != null)
			{
				album.AlbumId = XmlHelper.GetNodeText(node, "aid");
				album.CoverPhotoId = XmlHelper.GetNodeText(node, "cover_pid");
				album.OwnerUserId = XmlHelper.GetNodeText(node, "owner");
				album.Name = XmlHelper.GetNodeText(node, "name");
				album.Description = XmlHelper.GetNodeText(node, "description");
				album.Location = XmlHelper.GetNodeText(node, "location");

				// Dates are not returned as dates, but as seconds since 1970, so we need to convert if the field is populated
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "created")))
				{
					album.CreateDate =
						DateHelper.ConvertDoubleToDate(
							double.Parse(XmlHelper.GetNodeText(node, "created"), CultureInfo.InvariantCulture));
				}

				// Dates are not returned as dates, but as seconds since 1970, so we need to convert if the field is populated
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "modified")))
				{
					album.ModifiedDate =
						DateHelper.ConvertDoubleToDate(
							double.Parse(XmlHelper.GetNodeText(node, "modified"), CultureInfo.InvariantCulture));
				}
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "size")))
				{
					album.Size =
						int.Parse(XmlHelper.GetNodeText(node, "size"), CultureInfo.InvariantCulture);
				}
			}
			return album;
		}
	}
}