using System;
using System.IO;

namespace HC.Foundation.HttpHandlers.VerificationCode
{
    public class LZWEncoder
    {
        private static readonly int BITS = 12;
        private static readonly int EOF = -1;
        private static readonly int HSIZE = 0x138b;
        private readonly byte[] accum = new byte[0x100];
        private readonly int[] codetab = new int[HSIZE];
        private readonly int hsize = HSIZE;
        private readonly int[] htab = new int[HSIZE];
        private readonly int imgH;
        private readonly int imgW;
        private readonly int initCodeSize;

        private readonly int[] masks = new[]
            {
                0, 1, 3, 7, 15, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff,
                0xffff
            };

        private readonly int maxbits = BITS;
        private readonly int maxmaxcode = ((1) << BITS);
        private readonly byte[] pixAry;
        private int ClearCode;
        private int EOFCode;
        private int a_count;
        private bool clear_flg;
        private int curPixel;
        private int cur_accum;
        private int cur_bits;
        private int free_ent;
        private int g_init_bits;
        private int maxcode;
        private int n_bits;
        private int remaining;

        public LZWEncoder(int width, int height, byte[] pixels, int color_depth)
        {
            imgW = width;
            imgH = height;
            pixAry = pixels;
            initCodeSize = Math.Max(2, color_depth);
        }

        private void Add(byte c, Stream outs)
        {
            accum[a_count++] = c;
            if (a_count >= 0xfe)
            {
                Flush(outs);
            }
        }

        private void ClearTable(Stream outs)
        {
            ResetCodeTable(hsize);
            free_ent = ClearCode + 2;
            clear_flg = true;
            Output(ClearCode, outs);
        }

        private void Compress(int init_bits, Stream outs)
        {
            int num5;
            g_init_bits = init_bits;
            clear_flg = false;
            n_bits = g_init_bits;
            maxcode = MaxCode(n_bits);
            ClearCode = (1) << (init_bits - 1);
            EOFCode = ClearCode + 1;
            free_ent = ClearCode + 2;
            a_count = 0;
            int code = NextPixel();
            int num2 = 0;
            int hsize = this.hsize;
            while (hsize < 0x10000)
            {
                num2++;
                hsize *= 2;
            }
            num2 = 8 - num2;
            int num4 = this.hsize;
            ResetCodeTable(num4);
            Output(ClearCode, outs);
            while ((num5 = NextPixel()) != EOF)
            {
                hsize = (num5 << maxbits) + code;
                int index = (num5 << num2) ^ code;
                if (htab[index] == hsize)
                {
                    code = codetab[index];
                }
                else
                {
                    if (htab[index] >= 0)
                    {
                        int num7 = num4 - index;
                        if (index == 0)
                        {
                            num7 = 1;
                        }
                        do
                        {
                            index -= num7;
                            if (index < 0)
                            {
                                index += num4;
                            }
                            if (htab[index] == hsize)
                            {
                                code = codetab[index];
                                continue;
                            }
                        } while (htab[index] >= 0);
                    }
                    Output(code, outs);
                    code = num5;
                    if (free_ent < maxmaxcode)
                    {
                        codetab[index] = free_ent++;
                        htab[index] = hsize;
                    }
                    else
                    {
                        ClearTable(outs);
                    }
                }
            }
            Output(code, outs);
            Output(EOFCode, outs);
        }

        public void Encode(Stream os)
        {
            os.WriteByte(Convert.ToByte(initCodeSize));
            remaining = imgW*imgH;
            curPixel = 0;
            Compress(initCodeSize + 1, os);
            os.WriteByte(0);
        }

        private void Flush(Stream outs)
        {
            if (a_count > 0)
            {
                outs.WriteByte(Convert.ToByte(a_count));
                outs.Write(accum, 0, a_count);
                a_count = 0;
            }
        }

        private int MaxCode(int n_bits)
        {
            return (((1) << n_bits) - 1);
        }

        private int NextPixel()
        {
            if (remaining == 0)
            {
                return EOF;
            }
            remaining--;
            int num2 = curPixel + 1;
            if (num2 < pixAry.GetUpperBound(0))
            {
                byte num4 = pixAry[curPixel++];
                return (num4 & 0xff);
            }
            return 0xff;
        }

        private void Output(int code, Stream outs)
        {
            cur_accum &= masks[cur_bits];
            if (cur_bits > 0)
            {
                cur_accum |= code << cur_bits;
            }
            else
            {
                cur_accum = code;
            }
            cur_bits += n_bits;
            while (cur_bits >= 8)
            {
                Add((byte) (cur_accum & 0xff), outs);
                cur_accum = cur_accum >> 8;
                cur_bits -= 8;
            }
            if ((free_ent > maxcode) || clear_flg)
            {
                if (clear_flg)
                {
                    maxcode = MaxCode(n_bits = g_init_bits);
                    clear_flg = false;
                }
                else
                {
                    n_bits++;
                    if (n_bits == maxbits)
                    {
                        maxcode = maxmaxcode;
                    }
                    else
                    {
                        maxcode = MaxCode(n_bits);
                    }
                }
            }
            if (code == EOFCode)
            {
                while (cur_bits > 0)
                {
                    Add((byte) (cur_accum & 0xff), outs);
                    cur_accum = cur_accum >> 8;
                    cur_bits -= 8;
                }
                Flush(outs);
            }
        }

        private void ResetCodeTable(int hsize)
        {
            for (int i = 0; i < hsize; i++)
            {
                htab[i] = -1;
            }
        }
    }
}