using System;
using System.Drawing;
using System.Net;
using System.Xml.Serialization;
using Facebook.Properties;
using Facebook.Utility;

namespace Facebook.Entity
{
	[Serializable]
	public class FacebookEvent
	{
		#region Private Data

		private Image _picture;
		private byte[] _pictureBytes;
		private Uri _pictureUrl;

		#endregion Private Data

		#region Properties

		/// <summary>
		/// The facebook unique identifier of the event
		/// </summary>
		public string EventId { get; set; }

		/// <summary>
		/// The name of the event
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The event's tagline
		/// </summary>
		public string TagLine { get; set; }

		/// <summary>
		/// The facebook unique identifier of the network this event is affiliated with
		/// </summary>
		public string NetworkId { get; set; }

		/// <summary>
		/// The picture of the event.  This is not initially populated, but when accessed will stream the bytes of the picture
		/// from the url and provide an actual picture
		/// </summary>
		[XmlIgnore]
		public Image Picture
		{
			get
			{
				if (_pictureUrl == null)
				{
					return Resources.missingPicture;
				}
				else if (_picture == null)
				{
					var webClient = new WebClient();
					_pictureBytes = webClient.DownloadData(_pictureUrl);
					_picture = ImageHelper.ConvertBytesToImage(_pictureBytes);
					return _picture;
				}
				else
				{
					return _picture;
				}
			}
			set { _picture = value; }
		}

		/// <summary>
		/// This is only used for serialization.  Should not be accessed directly.
		/// </summary>
		[XmlElement("Picture")]
		public byte[] PictureBytes
		{
			get
			{
				if (_pictureUrl == null)
				{
					return null;
				}
				else if (_pictureBytes == null)
				{
					var webClient = new WebClient();
					_pictureBytes = webClient.DownloadData(_pictureUrl);
					_picture = ImageHelper.ConvertBytesToImage(_pictureBytes);
					return _pictureBytes;
				}
				else
				{
					return _pictureBytes;
				}
			}
		}

		/// <summary>
		/// The url of the event picture
		/// </summary>
		public Uri PictureUrl
		{
			get
			{
				if (_pictureUrl == null)
				{
					return new Uri(Resources.MissingPictureUrl);
				}
				else
				{
					return _pictureUrl;
				}
			}
			set { _pictureUrl = value; }
		}

		/// <summary>
		/// The name of the event host
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// The description of the event
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The type of the event
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// The sub-type of the event
		/// </summary>
		public string SubType { get; set; }

		/// <summary>
		/// The starting date of the event
		/// </summary>
		public DateTime StartDate { get; set; }

		/// <summary>
		/// The ending date of the event
		/// </summary>
		public DateTime EndDate { get; set; }

		/// <summary>
		/// The creator of the event
		/// </summary>
		public string Creator { get; set; }

		/// <summary>
		/// The last time the event was updated
		/// </summary>
		public DateTime UpdateDate { get; set; }

		/// <summary>
		/// The location of the event
		/// </summary>
		public string Location { get; set; }

		/// <summary>
		/// The venue of the event
		/// </summary>
		public Location Venue { get; set; }

		#endregion Properties
	}
}