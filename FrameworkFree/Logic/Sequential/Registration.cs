using Own.Permanent;
using Own.Storage;
using Own.Types;
using Inclusions;
using System.Text.RegularExpressions;
using Own.MarkupHandlers;
using Own.Security;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        private static readonly object registrationLocker = new object();
        internal static void RegisterInBaseByTimerVoid()
        {
            int len = Fast.GetRegistrationLineCountLocked();

            if (len != Constants.Zero)
            {
                for (int i = Constants.Zero;
                        i < len; i++)
                {
                    RegBag regBag = new RegBag();
                    Fast.RegistrationLineRemoveLocked(i, out regBag);
                    SendToBaseVoid(regBag);
                }
            }
        }
        internal static bool CheckNick(in string nick)
        {
            bool result = false;
            int len = nick.Length;
            if ((len >= 4) && (len <= Constants.MaxNickTextLength))
            {
                if (new Regex(nick).Matches(" ").Count <= 3)
                {
                    if ((nick[Constants.Zero] != ' ')
                          && (nick[len - Constants.One] != ' '))
                    {
                        string nik = string.Join(Constants.SE, nick.Split(' '));
                        len = nik.Length;

                        if ((len >= Constants.One) && (len <= 22))
                        {
                            bool flag = false;

                            foreach (char x in nik)
                            {
                                if ((char.IsUpper(x) ?
                                        !Constants.AlphabetRusLower.Contains(char.ToLower(x)) :
                                        !Constants.AlphabetRusLower.Contains(x))
                                    && !char.IsDigit(x))
                                {
                                    flag = true;

                                    break;
                                }
                            }

                            if (!flag)
                                result = true;
                        }
                    }
                }
            }

            return result;
        }
        internal static void PutRegInfoByTimerVoid()
        {
            PreRegBag temp = new PreRegBag();
            if (Fast.GetPreRegistrationLineCountLocked() > Constants.Zero)
            {
                Fast.PreRegistrationLineRemoveLocked(Constants.Zero, out temp);
                if (temp.captcha == null || temp.login == null
            || temp.password == null || temp.secret == null || temp.nick == null)
                { }
                else
                {
                    CheckInputAndRegisterVoid(
                        temp.captcha,
                        temp.login,
                        temp.password,
                        temp.secret,
                        temp.nick
                        );
                }
            }
        }
        private static bool CheckSecret(in string secret)
        {
            return secret.Length <= 50;
        }
        internal static void RefreshLogRegPagesByTimerVoid()
        {
            var captchaData = Captcha.GenerateCaptchaStringAndImage();
            Fast.CaptchaMessagesRegistrationDataEnqueueLocked(captchaData.stringHash);

            if (Fast.GetCaptchaMessagesRegistrationDataCountLocked()
                == Constants.RegistrationPagesCount)
                Fast.CaptchaMessagesRegistrationDataDequeueLocked();
            Fast.SetPageToReturnRegistrationDataLocked(Marker
                .GetPageToReturnRegistrationData(captchaData.image));
        }
        private static void CheckInputAndRegisterVoid
            (in string captcha, in string login, in string password,
            in string secret, in string nick)
        {
            if (CheckNick(nick))
            {
                if (CheckSecret(secret))
                {
                    if (Own.InRace.Unstable.CheckPassword(password))
                    {
                        if (Own.InRace.Unstable.CheckLogin(login))
                        {
                            uint captchaHash = XXHash32.Hash(captcha);

                            if (Fast.CaptchaMessagesRegistrationDataContainsLocked(captchaHash)
#if DEBUG
                                    || captchaHash == Constants.TestRegistrationEnterCaptchaHash
#endif
                                )
                            {
                                uint loginHash = XXHash32.Hash(login);
                                uint passwordHash = XXHash32.Hash(password);
                                uint nickHash = XXHash32.Hash(nick);
                                uint secretHash = XXHash32.Hash(secret);

                                if (Register(loginHash, passwordHash, nickHash))
                                {
                                    Fast.RegistrationLineAddLocked(Fast.GetRegistrationLineCountLocked(),
                                        new RegBag
                                        {
                                            loginHash = loginHash,
                                            passwordHash = passwordHash,
                                            secretHash = secretHash,
                                            nick = nick
                                        }
                                        );
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool Register
                (in uint loginHash, in uint passwordHash, in uint nickHash)
        {
            bool result = false;
            var pair = new Pair
            { LoginHash = loginHash, PasswordHash = passwordHash };
            lock (registrationLocker)
                if (!Fast.LoginPasswordHashesContainsKeyLocked(pair))
                    if (!Fast.NicksHashesKeysContainsLocked(nickHash))
                        if (!Fast.LoginPasswordHashesDeltaContainsKeyLocked(pair))
                        {
                            Fast.LoginPasswordHashesAdd(pair, null);
                            Fast.NicksHashesAddLocked(nickHash, Constants.Zero);
                            Fast.LoginPasswordHashesDeltaAddLocked(pair, Constants.Zero);
                            Fast.CopyDialogPagesArraysToIncreasedSizeArraysAndFillGapsLocked();
                            result = true;
                        }

            return result;
        }
        private static void SendToBaseVoid(in RegBag account)
        {
            var dbAccount = new Own.Database.Account
            {
                Nick = account.nick,
                Identifier = (int)account.loginHash,
                Passphrase = (int)account.passwordHash,
                SecretHash = (int)account.secretHash
            };

            Slow.AddAccountVoid(dbAccount);
        }
    }
}