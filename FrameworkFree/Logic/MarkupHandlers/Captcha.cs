namespace Own.MarkupHandlers
{
    internal static partial class Marker
    {
        internal static string GenerateCaptchaMarkup(in string captcha)
        {
            return string.Concat("<img height='50px' width='150px' src='data:image/jpeg;base64,",
                captcha,
                "' />");
        }
    }
}