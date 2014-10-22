using System;

namespace HC.Foundation.HttpHandlers.VerificationCode
{
    public class NeuQuant
    {
        protected static readonly int alphabiasshift = 10;
        protected static readonly int alpharadbias = ((1) << alpharadbshift);
        protected static readonly int alpharadbshift = (alphabiasshift + radbiasshift);
        protected static readonly int beta = (intbias >> betashift);
        protected static readonly int betagamma = (intbias << (gammashift - betashift));
        protected static readonly int betashift = 10;
        protected static readonly int gamma = ((1) << gammashift);
        protected static readonly int gammashift = 10;
        protected static readonly int initalpha = ((1) << alphabiasshift);
        protected static readonly int initrad = (netsize >> 3);
        protected static readonly int initradius = (initrad*radiusbias);
        protected static readonly int intbias = ((1) << intbiasshift);
        protected static readonly int intbiasshift = 0x10;
        protected static readonly int maxnetpos = (netsize - 1);
        protected static readonly int minpicturebytes = (3*prime4);
        protected static readonly int ncycles = 100;
        protected static readonly int netbiasshift = 4;
        protected static readonly int netsize = 0x100;
        protected static readonly int prime1 = 0x1f3;
        protected static readonly int prime2 = 0x1eb;
        protected static readonly int prime3 = 0x1e7;
        protected static readonly int prime4 = 0x1f7;
        protected static readonly int radbias = ((1) << radbiasshift);
        protected static readonly int radbiasshift = 8;
        protected static readonly int radiusbias = ((1) << radiusbiasshift);
        protected static readonly int radiusbiasshift = 6;
        protected static readonly int radiusdec = 30;
        protected int alphadec;
        protected int[] bias = new int[netsize];
        protected int[] freq = new int[netsize];
        protected int lengthcount;
        protected int[] netindex = new int[0x100];
        protected int[][] network;
        protected int[] radpower = new int[initrad];
        protected int samplefac;
        protected byte[] thepicture;

        public NeuQuant(byte[] thepic, int len, int sample)
        {
            thepicture = thepic;
            lengthcount = len;
            samplefac = sample;
            network = new int[netsize][];
            for (int i = 0; i < netsize; i++)
            {
                int num2;
                network[i] = new int[4];
                int[] numArray = network[i];
                numArray[2] = num2 = (i << (netbiasshift + 8))/netsize;
                numArray[0] = numArray[1] = num2;
                freq[i] = intbias/netsize;
                bias[i] = 0;
            }
        }

        protected void Alterneigh(int rad, int i, int b, int g, int r)
        {
            int num = i - rad;
            if (num < -1)
            {
                num = -1;
            }
            int netsize = i + rad;
            if (netsize > NeuQuant.netsize)
            {
                netsize = NeuQuant.netsize;
            }
            int num3 = i + 1;
            int num4 = i - 1;
            int num5 = 1;
            while ((num3 < netsize) || (num4 > num))
            {
                int[] numArray;
                Exception exception;
                int num6 = radpower[num5++];
                if (num3 < netsize)
                {
                    numArray = network[num3++];
                    try
                    {
                        numArray[0] -= (num6*(numArray[0] - b))/alpharadbias;
                        numArray[1] -= (num6*(numArray[1] - g))/alpharadbias;
                        numArray[2] -= (num6*(numArray[2] - r))/alpharadbias;
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                    }
                }
                if (num4 > num)
                {
                    numArray = network[num4--];
                    try
                    {
                        numArray[0] -= (num6*(numArray[0] - b))/alpharadbias;
                        numArray[1] -= (num6*(numArray[1] - g))/alpharadbias;
                        numArray[2] -= (num6*(numArray[2] - r))/alpharadbias;
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                    }
                }
            }
        }

        protected void Altersingle(int alpha, int i, int b, int g, int r)
        {
            int[] numArray = network[i];
            numArray[0] -= (alpha*(numArray[0] - b))/initalpha;
            numArray[1] -= (alpha*(numArray[1] - g))/initalpha;
            numArray[2] -= (alpha*(numArray[2] - r))/initalpha;
        }

