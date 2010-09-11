using System;
using System.Drawing;
using System.Xml.Serialization;
using Facebook.Properties;
using Facebook.Utility;

namespace Facebook.Entity
{
	[Serializable]
	public class Photo
	{
		private Image _pictureBig;
		private byte[] _pictureBigBytes;
		private Uri _pictureBigUrl;

		private Image _picture;
		private byte[] _pictureBytes;
		private Uri _pictureUrl;

		private Image _pictureSmall;
		private byte[] _pictureSmallBytes;
		private Uri _pictureSmallUrl;

		/// <summary>
		/// The facebook unique identifier of the photo
		/// </summary>
		public string PhotoId { get; set; }

		/// <summary>
		/// The facebook unique identifier of the album that this photo is part of
		/// </summary>
		public string AlbumId { get; set; }

		/// <summary>
		/// The facebook unique identifier of the user who owns the picture and album
		/// </summary>
		public string OwnerUserId { get; set; }

		/// <summary>
		/// The facebook unique identifier of the user who owns the picture and album
		/// </summary>
		public Uri Link { get; set; }

		/// <summary>
		/// Will stream the bytes of the picture from the url and provide an actual picture.
		/// Default is image of <see cref="Resources.missingPicture"/>
		/// </summary>
		/// <remarks>Never returns a null object.</remarks>
		[XmlIgnore]
		public Image Picture
		{
			get { return _picture = ImageHelper.GetImage(_pictureUrl, _picture, out _pictureBytes); }
			set { _picture = value ?? Resources.missingPicture; }
		}

		/// <summary>
		/// This is only used for serialization.  Should not be accessed directly.
		/// </summary>
		[XmlElement("Picture")]
		public byte[] PictureBytes
		{
			get { return _pictureBytes = ImageHelper.GetBytes(_pictureUrl, out _picture, _pictureBytes); }
		}

		/// <summary>
		/// The URL of the Image. Default is value of <see cref="Resources.MissingPictureUrl"/>.
		/// </summary>
		/// <remarks>Never returns a null object.</remarks>
		public Uri PictureUrl
		{
			get { return _pictureUrl ?? new Uri(Resources.MissingPictureUrl); }
			set { _pictureUrl = value ?? new Uri(Resources.MissingPictureUrl); }
		}


		/// <summary>
		/// Will stream the bytes of the picture from the url and provide an actual picture.
		/// Default is image of <see cref="Resources.missingPicture"/>
		/// </summary>
		/// <remarks>Never returns a null object.</remarks>
		[XmlIgnore]
		public Image PictureSmall
		{
			get { return _pictureSmall = ImageHelper.GetImage(_pictureSmallUrl, _pictureSmall, out _pictureSmallBytes); }
			set { _pictureSmall = value ?? Resources.missingPicture; }
		}

		/// <summary>
		/// This is only used for serialization.  Should not be accessed directly.
		/// </summary>
		[XmlElement("PictureSmall")]
		public byte[] PictureSmallBytes
		{
			get { return _pictureSmallBytes = ImageHelper.GetBytes(_pictureSmallUrl, out _pictureSmall, _pictureSmallBytes); }
		}

		/// <summary>
		/// The URL of the Image. Default is value of <see cref="Resources.MissingPictureUrl"/>.
		/// </summary>
		/// <remarks>Never returns a null object.</remarks>
		public Uri PictureSmallUrl
		{
			get { return _pictureSmallUrl ?? new Uri(Resources.MissingPictureUrl); }
			set { _pictureSmallUrl = value ?? new Uri(Resources.MissingPictureUrl); }
		}


		/// <summary>
		/// Will stream the bytes of the picture from the url and provide an actual picture.
		/// Default is image of <see cref="Resources.missingPicture"/>
		/// </summary>
		/// <remarks>Never returns a null object.</remarks>
		[XmlIgnore]
		public Image PictureBig
		{
			get { return _pictureBig = ImageHelper.GetImage(_pictureBigUrl, _pictureBig, out _pictureBigBytes); }
			set { _pictureBig = value ?? Resources.missingPicture; }
		}

		/// <summary>
		/// This is only used for serialization.  Should not be accessed directly.
		/// </summary>
		[XmlElement("PictureBig")]
		public byte[] PictureBigBytes
		{
			get { return _pictureBigBytes = ImageHelper.GetBytes(_pictureBigUrl, out _pictureBig, _pictureBigBytes); }
		}

		/// <summary>
		/// The URL of the Image. Default is value of <see cref="Resources.MissingPictureUrl"/>.
		/// </summary>
		/// <remarks>Never returns a null object.</remarks>
		public Uri PictureBigUrl
		{
			get { return _pictureBigUrl ?? new Uri(Resources.MissingPictureUrl); }
			set { _pictureBigUrl = value ?? new Uri(Resources.MissingPictureUrl); }
		}

		/// <summary>
		/// The caption associated with the picture
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// The date picture was created
		/// </summary>
		public DateTime CreateDate { get; set; }
	}
}