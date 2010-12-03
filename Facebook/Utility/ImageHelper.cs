using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using Facebook.Properties;

namespace Facebook.Utility
{
	internal static class ImageHelper
	{
		internal static Image ConvertBytesToImage(byte[] imageBytes)
		{
			using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
			{
				ms.Write(imageBytes, 0, imageBytes.Length);
				return new Bitmap(ms);
			}
		}

		internal static byte[] ConvertImageToBytes(Image image)
		{
			using (var ms = new MemoryStream())
			{
				image.Save(ms, ImageFormat.Jpeg);
				return ms.ToArray();
			}
		}

		internal static Image GetImage(Uri imageURL, Image image, out byte[] imageBytes)
		{
			imageBytes = ConvertImageToBytes(Resources.missingPicture);
			if (image == null || image.Equals(Resources.missingPicture))
			{
				imageBytes = GetBytesFromWeb(imageURL);
				return ConvertBytesToImage(imageBytes);
			}
			return image;
		}

		private static byte[] GetBytesFromWeb(Uri imageURL)
		{
			if (imageURL.Equals(Resources.MissingPictureUrl)) return ConvertImageToBytes(Resources.missingPicture);
			try
			{
				var webClient = new WebClient();
				return webClient.DownloadData(imageURL);
			}
			catch
			{
				return ConvertImageToBytes(Resources.missingPicture);
			}
		}

		internal static byte[] GetBytes(Uri imageURL, byte[] imageBytes)
		{
			return imageBytes.Equals(ConvertImageToBytes(Resources.missingPicture)) ? GetBytesFromWeb(imageURL) : imageBytes;
		}

		internal static byte[] GetBytes(Uri imageURL, out Image image, byte[] imageBytes)
		{
			imageBytes = GetBytes(imageURL, imageBytes);
			image = ConvertBytesToImage(imageBytes);

			return imageBytes;
		}
	}
}