        public byte[] ColorMap()
        {
            int num;
            var buffer = new byte[3*netsize];
            var numArray = new int[netsize];
            for (num = 0; num < netsize; num++)
            {
                numArray[network[num][3]] = num;
            }
            int num2 = 0;
            for (num = 0; num < netsize; num++)
            {
                int index = numArray[num];
                buffer[num2++] = (byte) network[index][0];
                buffer[num2++] = (byte) network[index][1];
                buffer[num2++] = (byte) network[index][2];
            }
            return buffer;
        }

        protected int Contest(int b, int g, int r)
        {
            int num = 0x7fffffff;
            int num2 = num;
            int index = -1;
            int num4 = index;
            for (int i = 0; i < netsize; i++)
            {
                int[] numArray = network[i];
                int num6 = numArray[0] - b;
                if (num6 < 0)
                {
                    num6 = -num6;
                }
                int num7 = numArray[1] - g;
                if (num7 < 0)
                {
                    num7 = -num7;
                }
                num6 += num7;
                num7 = numArray[2] - r;
                if (num7 < 0)
                {
                    num7 = -num7;
                }
                num6 += num7;
                if (num6 < num)
                {
                    num = num6;
                    index = i;
                }
                int num8 = num6 - (bias[i] >> (intbiasshift - netbiasshift));
                if (num8 < num2)
                {
                    num2 = num8;
                    num4 = i;
                }
                int num9 = freq[i] >> betashift;
                freq[i] -= num9;
                bias[i] += num9 << gammashift;
            }
            freq[index] += beta;
            bias[index] -= betagamma;
            return num4;
        }

        public void Inxbuild()
        {
            int num6;
            int index = 0;
            int num2 = 0;
            for (int i = 0; i < netsize; i++)
            {
                int[] numArray2;
                int[] numArray = network[i];
                int num4 = i;
                int num5 = numArray[1];
                num6 = i + 1;
                while (num6 < netsize)
                {
                    numArray2 = network[num6];
                    if (numArray2[1] < num5)
                    {
                        num4 = num6;
                        num5 = numArray2[1];
                    }
                    num6++;
                }
                numArray2 = network[num4];
                if (i != num4)
                {
                    num6 = numArray2[0];
                    numArray2[0] = numArray[0];
                    numArray[0] = num6;
                    num6 = numArray2[1];
                    numArray2[1] = numArray[1];
                    numArray[1] = num6;
                    num6 = numArray2[2];
                    numArray2[2] = numArray[2];
                    numArray[2] = num6;
                    num6 = numArray2[3];
                    numArray2[3] = numArray[3];
                    numArray[3] = num6;
                }
                if (num5 != index)
                {
                    netindex[index] = (num2 + i) >> 1;
                    num6 = index + 1;
                    while (num6 < num5)
                    {
                        netindex[num6] = i;
                        num6++;
                    }
                    index = num5;
                    num2 = i;
                }
            }
            netindex[index] = (num2 + maxnetpos) >> 1;
            for (num6 = index + 1; num6 < 0x100; num6++)
            {
                netindex[num6] = maxnetpos;
            }
        }

