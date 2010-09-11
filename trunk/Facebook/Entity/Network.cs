using System;

namespace Facebook.Entity
{
	public enum NetworkType
	{
		Unknown,
		College,
		HighSchool,
		Work,
		Region
	}

	[Serializable]
	public class Network
	{
		#region Private Data

		#endregion Private Data

		#region Properties

		/// <summary>
		/// Facebook unique identifier of the network
		/// </summary>
		public string NetworkId { get; set; }

		/// <summary>
		/// The name of the network
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The type of the network (College, High School, Work or Region)
		/// </summary>
		public NetworkType Type { get; set; }

		/// <summary>
		/// The year the network started
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// The status of the network
		/// </summary>
		public string Status { get; set; }

		#endregion Properties
	}
}