namespace MarkupHandlers
{
    internal sealed class CaptchaMarkupHandler
    {
        internal string GenerateCaptchaMarkup(in string captcha)
        {
            return string.Concat("<img src='data:image/gif;base64,",
                captcha,
                "' />");
        }
    }
}