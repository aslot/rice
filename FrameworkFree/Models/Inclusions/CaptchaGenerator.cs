using System.Collections.Generic;
using Own.Permanent;
using SkiaSharp;
using Own.Storage;
namespace Inclusions
{
    internal static class CaptchaGenerator
    {
        private static SKColor GetRandomDeepColor()
        {
            return new SKColor((byte)Fast.RandomStatic.Next(160), (byte)Fast.RandomStatic.Next(100), (byte)Fast.RandomStatic.Next(160));
        }

        private static SKColor GetRandomLightColor()
        {
            const byte low = 180, high = 255;

            return new SKColor((byte)(Fast.RandomStatic.Next(high) % (high - low) + low),
                (byte)(Fast.RandomStatic.Next(high) % (high - low) + low),
                (byte)(Fast.RandomStatic.Next(high) % (high - low) + low));
        }
        internal static byte[] GenerateImageAsByteArray(string captchaCode, int width = 150, int height = 50)
        {
            var info = new SKImageInfo(width, height);

            using (var surface = SKSurface.Create(info))
            {
                SKColor canvasColor;
                bool chooseLightFontColor = Fast.RandomStatic.Next(Constants.Zero, 11) > 5;

                if (chooseLightFontColor)
                    canvasColor = GetRandomDeepColor();
                else
                    canvasColor = GetRandomLightColor();

                SKCanvas canvas = surface.Canvas;
                canvas.Clear(canvasColor);
                SKPaint paint = new SKPaint
                {
                    TextSize = Fast.RandomStatic.Next(height / 2, height),
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    TextEncoding = SKTextEncoding.Utf8
                };

                float symbolPlaceholder = width / Constants.CaptchaLength;
                byte count = (byte)Fast.RandomStatic.Next(10, byte.MaxValue);
                List<SKPoint> points = new List<SKPoint>(count);

                for (byte i = 0; i < count; i++)
                    points.Add(new SKPoint(Fast.RandomStatic.Next(0, width + Constants.One), Fast.RandomStatic.Next(0, height + Constants.One)));

                for (byte i = Constants.Zero; i < Constants.CaptchaLength; i++)
                {
                    paint.Color = chooseLightFontColor ? GetRandomLightColor() : GetRandomDeepColor();
                    paint.StrokeWidth = Fast.RandomStatic.Next(Constants.Zero, 4);


                    canvas.DrawText(captchaCode[i].ToString(), symbolPlaceholder * i + Fast.RandomStatic.Next((int)(-symbolPlaceholder / 3),
                        (int)(symbolPlaceholder / 3)),
                        paint.TextSize + Fast.RandomStatic.Next((int)(-paint.TextSize / 3),
                        (int)(paint.TextSize / 3)), paint);
                }
                paint.StrokeWidth = 3;
                paint.Color = chooseLightFontColor ? GetRandomLightColor() : GetRandomDeepColor();
                canvas.DrawPoints(SKPointMode.Points, points.ToArray(), paint);
                canvas.Flush();

                return surface.Snapshot().Encode(SKEncodedImageFormat.Jpeg, 10).ToArray(); // падает на Gif
            }
        }
    }
}