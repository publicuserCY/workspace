using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace Demo4DotNetCore.Tools
{
    public class ImageTool
    {
        /// <summary>
        /// Image转byte[]
        /// </summary>
        /// <param name="img">Img格式数据</param>
        /// <returns>Base64格式数据</returns>
        public static string ImageToBase64(Image<Rgba32> image)
        {
            IImageFormat format = null;
            return image.ToBase64String(format);
        }

        public static Image<Rgba32> Base64ToImage(string basestr)
        {
            string inputStr = basestr;
            byte[] bytes = Convert.FromBase64String(inputStr);
            var image = Image.Load(bytes, out IImageFormat format);
            return image;
        }
    }
}