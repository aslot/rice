using SysThread = System.Threading;
using SysTime = System.Timers;
using System;
using System.Diagnostics;
using System.Net;
using static Data.DataLockers.Lockers;
namespace Data
{//временная замена Storage - проброс вызовов - Slow делает Fast = Slow
    public sealed class FriendlyFire : IFriendlyFire
    {
        public readonly IPrivateDialogLogic PrivateDialogLogic;
        public readonly IPrivateMessageLogic PrivateMessageLogic;
        public readonly IAccountLogic AccountLogic;
        public readonly IEndPointLogic EndPointLogic;
        public readonly IForumLogic ForumLogic;
        public readonly IStorage Storage;
        public readonly INewPrivateDialogLogic NewPrivateDialogLogic;
        public readonly INewPrivateMessageLogic NewPrivateMessageLogic;
        public readonly ISectionLogic SectionLogic;
        public readonly INewTopicLogic NewTopicLogic;
        public readonly IThreadLogic ThreadLogic;
        public readonly IReplyLogic ReplyLogic;
        public readonly IRegistrationLogic RegistrationLogic;
        public readonly ILoginLogic LoginLogic;
        public readonly Captcha Captcha;
        public readonly IAuthenticationLogic AuthenticationLogic;
        public readonly IProfileLogic ProfileLogic;
        public FriendlyFire(IAccountLogic accountLogic,
        IStorage storage,
        IEndPointLogic endPointLogic,
        IPrivateDialogLogic privateDialogLogic,
        IPrivateMessageLogic privateMessageLogic,
        IForumLogic forumLogic,
        INewPrivateDialogLogic newPrivateDialogLogic,
        INewPrivateMessageLogic newPrivateMessageLogic,
        ISectionLogic sectionLogic,
        INewTopicLogic newTopicLogic,
        IThreadLogic threadLogic,
        IReplyLogic replyLogic,
        IRegistrationLogic registrationLogic,
        ILoginLogic loginLogic,
        Captcha captcha,
        IAuthenticationLogic authenticationLogic,
        IProfileLogic profileLogic)
        {
            Storage = storage;
            AccountLogic = accountLogic;
            EndPointLogic = endPointLogic;
            PrivateDialogLogic = privateDialogLogic;
            PrivateMessageLogic = privateMessageLogic;
            ForumLogic = forumLogic;
            NewPrivateMessageLogic = newPrivateMessageLogic;
            SectionLogic = sectionLogic;
            NewTopicLogic = newTopicLogic;
            ThreadLogic = threadLogic;
            ReplyLogic = replyLogic;
            RegistrationLogic = registrationLogic;
            LoginLogic = loginLogic;
            Captcha = captcha;
            AuthenticationLogic = authenticationLogic;
            NewPrivateDialogLogic = newPrivateDialogLogic;
            ProfileLogic = profileLogic;
            Initialize();
        }
        public void FillStorage()
        { // перед изменением порядка следования проверять корректность правки
            AccountLogic.LoadAccounts();
            AccountLogic.LoadNicks();
            ForumLogic.LoadMainPage();
            SectionLogic.LoadSectionPages();
            ThreadLogic.LoadThreadPages();
            EndPointLogic.LoadEndPointPages();
            PrivateDialogLogic.LoadDialogPages();
            PrivateMessageLogic.LoadPersonalPages();
            ProfileLogic.LoadProfiles();
            Storage.Fast.InitializeCaptchaMessagesRegistrationData();
            Storage.Fast.InitializeCaptchaMessages();
            Storage.Slow.InitializeBlockedIpsHashes();
        }
        public void StartTimer()
        {
            Process.GetCurrentProcess().PriorityBoostEnabled = true;
            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; Exception - permission denied
            SysThread.Thread.CurrentThread.Priority = SysThread.ThreadPriority.Highest;
            Storage.Fast.InitializeRegistrationLine();
            SysTime.Timer commonTimer = new SysTime.Timer(1000);
            commonTimer.Elapsed += TimerEventHandler;
            commonTimer.AutoReset = true;
            commonTimer.Start();
        }
        private void TimerEventHandler(Object source, SysTime.ElapsedEventArgs e)
        {
            if (Storage.Fast.CheckIfTimerIsWorking())
            { }
            else
            {
                Storage.Fast.SetTimerIsWorkingFlag();
                AuthenticationLogic.FlushAccountIdentifierRemoteIpLogByTimer();
                Captcha.RefreshLogRegPagesByTimer();
                AccountLogic.CheckAccountIdByTimer();
                RegistrationLogic.RegisterInBaseByTimer();
                NewTopicLogic.StartNextTopicByTimer();
                ReplyLogic.PublishNextMessageByTimer();
                NewPrivateMessageLogic.PublishNextPrivateMessageByTimer();
                NewPrivateDialogLogic.StartNextDialogByTimer();
                RegistrationLogic.PutRegInfoByTimer();
                Storage.Fast.DecrementAllRemoteIpHashesAttemptsCountersAndRemoveUnnecessaryByTimer();
                ProfileLogic.HandleAndSaveProfilesByTimer();
                Storage.Fast.ResetTimerIsWorkingFlag();
            }
        }
        public void InitializeStorage()
        {
            Storage.Fast.InitializePreSaveProfilesLine();
            Storage.Fast.InitializeOwnProfilePages();
            Storage.Fast.InitializePublicProfilePages();
            Storage.Fast.InitializeAccountIdentifierRemoteIpLog();
            Storage.Fast.InitializePreRegistrationLine();
            Storage.Fast.InitializeTopicsToStart();
            Storage.Fast.InitializeMessagesToPublish();
            Storage.Fast.InitializePrivateMessages();
            Storage.Fast.InitializePersonalMessagesToPublish();
            Storage.Fast.InitializeDialogsToStart();
            Storage.Fast.InitializeRemoteIpHashesAttemptsCounter();
        }
        public void Initialize()
        {
            lock (InitializationTransactionLocker)
            {
                InitializeStorage();
                FillStorage();
                StartTimer();
            }
        }
        public bool CheckIp(IPAddress ipAddress, byte incValue = Constants.Fifty)
        => Storage.Fast.CheckIp(ipAddress, incValue);
        public void RemoveAccountByNickIfExists(string uniqueNick)
            => Storage.Slow.RemoveAccountByNickIfExists(uniqueNick);
        public string ForumLogic_GetMainPageLocked()
           => Storage.Fast.GetMainPageLocked();
        public Tuple<bool, int> AuthneticationLogic_AccessGrantedExtended(string token)
            => AuthenticationLogic.AccessGrantedEntended(token);
        public string GetPublicProfilePageIfExists(int accountId)
            => Storage.Fast.GetPublicProfilePage(accountId);
        public string GetOwnProfilePage(int accountId)
            => Storage.Fast.GetOwnProfilePage(accountId);
        public string ThreadData_GetThreadPage(int? id, int? page)
           => ThreadLogic.GetThreadPage(id, page);
        public string ForumLogic_GetMainContentLocked()
           => Storage.Fast.GetMainContentLocked();
        public string SectionLogic_GetSectionPage(int? id, int? page)
         => SectionLogic.GetSectionPage(id, page);
        public string EndPointLogic_GetEndPointPage(int? id)
        => EndPointLogic.GetEndPointPage(id);
        public string LoginData_CheckAndAuth(IPAddress ip, string captcha, string login, string password)
         => LoginLogic.CheckAndAuth(ip, captcha, login, password);
        public string GetRegistrationDataPageToReturn()
        => Storage.Fast.GetPageToReturnRegistrationData();
        public bool AuthenticationLogic_AccessGranted(string token)
        => AuthenticationLogic.AccessGranted(token);
        public void ReplyData_Start(int? id, Pair pair, string t)
        => ReplyLogic.Start(id, pair, t);
        public Pair AuthenticationLogic_GetPair(string token)
        => AuthenticationLogic.GetPair(token);
        public string LoginData_GetPageToReturn()
        => Storage.Fast.GetCaptchaPageToReturn();
        public void ProfileLogic_Start(int accountId, string aboutMe,
            bool[] flags, byte[] file)
        => ProfileLogic.Start(accountId, aboutMe, flags, file);
        public int GetDialogPagesLengthFast()
        => Storage.Fast.GetDialogPagesLengthLocked();
        public void RegistrationData_PreRegistration(string captcha, string login,
            string password, string email, string nick)
        => RegistrationLogic.PreRegistration(captcha, login, password, email, nick);
        public void NewTopicData_Start(string t, int? id, Pair pair, string m)
        => NewTopicLogic.Start(t, id, pair, m);
        public string PrivateDialogLogic_GetDialogPage(int? id, Pair pair)
        => PrivateDialogLogic.GetDialogPage(id, pair);
        public string PrivateMessageLogic_GetPersonalPage(int? id, int? page, Pair pair)
        => PrivateMessageLogic.GetPersonalPage(id, page, pair);
        public void NewPrivateMessageLogic_Start(int? id, Pair pair, string t)
        => NewPrivateMessageLogic.Start(id, pair, t);
        public void NewPrivateDialogLogic_Start(string nick, Pair pair, string msg)
        => NewPrivateDialogLogic.Start(nick, pair, msg);
    }
}