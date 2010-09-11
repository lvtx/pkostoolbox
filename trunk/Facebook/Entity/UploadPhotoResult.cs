namespace Facebook.Entity
{
	public class UploadPhotoResult
	{
		private readonly string _albumId;
		private readonly string _photoId;

		public UploadPhotoResult(string photoId, string albumId)
		{
			_photoId = photoId;
			_albumId = albumId;
		}

		/// <summary>
		/// The id of the uploaded photo
		/// </summary>
		public string PhotoId
		{
			get { return _photoId; }
		}

		/// <summary>
		/// The id of the album the photo was uploaded to.
		/// </summary>
		public string AlbumId
		{
			get { return _albumId; }
		}
	}
}