using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Threading.Tasks;

namespace Demo4OAuth.Tools
{
    public class ValidateCodeManager
    {
        private IValidateCode codeTool;
        public ValidateCodeManager(ValidateCodeType codeType)
        {
            switch (codeType)
            {
                case ValidateCodeType.Simple:
                    codeTool = new SimpleValidateCode();
                    break;
                case ValidateCodeType.Complex:
                    codeTool = new ComplexValidateCode();
                    break;
            }
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        public Task<Model.Core.OperationResult<string>> GenerateCode(int length)
        {
            var opResult = new Model.Core.OperationResult<string>(true);
            try
            {
                opResult.Data = codeTool.GenerateCode(length);
            }
            catch (Exception ex)
            {
                opResult = new Model.Core.OperationResult<string>(false);
                opResult.Message = ex.Message;
            }
            return Task.FromResult(opResult);
        }

        /// <summary>
        /// 生成验证图形
        /// </summary>
        /// <param name="code">验证码</param>
        public Task<Model.Core.OperationResult<byte[]>> GenerateGraphic(string code)
        {
            var opResult = new Model.Core.OperationResult<byte[]>(true);
            try
            {
                opResult.Data = codeTool.GenerateGraphic(code);
            }
            catch (Exception ex)
            {
                opResult = new Model.Core.OperationResult<byte[]>(false);
                opResult.Message = ex.Message;
            }
            return Task.FromResult(opResult);
        }
    }

    /// <summary>
    /// 验证码生成接口
    /// </summary>
    public interface IValidateCode
    {
        string GenerateCode(int length);
        byte[] GenerateGraphic(string code);
    }

    /// <summary>
    /// 简单验证码生成工具
    /// </summary>
    public class SimpleValidateCode : IValidateCode
    {
        ///<summary>
        /// 生成验证码
        ///</summary>
        ///<param name="length">指定验证码的长度</param>
        ///<returns></returns>
        public string GenerateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, int.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = int.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }
        ///<summary>
        /// 创建验证码的图片
        ///</summary>
        ///<param name="code">验证码</param>
        public byte[] GenerateGraphic(string code)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(code.Length * 14.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                Color.Blue, Color.DarkRed, 1.5f, true);
                g.DrawString(code, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }

    /// <summary>
    /// 复杂验证码生成工具
    /// </summary>
    public class ComplexValidateCode : IValidateCode
    {
        public string GenerateCode(int length)
        {
            char[] s = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            string num = "";
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                num += s[r.Next(0, s.Length)].ToString();
            }
            return num;
        }

        public byte[] GenerateGraphic(string code)
        {
            Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Brown, Color.DarkCyan, Color.Purple };
            int len = colors.Length;
            Random random = new Random(DateTime.Now.Millisecond);
            Bitmap image = new Bitmap((int)Math.Ceiling(code.Length * 22d), 40);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            Color color = colors[random.Next(0, len)];
            Brush brush = new SolidBrush(color);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int x1 = random.Next(0, image.Width / 3);
            int x2 = random.Next(2 * image.Width / 3, image.Width);
            int y1 = random.Next(image.Height / 4, image.Height / 2);
            int y2 = random.Next(3 * image.Height / 4, 2 * image.Height);
            g.DrawLine(new Pen(color, 2), x1, y1, x2, y2);
            Twist tw = new Twist();
            image = tw.TwistBitmap(image, false, 4, 2 * Math.PI);
            g.Dispose();

