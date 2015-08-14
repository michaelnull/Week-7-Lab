using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace pickme.Models
{
    public class Pick
    {
        public int Id { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public DateTime PostedOn { get; set; }

        public byte[] Image { get; set; }

        public ApplicationUser PostedBy { get; set; }

        //public static byte[] ResizeImage(byte[] source, int width, int height)
        //{
        //    var ms = new MemoryStream(source);
        //    var image = System.Drawing.Image.FromStream(ms);
        //    Image image2 = new Bitmap(image, new Size(width, height));
        //    var ms2 = new MemoryStream();
        //    image2.Save(ms2, ImageFormat.Jpeg);
        //    return ms2.ToArray();
        //}

        public static byte[] ScaleImage(byte[] source, int maxWidth, int maxHeight)
        {
            var image = System.Drawing.Image.FromStream(new MemoryStream(source));

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            Bitmap bmp = new Bitmap(newImage);

            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public byte[] GetBytes(string url)
        {
            var client = new WebClient();
            var imageArray = client.DownloadData(url);
            return ScaleImage(imageArray, 100, 100);
        }
    }
   
}