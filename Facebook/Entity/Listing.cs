using System;
using System.Drawing;
using System.Net;
using System.Xml.Serialization;
using Facebook.Properties;
using Facebook.Utility;

namespace Facebook.Entity
{
	[Serializable]
	public class Listing
	{
		#region Private Data

		private string _privateProperty;

		#endregion Private Data

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public string AutoTypedProperty { get; set; }

		#endregion Properties
	}
}
