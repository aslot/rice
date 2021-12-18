using System;
using System.Text.RegularExpressions;
using MarkupHandlers;
using Models;
using XXHash;

namespace Data
{
    internal sealed class RegistrationLogic : IRegistrationLogic
    {
        private readonly IStorage Storage;
        private readonly RegistrationMarkupHandler RegistrationMarkupHandler;
        public RegistrationLogic(IStorage storage,
            RegistrationMarkupHandler registrationMarkupHandler)
        {
            Storage = storage;
            RegistrationMarkupHandler = registrationMarkupHandler;
        }
        public void PreRegistration
            (in string captcha, in string login, in string password, in string email, in string nick)
        {
            if (Storage.Fast.GetPreRegistrationLineCount() < Constants.MaxFirstLineLength)
            {
                var bag = new PreRegBag()
                {
                    captcha = captcha,
                    login = login,
                    password = password,
                    email = email,
                    nick = nick
                };
                Storage.Fast.PreRegistrationLineAdd(Storage.Fast.GetPreRegistrationLineCount(), bag);
            }
        }

        private static readonly object locker = new object();

        public void InitPage()
        {
            var captchaData = Captcha.GenerateCaptchaStringAndImage();
            Storage.Fast.CaptchaMessagesRegistrationDataEnqueue(captchaData.stringHash);

            if (Storage.Fast.GetCaptchaMessagesRegistrationDataCount()
                == Constants.RegistrationPagesCount)
                Storage.Fast.CaptchaMessagesRegistrationDataDequeue();
            Storage.Fast.SetPageToReturnRegistrationData(RegistrationMarkupHandler
                .GetPageToReturnRegistrationData(captchaData.image));
        }
        public void RegisterInBaseByTimer()
        {
            int len = Storage.Fast.GetRegistrationLineCount();

            if (len != Constants.Zero)
            {
                for (int i = Constants.Zero;
                        i < len; i++)
                {
                    RegBag regBag = new RegBag();
                    Storage.Fast.RegistrationLineRemove(i, out regBag);
                    SendToBase(regBag);
                }
            }
        }

        private void SendToBase(in RegBag account)
        {
            var dbAccount = new Account
            {
                Nick = account.nick,
                Identifier = (int)account.loginHash,
                Passphrase = (int)account.passwordHash,
                EmailHash = (int)account.emailHash
            };

            Storage.Slow.AddAccount(dbAccount);
        }

        public void PutRegInfoByTimer()
        {
            PreRegBag temp = new PreRegBag();
            if (Storage.Fast.GetPreRegistrationLineCount() > Constants.Zero)
            {
                Storage.Fast.PreRegistrationLineRemove(Constants.Zero, out temp);
                if (temp.captcha == null || temp.login == null
            || temp.password == null || temp.email == null || temp.nick == null)
                { }
                else
                {
                    CheckInputAndRegister(
                        temp.captcha,
                        temp.login,
                        temp.password,
                        temp.email,
                        temp.nick
                        );
                }
            }
        }

        private bool Register
                (in uint loginHash, in uint passwordHash, in uint nickHash)
        {
            bool result = false;
            var pair = new Pair
            { LoginHash = loginHash, PasswordHash = passwordHash };
            lock (locker)
                if (!Storage.Fast.LoginPasswordHashesContainsKey(pair))
                    if (!Storage.Fast.NicksHashesKeysContains(nickHash))
                        if (!Storage.Fast.LoginPasswordHashesDeltaContainsKey(pair))
                        {
                            Storage.Fast.LoginPasswordHashesAdd(pair, null);
                            Storage.Fast.NicksHashesAdd(nickHash, Constants.Zero);
                            Storage.Fast.LoginPasswordHashesDeltaAdd(pair, Constants.Zero);
                            Storage.Fast.CopyDialogPagesArraysToIncreasedSizeArraysAndFillGapsLocked();
                            result = true;
                        }

            return result;
        }

        private void CheckInputAndRegister
            (in string captcha, in string login, in string password,
            in string email, in string nick)
        {
            if (CheckNick(nick))
            {
                if (CheckEmail(email))
                {
                    if (CheckPassword(password))
                    {
                        if (CheckLogin(login))
                        {
                            uint captchaHash = XXHash32.Hash(captcha);

                            if (Storage.Fast.CaptchaMessagesRegistrationDataContains(captchaHash)
                                || captchaHash == Constants.TestRegistrationEnterCaptchaHash)
                            {
                                uint loginHash = XXHash32.Hash(login);
                                uint passwordHash = XXHash32.Hash(password);
                                uint nickHash = XXHash32.Hash(nick);
                                uint emailHash = XXHash32.Hash(email);

                                if (Register(loginHash, passwordHash, nickHash))
                                {
                                    Storage.Fast.RegistrationLineAdd(Storage.Fast.GetRegistrationLineCount(),
                                        new RegBag
                                        {
                                            loginHash = loginHash,
                                            passwordHash = passwordHash,
                                            emailHash = emailHash,
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

        public bool CheckLogin(in string login)
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

        public bool CheckPassword(in string password)
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
                        (!Storage.Fast.SpecialSearch(x)))
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
        private static bool CheckEmail(in string email)
        {
            if (email.Length > 320)
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckNick(in string nick)
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
                        string nik = String.Join(Constants.SE, nick.Split(' '));
                        len = nik.Length;

                        if ((len >= Constants.One) && (len <= 22))
                        {
                            bool flag = false;

                            foreach (char x in nik)
                            {
                                if ((Char.IsUpper(x) ?
                                        !Constants.AlphabetRusLower.Contains(Char.ToLower(x)) :
                                        !Constants.AlphabetRusLower.Contains(x))
                                    && !Char.IsDigit(x))
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
    }
}
