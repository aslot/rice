using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Data;
// source - https://edi.wang/post/2018/10/13/generate-captcha-code-aspnet-core
public sealed class CaptchaGenerator : ICaptchaGenerator
{
    public byte[] GenerateImageAsByteArray(string captchaCode, int width, int height)
    {
        using (Bitmap baseMap = new Bitmap(width, height))
        using (Graphics graph = Graphics.FromImage(baseMap))
        {
            Random rand = new Random();
            graph.Clear(GetRandomLightColor());
            DrawCaptchaCode();
            DrawDisorderLine();

            return AdjustRippleEffect();

            int GetFontSize(int imageWidth, int captchCodeCount)
            {
                var averageSize = imageWidth / captchCodeCount;

                return Convert.ToInt32(averageSize);
            }

            Color GetRandomDeepColor()
            {
                int redlow = 160, greenLow = 100, blueLow = 160;
                return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
            }

            Color GetRandomLightColor()
            {
                int low = 180, high = 255;

                int nRend = rand.Next(high) % (high - low) + low;
                int nGreen = rand.Next(high) % (high - low) + low;
                int nBlue = rand.Next(high) % (high - low) + low;

                return Color.FromArgb(nRend, nGreen, nBlue);
            }

            void DrawCaptchaCode()
            {
                SolidBrush fontBrush = new SolidBrush(Color.Black);
                int fontSize = GetFontSize(width, captchaCode.Length);
                Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                for (int i = Constants.Zero; i < captchaCode.Length; i++)
                {
                    fontBrush.Color = GetRandomDeepColor();

                    int shiftPx = fontSize / 6;

                    float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                    int maxY = height - fontSize;
                    if (maxY < Constants.Zero) maxY = Constants.Zero;
                    float y = rand.Next(Constants.Zero, maxY);

                    graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                }
            }

            void DrawDisorderLine()
            {
                Pen linePen = new Pen(new SolidBrush(Color.Black), 3);
                for (byte i = Constants.Zero; i < rand.Next(3, 5); i++)
                {
                    linePen.Color = GetRandomDeepColor();
                    Point startPoint = new Point(rand.Next(Constants.Zero, width), rand.Next(Constants.Zero, height));
                    Point endPoint = new Point(rand.Next(Constants.Zero, width), rand.Next(Constants.Zero, height));
                    graph.DrawLine(linePen, startPoint, endPoint);
                }
            }

            byte[] AdjustRippleEffect()
            {
                short nWave = 6;
                int nWidth = baseMap.Width;
                int nHeight = baseMap.Height;

                Point[,] pt = new Point[nWidth, nHeight];

                for (int x = Constants.Zero; x < nWidth; ++x)
                {
                    for (int y = Constants.Zero; y < nHeight; ++y)
                    {
                        var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                        var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

                        var newX = x + xo;
                        var newY = y + yo;

                        if (newX > Constants.Zero && newX < nWidth)
                        {
                            pt[x, y].X = (int)newX;
                        }
                        else
                        {
                            pt[x, y].X = Constants.Zero;
                        }


                        if (newY > Constants.Zero && newY < nHeight)
                        {
                            pt[x, y].Y = (int)newY;
                        }
                        else
                        {
                            pt[x, y].Y = Constants.Zero;
                        }
                    }
                }

                Bitmap bSrc = (Bitmap)baseMap.Clone();

                BitmapData bitmapData = baseMap.LockBits(new Rectangle(Constants.Zero, Constants.Zero, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData bmSrc = bSrc.LockBits(new Rectangle(Constants.Zero, Constants.Zero, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                int scanline = bitmapData.Stride;

                IntPtr scan0 = bitmapData.Scan0;
                IntPtr srcScan0 = bmSrc.Scan0;

                unsafe
                {
                    byte* p = (byte*)(void*)scan0;
                    byte* pSrc = (byte*)(void*)srcScan0;

                    int nOffset = bitmapData.Stride - baseMap.Width * 3;

                    for (int y = Constants.Zero; y < nHeight; ++y)
                    {
                        for (int x = Constants.Zero; x < nWidth; ++x)
                        {
                            var xOffset = pt[x, y].X;
                            var yOffset = pt[x, y].Y;

                            if (yOffset >= Constants.Zero && yOffset < nHeight && xOffset >= Constants.Zero && xOffset < nWidth)
                            {
                                if (pSrc != null)
                                {
                                    p[Constants.Zero] = pSrc[yOffset * scanline + xOffset * 3];
                                    p[Constants.One] = pSrc[yOffset * scanline + xOffset * 3 + Constants.One];
                                    p[2] = pSrc[yOffset * scanline + xOffset * 3 + 2];
                                }
                            }

                            p += 3;
                        }
                        p += nOffset;
                    }
                }

                MemoryStream ms = new MemoryStream();
                baseMap.Save(ms, ImageFormat.Png);
                baseMap.UnlockBits(bitmapData);
                bSrc.UnlockBits(bmSrc);
                bSrc.Dispose();

                return ms.ToArray();
            }
        }
    }
}
