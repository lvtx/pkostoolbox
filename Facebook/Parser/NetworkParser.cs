using System;
using System.Collections.ObjectModel;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class NetworkParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a collection of Networks given the xml returned from facebook
		/// call ParseNetwork in a loop
		/// </summary>
		internal static void ParseNetworks(XmlNode networksNode, Collection<Network> networks)
		{
			if (networksNode == null) return;
			var networkNodes = ((XmlElement) networksNode).GetElementsByTagName("affiliation");
			foreach (XmlNode networkNode in networkNodes)
			{
				networks.Add(ParseNetwork(networkNode));
			}
		}

		/// <summary>
		/// Uses DOM parsing to constitute a Network given the xml returned from facebook
		/// </summary>
		public static Network ParseNetwork(XmlNode node)
		{
			var network = new Network
			              	{
			              		Name = XmlHelper.GetNodeText(node, "name"),
			              		NetworkId = XmlHelper.GetNodeText(node, "nid"),
			              		Status = XmlHelper.GetNodeText(node, "status")
			              	};
			if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "type")))
			{
				network.Type =
					(NetworkType) Enum.Parse(typeof (NetworkType), XmlHelper.GetNodeText(node, "type").Replace(" ", ""), true);
			}
			int tempInt;
#if NETCF
            try
            {
                tempInt = int.Parse(XmlHelper.GetNodeText(node, "year"));
                network.Year = tempInt;
            }
            catch
            {
            }
#else
			if (int.TryParse(XmlHelper.GetNodeText(node, "year"), out tempInt))
			{
				network.Year = tempInt;
			}
#endif
			return network;
		}
	}
}