using System;

namespace Facebook.Entity
{
	[Serializable]
	public class Work
	{
		/// <summary>
		/// city/state where work took place
		/// </summary>
		public Location Location { get; set; }

		/// <summary>
		/// The name of the company
		/// </summary>
		public string CompanyName { get; set; }

		/// <summary>
		/// The person's job title
		/// </summary>
		public string Position { get; set; }

		/// <summary>
		/// description of job
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// date person started the job
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// date person ended the job
		/// </summary>
		public DateTime EndDate { get; set; }
	}
}