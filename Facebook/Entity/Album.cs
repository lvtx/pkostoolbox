using System;

namespace Facebook.Entity
{
	[Serializable]
	public class Album
	{
		#region Private Data

		#endregion Private Data

		#region Properties

		/// <summary>
		/// The facebook unique identifier of the album
		/// </summary>
		public string AlbumId { get; set; }

		/// <summary>
		/// The facebook unique identifier of the photo that is the cover photo for this album
		/// </summary>
		public string CoverPhotoId { get; set; }

		/// <summary>
		/// The facebook unique identifier of the user that created the album
		/// </summary>
		public string OwnerUserId { get; set; }

		/// <summary>
		/// The name of the album
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The date the album was created
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// The date the album was last updated
		/// </summary>
		public DateTime ModifiedDate { get; set; }

		/// <summary>
		/// The description of the album
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The location where the pictures took place
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// The number of pictures in the album
		/// </summary>
		public int Size { get; set; }

		#endregion Properties
	}
}