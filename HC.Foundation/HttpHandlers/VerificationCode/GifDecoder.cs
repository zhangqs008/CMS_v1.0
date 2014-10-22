using System;
using System.Collections;
using System.Drawing;
using System.IO;

namespace HC.Foundation.HttpHandlers.VerificationCode
{
    public class GifDecoder
    {
        protected static readonly int MaxStackSize = 0x1000;
        public static readonly int STATUS_FORMAT_ERROR = 1;
        public static readonly int STATUS_OK = 0;
        public static readonly int STATUS_OPEN_ERROR = 2;
        protected int[] act;
        protected int bgColor;
        protected int bgIndex;
        protected Bitmap bitmap;
        protected byte[] block = new byte[0x100];
        protected int blockSize = 0;
        protected int delay = 0;
        protected int dispose = 0;
        protected int frameCount;
        protected ArrayList frames;
        protected int[] gct;
        protected bool gctFlag;
        protected int gctSize;
        protected int height;
        protected int ih;
        protected Image image;
        protected Stream inStream;
        protected bool interlace;
        protected int iw;
        protected int ix;
        protected int iy;
        protected int lastBgColor;
        protected int lastDispose = 0;
        protected Image lastImage;
        protected Rectangle lastRect;
        protected int[] lct;
        protected bool lctFlag;
        protected int lctSize;
        protected int loopCount = 1;
        protected int pixelAspect;
        protected byte[] pixelStack;
        protected byte[] pixels;
        protected short[] prefix;
        protected int status;
        protected byte[] suffix;
        protected int transIndex;
        protected bool transparency = false;
        protected int width;

        protected void DecodeImageData()
        {
            int num11;
            int num12;
            int num13;
            int num14;
            int num15;
            int num16;
            int num = -1;
            int num2 = iw*ih;
            if ((pixels == null) || (pixels.Length < num2))
            {
                pixels = new byte[num2];
            }
            if (prefix == null)
            {
                prefix = new short[MaxStackSize];
            }
            if (suffix == null)
            {
                suffix = new byte[MaxStackSize];
            }
            if (pixelStack == null)
            {
                pixelStack = new byte[MaxStackSize + 1];
            }
            int num3 = Read();
            int num4 = (1) << num3;
            int num5 = num4 + 1;
            int index = num4 + 2;
            int num7 = num;
            int num8 = num3 + 1;
            int num9 = ((1) << num8) - 1;
            int num10 = 0;
            while (num10 < num4)
            {
                prefix[num10] = 0;
                suffix[num10] = (byte) num10;
                num10++;
            }
            int num17 = num16 = num15 = num14 = num13 = num12 = num11 = 0;
            int num18 = 0;
            while (num18 < num2)
            {
                if (num13 == 0)
                {
                    if (num16 < num8)
                    {
                        if (num15 == 0)
                        {
                            num15 = ReadBlock();
                            if (num15 <= 0)
                            {
                                break;
                            }
                            num11 = 0;
                        }
                        num17 += (block[num11] & 0xff) << num16;
                        num16 += 8;
                        num11++;
                        num15--;
                        continue;
                    }
                    num10 = num17 & num9;
                    num17 = num17 >> num8;
                    num16 -= num8;
                    if ((num10 > index) || (num10 == num5))
                    {
                        break;
                    }
                    if (num10 == num4)
                    {
                        num8 = num3 + 1;
                        num9 = ((1) << num8) - 1;
                        index = num4 + 2;
                        num7 = num;
                        continue;
                    }
                    if (num7 == num)
                    {
                        pixelStack[num13++] = suffix[num10];
                        num7 = num10;
                        num14 = num10;
                        continue;
                    }
                    int num19 = num10;
                    if (num10 == index)
                    {
                        pixelStack[num13++] = (byte) num14;
                        num10 = num7;
                    }
                    while (num10 > num4)
                    {
                        pixelStack[num13++] = suffix[num10];
                        num10 = prefix[num10];
                    }
                    num14 = suffix[num10] & 0xff;
                    if (index >= MaxStackSize)
                    {
                        break;
                    }
                    pixelStack[num13++] = (byte) num14;
                    prefix[index] = (short) num7;
                    suffix[index] = (byte) num14;
                    index++;
                    if (((index & num9) == 0) && (index < MaxStackSize))
                    {
                        num8++;
                        num9 += index;
                    }
                    num7 = num19;
                }
                num13--;
                pixels[num12++] = pixelStack[num13];
                num18++;
            }
            for (num18 = num12; num18 < num2; num18++)
            {
                pixels[num18] = 0;
            }
        }

        protected bool Error()
        {
            return (status != STATUS_OK);
        }

        public int GetDelay(int n)
        {
            delay = -1;
            if ((n >= 0) && (n < frameCount))
            {
                delay = ((GifFrame) frames[n]).delay;
            }
            return delay;
        }

