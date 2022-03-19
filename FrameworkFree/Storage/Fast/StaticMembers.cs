using System.Collections.Generic;
using System;
using Own.Types;
namespace Own.Storage
{
    internal static partial class Fast
    {
        internal static Random RandomStatic;
        internal static
            Dictionary<Pair, int> LoginPasswordAccIdHashesStatic;
        internal static
            Dictionary<uint, byte> NicksHashesStatic;
        internal static
            Dictionary<Pair, Guid?> LoginPasswordHashesStatic;
        internal static
            Dictionary<Pair, byte> LoginPasswordHashesDeltaStatic;
        internal static
            Dictionary<OwnerId, Dictionary<CompanionId, PrivateMessages>> PersonalPagesStatic;
        private static
            Dictionary<OwnerId, Dictionary<CompanionId, int>> PersonalPagesDepthsStatic;

        private static Dictionary<int, RegBag> RegistrationLineStatic;
        private static Dictionary<int, PreRegBag> PreRegistrationLineStatic;
        internal static Queue<DialogData> DialogsToStartStatic;
        internal static Queue<MessageData> PersonalMessagesToPublishStatic;
        internal static Queue<MessageData> MessagesToPublishStatic;
        internal static Queue<TopicData> TopicsToStartStatic;
        internal static Dictionary<uint, byte> RemoteIpHashesAttemptsCounterStatic;
        internal static Queue<AccountIdentifierRemoteIp> AccountIdentifierRemoteIpLogStatic;
        internal static Queue<uint> CaptchaMessagesStatic;
        internal static Queue<uint> CaptchaMessagesRegistrationDataStatic;
        internal static ICollection<uint> BlockedRemoteIpsHashesStatic;
        internal static string[][] DialogPagesStatic;
        internal static string[][] SectionPagesStatic;
        internal static Queue<PreProfile> PreSaveProfilesLineStatic;
        internal static Dictionary<int, string[]> ThreadPagesStatic;
        internal static Dictionary<int, string> PublicProfilePagesStatic;
        internal static Dictionary<int, string> OwnProfilePagesStatic;
        internal static string[] EndPointPagesStatic;
        internal static string[] PagesStatic;
        internal static int[] DialogPagesPageDepthStatic;
        internal static int[] SectionPagesPageDepthStatic;
        internal static Dictionary<int, int> ThreadPagesPageDepthStatic;
        internal static string CaptchaPageToReturnStatic;
        internal static string PageToReturnRegistrationDataStatic;
        internal static string MainPageStatic;
        internal static string MainContentStatic;
        internal static string TempStatic;
        internal static string ConnectionStringStatic;
        internal static int ThreadsCountStatic;
        internal static int PosStatic;
        internal static int DialogPagesLengthStatic;
        internal static int SectionPagesLengthStatic;
        internal static byte TimerIsWorkingStatic;
    }
}