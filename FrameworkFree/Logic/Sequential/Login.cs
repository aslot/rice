using Own.MarkupHandlers;
using Own.Permanent;
using Own.Storage;
using Own.Security;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void InitPageByTimerVoid()
        {
            var captchaData = Captcha.GenerateCaptchaStringAndImage();
            Fast.CaptchaMessagesEnqueueLocked(captchaData.stringHash);

            if (Fast.GetCaptchaMessagesCountLocked() == Constants.LoginPagesCount)
                Fast.CaptchaMessagesDequeueLocked();
            Fast.SetCaptchaPageToReturnLocked(Marker.GenerateLoginPage(captchaData.image));
        }
    }
}