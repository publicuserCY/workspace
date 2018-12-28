using System;
using System.Drawing;
using System.IO;

namespace Demo4OAuth.Tools
{
    public class ImageTool
    {
        /// <summary>
        /// Image转byte[]
        /// </summary>
        /// <param name="img">Img格式数据</param>
        /// <returns>Base64格式数据</returns>
        public static string ImageToBase64(Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return Convert.ToBase64String(bytes);
        }

        public static Bitmap Base64ToImage(string basestr)
        {
            String inputStr = basestr;
            byte[] arr = Convert.FromBase64String(inputStr);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);
            ms.Close();
            return bmp;
        }
    }
}