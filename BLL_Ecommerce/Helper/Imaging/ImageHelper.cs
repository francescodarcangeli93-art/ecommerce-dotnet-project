using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace BLL_Ecommerce.Helper.Imaging
{
    public static class ImageHelper
    {
        public static byte[] CropAndResize(byte[] originalBytes, int targetWidth, int targetHeight)
        {
            using (MemoryStream msInput = new MemoryStream(originalBytes))
                
            using (Image original = Image.FromStream(msInput))
            {

                //if (original.Width <= targetWidth && original.Height <= targetHeight)
                //{
                //    return originalBytes; 
                //}

                double sourceRatio = (double)original.Width / original.Height;
                double targetRatio = (double)targetWidth / targetHeight;

                int cropWidth = original.Width;
                int cropHeight = original.Height;
                int posX = 0;
                int posY = 0;

                if (sourceRatio > targetRatio)
                {
                    cropWidth = (int)Math.Round(original.Height * targetRatio);
                    posX = (original.Width - cropWidth) / 2;
                }
                else
                {
                    cropHeight = (int)Math.Round(original.Width / targetRatio);
                    posY = (original.Height - cropHeight) / 2;
                }

                int finalWidth = cropWidth;
                int finalHeight = cropHeight;

                if (cropWidth > targetWidth || cropHeight > targetHeight)
                {
                    finalWidth = targetWidth;
                    finalHeight = targetHeight;
                }

                Rectangle cutter = new Rectangle(posX, posY, cropWidth, cropHeight);
                Rectangle finaleImage = new Rectangle(0, 0, finalWidth, finalHeight);

                using (Bitmap bitmap = new Bitmap(finalWidth, finalHeight))
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    graphics.DrawImage(original, finaleImage, cutter, GraphicsUnit.Pixel);

                    using (MemoryStream msOut = new MemoryStream())
                    {
                        bitmap.Save(msOut, ImageFormat.Jpeg);
                        return msOut.ToArray();
                    }
                }
            }
        }
    }
}