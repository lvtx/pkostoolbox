using System.Collections.Generic;
using System.Xml;
using Facebook.Entity;
using Facebook.Utility;

namespace Facebook.Parser
{
	internal static class SchoolHistoryParser
	{
		/// <summary>
		/// Uses DOM parsing to constitute a SchoolHistory data object given the xml returned from facebook
		/// This includes populating the high school object and collection of higher education objects
		/// </summary>
		internal static SchoolHistory ParseSchoolHistory(XmlNode highSchoolNode, XmlNode higherEducationNode)
		{
			var schoolHistory = new SchoolHistory {HighSchool = ParseHighSchool(highSchoolNode)};
			ParseHigherEducations(higherEducationNode, schoolHistory.HigherEducation);

			return schoolHistory;
		}

		/// <summary>
		/// Uses DOM parsing to constitute a High School data object given the xml returned from facebook
		/// </summary>
		private static HighSchool ParseHighSchool(XmlNode node)
		{
			var highSchool = new HighSchool
			                 	{
			                 		HighSchoolOneId = XmlHelper.GetNodeText(node, "hs1_id"),
			                 		HighSchoolOneName = XmlHelper.GetNodeText(node, "hs1_name"),
			                 		HighSchoolTwoId = XmlHelper.GetNodeText(node, "hs2_id"),
			                 		HighSchoolTwoName = XmlHelper.GetNodeText(node, "hs2_name")
			                 	};
			int tempInt;
#if NETCF
            try
            {
                tempInt = int.Parse(XmlHelper.GetNodeText(node, "grad_year"));
                highSchool.GraduationYear = tempInt;
            }
            catch
            {
            }
#else
			if (int.TryParse(XmlHelper.GetNodeText(node, "grad_year"), out tempInt))
			{
				highSchool.GraduationYear = tempInt;
			}
#endif
			return highSchool;
		}

		/// <summary>
		/// Uses DOM parsing to constitute a collection of higher education objects given the xml returned from facebook
		/// </summary>
		private static void ParseHigherEducations(XmlNode node, ICollection<HigherEducation> higherEducationList)
		{
			var xmlElement = node as XmlElement;
			if (xmlElement != null)
				foreach (XmlNode educationInfoNode in xmlElement.GetElementsByTagName("education_info"))
				{
					var higherEducation = new HigherEducation
					                      	{
					                      		School = XmlHelper.GetNodeText(educationInfoNode, "name")
					                      	};
					var year = XmlHelper.GetNodeText(educationInfoNode, "year");

					int tempInt;
#if NETCF
                try
                {
                    tempInt = int.Parse(year);
                    higherEducation.ClassYear = tempInt;
                }
                catch
                {
                }
#else
					if (int.TryParse(year, out tempInt))
					{
						higherEducation.ClassYear = tempInt;
					}
#endif
					ParseConcentrations(((XmlElement) educationInfoNode).GetElementsByTagName("concentrations")[0],
					                    higherEducation.Concentration);
					higherEducationList.Add(higherEducation);
				}
		}

		/// <summary>
		/// Uses DOM parsing to constitute a collection of concentration objects given the xml returned from facebook
		/// </summary>
		private static void ParseConcentrations(XmlNode concentrationsNode, ICollection<string> concentrations)
		{
			foreach (XmlNode concentrationNode in ((XmlElement) concentrationsNode).GetElementsByTagName("concentration"))
			{
				concentrations.Add((concentrationNode).InnerText);
			}
		}
	}
}