        public Image GetFrame(int n)
        {
            Image image = null;
            if ((n >= 0) && (n < frameCount))
            {
                image = ((GifFrame) frames[n]).image;
            }
            return image;
        }

        public int GetFrameCount()
        {
            return frameCount;
        }

        public Size GetFrameSize()
        {
            return new Size(width, height);
        }

        public Image GetImage()
        {
            return GetFrame(0);
        }

        public int GetLoopCount()
        {
            return loopCount;
        }

        private int[] GetPixels(Bitmap bitmap)
        {
            var numArray = new int[(3*image.Width)*image.Height];
            int index = 0;
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    numArray[index] = pixel.R;
                    index++;
                    numArray[index] = pixel.G;
                    index++;
                    numArray[index] = pixel.B;
                    index++;
                }
            }
            return numArray;
        }

        protected void Init()
        {
            status = STATUS_OK;
            frameCount = 0;
            frames = new ArrayList();
            gct = null;
            lct = null;
        }

        protected int Read()
        {
            int num = 0;
            try
            {
                num = inStream.ReadByte();
            }
            catch (IOException)
            {
                status = STATUS_FORMAT_ERROR;
            }
            return num;
        }

        public int Read(Stream inStream)
        {
            Init();
            if (inStream != null)
            {
                this.inStream = inStream;
                ReadHeader();
                if (!Error())
                {
                    ReadContents();
                    if (frameCount < 0)
                    {
                        status = STATUS_FORMAT_ERROR;
                    }
                }
                inStream.Close();
            }
            else
            {
                status = STATUS_OPEN_ERROR;
            }
            return status;
        }

        public int Read(string name)
        {
            status = STATUS_OK;
            try
            {
                name = name.Trim().ToLower();
                status = Read(new FileInfo(name).OpenRead());
            }
            catch (IOException)
            {
                status = STATUS_OPEN_ERROR;
            }
            return status;
        }

        protected int ReadBlock()
        {
            blockSize = Read();
            int offset = 0;
            if (blockSize <= 0)
            {
                return offset;
            }
            try
            {
                int num2 = 0;
                while (offset < blockSize)
                {
                    num2 = inStream.Read(block, offset, blockSize - offset);
                    if (num2 == -1)
                    {
                        goto Label_007A;
                    }
                    offset += num2;
                }
            }
            catch (IOException)
            {
            }
            Label_007A:
            if (offset < blockSize)
            {
                status = STATUS_FORMAT_ERROR;
            }
            return offset;
        }

        protected int[] ReadColorTable(int ncolors)
        {
            int num = 3*ncolors;
            int[] numArray = null;
            var buffer = new byte[num];
            int num2 = 0;
            try
            {
                num2 = inStream.Read(buffer, 0, buffer.Length);
            }
            catch (IOException)
            {
            }
            if (num2 < num)
            {
                status = STATUS_FORMAT_ERROR;
                return numArray;
            }
            numArray = new int[0x100];
            int num3 = 0;
            int num4 = 0;
            while (num3 < ncolors)
            {
                int num5 = buffer[num4++] & 0xff;
                int num6 = buffer[num4++] & 0xff;
                int num7 = buffer[num4++] & 0xff;
                numArray[num3++] = (((unchecked((int) 0xff000000L)) | (num5 << 0x10)) | (num6 << 8)) | num7;
            }
            return numArray;
        }

        protected void ReadContents()
        {
            bool flag = false;
            while (!flag && !Error())
            {
                switch (Read())
                {
                    case 0x2c:
                        ReadImage();
                        break;

                    case 0x3b:
                        flag = true;
                        break;

                    case 0:
                        break;

                    case 0x21:
                        switch (Read())
                        {
                            case 0xf9:
                                ReadGraphicControlExt();
                                break;

                            case 0xff:
                                {
                                    ReadBlock();
                                    string str = "";
                                    for (int i = 0; i < 11; i++)
                                    {
                                        str = str + ((char) block[i]);
                                    }
                                    if (str.Equals("NETSCAPE2.0"))
                                    {
                                        ReadNetscapeExt();
                                    }
                                    else
                                    {
                                        Skip();
                                    }
                                    break;
                                }
                        }
                        Skip();
                        break;

                    default:
                        status = STATUS_FORMAT_ERROR;
                        break;
                }
            }
        }

        protected void ReadGraphicControlExt()
        {
            Read();
            int num = Read();
            dispose = (num & 0x1c) >> 2;
            if (dispose == 0)
            {
                dispose = 1;
            }
            transparency = (num & 1) != 0;
            delay = ReadShort()*10;
            transIndex = Read();
            Read();
        }

        protected void ReadHeader()
        {
            string str = "";
            for (int i = 0; i < 6; i++)
            {
                str = str + ((char) Read());
            }
            if (!str.StartsWith("GIF"))
            {
                status = STATUS_FORMAT_ERROR;
            }
            else
            {
                ReadLSD();
                if (!(!gctFlag || Error()))
                {
                    gct = ReadColorTable(gctSize);
                    bgColor = gct[bgIndex];
                }
            }
        }

        protected void ReadImage()
        {
            ix = ReadShort();
            iy = ReadShort();
            iw = ReadShort();
            ih = ReadShort();
            int num = Read();
            lctFlag = (num & 0x80) != 0;
            interlace = (num & 0x40) != 0;
            lctSize = (2) << (num & 7);
            if (lctFlag)
            {
                lct = ReadColorTable(lctSize);
                act = lct;
            }
            else
            {
                act = gct;
                if (bgIndex == transIndex)
                {
                    bgColor = 0;
                }
            }
            int num2 = 0;
            if (transparency)
            {
                num2 = act[transIndex];
                act[transIndex] = 0;
            }
            if (act == null)
            {
                status = STATUS_FORMAT_ERROR;
            }
            if (!Error())
            {
                DecodeImageData();
                Skip();
                if (!Error())
                {
                    frameCount++;
                    bitmap = new Bitmap(width, height);
                    image = bitmap;
                    SetPixels();
                    frames.Add(new GifFrame(bitmap, delay));
                    if (transparency)
                    {
                        act[transIndex] = num2;
                    }
                    ResetFrame();
                }
            }
        }

        protected void ReadLSD()
        {
            width = ReadShort();
            height = ReadShort();
            int num = Read();
            gctFlag = (num & 0x80) != 0;
            gctSize = (2) << (num & 7);
            bgIndex = Read();
            pixelAspect = Read();
        }

        protected void ReadNetscapeExt()
        {
            do
            {
                ReadBlock();
                if (block[0] == 1)
                {
                    int num = block[1] & 0xff;
                    int num2 = block[2] & 0xff;
                    loopCount = (num2 << 8) | num;
                }
            } while ((blockSize > 0) && !Error());
        }

        protected int ReadShort()
        {
            return (Read() | (Read() << 8));
        }

        protected void ResetFrame()
        {
            lastDispose = dispose;
            lastRect = new Rectangle(ix, iy, iw, ih);
            lastImage = image;
            lastBgColor = bgColor;
            lct = null;
        }

        protected void SetPixels()
        {
            int[] pixels = GetPixels(bitmap);
            if (lastDispose > 0)
            {
                if (lastDispose == 3)
                {
                    int num = frameCount - 2;
                    if (num > 0)
                    {
                        lastImage = GetFrame(num - 1);
                    }
                    else
                    {
                        lastImage = null;
                    }
                }
                if (lastImage != null)
                {
                    Array.Copy(GetPixels(new Bitmap(lastImage)), 0, pixels, 0, width*height);
                    if (lastDispose == 2)
                    {
                        Graphics graphics = Graphics.FromImage(image);
                        Color empty = Color.Empty;
                        if (transparency)
                        {
                            empty = Color.FromArgb(0, 0, 0, 0);
                        }
                        else
                        {
                            empty = Color.FromArgb(lastBgColor);
                        }
                        Brush brush = new SolidBrush(empty);
                        graphics.FillRectangle(brush, lastRect);
                        brush.Dispose();
                        graphics.Dispose();
                    }
                }
            }
            int num2 = 1;
            int num3 = 8;
            int num4 = 0;
            for (int i = 0; i < ih; i++)
            {
                int num6 = i;
                if (interlace)
                {
                    if (num4 >= ih)
                    {
                        num2++;
                        switch (num2)
                        {
                            case 2:
                                num4 = 4;
                                break;

                            case 3:
                                num4 = 2;
                                num3 = 4;
                                break;

                            case 4:
                                num4 = 1;
                                num3 = 2;
                                break;
                        }
                    }
                    num6 = num4;
                    num4 += num3;
                }
                num6 += iy;
                if (num6 < height)
                {
                    int num8 = num6*width;
                    int index = num8 + ix;
                    int num10 = index + iw;
                    if ((num8 + width) < num10)
                    {
                        num10 = num8 + width;
                    }
                    int num11 = i*iw;
                    while (index < num10)
                    {
                        int num12 = this.pixels[num11++] & 0xff;
                        int num13 = act[num12];
                        if (num13 != 0)
                        {
                            pixels[index] = num13;
                        }
                        index++;
                    }
                }
            }
            SetPixels(pixels);
        }

        private void SetPixels(int[] pixels)
        {
            int num = 0;
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color color = Color.FromArgb(pixels[num++]);
                    bitmap.SetPixel(j, i, color);
                }
            }
        }

        protected void Skip()
        {
            do
            {
                ReadBlock();
            } while ((blockSize > 0) && !Error());
        }

        public class GifFrame
        {
            public int delay;
            public Image image;

            public GifFrame(Image im, int del)
            {
                image = im;
                delay = del;
            }
        }
    }
}