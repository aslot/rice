using System.Collections.Generic;
using System;
using XXHash;
using System.Net;
using System.Linq;
using static Data.DataLockers.Lockers;
namespace Data
{ // Перекрёстные ссылки между методами исключены.
    public sealed partial class Memory
    {
        public bool LoginPasswordAccIdHashesContainsKey(in Pair pair)
        {
            lock (LoginPasswordAccIdHashesLocker)
                return LoginPasswordAccIdHashes.ContainsKey(pair);
        }
        public void LoginPasswordAccIdHashesAdd(in Pair pair, in int accountId)
        {
            lock (LoginPasswordAccIdHashesLocker)
                LoginPasswordAccIdHashes.Add(pair, accountId);
        }
        public void LoginPasswordHashesDeltaRemove(in Pair pair, out byte result)
        {
            lock (LoginPasswordHashesDeltaLocker)
                LoginPasswordHashesDelta.Remove(pair, out result);
        }
        public IEnumerable<int> IterateThroughAccountIds()
        {
            lock (LoginPasswordAccIdHashesLocker)
                foreach (int accountId in LoginPasswordAccIdHashes.Values)
                    yield return accountId;
        }
        public void LoginPasswordHashesDeltaAdd(in Pair pair, in byte val)
        {
            lock (LoginPasswordHashesDeltaLocker)
                LoginPasswordHashesDelta.Add(pair, Constants.Zero);
        }
        public bool LoginPasswordHashesDeltaContainsKey(in Pair pair)
        {
            lock (LoginPasswordHashesDeltaLocker)
                return LoginPasswordHashesDelta.ContainsKey(pair);
        }
        public int GetLoginPasswordAccIdHashes(in Pair pair)
        {
            lock (LoginPasswordAccIdHashesLocker)
                return LoginPasswordAccIdHashes[pair];
        }
        public void InitializeLoginPasswordAccIdHashes()
        {
            lock (LoginPasswordAccIdHashesLocker)
                Memory.LoginPasswordAccIdHashes = new Dictionary<Pair, int>();
        }
        public void InitializeLoginPasswordHashes()
        {
            lock (LoginPasswordHashesLocker)
                Memory.LoginPasswordHashes = new Dictionary<Pair, Guid?>();
        }
        public IEnumerable<Pair> GetLoginPasswordHashesDeltaKeys()
        {
            lock (LoginPasswordHashesDeltaLocker)
                return LoginPasswordHashesDelta.Keys;
        }
        public void InitializeLoginPasswordHashesDelta()
        {
            lock (LoginPasswordHashesDeltaLocker)
                Memory.LoginPasswordHashesDelta = new Dictionary<Pair, byte>();
        }
        public void InitializePrivateMessages()
        {
            lock (PersonalPagesLocker)
            {
                Memory.PersonalPages = new
                    Dictionary
                        <OwnerId, Dictionary<CompanionId, PrivateMessages>>();

                lock (PersonalPagesDepthsLocker)
                    Memory.PersonalPagesDepths = new
                    Dictionary<OwnerId, Dictionary<CompanionId, int>>();
            }
        }
        public bool LoginPasswordHashesContainsKey(in Pair pair)
        {
            lock (LoginPasswordHashesLocker)
                return LoginPasswordHashes.ContainsKey(pair);
        }
        public void LoginPasswordHashesAdd(in Pair pair, in Guid? guid)
        => LoginPasswordHashes.Add(pair, null);
        public bool NicksHashesKeysContains(in uint hash)
        {
            lock (NicksHashesLocker)
                return NicksHashes.ContainsKey(hash);
        }
        public void InitializeNicksHashes()
        {
            lock (NicksHashesLocker)
                NicksHashes = new Dictionary<uint, byte>();
        }
        private bool CheckIfIpIsNotBanned(in uint ipHash)
        {
            lock (BlockedRemoteIpsHashesLocker)
                return !BlockedRemoteIpsHashes.Contains(ipHash);
        }
        public void InitializeAccountIdentifierRemoteIpLog()
        {
            lock (AccountIdentifierRemoteIpLogLocker)
                AccountIdentifierRemoteIpLog = new Queue<AccountIdentifierRemoteIp>();
        }
        public void AccountIdentifierRemoteIpLogEnqueue(in AccountIdentifierRemoteIp value)
        {
            lock (AccountIdentifierRemoteIpLogLocker)
                AccountIdentifierRemoteIpLog.Enqueue(value);
        }
        public Queue<AccountIdentifierRemoteIp> GetAccountIdentifierRemoteIps()
        {
            lock (AccountIdentifierRemoteIpLogLocker)
                return AccountIdentifierRemoteIpLog;
        }
        public void ClearAccountIdentifierRemoteIps()
        {
            lock (AccountIdentifierRemoteIpLogLocker)
                AccountIdentifierRemoteIpLog.Clear();
        }
        public void NicksHashesAdd(in uint nickHash, in byte temp)
        {
            lock (NicksHashesLocker)
                NicksHashes.Add(nickHash, temp);
        }
        public string GetEndPointPageLocked(in int index)
        {
            lock (EndPointPagesLocker)
                return EndPointPages[index];
        }
        public void SetEndPointPageLocked(in int index, in string value)
        {
            lock (EndPointPagesLocker)
                EndPointPages[index] = value;
        }
        public void InitializeEndPointPagesLocked(in int size)
        {
            lock (EndPointPagesLocker)
                EndPointPages = new string[size];
        }
        public string GetMainContentLocked()
        {
            lock (MainContentLocker)
                return MainContent;
        }
        public void SetMainContentLocked(in string value)
        {
            lock (MainContentLocker)
                MainContent = value;
        }
        public bool CheckIp(in IPAddress ip, in byte value)
        {
            uint hash;

            if (ip == null)
                return false;
            else
                hash = XXHash32.Hash(ip.ToString());

            return CheckIfIpIsNotBanned(hash) && IncrementWithValueRemoteIpHashesAttemptsCountersAndGrantAccessAndAddIfNotPresented(hash, value);
        }
        public string GetMainPageLocked()
        {
            lock (MainPageLocker)
                return MainPage;
        }
        public void SetMainPageLocked(in string value)
        {
            lock (MainPageLocker)
                MainPage = value;
        }
        public void AddToMainPageLocked(in string value)
        {
            lock (MainPageLocker)
                MainPage += value;
        }
        public void DialogsToStartEnqueue(in DialogData value)
        {
            lock (DialogsToStartLocker)
                DialogsToStart.Enqueue(value);
        }
        public void InitializeDialogsToStart()
        {
            lock (DialogsToStartLocker)
                DialogsToStart = new Queue<DialogData>();
        }
        public DialogData DialogsToStartDequeue()
        {
            lock (DialogsToStartLocker)
                return DialogsToStart.Dequeue();
        }
        public void InitializePersonalMessagesToPublish()
        {
            lock (PersonalMessagesToPublishLocker)
                PersonalMessagesToPublish = new Queue<MessageData>();
        }
        public bool CheckIfTimerIsWorking()
        {
            lock (TimerIsWorkingLocker)
                return TimerIsWorking == Constants.One;
        }
        public void ResetTimerIsWorkingFlag()
        {
            lock (TimerIsWorkingLocker)
                TimerIsWorking = Constants.Zero;
        }
        public void SetTimerIsWorkingFlag()
        {
            lock (TimerIsWorkingLocker)
                TimerIsWorking = Constants.One;
        }
        public void PersonalMessagesToPublishEnqueue(in MessageData value)
        {
            lock (PersonalMessagesToPublishLocker)
                PersonalMessagesToPublish.Enqueue(value);
        }
        public MessageData PersonalMessagesToPublishDequeue()
        {
            lock (PersonalMessagesToPublishLocker)
                return PersonalMessagesToPublish.Dequeue();
        }
        public string[] GetDialogPagesArrayLocked(in int index)
        {
            lock (DialogPagesLocker)
                return DialogPages[index];
        }
        public void SetDialogPagesArrayLocked(in int index, in string[] value)
        {
            lock (DialogPagesLocker)
                DialogPages[index] = value;
        }
        public string GetDialogPagesPageLocked(in int arrayIndex, in int pageIndex)
        {
            lock (DialogPagesLocker)
                return DialogPages[arrayIndex][pageIndex];
        }
        public void SetDialogPagesPageLocked
            (in int arrayIndex, in int pageIndex, in string value)
        {
            lock (DialogPagesLocker)
                DialogPages[arrayIndex][pageIndex] = value;
        }
        public void AddToDialogPagesPageLocked
            (in int arrayIndex, in int pageIndex, in string value)
        {
            lock (DialogPagesLocker)
                DialogPages[arrayIndex][pageIndex] += value;
        }
        public void InitializeDialogPagesLocked(in string[][] value)
        {
            lock (DialogPagesLocker)
                DialogPages = value;
        }
        public int GetDialogPagesLengthLocked()
        {
            lock (DialogPagesLengthLocker)
                return DialogPagesLength;
        }
        public void CopyDialogPagesArraysToIncreasedSizeArraysAndFillGapsLocked()
        {
            lock (DialogPagesLocker)
            {
                lock (DialogPagesPageDepthLocker)
                {
                    lock (DialogPagesLengthLocker)
                    {
                        int len = DialogPagesLength;
                        DialogPagesLength++;
                        string[][] temp = new string[DialogPagesLength][];
                        Array.Copy(DialogPages, temp, len);
                        DialogPages = temp;
                        DialogPages[len] = new string[Constants.One] { Constants.NewDialog };
                        int[] temp1 = new int[DialogPagesLength];
                        Array.Copy(DialogPagesPageDepth, temp1, len);
                        DialogPagesPageDepth = temp1;
                        DialogPagesPageDepth[len] = Constants.One;
                    }
                }
            }
        }
        public void SetDialogPagesLengthLocked(in int value)
        {
            lock (DialogPagesLengthLocker)
                DialogPagesLength = value;
        }
        public void SetDialogPagesPageDepthLocked(in int index, in int value)
        {
            lock (DialogPagesPageDepthLocker)
                DialogPagesPageDepth[index] = value;
        }
        public int GetDialogPagesPageDepthLocked(in int index)
        {
            lock (DialogPagesPageDepthLocker)
                return DialogPagesPageDepth[index];
        }
        public void InitializeDialogPagesPageDepthLocked(in int[] value)
        {
            lock (DialogPagesPageDepthLocker)
                DialogPagesPageDepth = value;
        }
        public string GetMessage(in int ownerId, in int companionId, in int messageId)
        {
            lock (PersonalPagesLocker)
                return PersonalPages
                              [new OwnerId { Id = ownerId }]
                             [new CompanionId { Id = companionId }]
                             .Messages[messageId];
        }
        public string[] GetMessages(in int ownerId, in int companionId)
        {
            lock (PersonalPagesLocker)
                return PersonalPages
                    [new OwnerId { Id = ownerId }]
                    [new CompanionId { Id = companionId }]
                    .Messages;
        }
        public void AddToPersonalPagesDepth(in int id, in int accountId)
        {
            lock (PersonalPagesDepthsLocker)
                PersonalPagesDepths
                 [new OwnerId { Id = accountId }]
                 [new CompanionId { Id = id }]
                 ++;
        }
        public int GetPersonalPagesDepth(in int id, in int accountId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepths
                    [new OwnerId { Id = accountId }]
                    [new CompanionId { Id = id }];
        }
        public void SetPersonalPagesPage
                (in int id, in int accountId, in int depth, in string page)
        {
            lock (PersonalPagesLocker)
                PersonalPages
                            [new OwnerId { Id = accountId }]
                            [new CompanionId { Id = id }]
                            .Messages[depth] = page;
        }
        public void SetPersonalPagesMessagesArray
                    (in int id, in int accountId, in string[] value)
        {
            lock (PersonalPagesLocker)
                PersonalPages
                            [new OwnerId { Id = accountId }]
                            [new CompanionId { Id = id }]
                            = new PrivateMessages { Messages = value };
        }
        public bool PersonalPagesContainsKey(in OwnerId ownerId, in CompanionId companionId, in bool flag)
        {
            lock (PersonalPagesLocker)
                return flag && PersonalPages[ownerId].ContainsKey(companionId);
        }
        public void PersonalPagesAdd(in OwnerId ownerId, in CompanionId companionId, in string[] newMsg, in bool flag)
        {
            lock (PersonalPagesLocker)
            {
                if (!flag)
                    PersonalPages.Add(ownerId, new Dictionary<CompanionId, PrivateMessages>());
                PersonalPages[ownerId].Add(companionId,
                        new PrivateMessages { Messages = newMsg });
            }
        }
        public bool PersonalPagesKeysContains(in OwnerId ownerId)
        {
            lock (PersonalPagesLocker)
                return PersonalPages.ContainsKey(ownerId);
        }
        public bool PersonalPagesDepthsContainsKey(in OwnerId ownerId, in CompanionId companionId, in bool flag)
        {
            lock (PersonalPagesDepthsLocker)
                return flag && PersonalPagesDepths[ownerId].ContainsKey(companionId);
        }
        public void PersonalPagesDepthsAdd(in OwnerId ownerId, in CompanionId companionId, in bool flag)
        {
            lock (PersonalPagesDepthsLocker)
            {
                if (!flag)
                    PersonalPagesDepths.Add(ownerId, new Dictionary<CompanionId, int>());
                PersonalPagesDepths[ownerId].Add(companionId, Constants.One);
            }
        }
        public bool PersonalPagesDepthsKeysContains(in OwnerId ownerId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepths.ContainsKey(ownerId);
        }
        public int GetPersonalPagesPageDepth(in int accountId, in int companionId)
        {
            lock (PersonalPagesDepthsLocker)
                return PersonalPagesDepths
                    [new OwnerId { Id = accountId }]
                    [new CompanionId { Id = companionId }];//проверить границы 
        }
        public void PersonalPagesAdd(in OwnerId ownerId,
                                         in Dictionary<CompanionId, PrivateMessages> temp1)
        {
            lock (PersonalPagesLocker)
                PersonalPages.Add(ownerId, temp1);
        }
        public void PersonalPagesDepthsAdd(in OwnerId ownerId,
                                                in Dictionary<CompanionId, int> temp2)
        {
            lock (PersonalPagesDepthsLocker)
                PersonalPagesDepths.Add(ownerId, temp2);
        }
        public string[] GetSectionPagesArrayLocked(in int index)
        {
            lock (SectionPagesLocker)
                return SectionPages[index];
        }
        public void SetSectionPagesArrayLocked(in int index, in string[] value)
        {
            lock (SectionPagesLocker)
                SectionPages[index] = value;
        }
        public string GetSectionPagesPageLocked(in int arrayIndex, in int pageIndex)
        {
            lock (SectionPagesLocker)
                return SectionPages[arrayIndex][pageIndex];
        }
        public void SetSectionPagesPageLocked(in int arrayIndex, in int pageIndex, in string value)
        {
            lock (SectionPagesLocker)
                SectionPages[arrayIndex][pageIndex] = value;
        }
        public void AddToSectionPagesPageLocked(in int arrayIndex, in int pageIndex, in string value)
        {
            lock (SectionPagesLocker)
                SectionPages[arrayIndex][pageIndex] =
                    string.Concat(SectionPages[arrayIndex][pageIndex], value);
        }
        public void InitializeSectionPagesLocked(in string[][] value)
        {
            lock (SectionPagesLocker)
                SectionPages = value;
        }
        public int GetSectionPagesLengthLocked()
        {
            lock (SectionPagesLengthLocker)
                return SectionPagesLength;
        }
        public void SetSectionPagesLengthLocked(in int value)
        {
            lock (SectionPagesLengthLocker)
                SectionPagesLength = value;
        }
        public void SetSectionPagesPageDepthLocked(in int index, in int value)
        {
            lock (SectionPagesPageDepthLocker)
                SectionPagesPageDepth[index] = value;
        }
        public int GetSectionPagesPageDepthLocked(in int index)
        {
            lock (SectionPagesPageDepthLocker)
                return SectionPagesPageDepth[index];
        }
        public void InitializeSectionPagesPageDepthLocked(in int[] value)
        {
            lock (SectionPagesPageDepthLocker)
                SectionPagesPageDepth = value;
        }
        public void InitializeTopicsToStart()
        {
            lock (TopicsToStartLocker)
                TopicsToStart = new Queue<TopicData>();
        }
        public void SetSectionPagesArray(in int endpointId)
        {
            lock (SectionPagesLocker)
                lock (pagesLocker)
                    pages = SectionPages[endpointId - Constants.One];
        }
        public int GetPagesLength()
        {
            lock (pagesLocker)
                return pages.Length;
        }
        public string GetLastPage()
        {
            lock (pagesLocker)
                return pages[pages.Length - Constants.One];
        }
        public int GetPreRegistrationLineCount()
        {
            lock (PreRegistrationLineLocker)
                return PreRegistrationLine.Count;
        }
        public int GetCaptchaMessagesRegistrationDataCount()
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                return CaptchaMessages_RegistrationData.Count;
        }
        public string GetCaptchaPageToReturn()
        {
            lock (CaptchaPageToReturnLocker)
                return CaptchaPageToReturn;
        }
        public string GetPageToReturnRegistrationData()
        {
            lock (PageToReturn_RegistrationDataLocker)
                return PageToReturn_RegistrationData;
        }
        public int GetPos()
        {
            lock (posLocker)
                return pos;
        }
        public string GetTemp()
        {
            lock (tempLocker)
                return temp;
        }
        public void SetTemp(in string value)
        {
            lock (tempLocker)
                temp = value;
        }
        public void SetPos(in int value)
        {
            lock (posLocker)
                pos = value;
        }
        public void SetPageToReturnRegistrationData(in string value)
        {
            lock (PageToReturn_RegistrationDataLocker)
                PageToReturn_RegistrationData = value;
        }
        public void SetCaptchaPageToReturn(in string value)
        {
            lock (CaptchaPageToReturnLocker)
                CaptchaPageToReturn = value;
        }
        public int GetCaptchaMessagesCount()
        {
            lock (CaptchaMessagesLocker)
                return CaptchaMessages.Count;
        }
        public int GetRegistrationLineCount()
        {
            lock (RegistrationLineLocker)
                return RegistrationLine.Count;
        }
        public int GetPersonalMessagesToPublishCount()
        {
            lock (PersonalMessagesToPublishLocker)
                return PersonalMessagesToPublish.Count;
        }
        public int GetDialogsToStartCount()
        {
            lock (DialogsToStartLocker)
                return DialogsToStart.Count;
        }
        public int GetMessagesToPublishCount()
        {
            lock (MessagesToPublishLocker)
                return MessagesToPublish.Count;
        }
        public int GetTopicsToStartCount()
        {
            lock (TopicsToStartLocker)
                return TopicsToStart.Count;
        }
        public int GetThreadsCount()
        {
            lock (threadsCountLocker)
                return threadsCount;
        }
        public string[] GetPages()
        {
            lock (pagesLocker)
                return pages;
        }
        public void SetThreadsCount(in int threadsCount)
        {
            lock (threadsCountLocker)
                Memory.threadsCount = threadsCount;
        }
        public void SetPages(in string[] temp)
        {
            lock (pagesLocker)
                pages = temp;
        }
        public bool SpecialSearch(in char c)
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
        public void SetLastPage(in string value)
        {
            lock (pagesLocker)
                pages[pages.Length - Constants.One] = value;
        }
        public string GetPage(in int num)
        {
            lock (pagesLocker)
                return pages[num];
        }
        public void SetPage(in int index, in string value)
        {
            lock (pagesLocker)
                pages[index] = value;
        }
        public void TopicsToStartEnqueue(in TopicData value)
        {
            lock (TopicsToStartLocker)
                TopicsToStart.Enqueue(value);
        }
        public int GetProfilesOnPreSaveLineCount()
        {
            lock (PreSaveProfilesLineLocker)
                return PreSaveProfilesLine.Count;
        }
        public void PreSaveProfilesLineEnqueueLocked(in PreProfile preProfile)
        {
            lock (PreSaveProfilesLineLocker)
                PreSaveProfilesLine.Enqueue(preProfile);
        }
        public PreProfile PreSaveProfilesLineDequeueLocked()
        {
            lock (PreSaveProfilesLineLocker)
                return PreSaveProfilesLine.Dequeue();
        }
        public char GetNextRandomCaptchaSymbol()
        {
            lock (RandomLocker)
                return Constants.CaptchaLetters[Random.Next(Constants.CaptchaLetters.Length - Constants.One)];
        }
        public void InitializePreSaveProfilesLine()
        {
            lock (PreSaveProfilesLineLocker)
                PreSaveProfilesLine = new Queue<PreProfile>();
        }