        public void Learn()
        {
            int num8;
            int num9;
            if (this.lengthcount < minpicturebytes)
            {
                samplefac = 1;
            }
            alphadec = 30 + ((samplefac - 1)/3);
            byte[] thepicture = this.thepicture;
            int index = 0;
            int lengthcount = this.lengthcount;
            int num3 = this.lengthcount/(3*samplefac);
            int num4 = num3/ncycles;
            int initalpha = NeuQuant.initalpha;
            int initradius = NeuQuant.initradius;
            int rad = initradius >> radiusbiasshift;
            if (rad <= 1)
            {
                rad = 0;
            }
            for (num8 = 0; num8 < rad; num8++)
            {
                radpower[num8] = initalpha*((((rad*rad) - (num8*num8))*radbias)/(rad*rad));
            }
            if (this.lengthcount < minpicturebytes)
            {
                num9 = 3;
            }
            else if ((this.lengthcount%prime1) != 0)
            {
                num9 = 3*prime1;
            }
            else if ((this.lengthcount%prime2) != 0)
            {
                num9 = 3*prime2;
            }
            else if ((this.lengthcount%prime3) != 0)
            {
                num9 = 3*prime3;
            }
            else
            {
                num9 = 3*prime4;
            }
            num8 = 0;
            while (num8 < num3)
            {
                int b = (thepicture[index] & 0xff) << netbiasshift;
                int g = (thepicture[index + 1] & 0xff) << netbiasshift;
                int r = (thepicture[index + 2] & 0xff) << netbiasshift;
                int i = Contest(b, g, r);
                Altersingle(initalpha, i, b, g, r);
                if (rad != 0)
                {
                    Alterneigh(rad, i, b, g, r);
                }
                index += num9;
                if (index >= lengthcount)
                {
                    index -= this.lengthcount;
                }
                num8++;
                if (num4 == 0)
                {
                    num4 = 1;
                }
                if ((num8%num4) == 0)
                {
                    initalpha -= initalpha/alphadec;
                    initradius -= initradius/radiusdec;
                    rad = initradius >> radiusbiasshift;
                    if (rad <= 1)
                    {
                        rad = 0;
                    }
                    for (i = 0; i < rad; i++)
                    {
                        radpower[i] = initalpha*((((rad*rad) - (i*i))*radbias)/(rad*rad));
                    }
                }
            }
        }

        public int Map(int b, int g, int r)
        {
            int num = 0x3e8;
            int num2 = -1;
            int index = netindex[g];
            int num4 = index - 1;
            while ((index < netsize) || (num4 >= 0))
            {
                int[] numArray;
                int num5;
                int num6;
                if (index < netsize)
                {
                    numArray = network[index];
                    num5 = numArray[1] - g;
                    if (num5 >= num)
                    {
                        index = netsize;
                    }
                    else
                    {
                        index++;
                        if (num5 < 0)
                        {
                            num5 = -num5;
                        }
                        num6 = numArray[0] - b;
                        if (num6 < 0)
                        {
                            num6 = -num6;
                        }
                        num5 += num6;
                        if (num5 < num)
                        {
                            num6 = numArray[2] - r;
                            if (num6 < 0)
                            {
                                num6 = -num6;
                            }
                            num5 += num6;
                            if (num5 < num)
                            {
                                num = num5;
                                num2 = numArray[3];
                            }
                        }
                    }
                }
                if (num4 >= 0)
                {
                    numArray = network[num4];
                    num5 = g - numArray[1];
                    if (num5 >= num)
                    {
                        num4 = -1;
                    }
                    else
                    {
                        num4--;
                        if (num5 < 0)
                        {
                            num5 = -num5;
                        }
                        num6 = numArray[0] - b;
                        if (num6 < 0)
                        {
                            num6 = -num6;
                        }
                        num5 += num6;
                        if (num5 < num)
                        {
                            num6 = numArray[2] - r;
                            if (num6 < 0)
                            {
                                num6 = -num6;
                            }
                            num5 += num6;
                            if (num5 < num)
                            {
                                num = num5;
                                num2 = numArray[3];
                            }
                        }
                    }
                }
            }
            return num2;
        }

        public byte[] Process()
        {
            Learn();
            Unbiasnet();
            Inxbuild();
            return ColorMap();
        }

        public void Unbiasnet()
        {
            for (int i = 0; i < netsize; i++)
            {
                network[i][0] = network[i][0] >> netbiasshift;
                network[i][1] = network[i][1] >> netbiasshift;
                network[i][2] = network[i][2] >> netbiasshift;
                network[i][3] = i;
            }
        }
    }
}