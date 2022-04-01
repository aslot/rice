namespace Own.MarkupHandlers
{
    internal static partial class Marker
    {
        internal static string GenerateCaptchaMarkup(in string captcha)
        {
            return string.Concat("<img src='data:image/jpeg;base64,",
                captcha,
                "' />");
        }
    }
}