            g = Graphics.FromImage(image);

            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.FormatFlags = StringFormatFlags.NoWrap;
                Matrix matrix = new Matrix();
                float offsetx = -30, offsety = 0;
                for (int i = 0; i < code.Length; i++)
                {
                    int fsize = random.Next(14, 20);
                    Font f = new Font("AKANGEL", fsize, FontStyle.Bold, GraphicsUnit.Point, 0);
                    // Font f = CreateFont(System.Web.HttpContext.Current.Server.MapPath("AKANGEL.ttf"), fsize, FontStyle.Bold, GraphicsUnit.Point, 0);
                    SizeF size = g.MeasureString(code[i].ToString(), f);
                    matrix.RotateAt(random.Next(-15, 15), new PointF(offsetx + size.Width / 2, offsety + size.Height / 2));
                    g.Transform = matrix;
                    g.DrawString(
                                 code[i].ToString(),
                                 f,
                                 brush,
                                 new RectangleF(
                                 offsetx,
                                 offsety,
                                 image.Width,
                                 image.Height),
                                format);
                    offsetx += size.Width * 3 / 5;
                    offsety += -0;
                    g.RotateTransform(0);
                    matrix.Reset();
                    f.Dispose();
                }
            }

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, ImageFormat.Gif);
            return ms.ToArray();
        }

        public static Font CreateFont(string fontFile, float fontSize, FontStyle fontStyle, GraphicsUnit graphicsUnit, byte gdiCharSet)
        {
            //PrivateFontCollection 类允许应用程序安装现有字体的私有版本，而无需替换该字体的系统版本。例如，除系统使用的 Arial 字体外，GDI+ 还可以创建 Arial 字体的私有版本。PrivateFontCollection 还可以用于安装操作系统中不存在的字体。这种临时的字体安装不会影响系统安装的字体集合。若要查看已安装的字体集合，请使用 InstalledFontCollection 类。
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(fontFile);
            return new Font(pfc.Families[0], fontSize, fontStyle, graphicsUnit, gdiCharSet);
        }

        class Twist
        {
            /// <summary>
            /// 正弦曲线Wave扭曲图片   
            /// </summary>
            /// <param name="srcBmp">图片路径</param>
            /// <param name="bXDir">沿Y轴扭曲则选择为True</param>
            /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
            /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
            /// <returns>扭曲后的位图</returns>
            public Bitmap TwistBitmap(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
            {
                int w = srcBmp.Width;
                int h = srcBmp.Height;
                Bitmap destBmp = new Bitmap(w, h);
                using (Graphics graph = Graphics.FromImage(destBmp))
                {
                    graph.Clear(Color.White);
                }
                double dBaseAxisLen = bXDir ? h : w;
                BitmapData destData = destBmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, srcBmp.PixelFormat);
                BitmapData srcData = srcBmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, srcBmp.PixelFormat);
                //调式一定注意PixelFormat是多少，别处都没有解释的。如果是24下面指针和跨度就3倍，要是32就是4倍。
                unsafe
                {
                    byte* p = (byte*)srcData.Scan0;
                    byte* p2 = (byte*)destData.Scan0;
                    for (int j = 0; j < h; j++)
                    {
                        for (int i = 0; i < w; i++)
                        {
                            double dx = 0;
                            dx = bXDir ? (Math.PI * j * 2) / dBaseAxisLen : (Math.PI * i * 2) / dBaseAxisLen;
                            dx += dPhase;
                            double dy = Math.Sin(dx);
                            // 取得当前点  
                            int nOldX = 0, nOldY = 0;
                            nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                            nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                            if ((nOldX >= 0 && nOldX < w) && (nOldY >= 0 && nOldY < h))
                            {
                                p2[(nOldY * destData.Stride) + (nOldX * 4)] = p[(j * srcData.Stride) + (i * 4)];
                                p2[(nOldY * destData.Stride) + (nOldX * 4) + 1] = p[(j * srcData.Stride) + (i * 4) + 1];
                                p2[(nOldY * destData.Stride) + (nOldX * 4) + 2] = p[(j * srcData.Stride) + (i * 4) + 2];
                            }
                        }
                    }
                }
                destBmp.UnlockBits(destData);
                srcBmp.UnlockBits(srcData);
                if (srcBmp != null)
                    srcBmp.Dispose();
                return destBmp;
            }
        }
    }

    public enum ValidateCodeType { Simple, Complex }
}

