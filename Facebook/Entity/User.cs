using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Xml.Serialization;
using Facebook.Properties;
using Facebook.Utility;

namespace Facebook.Entity
{
	public enum Gender
	{
		Unknown,
		Male,
		Female
	}

	public enum LookingFor
	{
		Unknown,
		Friendship,
		ARelationship,
		Dating,
		RandomPlay,
		WhateverIcanget,
		Networking
	}

	public enum RelationshipStatus
	{
		Unknown,
		Single,
		InARelationship,
		InAnOpenRelationship,
		Engaged,
		Married,
		ItsComplicated
	}

	public enum PoliticalView
	{
		Unknown,
		VeryLiberal,
		Liberal,
		Moderate,
		Conservative,
		VeryConservative,
		Apathetic,
		Libertarian,
		Other
	}

	[Serializable]
	public class User
	{
		#region Private Data

		private readonly Status _status = new Status();
		private Collection<Network> _affiliations;
		private Image _picture;
		private Image _pictureBig;
		private byte[] _pictureBigBytes;
		private Uri _pictureBigUrl;
		private byte[] _pictureBytes;
		private Image _pictureSmall;
		private byte[] _pictureSmallBytes;
		private Uri _pictureSmallUrl;
		private Image _pictureSquare;
		private byte[] _pictureSquareBytes;
		private Uri _pictureSquareUrl;
		private Uri _pictureUrl;

		#endregion Private Data

		#region Properties

		/// <summary>
		/// Facebook unique identifier of the user
		/// </summary>
		public string UserId { get; set; }

		/// <summary>
		/// User's first name
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// User's last name
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// User's name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// User's birthday
		/// </summary>
		public DateTime? Birthday { get; set; }

		/// <summary>
		/// User's birthday
		/// </summary>
		public DateTime ProfileUpdateDate { get; set; }

		/// <summary>
		/// Free form text of music this user likes
		/// </summary>
		public string Music { get; set; }

		/// <summary>
		/// Free form text of activities this user does
		/// </summary>
		public string Activities { get; set; }

		/// <summary>
		/// Free form text of this user's interests
		/// </summary>
		public string Interests { get; set; }

		/// <summary>
		/// Free form text of this user's favorite tv shows
		/// </summary>
		public string TVShows { get; set; }

		/// <summary>
		/// Free form text of this user's favorite movies
		/// </summary>
		public string Movies { get; set; }

		/// <summary>
		/// Free form text of this user's favorite books
		/// </summary>
		public string Books { get; set; }

		/// <summary>
		/// Free form text of this user's favorite quotes
		/// </summary>
		public string Quotes { get; set; }

		/// <summary>
		/// Free form text describing this user
		/// </summary>
		public string AboutMe { get; set; }


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
		/// Will stream the bytes of the picture from the url and provide an actual picture.
		/// Default is image of <see cref="Resources.missingPicture"/>
		/// </summary>
		/// <remarks>Never returns a null object.</remarks>
		[XmlIgnore]
		public Image PictureSquare
		{
			get { return _pictureSquare = ImageHelper.GetImage(_pictureSquareUrl, _pictureSquare, out _pictureSquareBytes); }
			set { _pictureSquare = value ?? Resources.missingPicture; }
		}

		/// <summary>
		/// This is only used for serialization.  Should not be accessed directly.
		/// </summary>
		[XmlElement("PictureSquare")]
		public byte[] PictureSquareBytes
		{
			get { return _pictureSquareBytes = ImageHelper.GetBytes(_pictureSquareUrl, out _pictureSquare, _pictureSquareBytes); }
		}

		/// <summary>
		/// The URL of the Image. Default is value of <see cref="Resources.MissingPictureUrl"/>.
		/// </summary>
		/// <remarks>Never returns a null object.</remarks>
		public Uri PictureSquareUrl
		{
			get { return _pictureSquareUrl ?? new Uri(Resources.MissingPictureUrl); }
			set { _pictureSquareUrl = value ?? new Uri(Resources.MissingPictureUrl); }
		}

		/// <summary>
		/// Collection of networks this person is affiliated with
		/// </summary>
		public Collection<Network> Affiliations
		{
			get
			{
				if (_affiliations == null)
					_affiliations = new Collection<Network>();

				return _affiliations;
			}
		}

		/// <summary>
		/// user's gender
		/// </summary>
		public Gender Sex { get; set; }

		/// <summary>
		/// user's hometown
		/// </summary>
		public Location HometownLocation { get; set; }

		/// <summary>
		/// user's current location
		/// </summary>
		public Location CurrentLocation { get; set; }

		/// <summary>
		/// collection of genders this user is interested in
		/// </summary>
		public Collection<Gender> InterestedInGenders { get; set; }

		/// <summary>
		/// collection of relationship types this user is interested in
		/// </summary>
		public Collection<LookingFor> InterstedInRelationshipTypes { get; set; }

		/// <summary>
		/// user's relationship status
		/// </summary>
		public RelationshipStatus RelationshipStatus { get; set; }

		/// <summary>
		/// user's political view
		/// </summary>
		public PoliticalView PoliticalView { get; set; }

		/// <summary>
		/// facebook unique identifier of the significant other of this user
		/// </summary>
		public string SignificantOtherId { get; set; }

		/// <summary>
		/// user's school history
		/// </summary>
		public SchoolHistory SchoolHistory { get; set; }

		/// <summary>
		/// user's work history
		/// </summary>
		public Collection<Work> WorkHistory { get; set; }

		/// <summary>
		/// user's religion
		/// </summary>
		public string Religion { get; set; }

		/// <summary>
		/// count of notes
		/// </summary>
		public int NotesCount { get; set; }

		/// <summary>
		/// Number of messages on the wall
		/// </summary>
		public int WallCount { get; set; }

		/// <summary>
		/// </summary>
		public Status Status
		{
			get { return _status; }
		}

        public string Email { get; set; }
		#endregion Properties

		public User()
		{
			Movies = string.Empty;
			TVShows = string.Empty;
			Interests = string.Empty;
			Activities = string.Empty;
			Music = string.Empty;
			Name = string.Empty;
			LastName = string.Empty;
			FirstName = string.Empty;
			UserId = string.Empty;
			Books = string.Empty;
			Quotes = string.Empty;
			AboutMe = string.Empty;
			SignificantOtherId = string.Empty;
			Religion = string.Empty;
            Email = string.Empty;
		}
	}
}