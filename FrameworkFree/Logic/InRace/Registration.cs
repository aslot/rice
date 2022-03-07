using System;
using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static void PreRegistrationVoid
            (in string captcha, in string login, in string password, in string secret, in string nick)
        {
            if (Fast.GetPreRegistrationLineCountLocked() < Constants.MaxFirstLineLength)
            {
                var bag = new PreRegBag()
                {
                    captcha = captcha,
                    login = login,
                    password = password,
                    secret = secret,
                    nick = nick
                };
                Fast.PreRegistrationLineAddLocked(Fast.GetPreRegistrationLineCountLocked(), bag);
            }
        }

        internal static bool CheckLogin(in string login)
        {
            bool result = false;
            int len = login.Length;

            if ((len >= 6) && (len <= 25))
            {
                bool flag = false;

                foreach (char x in login)
                {
                    if ((!Char.IsDigit(x))
                        && (Char.IsUpper(x) ?
                                 !Constants.AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !Constants.AlphabetRusLower.Contains(x)))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                    result = true;
            }

            return result;
        }

        internal static bool CheckPassword(in string password)
        {
            bool result = false;
            int len = password.Length;

            if ((len >= 8) && (len <= 50))
            {
                bool flag = false;
                foreach (char x in password)
                {
                    if (
                            (Char.IsUpper(x) ?
                                 !Constants.AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !Constants.AlphabetRusLower.Contains(x))
                            && (!Char.IsDigit(x)
                        )
                        &&
                            (Char.IsUpper(x) ?
                                 !Constants.AlphabetRusLower.Contains(Char.ToLower(x)) :
                                    !Constants.AlphabetRusLower.Contains(x)) &&
                        (!Fast.SpecialSearchLocked(x)))
                    {
                        flag = true;

                        break;
                    }
                }
                if (!flag)
                    result = true;
            }

            return result;
        }
    }
}
