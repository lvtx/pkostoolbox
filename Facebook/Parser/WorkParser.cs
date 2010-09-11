using System;
using System.Collections.ObjectModel;
using System.Xml;
using Facebook.Entity;
using Facebook.Types;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class WorkParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a collection of work objects given the xml returned from facebook
		/// </summary>
		internal static Collection<Work> ParseWorkHistory(XmlNode node)
		{
			var workHistoryList = new Collection<Work>();

			if (node != null)
			{
				foreach (XmlNode workNode in ((XmlElement) node).GetElementsByTagName("work_info"))
				{
					workHistoryList.Add(ParseWork(workNode));
				}
			}
			return workHistoryList;
		}

		/// <summary>
		/// Uses DOM parsing to constitute a work data object given the xml returned from facebook
		/// </summary>
		public static Work ParseWork(XmlNode node)
		{
			var work = new Work();
			if (node != null)
			{
				work.Location = LocationParser.ParseLocation(((XmlElement) node).GetElementsByTagName("location")[0]);
				work.CompanyName = XmlHelper.GetNodeText(node, "company_name");
				work.Position = XmlHelper.GetNodeText(node, "position");
				work.Description = XmlHelper.GetNodeText(node, "description");
				work.StartDate = DateTime.Parse(XmlHelper.GetNodeText(node, "start_date", Enums.NodeTypes.DateTime));
				work.EndDate = DateTime.Parse(XmlHelper.GetNodeText(node, "end_date", Enums.NodeTypes.DateTime));
			}
			return work;
		}
	}
}