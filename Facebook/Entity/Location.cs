using System;
using System.ComponentModel;
using System.Text;

namespace Facebook.Entity
{
	[Serializable]
	public class Location
	{
		#region Private Data

		private string _city = string.Empty;
		private StateAbbreviation _stateAbbreviation;
		private string _zipCode = string.Empty;

		#endregion Private Data

		#region Properties

		/// <summary>
		/// Name of the city
		/// </summary>
		public string City
		{
			get { return _city; }
			set { _city = value; }
		}

		/// <summary>
		/// State (US and Canada)
		/// </summary>
		public State State { get; set; }

		/// <summary>
		/// State Postal Abbreviation (US and Canada)
		/// </summary>
		public StateAbbreviation StateAbbreviation
		{
			get { return _stateAbbreviation; }
			set { _stateAbbreviation = value; }
		}

		/// <summary>
		/// Country
		/// </summary>
		public Country Country { get; set; }

		/// <summary>
		/// Zip Code
		/// </summary>
		public string ZipCode
		{
			get { return _zipCode; }
			set { _zipCode = value; }
		}

		#endregion Properties

		/// <summary>
		/// Override of ToString();  Used when displaying the location as a field on the user interface
		/// </summary>
		public override string ToString()
		{
			var location = new StringBuilder();
			if (string.IsNullOrEmpty(_city) && _stateAbbreviation == StateAbbreviation.Unknown)
			{
				location.Append(string.Empty);
			}
			else
			{
				location.Append(_city);
				if (_stateAbbreviation != StateAbbreviation.Unknown)
				{
					location.Append(", ");
					location.Append(_stateAbbreviation.ToString());
				}
			}
			return location.ToString();
		}
	}

	/// <summary>
	/// Represents the states 
	/// </summary>
	public enum State
	{
#if !NETCF
		[Description("")]
#endif
		Unknown,
		Alaska,
		Alabama,
		Arkansas,
		Arizona,
		California,
		Colorado,
		Connecticut,
		Delaware,
		Florida,
		Georgia,
		Hawaii,
		Iowa,
		Idaho,
		Illinois,
		Indiana,
		Kansas,
		Kentucky,
		Louisiana,
		Massachusetts,
		Maryland,
		Maine,
		Michigean,
		Minnesota,
		Missouri,
		Mississippi,
		Montana,
		NorthCarolina,
		NorthDakota,
		Nebraska,
		NewHampshire,
		NewJersey,
		NewMexico,
		Nevada,
		NewYork,
		Ohio,
		Oklahoma,
		Oregon,
		Pennslyvania,
		PuertoRico,
		RhodeIsland,
		SouthCarolina,
		SouthDakota,
		Tennesse,
		Texas,
		Utah,
		Virginia,
		Vermont,
		Washington,
		WashingtonDC,
		Wisconsin,
		WestVirginia,
		Wyoming,
		Alberta,
		BritishColumbia,
		Manitoba,
		NewBrunswick,
		Newfoundland,
		NorthwestTerritories,
		NovaScotia,
		Nunavut,
		Ontario,
		PrinceEdwardIsland,
		Quebec,
		Saskatchewan,
		YukonTerritory
	}

	/// <summary>
	/// Represents the abbreviations for states specified in States enum
	/// </summary>
	public enum StateAbbreviation
	{
#if !NETCF
		[Description("")]
#endif
		Unknown,
		AK,
		AL,
		AR,
		AZ,
		CA,
		CO,
		CT,
		DE,
		FL,
		GA,
		HI,
		IA,
		ID,
		IL,
		IN,
		KS,
		KY,
		LA,
		MA,
		MD,
		ME,
		MI,
		MN,
		MO,
		MS,
		MT,
		NC,
		ND,
		NE,
		NH,
		NJ,
		NM,
		NV,
		NY,
		OH,
		OK,
		OR,
		PA,
		PR,
		RI,
		SC,
		SD,
		TN,
		TX,
		UT,
		VA,
		VT,
		WA,
		DC,
		WI,
		WV,
		WY,
		AB,
		BC,
		MB,
		NB,
		NL,
		NT,
		NS,
		NU,
		ON,
		PE,
		QC,
		SK,
		YT
	}

