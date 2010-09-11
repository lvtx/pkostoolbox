using System;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class LocationParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a Location data object given the xml returned from facebook
		/// </summary>
		internal static Location ParseLocation(XmlNode node)
		{
			var location = new Location();
			if (node != null)
			{
				location.City = XmlHelper.GetNodeText(node, "city");
				location.ZipCode = XmlHelper.GetNodeText(node, "zip");

				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "country")))
				{
					try
					{
						location.Country =
							(Country)
							Enum.Parse(typeof (Country),
							           XmlHelper.GetNodeText(node, "country").Replace(" ", "").Replace(".", "").Replace("'", "").Replace(
							           	"-", ""), true);
					}
					catch (ArgumentException)
					{
					}
				}
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "state")))
				{
					try
					{
						if (location.Country == Country.Canada)
						{
							try
							{
								location.State = (State) Enum.Parse(typeof (State), XmlHelper.GetNodeText(node, "state").Replace(" ", ""), true);
								location.StateAbbreviation = (StateAbbreviation) location.State;
							}
							catch
							{
								location.StateAbbreviation =
									(StateAbbreviation)
									Enum.Parse(typeof (StateAbbreviation), XmlHelper.GetNodeText(node, "state").Replace(" ", ""), true);
								location.State = (State) location.StateAbbreviation;
							}
						}
						else
						{
							location.StateAbbreviation =
								(StateAbbreviation)
								Enum.Parse(typeof (StateAbbreviation), XmlHelper.GetNodeText(node, "state").Replace(" ", ""), true);
							location.State = (State) location.StateAbbreviation;
						}
					}
					catch
					{
					}
				}
			}
			return location;
		}
	}
}