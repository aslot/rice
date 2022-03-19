using System.Collections.Generic;
using System;
using Inclusions;
using System.Net;
using System.Linq;
using Own.Permanent;
using Own.Types;
using static Own.DataLockers.Lockers;
namespace Own.Storage
{
    internal static partial class Fast
    {
        internal static bool LoginPasswordAccIdHashesContainsKeyLocked(in Pair pair)
        {
            lock (LoginPasswordAccIdHashesLocker)
                return LoginPasswordAccIdHashesStatic.ContainsKey(pair);
        }
        internal static string GetConnectionStringLocked()
        {
            lock (ConnectionStringLocker)
                return ConnectionStringStatic;
        }
        internal static void SetConnectionStringLocked(in string value)
        {
            lock (ConnectionStringLocker)
                ConnectionStringStatic = value;
        }
        internal static void LoginPasswordAccIdHashesAddLocked(in Pair pair, in int accountId)
        {
            lock (LoginPasswordAccIdHashesLocker)
                LoginPasswordAccIdHashesStatic.Add(pair, accountId);
        }
        internal static void LoginPasswordHashesDeltaRemoveLocked(in Pair pair, out byte result)
        {
            lock (LoginPasswordHashesDeltaLocker)
                LoginPasswordHashesDeltaStatic.Remove(pair, out result);
        }
        internal static IEnumerable<int> IterateThroughAccountIdsLocked()
        {
            lock (LoginPasswordAccIdHashesLocker)
                foreach (int accountId in LoginPasswordAccIdHashesStatic.Values)
                    yield return accountId;
        }
        internal static void LoginPasswordHashesDeltaAddLocked(in Pair pair, in byte val)
        {
            lock (LoginPasswordHashesDeltaLocker)
                LoginPasswordHashesDeltaStatic.Add(pair, Constants.Zero);
        }
        internal static bool LoginPasswordHashesDeltaContainsKeyLocked(in Pair pair)
        {
            lock (LoginPasswordHashesDeltaLocker)
                return LoginPasswordHashesDeltaStatic.ContainsKey(pair);
        }
        internal static int? GetLoginPasswordAccIdHashesLocked(in Pair pair)
        {
            lock (LoginPasswordAccIdHashesLocker)
                return LoginPasswordAccIdHashesStatic.ContainsKey(pair) ? LoginPasswordAccIdHashesStatic[pair] : null;
        }
        internal static void InitializeLoginPasswordAccIdHashesLocked()
        {
            lock (LoginPasswordAccIdHashesLocker)
                Fast.LoginPasswordAccIdHashesStatic = new Dictionary<Pair, int>();
        }
        internal static void InitializeLoginPasswordHashesLocked()
        {
            lock (LoginPasswordHashesLocker)
                Fast.LoginPasswordHashesStatic = new Dictionary<Pair, Guid?>();
        }
        internal static IEnumerable<Pair> GetLoginPasswordHashesDeltaKeysLocked()
        {
            lock (LoginPasswordHashesDeltaLocker)
                return LoginPasswordHashesDeltaStatic.Keys;
        }
        internal static void InitializeLoginPasswordHashesDeltaLocked()
        {
            lock (LoginPasswordHashesDeltaLocker)
                Fast.LoginPasswordHashesDeltaStatic = new Dictionary<Pair, byte>();
        }
        internal static void InitializePrivateMessagesLocked()
        {
            lock (PersonalPagesLocker)
            {
                Fast.PersonalPagesStatic = new
                    Dictionary
                        <OwnerId, Dictionary<CompanionId, PrivateMessages>>();

                lock (PersonalPagesDepthsLocker)
                    Fast.PersonalPagesDepthsStatic = new
                    Dictionary<OwnerId, Dictionary<CompanionId, int>>();
            }
        }
        internal static bool LoginPasswordHashesContainsKeyLocked(in Pair pair)
        {
            lock (LoginPasswordHashesLocker)
                return LoginPasswordHashesStatic.ContainsKey(pair);
        }
        internal static void LoginPasswordHashesAdd(in Pair pair, in Guid? guid)
        => LoginPasswordHashesStatic.Add(pair, null);
        internal static bool NicksHashesKeysContainsLocked(in uint hash)
        {
            lock (NicksHashesLocker)
                return NicksHashesStatic.ContainsKey(hash);
        }
        internal static void InitializeNicksHashesLocked()
        {
            lock (NicksHashesLocker)
                NicksHashesStatic = new Dictionary<uint, byte>();
        }
        private static bool CheckIfIpIsNotBannedLocked(in uint ipHash)
        {
            lock (BlockedRemoteIpsHashesLocker)
                return !BlockedRemoteIpsHashesStatic.Contains(ipHash);
        }
        internal static void InitializeAccountIdentifierRemoteIpLogLocked()
        {
            lock (AccountIdentifierRemoteIpLogLocker)
                AccountIdentifierRemoteIpLogStatic = new Queue<AccountIdentifierRemoteIp>();
        }
        internal static void AccountIdentifierRemoteIpLogEnqueueLocked(in AccountIdentifierRemoteIp value)
        {
            lock (AccountIdentifierRemoteIpLogLocker)
                AccountIdentifierRemoteIpLogStatic.Enqueue(value);
        }
        internal static Queue<AccountIdentifierRemoteIp> GetAccountIdentifierRemoteIpsLocked()
        {
            lock (AccountIdentifierRemoteIpLogLocker)
                return AccountIdentifierRemoteIpLogStatic;
        }
        internal static void ClearAccountIdentifierRemoteIpsLocked()
        {
            lock (AccountIdentifierRemoteIpLogLocker)
                AccountIdentifierRemoteIpLogStatic.Clear();
        }
        internal static void NicksHashesAddLocked(in uint nickHash, in byte temp)
        {
            lock (NicksHashesLocker)
                NicksHashesStatic.Add(nickHash, temp);
        }
        internal static string GetEndPointPageLocked(in int index)
        {
            lock (EndPointPagesLocker)
                return EndPointPagesStatic[index];
        }
        internal static void SetEndPointPageLocked(in int index, in string value)
        {
            lock (EndPointPagesLocker)
                EndPointPagesStatic[index] = value;
        }
        internal static void InitializeEndPointPagesLocked(in int size)
        {
            lock (EndPointPagesLocker)
                EndPointPagesStatic = new string[size];
        }
        internal static string GetMainContentLocked()
        {
            lock (MainContentLocker)
                return MainContentStatic;
        }
        internal static void SetMainContentLocked(in string value)
        {
            lock (MainContentLocker)
                MainContentStatic = value;
        }
        internal static bool CheckIp(in IPAddress ip, in byte value)
        {
            uint hash;

            if (ip == null)
                return false;
            else
                hash = XXHash32.Hash(ip.ToString());

            return CheckIfIpIsNotBannedLocked(hash) && IncrementWithValueRemoteIpHashesAttemptsCountersAndGrantAccessAndAddIfNotPresentedLocked(hash, value);
        }
        internal static string GetMainPageLocked()
        {
            lock (MainPageLocker)
                return MainPageStatic;
        }
        internal static void SetMainPageLocked(in string value)
        {
            lock (MainPageLocker)
                MainPageStatic = value;
        }
        internal static void AddToMainPageLocked(in string value)
        {
            lock (MainPageLocker)
                MainPageStatic += value;
        }
        internal static void DialogsToStartEnqueueLocked(in DialogData value)
        {
            lock (DialogsToStartLocker)
                DialogsToStartStatic.Enqueue(value);
        }
        internal static void InitializeDialogsToStartLocked()
        {
            lock (DialogsToStartLocker)
                DialogsToStartStatic = new Queue<DialogData>();
        }
        internal static DialogData DialogsToStartDequeueLocked()
        {
            lock (DialogsToStartLocker)
                return DialogsToStartStatic.Dequeue();
        }
        internal static void InitializePersonalMessagesToPublishLocked()
        {
            lock (PersonalMessagesToPublishLocker)
                PersonalMessagesToPublishStatic = new Queue<MessageData>();
        }
        internal static bool CheckIfTimerIsWorkingLocked()
        {
            lock (TimerIsWorkingLocker)
                return TimerIsWorkingStatic == Constants.One;
        }
        internal static void ResetTimerIsWorkingFlagLocked()
        {
            lock (TimerIsWorkingLocker)
                TimerIsWorkingStatic = Constants.Zero;
        }
        internal static void SetTimerIsWorkingFlagLocked()
        {
            lock (TimerIsWorkingLocker)
                TimerIsWorkingStatic = Constants.One;
        }
        internal static void PersonalMessagesToPublishEnqueueLocked(in MessageData value)
        {
            lock (PersonalMessagesToPublishLocker)
                PersonalMessagesToPublishStatic.Enqueue(value);
        }
        internal static MessageData PersonalMessagesToPublishDequeueLocked()
        {
            lock (PersonalMessagesToPublishLocker)
                return PersonalMessagesToPublishStatic.Dequeue();
        }
        internal static string[] GetDialogPagesArrayLockedLocked(in int index)
        {
            lock (DialogPagesLocker)
                return DialogPagesStatic[index];
        }
        internal static void SetDialogPagesArrayLocked(in int index, in string[] value)
        {
            lock (DialogPagesLocker)
                DialogPagesStatic[index] = value;
        }
        internal static string GetDialogPagesPageLocked(in int arrayIndex, in int pageIndex)
        {
            lock (DialogPagesLocker)
                return DialogPagesStatic[arrayIndex][pageIndex];
        }
        internal static void SetDialogPagesPageLocked
            (in int arrayIndex, in int pageIndex, in string value)
        {
            lock (DialogPagesLocker)
                DialogPagesStatic[arrayIndex][pageIndex] = value;
        }
        internal static void AddToDialogPagesPageLocked
            (in int arrayIndex, in int pageIndex, in string value)
        {
            lock (DialogPagesLocker)
                DialogPagesStatic[arrayIndex][pageIndex] += value;
        }
        internal static void InitializeDialogPagesLocked(in string[][] value)
        {
            lock (DialogPagesLocker)
                DialogPagesStatic = value;
        }
        internal static int GetDialogPagesLengthLocked()
        {
            lock (DialogPagesLengthLocker)
                return DialogPagesLengthStatic;
        }
        internal static void CopyDialogPagesArraysToIncreasedSizeArraysAndFillGapsLocked()
        {
            lock (DialogPagesLocker)
            {
                lock (DialogPagesPageDepthLocker)
                {
                    lock (DialogPagesLengthLocker)
                    {
                        int len = DialogPagesLengthStatic;
                        DialogPagesLengthStatic++;
                        string[][] temp = new string[DialogPagesLengthStatic][];
                        Array.Copy(DialogPagesStatic, temp, len);
                        DialogPagesStatic = temp;
                        DialogPagesStatic[len] = new string[Constants.One] { Constants.NewDialog };
                        int[] temp1 = new int[DialogPagesLengthStatic];
                        Array.Copy(DialogPagesPageDepthStatic, temp1, len);
                        DialogPagesPageDepthStatic = temp1;
                        DialogPagesPageDepthStatic[len] = Constants.One;
                    }
                }
            }
        }
        internal static void SetDialogPagesLengthLocked(in int value)
        {
            lock (DialogPagesLengthLocker)
                DialogPagesLengthStatic = value;
        }
        internal static void SetDialogPagesPageDepthLocked(in int index, in int value)
        {
            lock (DialogPagesPageDepthLocker)
                DialogPagesPageDepthStatic[index] = value;
        }
        internal static int GetDialogPagesPageDepthLocked(in int index)
        {
            lock (DialogPagesPageDepthLocker)
                return DialogPagesPageDepthStatic[index];
        }
        internal static void InitializeDialogPagesPageDepthLocked(in int[] value)
        {
            lock (DialogPagesPageDepthLocker)
                DialogPagesPageDepthStatic = value;
        }
        internal static string GetMessageLocked(in int ownerId, in int companionId, in int messageId)
        {
            lock (PersonalPagesLocker)
                return PersonalPagesStatic
                              [new OwnerId { Id = ownerId }]
                             [new CompanionId { Id = companionId }]
                             .Messages[messageId];
        }
        internal static string[] GetMessagesLocked(in int ownerId, in int companionId)
        {
            lock (PersonalPagesLocker)
                return PersonalPagesStatic
                    [new OwnerId { Id = ownerId }]
                    [new CompanionId { Id = companionId }]
                    .Messages;
        }
        internal static void AddToPersonalPagesDepthLocked(in int id, in int accountId)
        {
            lock (PersonalPagesDepthsLocker)
                PersonalPagesDepthsStatic
                 [new OwnerId { Id = accountId }]
                 [new CompanionId { Id = id }]
                 ++;
        }
        internal static int GetPersonalPagesDepthLocked(in int id, in int accountId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepthsStatic
                    [new OwnerId { Id = accountId }]
                    [new CompanionId { Id = id }];
        }
        internal static void SetPersonalPagesPageLocked
                (in int id, in int accountId, in int depth, in string page)
        {
            lock (PersonalPagesLocker)
                PersonalPagesStatic
                            [new OwnerId { Id = accountId }]
                            [new CompanionId { Id = id }]
                            .Messages[depth] = page;
        }
        internal static void SetPersonalPagesMessagesArrayLocked
                    (in int id, in int accountId, in string[] value)
        {
            lock (PersonalPagesLocker)
                PersonalPagesStatic
                            [new OwnerId { Id = accountId }]
                            [new CompanionId { Id = id }]
                            = new PrivateMessages { Messages = value };
        }
        internal static bool PersonalPagesContainsKeyLocked(in OwnerId ownerId, in CompanionId companionId, in bool flag)
        {
            lock (PersonalPagesLocker)
                return flag && PersonalPagesStatic[ownerId].ContainsKey(companionId);
        }
        internal static void PersonalPagesAddLocked(in OwnerId ownerId, in CompanionId companionId, in string[] newMsg, in bool flag)
        {
            lock (PersonalPagesLocker)
            {
                if (!flag)
                    PersonalPagesStatic.Add(ownerId, new Dictionary<CompanionId, PrivateMessages>());
                PersonalPagesStatic[ownerId].Add(companionId,
                        new PrivateMessages { Messages = newMsg });
            }
        }
        internal static bool PersonalPagesKeysContainsLocked(in OwnerId ownerId)
        {
            lock (PersonalPagesLocker)
                return PersonalPagesStatic.ContainsKey(ownerId);
        }
        internal static bool PersonalPagesDepthsContainsKeyLocked(in OwnerId ownerId, in CompanionId companionId, in bool flag)
        {
            lock (PersonalPagesDepthsLocker)
                return flag && PersonalPagesDepthsStatic[ownerId].ContainsKey(companionId);
        }
        internal static void PersonalPagesDepthsAddLocked(in OwnerId ownerId, in CompanionId companionId, in bool flag)
        {
            lock (PersonalPagesDepthsLocker)
            {
                if (!flag)
                    PersonalPagesDepthsStatic.Add(ownerId, new Dictionary<CompanionId, int>());
                PersonalPagesDepthsStatic[ownerId].Add(companionId, Constants.One);
            }
        }
        internal static bool PersonalPagesDepthsKeysContainsLocked(in OwnerId ownerId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepthsStatic.ContainsKey(ownerId);
        }
        internal static int GetPersonalPagesPageDepthLocked(in int accountId, in int companionId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepthsStatic
                    [new OwnerId { Id = accountId }]
                    [new CompanionId { Id = companionId }];
        }
        internal static void PersonalPagesAddLocked(in OwnerId ownerId,
                                         in Dictionary<CompanionId, PrivateMessages> temp1)
        {
            lock (PersonalPagesLocker)
                PersonalPagesStatic.Add(ownerId, temp1);
        }
        internal static void PersonalPagesDepthsAddLocked(in OwnerId ownerId,
                                                in Dictionary<CompanionId, int> temp2)
        {
            lock (PersonalPagesDepthsLocker)
                PersonalPagesDepthsStatic.Add(ownerId, temp2);
        }
        internal static string[] GetSectionPagesArrayLocked(in int index)
        {
            lock (SectionPagesLocker)
                return SectionPagesStatic[index];
        }
        internal static void SetSectionPagesArrayLocked(in int index, in string[] value)
        {
            lock (SectionPagesLocker)
                SectionPagesStatic[index] = value;
        }
        internal static string GetSectionPagesPageLocked(in int arrayIndex, in int pageIndex)
        {
            lock (SectionPagesLocker)
                return SectionPagesStatic[arrayIndex][pageIndex];
        }
        internal static void SetSectionPagesPageLocked(in int arrayIndex, in int pageIndex, in string value)
        {
            lock (SectionPagesLocker)
                SectionPagesStatic[arrayIndex][pageIndex] = value;
        }
        internal static void AddToSectionPagesPageLocked(in int arrayIndex, in int pageIndex, in string value)
        {
            lock (SectionPagesLocker)
                SectionPagesStatic[arrayIndex][pageIndex] =
                    string.Concat(SectionPagesStatic[arrayIndex][pageIndex], value);
        }
        internal static void InitializeSectionPagesLocked(in string[][] value)
        {
            lock (SectionPagesLocker)
                SectionPagesStatic = value;
        }
        internal static int GetSectionPagesLengthLocked()
        {
            lock (SectionPagesLengthLocker)
                return SectionPagesLengthStatic;
        }
        internal static void SetSectionPagesLengthLocked(in int value)
        {
            lock (SectionPagesLengthLocker)
                SectionPagesLengthStatic = value;
        }
        internal static void SetSectionPagesPageDepthLocked(in int index, in int value)
        {
            lock (SectionPagesPageDepthLocker)
                SectionPagesPageDepthStatic[index] = value;
        }
        internal static int GetSectionPagesPageDepthLocked(in int index)
        {
            lock (SectionPagesPageDepthLocker)
                return SectionPagesPageDepthStatic[index];
        }
        internal static void InitializeSectionPagesPageDepthLocked(in int[] value)
        {
            lock (SectionPagesPageDepthLocker)
                SectionPagesPageDepthStatic = value;
        }
        internal static void InitializeTopicsToStartLocked()
        {
            lock (TopicsToStartLocker)
                TopicsToStartStatic = new Queue<TopicData>();
        }
        internal static void SetSectionPagesArrayLocked(in int endpointId)
        {
            lock (SectionPagesLocker)
                lock (pagesLocker)
                    PagesStatic = SectionPagesStatic[endpointId - Constants.One];
        }
        internal static int GetPagesLengthLocked()
        {
            lock (pagesLocker)
                return PagesStatic.Length;
        }
        internal static string GetLastPageLocked()
        {
            lock (pagesLocker)
                return PagesStatic[PagesStatic.Length - Constants.One];
        }
        internal static int GetPreRegistrationLineCountLocked()
        {
            lock (PreRegistrationLineLocker)
                return PreRegistrationLineStatic.Count;
        }
        internal static int GetCaptchaMessagesRegistrationDataCountLocked()
        {
            lock (CaptchaMessagesRegistrationDataLocker)
                return CaptchaMessagesRegistrationDataStatic.Count;
        }
        internal static string GetCaptchaPageToReturnLocked()
        {
            lock (CaptchaPageToReturnLocker)
                return CaptchaPageToReturnStatic;
        }
        internal static string GetPageToReturnRegistrationDataLocked()
        {
            lock (PageToReturnRegistrationDataLocker)
                return PageToReturnRegistrationDataStatic;
        }
        internal static int GetPosLocked()
        {
            lock (posLocker)
                return PosStatic;
        }
        internal static string GetTempLocked()
        {
            lock (tempLocker)
                return TempStatic;
        }
        internal static void SetTempLocked(in string value)
        {
            lock (tempLocker)
                TempStatic = value;
        }
        internal static void SetPosLocked(in int value)
        {
            lock (posLocker)
                PosStatic = value;
        }
        internal static void SetPageToReturnRegistrationDataLocked(in string value)
        {
            lock (PageToReturnRegistrationDataLocker)
                PageToReturnRegistrationDataStatic = value;
        }
        internal static void SetCaptchaPageToReturnLocked(in string value)
        {
            lock (CaptchaPageToReturnLocker)
                CaptchaPageToReturnStatic = value;
        }
        internal static int GetCaptchaMessagesCountLocked()
        {
            lock (CaptchaMessagesLocker)
                return CaptchaMessagesStatic.Count;
        }
        internal static int GetRegistrationLineCountLocked()
        {
            lock (RegistrationLineLocker)
                return RegistrationLineStatic.Count;
        }
        internal static int GetPersonalMessagesToPublishCountLocked()
        {
            lock (PersonalMessagesToPublishLocker)
                return PersonalMessagesToPublishStatic.Count;
        }
        internal static int GetDialogsToStartCountLocked()
        {
            lock (DialogsToStartLocker)
                return DialogsToStartStatic.Count;
        }
        internal static int GetMessagesToPublishCountLocked()
        {
            lock (MessagesToPublishLocker)
                return MessagesToPublishStatic.Count;
        }
        internal static int GetTopicsToStartCountLocked()
        {
            lock (TopicsToStartLocker)
                return TopicsToStartStatic.Count;
        }
        internal static int GetThreadsCountLocked()
        {
            lock (threadsCountLocker)
                return ThreadsCountStatic;
        }
        internal static string[] GetPagesLocked()
        {
            lock (pagesLocker)
                return PagesStatic;
        }
        internal static void SetThreadsCountLocked(in int threadsCount)
        {
            lock (threadsCountLocker)
                Fast.ThreadsCountStatic = threadsCount;
        }
        internal static void SetPagesLocked(in string[] temp)
        {
            lock (pagesLocker)
                PagesStatic = temp;
        }
        internal static bool SpecialSearchLocked(in char c)
        {
            lock (SpecialLocker)
            {
                byte i = 0;

                while (Constants.Special[i] != '\0')
                {
                    if (Constants.Special[i] == c)
                        return true;
                    else
                        i++;
                }

                return false;
            }
        }
        internal static void SetLastPageLocked(in string value)
        {
            lock (pagesLocker)
                PagesStatic[PagesStatic.Length - Constants.One] = value;
        }
        internal static string GetPageLocked(in int num)
        {
            lock (pagesLocker)
                return PagesStatic[num];
        }
        internal static void SetPageLocked(in int index, in string value)
        {
            lock (pagesLocker)
                PagesStatic[index] = value;
        }
        internal static void TopicsToStartEnqueueLocked(in TopicData value)
        {
            lock (TopicsToStartLocker)
                TopicsToStartStatic.Enqueue(value);
        }
        internal static int GetProfilesOnPreSaveLineCountLocked()
        {
            lock (PreSaveProfilesLineLocker)
                return PreSaveProfilesLineStatic.Count;
        }
        internal static void PreSaveProfilesLineEnqueueLocked(in PreProfile preProfile)
        {
            lock (PreSaveProfilesLineLocker)
                PreSaveProfilesLineStatic.Enqueue(preProfile);
        }
        internal static PreProfile PreSaveProfilesLineDequeueLocked()
        {
            lock (PreSaveProfilesLineLocker)
                return PreSaveProfilesLineStatic.Dequeue();
        }
        internal static char GetNextRandomCaptchaSymbolLocked()
        {
            lock (RandomLocker)
                return Constants.CaptchaLetters[RandomStatic.Next(Constants.CaptchaLetters.Length - Constants.One)];
        }
        internal static void InitializePreSaveProfilesLineLocked()
        {
            lock (PreSaveProfilesLineLocker)
                PreSaveProfilesLineStatic = new Queue<PreProfile>();
        }

