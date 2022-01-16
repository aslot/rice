using System.Collections.Generic;
using System;
namespace Data
{
    public sealed partial class Memory
    {
        public static Random Random;
        public static
            Dictionary<Pair, int> LoginPasswordAccIdHashes;
        public static
            Dictionary<uint, byte> NicksHashes;
        public static
            Dictionary<Pair, Guid?> LoginPasswordHashes;
        public static
            Dictionary<Pair, byte> LoginPasswordHashesDelta;
        public static
            Dictionary<OwnerId, Dictionary<CompanionId, PrivateMessages>> PersonalPages;
        private static
            Dictionary<OwnerId, Dictionary<CompanionId, int>> PersonalPagesDepths;

        private static Dictionary<int, RegBag> RegistrationLine;
        private static Dictionary<int, PreRegBag> PreRegistrationLine;
        public static Queue<DialogData> DialogsToStart;
        public static Queue<MessageData> PersonalMessagesToPublish;
        public static Queue<MessageData> MessagesToPublish;
        public static Queue<TopicData> TopicsToStart;
        public static Dictionary<uint, byte> RemoteIpHashesAttemptsCounter;
        public static Queue<AccountIdentifierRemoteIp> AccountIdentifierRemoteIpLog;
        public static Queue<uint> CaptchaMessages;
        public static Queue<uint> CaptchaMessages_RegistrationData;
        public static ICollection<uint> BlockedRemoteIpsHashes;
        public static string[][] DialogPages;
        public static string[][] SectionPages;
        public static Queue<PreProfile> PreSaveProfilesLine;
        public static Dictionary<int, string[]> ThreadPages;
        public static Dictionary<int, string> PublicProfilePages;
        public static Dictionary<int, string> OwnProfilePages;
        public static string[] EndPointPages;
        public static string[] pages;
        public static int[] DialogPagesPageDepth;
        public static int[] SectionPagesPageDepth;
        public static Dictionary<int, int> ThreadPagesPageDepth;
        public static string CaptchaPageToReturn;
        public static string PageToReturn_RegistrationData;
        public static string MainPage;
        public static string MainContent;
        public static string temp;
        public static int threadsCount;
        public static int pos;
        public static int DialogPagesLength;
        public static int SectionPagesLength;
        public static int ThreadPagesLength;
        public static byte TimerIsWorking;
    }
}