using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace BizWebAPI.Common
{
    public class ServiceHandler
    {
        public string ImagePath { get; set; }

        internal async Task<string> CommonMessage()
        {
            return await Task.FromResult("Something went wrong!");
        }

        public async Task<string> Upload_Images(string fileName, byte[] imagedata, string path)
        {
            string result = "";

            Image img;
            try
            {
                using (MemoryStream imgStream = new MemoryStream(imagedata))
                {
                    img = Image.FromStream(imgStream);
                }
                Bitmap b = new Bitmap(img);
                
                string imagePath = path + fileName;
                b.Save(imagePath);
                result = "000";
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                result = "Failed";
            }
            return await Task.FromResult(result);
        }

        public async Task<byte[]> Get_Images(string fileName, string path)
        {
            byte[] result = null;

            try
            {
                string imagePath = path + fileName;

                using (Image image = Image.FromFile(imagePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        // result = base64String;
                        result = imageBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                result = null;
            }
            return await Task.FromResult(result);
        }

        internal async Task<string> GetDateStringFromUTC(string strDate, string strFormat)
        {
            string date = Convert.ToDateTime(strDate.Substring(0, strDate.IndexOf('G'))).ToString(strFormat);
            return await Task.FromResult(date);
        }

        public byte[] GetImageFromDirectory(string imagePath)
        {
            byte[] rest = null;
            try
            {

                using (Image image = Image.FromFile(imagePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        rest = imageBytes;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                rest = null;

            }
            return rest;
        }

        internal string SaveImageDirectory(string fileName, byte[] ImageData)
        {

            string result = "";
            Image img;
            try
            {
                // byte[] bytes = Convert.FromBase64String(imagedata);
                using (MemoryStream imgStream = new MemoryStream(ImageData))
                {
                    img = Image.FromStream(imgStream);

                }
                Bitmap b = new Bitmap(img);

                Graphics g;
                using (g = Graphics.FromImage(b))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    g.DrawImage(img, new Point(0, 0));
                }

                // string filepath = AppDomain.CurrentDomain.BaseDirectory + "images\\" + FileName + ".jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\TaskManagement", fileName + ".jpg");
                string returnFilePath = @"images\TaskManagement\" + fileName + ".jpg";

                float thumbWidth = 3000F;
                float thumbHeight = 2500F;

                if (img.Width > img.Height)
                {
                    thumbHeight = ((float)img.Height / img.Width) * thumbWidth;
                }
                else
                {
                    thumbWidth = ((float)img.Width / img.Height) * thumbHeight;
                }

                int actualthumbWidth = Convert.ToInt32(Math.Floor(thumbWidth));
                int actualthumbHeight = Convert.ToInt32(Math.Floor(thumbHeight));
                var thumbnailBitmap = new Bitmap(actualthumbWidth, actualthumbHeight);
                var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, actualthumbWidth, actualthumbHeight);
                thumbnailGraph.DrawImage(img, imageRectangle);
                var ms = new MemoryStream();
                thumbnailBitmap.Save(filePath, ImageFormat.Jpeg);
                ms.Position = 0;
                GC.Collect();
                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                img.Dispose();

                //img = b;
                //img.Save(filepath, ImageFormat.Jpeg);
                //graphics.Save(filepath, ImageFormat.Jpeg);
                result = returnFilePath;
                //----------------------------
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                result = "";
            }
            return result;
        }

    }
   
}