        internal static void InitializeRandomLocked()
        {
            lock (RandomLocker)
                RandomStatic = new Random();
        }
        internal static TopicData TopicsToStartDequeueLocked()
        {
            lock (TopicsToStartLocker)
                return TopicsToStartStatic.Dequeue();
        }
        internal static void InitializeOwnProfilePagesLocked()
        {
            lock (OwnProfilePagesLocker)
                OwnProfilePagesStatic = new Dictionary<int, string>();
        }
        internal static void InitializePublicProfilePagesLocked()
        {
            lock (PublicProfilePagesLocker)
                PublicProfilePagesStatic = new Dictionary<int, string>();
        }
        internal static bool ThreadPagesContainsThreadIdLocked(in int threadId)
        {
            lock (ThreadPagesLocker)
                return ThreadPagesStatic.ContainsKey(threadId);
        }
        internal static void AddOrUpdateOwnProfilePageLocked(in int accountId, in string page)
        {
            lock (OwnProfilePagesLocker)
                if (OwnProfilePagesStatic.ContainsKey(accountId))
                    OwnProfilePagesStatic[accountId] = page;
                else
                    OwnProfilePagesStatic.Add(accountId, page);
        }
        internal static void AddOrUpdatePublicProfilePageLocked(in int accountId, in string page)
        {
            lock (PublicProfilePagesLocker)
                if (PublicProfilePagesStatic.ContainsKey(accountId))
                    PublicProfilePagesStatic[accountId] = page;
                else
                    PublicProfilePagesStatic.Add(accountId, page);
        }
        internal static string GetPublicProfilePageLocked(in int accountId)
        {
            lock (PublicProfilePagesLocker)
                if (PublicProfilePagesStatic.ContainsKey(accountId))
                    return PublicProfilePagesStatic[accountId];
                else
                    return null;
        }
        internal static string GetOwnProfilePageLocked(in int accountId)
        {
            lock (OwnProfilePagesLocker)
                return OwnProfilePagesStatic[accountId];
        }
        internal static void SetThreadPagesPageLocked
            (in int arrayIndex, in int pageIndex, in string value)
        {
            lock (ThreadPagesLocker)
                ThreadPagesStatic[arrayIndex][pageIndex] = value;
        }
        internal static string GetThreadPagesPageLocked
            (in int arrayIndex, in int pageIndex)
        {
            lock (ThreadPagesLocker)
                return ThreadPagesStatic[arrayIndex][pageIndex];
        }
        internal static string[] GetThreadPagesArrayLocked(in int arrayIndex)
        {
            lock (ThreadPagesLocker)
                return ThreadPagesStatic[arrayIndex];
        }
        internal static void SetThreadPagesArrayLocked
                (in int arrayIndex, in string[] value)
        {
            lock (ThreadPagesLocker)
                ThreadPagesStatic[arrayIndex] = value;
        }
        internal static void AddToThreadPagesPageLocked
                (in int arrayIndex, in int pageIndex, in string value)
        {
            lock (ThreadPagesLocker)
                ThreadPagesStatic[arrayIndex][pageIndex] =
                    string.Concat(ThreadPagesStatic[arrayIndex][pageIndex], value);
        }
        internal static void CorrectMessagesArrayLocked
            (Func<int, int, string, int, string, string, string> GetNewPage, in int endpointId, in int threadId, in string message,
                in int accountId, in string threadName, in string nick)
        {
            lock (ThreadPagesLocker)
            {
                lock (ThreadPagesPageDepthLocker)
                {
                    SetThreadPagesPageDepthLocked(threadId, Constants.One);
                    ThreadPagesStatic.Add(threadId, new string[] {
                GetNewPage(threadId, endpointId, threadName,
                    accountId, nick, message) });
                }
            }
        }
        internal static void InitializeThreadPagesLocked(in int threadsCount)
        {
            lock (ThreadPagesLocker)
                ThreadPagesStatic = new Dictionary<int, string[]>(threadsCount);
        }
        internal static Dictionary<int, string[]> GetThreadPagesLocked()
        {
            lock (ThreadPagesLocker)
                return ThreadPagesStatic;
        }
        internal static int GetThreadPagesPageDepthLocked(in int index)
        {
            lock (ThreadPagesPageDepthLocker)
                return ThreadPagesPageDepthStatic[index];
        }
        internal static void AddToThreadPagesPageDepthLocked(in int index, in int value)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepthStatic[index] += value;
        }
        internal static void SetThreadPagesPageDepthLocked(in int index, in int value)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepthStatic[index] = value;
        }
        internal static void InitializeThreadPagesPageDepthLocked(in int threadsCount)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepthStatic = new Dictionary<int, int>(threadsCount);
        }
        internal static void InitializeMessagesToPublishLocked()
        {
            lock (MessagesToPublishLocker)
                MessagesToPublishStatic = new Queue<MessageData>();
        }
        internal static void MessagesToPublishEnqueueLocked(in MessageData messageData)
        {
            lock (MessagesToPublishLocker)
                MessagesToPublishStatic.Enqueue(messageData);
        }
        internal static MessageData MessagesToPublishDequeueLocked()
        {
            lock (MessagesToPublishLocker)
                return MessagesToPublishStatic.Dequeue();
        }
        internal static void PreRegistrationLineAddLocked(in int val, in PreRegBag bag)
        {
            lock (PreRegistrationLineLocker)
                PreRegistrationLineStatic.Add(val, bag);
        }
        internal static void InitializePreRegistrationLineLocked()
        {
            lock (PreRegistrationLineLocker)
                PreRegistrationLineStatic = new Dictionary<int, PreRegBag>();
        }
        internal static void InitializeRegistrationLineLocked()
        {
            lock (RegistrationLineLocker)
                RegistrationLineStatic = new Dictionary<int, RegBag>();
        }
        internal static void CaptchaMessagesRegistrationDataEnqueueLocked(in uint captchaHash)
        {
            lock (CaptchaMessagesRegistrationDataLocker)
                CaptchaMessagesRegistrationDataStatic.Enqueue(captchaHash);
        }
        internal static void CaptchaMessagesRegistrationDataDequeueLocked()
        {
            lock (CaptchaMessagesRegistrationDataLocker)
                CaptchaMessagesRegistrationDataStatic.Dequeue();
        }
        internal static void InitializeCaptchaMessagesRegistrationDataLocked()
        {
            lock (CaptchaMessagesRegistrationDataLocker)
                CaptchaMessagesRegistrationDataStatic = new Queue<uint>(Constants.RegistrationPagesCount);
        }
        internal static bool CaptchaMessagesRegistrationDataContainsLocked(in uint captcha)
        {
            lock (CaptchaMessagesRegistrationDataLocker)
                return CaptchaMessagesRegistrationDataStatic.Contains(captcha);
        }
        internal static void RegistrationLineRemoveLocked(in int i, out RegBag regBag)
        {
            lock (RegistrationLineLocker)
            {
                RegistrationLineStatic.Remove(i, out RegBag bag);
                regBag = bag;
            }
        }
        internal static void PreRegistrationLineRemoveLocked(in int key, out PreRegBag preRegBag)
        {
            lock (PreRegistrationLineLocker)
            {
                PreRegistrationLineStatic.Remove(key, out PreRegBag temp);
                preRegBag = temp;
            }
        }
        internal static void RegistrationLineAddLocked(in int val, in RegBag regBag)
        {
            lock (RegistrationLineLocker)
                RegistrationLineStatic.Add(val, regBag);
        }
        internal static void CaptchaMessagesEnqueueLocked(in uint captchaHash)
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessagesStatic.Enqueue(captchaHash);
        }
        internal static void InitializeCaptchaMessagesLocked()
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessagesStatic = new Queue<uint>(Constants.LoginPagesCount);
        }
        internal static void CaptchaMessagesDequeueLocked()
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessagesStatic.Dequeue();
        }
        internal static bool CaptchaMessagesContainsLocked(in uint captchaHash)
        {
            lock (CaptchaMessagesLocker)
                return CaptchaMessagesStatic.Contains(captchaHash);
        }
        internal static bool LoginPasswordHashesValuesContainsLocked(in Guid guid)
        {
            lock (LoginPasswordHashesLocker)
                return LoginPasswordHashesStatic.ContainsValue(guid);
        }
        internal static Tuple<bool, int> CheckGuidAndGetOwnerAccountIdLocked(Guid guid)
        {
            lock (LoginPasswordHashesLocker)
                lock (LoginPasswordAccIdHashesLocker)
                    if (LoginPasswordHashesStatic.ContainsValue(guid))
                        return new Tuple<bool, int>(true, LoginPasswordAccIdHashesStatic[LoginPasswordHashesStatic.Single(p => p.Value == guid).Key]);
                    else
                        return new Tuple<bool, int>(false, Constants.Zero);
        }
        internal static void SetLoginPasswordHashesPairTokenLocked(in Pair pair, in Guid? token)
        {
            lock (LoginPasswordHashesLocker)
                LoginPasswordHashesStatic[pair] = token;
        }
        private static bool IncrementWithValueRemoteIpHashesAttemptsCountersAndGrantAccessAndAddIfNotPresentedLocked(in uint ipHash, in byte value)
        {
            lock (RemoteIpHashesAttemptsCounterLocker)
            {
                if (!RemoteIpHashesAttemptsCounterStatic.ContainsKey(ipHash))
                    RemoteIpHashesAttemptsCounterStatic.Add(ipHash, value);
                short sum = (short)(value + RemoteIpHashesAttemptsCounterStatic[ipHash]);

                if (sum > Constants.MaxAttemptsCountPerIp)
                {
                    RemoteIpHashesAttemptsCounterStatic[ipHash] = Constants.MaxAttemptsCountPerIp;

                    return false;
                }
                else
                {
                    RemoteIpHashesAttemptsCounterStatic[ipHash] = (byte)sum;

                    return true;
                }
            }
        }
        internal static void DecrementAllRemoteIpHashesAttemptsCountersAndRemoveUnnecessaryByTimerLocked()
        {
            lock (RemoteIpHashesAttemptsCounterLocker)
                foreach (var pair in RemoteIpHashesAttemptsCounterStatic)
                    if (pair.Value > Constants.One)
                        RemoteIpHashesAttemptsCounterStatic[pair.Key] = (byte)(pair.Value - Constants.One);
                    else
                        RemoteIpHashesAttemptsCounterStatic.Remove(pair.Key);
        }
        internal static void InitializeRemoteIpHashesAttemptsCounterLocked()
        {
            lock (RemoteIpHashesAttemptsCounterLocker)
                RemoteIpHashesAttemptsCounterStatic = new Dictionary<uint, byte>();
        }
        internal static void LoginPasswordHashesThroughIterationCheckLocked(out Pair pair, in Guid guid)
        {
            lock (LoginPasswordHashesLocker)
            {
                pair = new Pair();

                if (LoginPasswordHashesStatic.ContainsValue(guid))
                    foreach (var key in LoginPasswordHashesStatic.Keys)
                        if (LoginPasswordHashesStatic[key] == guid)
                        {
                            pair = key;

                            break;
                        }
            }
        }
    }
}