using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace BLL_Ecommerce.Helper.Imaging
{
    public static class ImageHelper
    {
        public static byte[] CropAndResize(byte[] originalBytes, int targetWidth = 0, int targetHeight = 0)
        {

            if (targetWidth <= 0) targetWidth = GetConfigInt("ImageCropWidth", 1200);
            if (targetHeight <= 0) targetHeight = GetConfigInt("ImageCropHeight", 1200);

            using (MemoryStream msInput = new MemoryStream(originalBytes))

            using (Image original = Image.FromStream(msInput))
            {

                //if (original.Width <= targetWidth && original.Height <= targetHeight)
                //{
                //    return originalBytes; 
                //}

                // catturare il formato originale prima  di elaborare l'immagine

                ImageFormat orignalFormat = original.RawFormat;

                if (Equals(orignalFormat, ImageFormat.MemoryBmp) || orignalFormat.Guid == Guid.Empty)
                {
                    orignalFormat = ImageFormat.Jpeg;
                }


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

                // Il canvas finale è SEMPRE targetWidth x targetHeight
                int outputWidth = targetWidth;
                int outputHeight = targetHeight;

                // Ma disegniamo l'immagine solo alla sua vera risoluzione (mai più grande del crop originale, niente upscale)
                int drawWidth = Math.Min(cropWidth, targetWidth);
                int drawHeight = Math.Min(cropHeight, targetHeight);

                //  Centriamo il disegno dentro il canvas finale
                int destX = (outputWidth - drawWidth) / 2;
                int destY = (outputHeight - drawHeight) / 2;

                Rectangle cutter = new Rectangle(posX, posY, cropWidth, cropHeight);
                Rectangle finaleImage = new Rectangle(destX, destY, drawWidth, drawHeight);

                using (Bitmap bitmap = new Bitmap(outputWidth, outputHeight, PixelFormat.Format32bppArgb))
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    //se il formato non supporta l'alpha 
                    if (!SupportsAlpha(orignalFormat))
                    {
                        graphics.Clear(Color.White);
                    }

                    graphics.DrawImage(original, finaleImage, cutter, GraphicsUnit.Pixel);

                    using (MemoryStream msOut = new MemoryStream())
                    {
                        bitmap.Save(msOut, orignalFormat);
                        return msOut.ToArray();
                    }
                }
            }
        }


        public static byte[] ApplyWatermark(
           byte[] imageBytes,
           string logoPath,
           float opacity = 0.5f,
           int logoFixedWidth = 300,      // larghezza fissa in pixel, uguale per tutte le immagini
           int marginRightPx = 25,        // margine fisso in pixel dal bordo destro
           int marginBottomPx = 25)       // margine fisso in pixel dal bordo inferiore
            {
            // Clamp difensivo sui parametri
            opacity = Math.Max(0f, Math.Min(1f, opacity));
            logoFixedWidth = Math.Max(1, logoFixedWidth);
            marginRightPx = Math.Max(0, marginRightPx);
            marginBottomPx = Math.Max(0, marginBottomPx);

            using (MemoryStream msInput = new MemoryStream(imageBytes))
            using (Image original = Image.FromStream(msInput))
            {
                ImageFormat originalFormat = original.RawFormat;

                using (Bitmap bitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb))
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(original, 0, 0, original.Width, original.Height);

                    using (Image logo = Image.FromFile(logoPath))
                    {
                        // Larghezza fissa, altezza proporzionale al logo (mantiene aspect ratio)
                        int logoWidth = logoFixedWidth;
                        int logoHeight = (int)(logo.Height * ((float)logoWidth / logo.Width));

                        // Margine fisso in pixel — identico su qualsiasi immagine
                        int posX = original.Width - logoWidth - marginRightPx;
                        int posY = original.Height - logoHeight - marginBottomPx;

                        // Clamp finale: il logo non può uscire dai bordi
                        posX = Math.Max(0, Math.Min(posX, original.Width - logoWidth));
                        posY = Math.Max(0, Math.Min(posY, original.Height - logoHeight));

                        ColorMatrix colorMatrix = new ColorMatrix { Matrix33 = opacity };
                        ImageAttributes imageAttributes = new ImageAttributes();
                        imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                        Rectangle destRect = new Rectangle(posX, posY, logoWidth, logoHeight);

                       

                        graphics.DrawImage(logo, destRect, 0, 0, logo.Width, logo.Height,
                            GraphicsUnit.Pixel, imageAttributes);
                    }

                    using (MemoryStream msOut = new MemoryStream())
                    {
                        bitmap.Save(msOut, originalFormat);
                        return msOut.ToArray();
                    }
                }
            }
        }


        private static bool SupportsAlpha(ImageFormat format)
        {
            return format.Equals(ImageFormat.Png) || format.Equals(ImageFormat.Gif) || format.Equals(ImageFormat.Tiff);
        }

        private static int GetConfigInt(string key, int defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            // con il tryParse se la chiave manca usa il defoult se la chiave è vuota o non numerica usa il default 
            //se la chiave contiene un valore negativo o zero per errore di configuarzion usa comunque il default,
            //invece di passare un valore strano
        

            if (int.TryParse(value, out int result) && result > 0)
            {
                return result;
            }
            return defaultValue;
        }
    }
}