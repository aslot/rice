using SkiaSharp;
using System.IO;
using Own.Permanent;
namespace Inclusions
{
    internal static class Graphics
    {
        internal static byte[] GetScaledProfileImage(in byte[] file)
        {
            SKBitmap image;

            using (var ms = new MemoryStream(file))
                image = SKBitmap.Decode(ms);
            image = image.Resize(new SKSizeI(
                Constants.ProfileImageWidthPixels,
            Constants.ProfileImageHeightPixels), SKFilterQuality.High);

            using (var ms = new MemoryStream())
            {
                image.Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(ms);
                image.Dispose();

                return ms.ToArray();
            }
        }
    }
}