        public void InitializeRandom()
        {
            lock (RandomLocker)
                Random = new Random();
        }
        public TopicData TopicsToStartDequeue()
        {
            lock (TopicsToStartLocker)
            {
                return TopicsToStart.Dequeue();
            }
        }
        public void InitializeOwnProfilePages()
        {
            lock (OwnProfilePagesLocker)
                OwnProfilePages = new Dictionary<int, string>();
        }
        public void InitializePublicProfilePages()
        {
            lock (PublicProfilePagesLocker)
                PublicProfilePages = new Dictionary<int, string>();
        }
        public bool ThreadPagesContainsThreadIdLocked(in int threadId)
        {
            lock (ThreadPagesLocker)
                return ThreadPages.ContainsKey(threadId);
        }
        public void AddOrUpdateOwnProfilePage(in int accountId, in string page)
        {
            lock (OwnProfilePagesLocker)
                if (OwnProfilePages.ContainsKey(accountId))
                    OwnProfilePages[accountId] = page;
                else
                    OwnProfilePages.Add(accountId, page);
        }
        public void AddOrUpdatePublicProfilePage(in int accountId, in string page)
        {
            lock (PublicProfilePagesLocker)
                if (PublicProfilePages.ContainsKey(accountId))
                    PublicProfilePages[accountId] = page;
                else
                    PublicProfilePages.Add(accountId, page);
        }
        public string GetPublicProfilePage(in int accountId)
        {
            lock (PublicProfilePagesLocker)
                if (PublicProfilePages.ContainsKey(accountId))
                    return PublicProfilePages[accountId];
                else
                    return null;
        }
        public string GetOwnProfilePage(in int accountId)
        {
            lock (OwnProfilePagesLocker)
                return OwnProfilePages[accountId];
        }
        public void SetThreadPagesPageLocked
            (in int arrayIndex, in int pageIndex, in string value)
        {
            lock (ThreadPagesLocker)
                ThreadPages[arrayIndex][pageIndex] = value;
        }
        public string GetThreadPagesPageLocked
            (in int arrayIndex, in int pageIndex)
        {
            lock (ThreadPagesLocker)
                return ThreadPages[arrayIndex][pageIndex];
        }
        public string[] GetThreadPagesArrayLocked(in int arrayIndex)
        {
            lock (ThreadPagesLocker)
                return ThreadPages[arrayIndex];
        }
        public void SetThreadPagesArrayLocked
                (in int arrayIndex, in string[] value)
        {
            lock (ThreadPagesLocker)
                ThreadPages[arrayIndex] = value;
        }
        public void AddToThreadPagesPageLocked
                (in int arrayIndex, in int pageIndex, in string value)
        {
            lock (ThreadPagesLocker)
                ThreadPages[arrayIndex][pageIndex] =
                    string.Concat(ThreadPages[arrayIndex][pageIndex], value);
        }
        public void CorrectMessagesArray
            (Func<int, int, string, int, string, string, string> GetNewPage, in int endpointId, in int threadId, in string message,
                in int accountId, in string threadName, in string nick)
        {
            Dictionary<int, string[]> threadPages = GetThreadPagesLocked();

            lock (ThreadPagesLocker)
            {
                lock (ThreadPagesPageDepthLocker)
                {
                    SetThreadPagesPageDepthLocked(threadId, Constants.One);
                    ThreadPages.Add(threadId, new string[] {
                GetNewPage(threadId, endpointId, threadName,
                    accountId, nick, message) });
                }
            }
        }
        public void InitializeThreadPagesLocked(in int threadsCount)
        {
            lock (ThreadPagesLocker)
                ThreadPages = new Dictionary<int, string[]>(threadsCount);
        }
        public Dictionary<int, string[]> GetThreadPagesLocked()
        {
            lock (ThreadPagesLocker)
                return ThreadPages;
        }
        public int GetThreadPagesPageDepthLocked(in int index)
        {
            lock (ThreadPagesPageDepthLocker)
                return ThreadPagesPageDepth[index];
        }
        public void AddToThreadPagesPageDepthLocked(in int index, in int value)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepth[index] += value;
        }
        public void SetThreadPagesPageDepthLocked(in int index, in int value)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepth[index] = value;
        }
        public void InitializeThreadPagesPageDepthLocked(in int threadsCount)
        {
            lock (ThreadPagesPageDepthLocker)
                ThreadPagesPageDepth = new Dictionary<int, int>(threadsCount);
        }
        public void InitializeMessagesToPublish()
        {
            lock (MessagesToPublishLocker)
                MessagesToPublish = new Queue<MessageData>();
        }
        public void MessagesToPublishEnqueue(in MessageData messageData)
        {
            lock (MessagesToPublishLocker)
                MessagesToPublish.Enqueue(messageData);
        }
        public MessageData MessagesToPublishDequeue()
        {
            lock (MessagesToPublishLocker)
            {
                return MessagesToPublish.Dequeue();
            }
        }
        public void PreRegistrationLineAdd(in int val, in PreRegBag bag)
        {
            lock (PreRegistrationLineLocker)
                PreRegistrationLine.Add(val, bag);
        }
        public void InitializePreRegistrationLine()
        {
            lock (PreRegistrationLineLocker)
                PreRegistrationLine = new Dictionary<int, PreRegBag>();
        }
        public void InitializeRegistrationLine()
        {
            lock (RegistrationLineLocker)
                RegistrationLine = new Dictionary<int, RegBag>();
        }
        public void CaptchaMessagesRegistrationDataEnqueue(in uint captchaHash)
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                CaptchaMessages_RegistrationData.Enqueue(captchaHash);
        }
        public void CaptchaMessagesRegistrationDataDequeue()
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                CaptchaMessages_RegistrationData.Dequeue();
        }
        public void InitializeCaptchaMessagesRegistrationData()
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                CaptchaMessages_RegistrationData = new Queue<uint>(Constants.RegistrationPagesCount);
        }
        public bool CaptchaMessagesRegistrationDataContains(in uint captcha)
        {
            lock (CaptchaMessages_RegistrationDataLocker)
                return CaptchaMessages_RegistrationData.Contains(captcha);
        }
        public void RegistrationLineRemove(in int i, out RegBag regBag)
        {
            lock (RegistrationLineLocker)
            {
                RegistrationLine.Remove(i, out RegBag bag);
                regBag = bag;
            }
        }
        public void PreRegistrationLineRemove(in int key, out PreRegBag preRegBag)
        {
            lock (PreRegistrationLineLocker)
            {
                PreRegistrationLine.Remove(key, out PreRegBag temp);
                preRegBag = temp;
            }
        }
        public void RegistrationLineAdd(in int val, in RegBag regBag)
        {
            lock (RegistrationLineLocker)
                RegistrationLine.Add(val, regBag);
        }
        public void CaptchaMessagesEnqueue(in uint captchaHash)
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessages.Enqueue(captchaHash);
        }
        public void InitializeCaptchaMessages()
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessages = new Queue<uint>(Constants.LoginPagesCount);
        }
        public void CaptchaMessagesDequeue()
        {
            lock (CaptchaMessagesLocker)
                CaptchaMessages.Dequeue();
        }
        public bool CaptchaMessagesContains(in uint captchaHash)
        {
            lock (CaptchaMessagesLocker)
                return CaptchaMessages.Contains(captchaHash);
        }
        public bool LoginPasswordHashesValuesContains(in Guid guid)
        {
            lock (LoginPasswordHashesLocker)
                return LoginPasswordHashes.ContainsValue(guid);
        }
        public Tuple<bool, int> CheckGuidAndGetOwnerAccountId(Guid guid)
        {
            lock (LoginPasswordHashesLocker)
                lock (LoginPasswordAccIdHashesLocker)
                    if (LoginPasswordHashes.ContainsValue(guid))
                        return new Tuple<bool, int>(true, LoginPasswordAccIdHashes[LoginPasswordHashes.Single(p => p.Value == guid).Key]);
                    else
                        return new Tuple<bool, int>(false, Constants.Zero);
        }
        public void SetLoginPasswordHashesPairToken(in Pair pair, in Guid? token)
        {
            lock (LoginPasswordHashesLocker)
                LoginPasswordHashes[pair] = token;
        }
        private bool IncrementWithValueRemoteIpHashesAttemptsCountersAndGrantAccessAndAddIfNotPresented(in uint ipHash, in byte value)
        {
            lock (RemoteIpHashesAttemptsCounterLocker)
            {
                if (!RemoteIpHashesAttemptsCounter.ContainsKey(ipHash))
                    RemoteIpHashesAttemptsCounter.Add(ipHash, value);
                short sum = (short)(value + RemoteIpHashesAttemptsCounter[ipHash]);

                if (sum > Constants.MaxAttemptsCountPerIp)
                {
                    RemoteIpHashesAttemptsCounter[ipHash] = Constants.MaxAttemptsCountPerIp;

                    return false;
                }
                else
                {
                    RemoteIpHashesAttemptsCounter[ipHash] = (byte)sum;

                    return true;
                }
            }
        }
        public void DecrementAllRemoteIpHashesAttemptsCountersAndRemoveUnnecessaryByTimer()
        {
            lock (RemoteIpHashesAttemptsCounterLocker)
                foreach (var pair in RemoteIpHashesAttemptsCounter)
                    if (pair.Value > Constants.One)
                        RemoteIpHashesAttemptsCounter[pair.Key] = (byte)(pair.Value - Constants.One);
                    else
                        RemoteIpHashesAttemptsCounter.Remove(pair.Key);
        }
        public void InitializeRemoteIpHashesAttemptsCounter()
        {
            lock (RemoteIpHashesAttemptsCounterLocker)
                RemoteIpHashesAttemptsCounter = new Dictionary<uint, byte>();
        }
        public void LoginPasswordHashesThroughIterationCheck(ref Pair pair, in Guid guid)
        {
            lock (LoginPasswordHashesLocker)
                if (LoginPasswordHashes.ContainsValue(guid))
                    foreach (var key in LoginPasswordHashes.Keys)
                        if (LoginPasswordHashes[key] == guid)
                        {
                            pair = key;

                            break;
                        }
        }
    }
}