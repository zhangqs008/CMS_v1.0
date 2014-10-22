using System;
using System.Drawing;
using System.IO;

namespace HC.Foundation.HttpHandlers.VerificationCode
{
    public class AnimatedGifEncoder
    {
        protected MemoryStream Memory;
        protected int colorDepth;
        protected byte[] colorTab;
        protected int delay = 0;
        protected int dispose = -1;
        protected bool firstFrame = true;
        protected int height;
        protected Image image;
        protected byte[] indexedPixels;
        protected int palSize = 7;
        protected byte[] pixels;
        protected int repeat = -1;
        protected int sample = 10;
        protected bool sizeSet = false;
        protected bool started = false;
        protected int transIndex;
        protected Color transparent = Color.Empty;
        protected bool[] usedEntry = new bool[0x100];
        protected int width;

        public bool AddFrame(Image im)
        {
            if (!((im != null) && started))
            {
                return false;
            }
            bool flag3 = true;
            try
            {
                if (!sizeSet)
                {
                    SetSize(im.Width, im.Height);
                }
                image = im;
                GetImagePixels();
                AnalyzePixels();
                if (firstFrame)
                {
                    WriteLSD();
                    WritePalette();
                    if (repeat >= 0)
                    {
                        WriteNetscapeExt();
                    }
                }
                WriteGraphicCtrlExt();
                WriteImageDesc();
                if (!firstFrame)
                {
                    WritePalette();
                }
                WritePixels();
                firstFrame = false;
            }
            catch (IOException)
            {
                flag3 = false;
            }
            return flag3;
        }

        protected void AnalyzePixels()
        {
            int length = pixels.Length;
            int num2 = length / 3;
            indexedPixels = new byte[num2];
            var quant = new NeuQuant(pixels, length, sample);
            colorTab = quant.Process();
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                int index = quant.Map(pixels[num3++] & 0xff, pixels[num3++] & 0xff, pixels[num3++] & 0xff);
                usedEntry[index] = true;
                indexedPixels[i] = (byte)index;
            }
            pixels = null;
            colorDepth = 8;
            palSize = 7;
            if (transparent != Color.Empty)
            {
                transIndex = FindClosest(transparent);
            }
        }

        protected int FindClosest(Color c)
        {
            if (colorTab == null)
            {
                return -1;
            }
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int num5 = 0;
            int num6 = 0x1000000;
            int length = colorTab.Length;
            for (int i = 0; i < length; i++)
            {
                int num9 = r - (colorTab[i++] & 0xff);
                int num10 = g - (colorTab[i++] & 0xff);
                int num11 = b - (colorTab[i] & 0xff);
                int num12 = ((num9 * num9) + (num10 * num10)) + (num11 * num11);
                int index = i / 3;
                if (usedEntry[index] && (num12 < num6))
                {
                    num6 = num12;
                    num5 = index;
                }
            }
            return num5;
        }

        protected void GetImagePixels()
        {
            int width = image.Width;
            int height = image.Height;
            if ((width != this.width) || (height != this.height))
            {
                Image timage = new Bitmap(this.width, this.height);
                Graphics graphics = Graphics.FromImage(timage);
                graphics.DrawImage(this.image, 0, 0);
                this.image = timage;
                graphics.Dispose();
            }
            pixels = new byte[(3 * image.Width) * image.Height];
            int index = 0;
            var bitmap = new Bitmap(image);
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    pixels[index] = pixel.R;
                    index++;
                    pixels[index] = pixel.G;
                    index++;
                    pixels[index] = pixel.B;
                    index++;
                }
            }
        }

        public void OutPut(ref MemoryStream MemoryResult)
        {
            started = false;
            Memory.WriteByte(0x3b);
            Memory.Flush();
            MemoryResult = Memory;
            Memory.Close();
            Memory.Dispose();
            transIndex = 0;
            Memory = null;
            image = null;
            pixels = null;
            indexedPixels = null;
            colorTab = null;
            firstFrame = true;
        }

        public void SetDelay(int ms)
        {
            delay = (int)Math.Round(((ms) / 10f));
        }

        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                dispose = code;
            }
        }

        public void SetFrameRate(float fps)
        {
            if (!(fps == 0f))
            {
                delay = (int)Math.Round((100f / fps));
            }
        }

        public void SetQuality(int quality)
        {
            if (quality < 1)
            {
                quality = 1;
            }
            sample = quality;
        }

        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                repeat = iter;
            }
        }

        public void SetSize(int w, int h)
        {
            if (!started || firstFrame)
            {
                width = w;
                height = h;
                if (width < 1)
                {
                    width = 320;
                }
                if (height < 1)
                {
                    height = 240;
                }
                sizeSet = true;
            }
        }

        public void SetTransparent(Color c)
        {
            transparent = c;
        }

        public void Start()
        {
            Memory = new MemoryStream();
            WriteString("GIF89a");
            started = true;
        }

        protected void WriteGraphicCtrlExt()
        {
            int num;
            int num2;
            Memory.WriteByte(0x21);
            Memory.WriteByte(0xf9);
            Memory.WriteByte(4);
            if (transparent == Color.Empty)
            {
                num = 0;
                num2 = 0;
            }
            else
            {
                num = 1;
                num2 = 2;
            }
            if (dispose >= 0)
            {
                num2 = dispose & 7;
            }
            num2 = num2 << 2;
            Memory.WriteByte(Convert.ToByte((num2 | num)));
            WriteShort(delay);
            Memory.WriteByte(Convert.ToByte(transIndex));
            Memory.WriteByte(0);
        }

        protected void WriteImageDesc()
        {
            Memory.WriteByte(0x2c);
            WriteShort(0);
            WriteShort(0);
            WriteShort(width);
            WriteShort(height);
            if (firstFrame)
            {
                Memory.WriteByte(0);
            }
            else
            {
                Memory.WriteByte(Convert.ToByte((0x80 | palSize)));
            }
        }

        protected void WriteLSD()
        {
            WriteShort(width);
            WriteShort(height);
            Memory.WriteByte(Convert.ToByte((240 | palSize)));
            Memory.WriteByte(0);
            Memory.WriteByte(0);
        }

        protected void WriteNetscapeExt()
        {
            Memory.WriteByte(0x21);
            Memory.WriteByte(0xff);
            Memory.WriteByte(11);
            WriteString("NETSCAPE2.0");
            Memory.WriteByte(3);
            Memory.WriteByte(1);
            WriteShort(repeat);
            Memory.WriteByte(0);
        }

        protected void WritePalette()
        {
            Memory.Write(colorTab, 0, colorTab.Length);
            int num = 0x300 - colorTab.Length;
            for (int i = 0; i < num; i++)
            {
                Memory.WriteByte(0);
            }
        }

        protected void WritePixels()
        {
            new LZWEncoder(width, height, indexedPixels, colorDepth).Encode(Memory);
        }

        protected void WriteShort(int value)
        {
            Memory.WriteByte(Convert.ToByte((value & 0xff)));
            Memory.WriteByte(Convert.ToByte(((value >> 8) & 0xff)));
        }

        protected void WriteString(string s)
        {
            char[] chArray = s.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                Memory.WriteByte((byte)chArray[i]);
            }
        }
    }
}