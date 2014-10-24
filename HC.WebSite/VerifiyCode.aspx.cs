using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using HC.Foundation.HttpHandlers.VerificationCode;

namespace HC.WebSite
{
    public partial class VerifiyCode : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OutPutValidate();
        }
        public string AllChar = "1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
        public Color DrawColor = Color.Green;
        public bool FontTextRenderingHint = false;
        public int ImageHeight = 0x17;
        protected string ValidateCode = "";
        public string ValidateCodeFont = "Arial";
        public float ValidateCodeSize = 13f;
        private byte _length = 4;

        public byte Length
        {
            get { return _length; }
            set
            {
                if (value > 4)
                {
                    _length = value;
                }
            }
        }

        #region 创建Bmp图片

        /// <summary>
        ///     创建图片
        /// </summary>
        /// <param name="imageFrame"></param>
        private void CreateImageBmp(out Bitmap imageFrame)
        {
            char[] chArray = ValidateCode.ToCharArray(0, Length);
            var width = (int)(((_length * ValidateCodeSize) * 1.3) + 4.0);
            imageFrame = new Bitmap(width, ImageHeight);
            Graphics graphics = Graphics.FromImage(imageFrame);
            graphics.Clear(Color.White);
            var font = new Font(ValidateCodeFont, ValidateCodeSize, FontStyle.Bold);
            Brush brush = new SolidBrush(DrawColor);
            var maxValue = (int)Math.Max(((ImageHeight - ValidateCodeSize) - 3f), 2f);
            var random = new Random();
            for (int i = 0; i < _length; i++)
            {
                var numArray = new[] { (((int)(i * ValidateCodeSize)) + random.Next(1)) + 3, random.Next(maxValue) };
                var point = new Point(numArray[0], numArray[1]);
                if (FontTextRenderingHint)
                {
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                }
                else
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                }
                graphics.DrawString(chArray[i].ToString(), font, brush, point);
            }
            graphics.Dispose();
        }

        #endregion

        #region  创建Gif图片

        /// <summary>
        ///     创建Gif图片
        /// </summary>
        private void CreateImageGif()
        {
            var encoder = new AnimatedGifEncoder();
            var stream = new MemoryStream();
            encoder.Start();
            encoder.SetDelay(5);
            encoder.SetRepeat(0);
            for (int i = 0; i < 10; i++)
            {
                Bitmap bitmap;
                CreateImageBmp(out bitmap);
                DisposeImageBmp(ref bitmap);
                bitmap.Save(stream, ImageFormat.Png);
                encoder.AddFrame(Image.FromStream(stream));
                stream = new MemoryStream();
            }
            encoder.OutPut(ref stream);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ContentType = "image/Gif";
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
            stream.Close();
            stream.Dispose();
        }

        #endregion

        private void CreateValidate()
        {
            ValidateCode = "";
            string[] strArray = AllChar.Split(new[] { ',' });
            int index = -1;
            var random = new Random();
            for (int i = 0; i < Length; i++)
            {
                if (index != -1)
                {
                    random = new Random((i * index) * ((int)DateTime.Now.Ticks));
                }
                int num3 = random.Next(0x23);
                if (index == num3)
                {
                    CreateValidate();
                }
                index = num3;
                ValidateCode = ValidateCode + strArray[index];
            }
            if (ValidateCode.Length > _length)
            {
                ValidateCode = ValidateCode.Remove(_length);
            }
        }

        private void DisposeImageBmp(ref Bitmap imageFrame)
        {
            Graphics graphics = Graphics.FromImage(imageFrame);
            var pen = new Pen(DrawColor, 1f);
            var random = new Random();
            var pointArray = new Point[2];
            for (int i = 0; i < 15; i++)
            {
                pointArray[0] = new Point(random.Next(imageFrame.Width), random.Next(imageFrame.Height));
                pointArray[1] = new Point(random.Next(imageFrame.Width), random.Next(imageFrame.Height));
                graphics.DrawLine(pen, pointArray[0], pointArray[1]);
            }
            graphics.Dispose();
        }

        public void OutPutValidate()
        {
            CreateValidate();
            CreateImageGif();
            HttpContext.Current.Session["ValidateCodeSession"] = ValidateCode;
        }
    }
}