using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Google.GData.Contacts;
using Google.GData.Client;
using System.IO;
using System.Net;
using Microsoft.Office.Interop.Outlook;
using Facebook;
using Facebook.Forms;
using Facebook.API;

namespace GoogleContactSync
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void mySyncButton_Click(object sender, EventArgs e)
        {
            //ContactsService service = new ContactsService("Google Contact Sync");
            //service.setUserCredentials(myUsername.Text, myPassword.Text);
            //ContactsQuery query = new ContactsQuery(ContactsQuery.CreateContactsUri("default"));
            //ContactsFeed feed;
            //List<ContactEntry> googleContacts = new List<ContactEntry>();
            //do
            //{
            //    feed = service.Query(query);
            //    googleContacts.AddRange(feed.Entries.Cast<ContactEntry>());
            //    query.StartIndex += feed.Entries.Count;
            //}
            //while (feed.TotalResults > query.StartIndex);

            //Microsoft.Office.Interop.Outlook.ApplicationClass app = new ApplicationClass();
            //var ns = app.GetNamespace("MAPI");
            //var contactsFolder = ns.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
            //IEnumerable<ContactItem> outlookContacts = contactsFolder.Items.Cast<ContactItem>();

            var api = new FacebookAPI();
            api.IsDesktopApplication = true;
            api.ApplicationKey = "e28e4b8c6f5e7507d9fd5ca4b7571ae8";
            api.Secret = "c67b52c879bac6ad4661fede91571fd8";
            api.ConnectToFacebook();

            string my = api.UserId;
            Facebook.Entity.User myuser = api.GetUserInfo(api.UserId).First();
            //var facebookContacts = api.GetFriends();
            //var matches = 
            //    from google in googleContacts 
            //    from facebook in facebookContacts 
            //    where google.Title.Text.Contains(facebook.FirstName) 
            //    && google.Title.Text.Contains(facebook.LastName) 
            //    select new { Google = google, Facebook = facebook };
            //var firstNames = 
            //    from match in matches 
            //    select match.Facebook.FirstName;
            //var googleFirstNames = 
            //    from google in googleContacts 
            //    select google.Title.Text;
            //foreach (var match in matches)
            //{
            //    try
            //    {
            //        Image image = match.Facebook.Picture;
            //        MemoryStream mem = new MemoryStream();
            //        Bitmap bitmap = new Bitmap(image);

            //        bitmap.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        mem.Seek(0, SeekOrigin.Begin);

            //        mem.Seek(0, SeekOrigin.Begin);
            //        service.Update(match.Google.PhotoUri, mem, "image/jpg", string.Format("{0}.jpg", Guid.NewGuid().ToString()));
            //    }
            //    catch (System.Exception ex)
            //    {
            //    }
            //}
            MessageBox.Show("Done!");
        }
    }
}
