using System;
using System.Globalization;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class PageParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a Photo data object given the xml returned from facebook
		/// </summary>
		internal static Page ParsePage(XmlNode node)
		{
			var page = new Page();
			if (node != null)
			{

			}
			return page;
		}
	}
}