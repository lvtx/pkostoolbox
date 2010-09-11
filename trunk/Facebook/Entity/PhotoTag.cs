using System;

namespace Facebook.Entity
{
	[Serializable]
	public class PhotoTag
	{
		#region Private Data

		#endregion Private Data

		#region Properties

		/// <summary>
		/// The facebook unique identifier of the photo
		/// </summary>
		public string PhotoId { get; set; }

		/// <summary>
		/// The facebook unique identifier of the user the tag points to
		/// </summary>
		public string SubjectUserId { get; set; }

		/// <summary>
		/// The x coordinate within the picture of the location of the tag
		/// </summary>
		public double XCoord { get; set; }

		/// <summary>
		/// The y coordinate within the picture of the location of the tag
		/// </summary>
		public double YCoord { get; set ; }

		/// <summary>
		/// The y coordinate within the picture of the location of the tag
		/// </summary>
		public DateTime Created { get; set; }

		#endregion Properties
	}
}