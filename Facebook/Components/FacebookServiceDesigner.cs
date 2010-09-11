using System.ComponentModel;
using System.ComponentModel.Design;

namespace Facebook.Components
{
	internal class FacebookServiceDesigner : ComponentDesigner
	{
		private DesignerActionListCollection _dalc;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (_dalc == null)
				{
					_dalc = new DesignerActionListCollection();
					_dalc.Add(new FacebookServiceDesignerActionList(Component));
				}

				return _dalc;
			}
		}
	}

	public class FacebookServiceDesignerActionList : DesignerActionList
	{
		public FacebookServiceDesignerActionList(IComponent component) : base(component)
		{
		}

		[Category("Setup")]
		[Description("The API key received from facebook for this application that is using the FacebookService component")]
		[DisplayName("API Key")]
		public string ApplicationKey
		{
			get { return FacebookService.ApplicationKey; }
			set { SetProperty("ApplicationKey", value); }
		}

		[Category("Setup")]
		[Description("The secret received from facebook for this application that is using the FacebookService component")]
		public string Secret
		{
			get { return FacebookService.Secret; }
			set { SetProperty("Secret", value); }
		}

		private FacebookService FacebookService
		{
			get { return (FacebookService) Component; }
		}

		private void SetProperty(string propertyName, object value)
		{
			PropertyDescriptor property = TypeDescriptor.GetProperties(FacebookService)[propertyName];
			property.SetValue(FacebookService, value);
		}
	}
}