	/// <summary>
	/// Represents the countries 
	/// </summary>
	public enum Country
	{
#if !NETCF
		[Description("")]
#endif
		Unknown,
		UnitedStates,
		Canada,
		England,
		Scotland,
		Wales,
		Abuja,
		Afghanistan,
		Akrotiri,
		Albania,
		Algeria,
		AmericanSamoa,
		Andorra,
		Angola,
		Anguilla,
		Antarctica,
		Antigua,
		APO,
		Argentina,
		Armenia,
		Aruba,
		AshmoreandCartierIslands,
		Australia,
		Austria,
		Azerbaijan,
		Bahrain,
		Bangladesh,
		Barbados,
		Belarus,
		Belgium,
		Belize,
		Benin,
		Bermuda,
		Bhutan,
		Bolivia,
		BosniaandHerzegovina,
		Botswana,
		Brazil,
		BritishVirginIslands,
		Brunei,
		Bulgaria,
		BurkinaFaso,
		Burundi,
		Cambodia,
		Cameroon,
		CapeVerde,
		CaymanIslands,
		CotedIvoire,
		CentralAfricanRepublic,
		Chad,
		ChannelIslands,
		Chile,
		China,
		Colombia,
		Comoros,
		CostaRica,
		Croatia,
		Cuba,
		Curacao,
		Cyprus,
		CzechRepublic,
		DemocraticRepublicCongo,
		Denmark,
		Djibouti,
		Dominica,
		DominicanRepublic,
		EastTimor,
		Ecuador,
		Egypt,
		ElSalvador,
		EquatorialGuinea,
		Eritrea,
		Estonia,
		Ethiopia,
		FalklandIslands,
		FaroeIslands,
		FederatedStatesofMicronesia,
		Fiji,
		Finland,
		France,
		FrenchGuiana,
		FrenchPolynesia,
		Gabon,
		Georgia,
		Germany,
		Ghana,
		Gibraltar,
		Greece,
		Greenland,
		Grenada,
		Guam,
		Guatemala,
		Guinea,
		GuineaBissau,
		Guyana,
		Haiti,
		Honduras,
		HongKong,
		Hungary,
		Iceland,
		India,
		Indonesia,
		Iran,
		Iraq,
		Ireland,
		IsleOfMan,
		Israel,
		Italy,
		Jamaica,
		Japan,
		Jordan,
		Kazakhstan,
		Kenya,
		Kiribati,
		Kuwait,
		Kyrgyzstan,
		Laos,
		Latvia,
		Lebanon,
		Lesotho,
		Liberia,
		Libya,
		Liechtenstein,
		Lithuania,
		Luxembourg,
		Macau,
		Macedonia,
		Madagascar,
		Malawi,
		Malaysia,
		Maldives,
		Mali,
		Malta,
		MarshallIslands,
		Martinique,
		Mauritania,
		Mauritius,
		Mexico,
		Moldova,
		Monaco,
		Mongolia,
		Montenegro,
		Morocco,
		Mozambique,
		Myanmar,
		Namibia,
		Nauru,
		Nepal,
		Netherlands,
		NetherlandsAntilles,
		NewZealand,
		Nicaragua,
		Niger,
		Nigeria,
		NorthKorea,
		NorthernIreland,
		NorthernMarianaIslands,
		Norway,
		Oman,
		Pakistan,
		Palau,
		Palestine,
		Panama,
		PapuaNewGuinea,
		Paraguay,
		Peru,
		Philippines,
		Poland,
		Portugal,
		Qatar,
		RepublicoftheCongo,
		Romania,
		Russia,
		Rwanda,
		SaintKittsandNevis,
		SaintVincentandtheGrenadines,
		Samoa,
		SanMarino,
		SaoTomeandPrincipe,
		SaudiArabia,
		Senegal,
		Serbia,
		Seychelles,
		SierraLeone,
		Singapore,
		Slovakia,
		Slovenia,
		SolomonIslands,
		Somalia,
		SouthAfrica,
		SouthKorea,
		Spain,
		SriLanka,
		StLucia,
		Sudan,
		Suriname,
		Swaziland,
		Sweden,
		Switzerland,
		Syria,
		Taiwan,
		Tajikistan,
		Tanzania,
		Thailand,
		TheBahamas,
		TheGambia,
		Togo,
		Tonga,
		TrinidadandTobago,
		Tunisia,
		Turkey,
		Turkmenistan,
		Tuvalu,
		Uganda,
		Ukraine,
		UnitedArabEmirates,
		Uruguay,
		USVirginIslands,
		Uzbekistan,
		Vanuatu,
		VaticanCity,
		Venezuela,
		Vietnam,
		WesternSahara,
		Yemen,
		Zambia
	}
}