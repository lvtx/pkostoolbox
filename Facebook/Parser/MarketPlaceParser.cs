using System;
using System.Globalization;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class MarketPlaceParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a data object given the xml returned from facebook
		/// </summary>
		internal static MarketPlace ParseMarketPlace(XmlNode node)
		{
			var market = new MarketPlace();
			if (node != null)
			{

			}
			return market;
		}
	}
}