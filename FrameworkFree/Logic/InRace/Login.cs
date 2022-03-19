using Inclusions;
using System.Net;
using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static string CheckAndAuthNullable
               (in IPAddress ip, in string captcha, in string login,
            in string password)
        {
            if (ip == null || captcha == null || login == null || password == null)
            { }
            else
            {
                if (Unstable.CheckPassword(password))
                {
                    if (Unstable.CheckLogin(login))
                    {
                        uint captchaHash = XXHash32.Hash(captcha);

                        if (Fast.CaptchaMessagesContainsLocked(captchaHash)
#if DEBUG
                        || captchaHash == Constants.TestAuthenticationEnterCaptchaHash
#endif
                        )
                        {
                            Pair? pair = Own.InRace.Unstable.CheckPairNullable(login, password);

                            if (pair.HasValue)
                                return Own.InRace.Unstable.AcceptNullable(ip, pair.Value);
                        }
                    }
                }
            }

            return Constants.SE;
        }
    }
}
