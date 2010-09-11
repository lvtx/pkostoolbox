using System;
using System.Collections.ObjectModel;

namespace Facebook.Entity
{
	[Serializable]
	public class HigherEducation
	{
		#region Private Data

		private Collection<string> _concentration;

		#endregion PrivateData

		#region Properties

		/// <summary>
		/// The name of the school
		/// </summary>
		public string School { get; set; }

		/// <summary>
		/// Collection of concentrations
		/// </summary>
		public Collection<string> Concentration
		{
			get
			{
				if (_concentration == null)
					_concentration = new Collection<string>();

				return _concentration;
			}
		}

		/// <summary>
		/// Graduation year
		/// </summary>
		public int ClassYear { get; set; }

		#endregion Properties
	}
}