using SysThread = System.Threading;
using SysTime = System.Timers;
using System;
using System.Diagnostics;
using System.Net;
using Own.Types;
using Own.Permanent;
using Own.Storage;
using static Own.DataLockers.Lockers;
namespace App.Controllers
{//временная замена Storage - проброс вызовов - Slow делает Fast = Slow
    internal static class FriendlyFire
    {
        internal static void FillStorageVoid()
        { // перед изменением порядка следования проверять корректность правки
            Fast.InitializeRandomLocked();
            Own.Sequential.Unstable.LoadAccountsVoid();
            Own.Sequential.Unstable.LoadNicksVoid();
            Own.Sequential.Unstable.LoadMainPageVoid();
            Own.Sequential.Unstable.LoadSectionPagesVoid();
            Own.Sequential.Unstable.LoadThreadPagesVoid();
            Own.Sequential.Unstable.LoadEndPointPagesVoid();
            Own.Sequential.Unstable.LoadDialogPagesVoid();
            Own.Sequential.Unstable.LoadPersonalPagesVoid();
            Own.Sequential.Unstable.LoadProfilesVoid();
            Fast.InitializeCaptchaMessagesRegistrationDataLocked();
            Fast.InitializeCaptchaMessagesLocked();
            Slow.InitializeBlockedIpsHashesVoid();
        }
        internal static void StartTimerVoid()
        {
            Process.GetCurrentProcess().PriorityBoostEnabled = true;
            SysThread.Thread.CurrentThread.Priority = SysThread.ThreadPriority.Highest;
            Fast.InitializeRegistrationLineLocked();
            SysTime.Timer commonTimer = new SysTime.Timer(Constants.TimerPeriodMilliseconds);
            commonTimer.Elapsed += TimerEventHandlerVoid;
            commonTimer.AutoReset = true;
            commonTimer.Start();
        }
        private static void TimerEventHandlerVoid(Object source, SysTime.ElapsedEventArgs e)
        {
            if (Fast.CheckIfTimerIsWorkingLocked())
            { }
            else
            {
                Fast.SetTimerIsWorkingFlagLocked();
                Own.Sequential.Unstable.FlushAccountIdentifierRemoteIpLogByTimerVoid();
                Own.Sequential.Unstable.InitPageByTimerVoid();
                Own.Sequential.Unstable.RefreshLogRegPagesByTimerVoid();
                Own.Sequential.Unstable.CheckAccountIdByTimerVoid();
                Own.Sequential.Unstable.RegisterInBaseByTimerVoid();
                Own.Sequential.Unstable.StartNextTopicByTimerVoid();
                Own.Sequential.Unstable.PublishNextMessageByTimerVoid();
                Own.Sequential.Unstable.PublishNextPrivateMessageByTimerVoid();
                Own.Sequential.Unstable.StartNextDialogByTimerVoid();
                Own.Sequential.Unstable.PutRegInfoByTimerVoid();
                Fast.DecrementAllRemoteIpHashesAttemptsCountersAndRemoveUnnecessaryByTimerLocked();
                Own.Sequential.Unstable.HandleAndSaveProfilesByTimerVoid();
                Fast.ResetTimerIsWorkingFlagLocked();
            }
        }
        internal static void InitializeStorageVoid()
        {
            Fast.InitializePreSaveProfilesLineLocked();
            Fast.InitializeOwnProfilePagesLocked();
            Fast.InitializePublicProfilePagesLocked();
            Fast.InitializeAccountIdentifierRemoteIpLogLocked();
            Fast.InitializePreRegistrationLineLocked();
            Fast.InitializeTopicsToStartLocked();
            Fast.InitializeMessagesToPublishLocked();
            Fast.InitializePrivateMessagesLocked();
            Fast.InitializePersonalMessagesToPublishLocked();
            Fast.InitializeDialogsToStartLocked();
            Fast.InitializeRemoteIpHashesAttemptsCounterLocked();
        }
        internal static void InitializeVoid()
        {
            lock (InitializationTransactionLocker)
            {
                InitializeStorageVoid();
                FillStorageVoid();
                StartTimerVoid();
            }
        }
        internal static bool CheckIp(IPAddress ipAddress, byte incValue = Constants.Fifty)
        => Fast.CheckIp(ipAddress, incValue);
#if DEBUG
        internal static void RemoveAccountByNickIfExistsVoid(string uniqueNick)
        => Slow.RemoveAccountByNickIfExistsVoid(uniqueNick);
#endif
        internal static string ForumGetMainPageNullable()
           => Fast.GetMainPageLocked();
        internal static Tuple<bool, int> AuthneticationLogicAccessGrantedExtendedNullable(string token)
            => Own.InRace.Unstable.AccessGrantedEntendedNullable(token);
        internal static string GetPublicProfilePageIfExistsNullable(int accountId)
            => Fast.GetPublicProfilePageLocked(accountId);
        internal static string GetOwnProfilePageNullable(int accountId)
            => Fast.GetOwnProfilePageLocked(accountId);
        internal static string ThreadGetThreadPageNullable(int? id, int? page)
           => Own.InRace.Unstable.GetThreadPageNullable(id, page);
        internal static string ForumGetMainContentLockedNullable()
           => Fast.GetMainContentLocked();
        internal static string SectionGetSectionPageNullable(int? id, int? page)
         => Own.InRace.Unstable.GetSectionPageNullable(id, page);
        internal static string EndPointGetEndPointPageNullable(int? id)
        => Own.InRace.Unstable.GetEndPointPageNullable(id);
        internal static string LoginCheckAndAuthNullable(IPAddress ip, string captcha, string login, string password)
         => Own.InRace.Unstable.CheckAndAuthNullable(ip, captcha, login, password);
        internal static string GetRegistrationDataPageToReturnNullable()
        => Fast.GetPageToReturnRegistrationDataLocked();
        internal static bool AuthenticationAccessGranted(string token)
        => Own.InRace.Unstable.AccessGranted(token);
        internal static void ReplyStartVoid(int? id, Pair pair, string t)
        => Own.InRace.Unstable.StartReplyVoid(id, pair, t);
        internal static Pair AuthenticationGetPair(string token)
        => Own.InRace.Unstable.GetPair(token);
        internal static string LoginGetPageToReturnNullable()
        => Fast.GetCaptchaPageToReturnLocked();
        internal static void ProfileStartVoid(int accountId, string aboutMe,
            bool[] flags, byte[] file)
        => Own.InRace.Unstable.StartProfileVoid(accountId, aboutMe, flags, file);
        internal static int GetDialogPagesLengthFast()
        => Fast.GetDialogPagesLengthLocked();
        internal static void RegistrationPreRegistrationVoid(string captcha, string login,
            string password, string secret, string nick)
        => Own.InRace.Unstable.PreRegistrationVoid(captcha, login, password, secret, nick);
        internal static void NewTopicStartVoid(string t, int? id, Pair pair, string m)
        => Own.InRace.Unstable.StartNewTopicVoid(t, id, pair, m);
        internal static string PrivateDialogGetDialogPageNullable(int? id, Pair pair)
        => Own.InRace.Unstable.GetDialogPageNullable(id, pair);
        internal static string PrivateMessageGetPersonalPageNullable(int? id, int? page, Pair pair)
        => Own.InRace.Unstable.GetPersonalPageNullable(id, page, pair);
        internal static void NewPrivateMessageStartVoid(int? id, Pair pair, string t)
        => Own.InRace.Unstable.StartNewPrivateMessageVoid(id, pair, t);
        internal static void NewPrivateDialogStartVoid(string nick, Pair pair, string msg)
        => Own.InRace.Unstable.StartNewPrivateDialogVoid(nick, pair, msg);
    }
}