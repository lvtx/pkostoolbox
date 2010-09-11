using System;

namespace Facebook.Entity
{
	[Serializable]
	public class HighSchool
	{
		/// <summary>
		/// The name of the first high school attended
		/// </summary>
		public string HighSchoolOneName { get; set; }

		/// <summary>
		/// The name of the second high school attended
		/// </summary>
		public string HighSchoolTwoName { get; set; }

		/// <summary>
		/// The facebook unique identifier of the first high school attended
		/// </summary>
		public string HighSchoolOneId { get; set; }

		/// <summary>
		/// The facebook unique identifier of the second high school attended
		/// </summary>
		public string HighSchoolTwoId { get; set; }

		/// <summary>
		/// The year this person graduated from high school
		/// </summary>
		public int GraduationYear { get; set; }
	}
}