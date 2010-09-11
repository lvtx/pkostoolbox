namespace Facebook.Entity
{
	public class PublishImage
	{
		#region Private Data

		#endregion Private Data

		#region Properties

		/// <summary>
		/// The url or facebook photoid of the image
		/// </summary>
		public string ImageLocation { get; set; }

		/// <summary>
		/// The url where clicking the image should go
		/// </summary>
		public string ImageLink { get; set; }

		#endregion Properties

		/// <summary>
		/// Default constructor
		/// </summary>
		public PublishImage()
		{
		}

		/// <summary>
		/// constructor
		/// </summary>
		public PublishImage(string imageLocation, string imageLink)
		{
			ImageLocation = imageLocation;
			ImageLink = imageLink;
		}
	}
}