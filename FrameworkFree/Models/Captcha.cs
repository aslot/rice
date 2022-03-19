using System;
using System.Text;
using Own.Types;
using Own.MarkupHandlers;
using Inclusions;
using Own.Permanent;
using Own.Storage;
namespace Own.Security
{
    internal static class Captcha
    {
        internal static CaptchaStringAndImage GenerateCaptchaStringAndImage()
        {
            var captchaString = GenerateCaptchaString();
            var bag = new CaptchaStringAndImage
            {
                stringHash = XXHash32.Hash(captchaString),
                image = Marker.GenerateCaptchaMarkup(
                    Convert.ToBase64String(
                    CaptchaGenerator.GenerateImageAsByteArray(captchaString)))
            };

            return bag;
        }

        private static string GenerateCaptchaString()
        {
            StringBuilder sb = new StringBuilder();

            for (byte i = Constants.Zero; i < Constants.CaptchaLength; i++)
                sb.Append(Fast.GetNextRandomCaptchaSymbolLocked());

            return sb.ToString();
        }
    }
}