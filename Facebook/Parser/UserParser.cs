using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using Facebook.Entity;
using Facebook.Types;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class UserParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a PhotoTag data object given the xml returned from facebook
		/// </summary>
		internal static User ParseUser(XmlNode node)
		{
			var user = new User();

			if (!Equals(node, null))
			{
				var nodeElement = node as XmlElement;

				user.UserId = XmlHelper.GetNodeText(node, "uid");
				user.FirstName = XmlHelper.GetNodeText(node, "first_name");
				user.LastName = XmlHelper.GetNodeText(node, "last_name");
				user.Name = XmlHelper.GetNodeText(node, "name");
				user.PictureUrl = new Uri(XmlHelper.GetNodeText(node, "pic", Enums.NodeTypes.ImageURL));
				user.PictureSmallUrl = new Uri(XmlHelper.GetNodeText(node, "pic_small", Enums.NodeTypes.ImageURL));
				user.PictureBigUrl = new Uri(XmlHelper.GetNodeText(node, "pic_big", Enums.NodeTypes.ImageURL));
				user.PictureSquareUrl = new Uri(XmlHelper.GetNodeText(node, "pic_square", Enums.NodeTypes.ImageURL));
				user.Religion = XmlHelper.GetNodeText(node, "religion");
				user.Birthday = DateTime.Parse(XmlHelper.GetNodeText(node, "birthday", Enums.NodeTypes.DateTime));
				user.SignificantOtherId = XmlHelper.GetNodeText(node, "significant_other_id");

				user.Activities = XmlHelper.GetNodeText(node, "activities");
				user.Interests = XmlHelper.GetNodeText(node, "interests");
				user.Music = XmlHelper.GetNodeText(node, "music");
				user.TVShows = XmlHelper.GetNodeText(node, "tv");
				user.Movies = XmlHelper.GetNodeText(node, "movies");
				user.Books = XmlHelper.GetNodeText(node, "books");
				user.Quotes = XmlHelper.GetNodeText(node, "quotes");
				user.AboutMe = XmlHelper.GetNodeText(node, "about_me");
				user.NotesCount = int.Parse(XmlHelper.GetNodeText(node, "notes_count", Enums.NodeTypes.Int));
				user.WallCount = int.Parse(XmlHelper.GetNodeText(node, "wall_count", Enums.NodeTypes.Int));


				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "profile_update_time")) &&
				    double.Parse(XmlHelper.GetNodeText(node, "profile_update_time")) > 0)
				{
					user.ProfileUpdateDate =
						DateHelper.ConvertDoubleToDate(double.Parse(XmlHelper.GetNodeText(node, "profile_update_time"),
						                                            CultureInfo.InvariantCulture));
				}

				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "sex")))
				{
					user.Sex = (Gender) Enum.Parse(typeof (Gender), XmlHelper.GetNodeText(node, "sex"), true);
				}

				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "relationship_status")))
				{
					user.RelationshipStatus =
						(RelationshipStatus)
						Enum.Parse(typeof (RelationshipStatus),
						           XmlHelper.GetNodeText(node, "relationship_status").Replace(" ", "").Replace("'", ""), true);
				}

                //if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(node, "political")))
                //{
                //    user.PoliticalView =
                //        (PoliticalView)
                //        Enum.Parse(typeof (PoliticalView), XmlHelper.GetNodeText(node, "political").Replace(" ", ""), true);
                //}

				var statusNodeList = nodeElement.GetElementsByTagName("status");
				user.Status.Message = XmlHelper.GetNodeText(statusNodeList[statusNodeList.Count - 1], "message");
				if (!String.IsNullOrEmpty(XmlHelper.GetNodeText(statusNodeList[statusNodeList.Count - 1], "time")))
				{
					user.Status.Time =
						DateHelper.ConvertDoubleToDate(double.Parse(
						                               	XmlHelper.GetNodeText(statusNodeList[statusNodeList.Count - 1], "time"),
						                               	CultureInfo.InvariantCulture));
				}

				//affiliations
				NetworkParser.ParseNetworks(nodeElement.GetElementsByTagName("affiliations")[0], user.Affiliations);

				//meeting_sex
				user.InterestedInGenders = ParseInterestedInGenders(nodeElement.GetElementsByTagName("meeting_sex")[0]);

				//interested_in
				user.InterstedInRelationshipTypes = ParseRelationshipTypes(nodeElement.GetElementsByTagName("meeting_for")[0]);

				//hometown_location
				user.HometownLocation = LocationParser.ParseLocation(nodeElement.GetElementsByTagName("hometown_location")[0]);

				//curent_location
				user.CurrentLocation = LocationParser.ParseLocation(nodeElement.GetElementsByTagName("current_location")[0]);

				//school_history
				user.SchoolHistory = SchoolHistoryParser.ParseSchoolHistory(nodeElement.GetElementsByTagName("hs_info")[0],
																			nodeElement.GetElementsByTagName("education_history")[0]);

				//work_history
				user.WorkHistory = WorkParser.ParseWorkHistory(nodeElement.GetElementsByTagName("work_history")[0]);
			}
			return user;
		}

		/// <summary>
		/// Uses DOM parsing to constitute a collection of relationshiptype object given the xml returned from facebook
		/// </summary>
		private static Collection<LookingFor> ParseRelationshipTypes(XmlNode node)
		{
			var relationshipTypeList = new Collection<LookingFor>();

			foreach (XmlNode seekingNode in ((XmlElement) node).GetElementsByTagName("seeking"))
			{
				try
				{
					relationshipTypeList.Add(
						(LookingFor) Enum.Parse(typeof (LookingFor), (seekingNode).InnerText.Replace(" ", "").Replace("'", ""), true));
				}
				catch (ArgumentException)
				{
					// if there was no enum for this relationship type, we set it to Unknown
					relationshipTypeList.Add(LookingFor.Unknown);
				}
			}
			return relationshipTypeList;
		}

		/// <summary>
		/// Uses DOM parsing to constitute a collection of genderlist object given the xml returned from facebook
		/// </summary>
		private static Collection<Gender> ParseInterestedInGenders(XmlNode node)
		{
			var genderList = new Collection<Gender>();

			foreach (XmlNode sexNode in ((XmlElement) node).GetElementsByTagName("sex"))
			{
				genderList.Add((Gender) Enum.Parse(typeof (Gender), (sexNode).InnerText, true));
			}
			return genderList;
		}
	}
}