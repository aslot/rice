namespace Own.DataLockers
{
    internal static class Lockers // использовать только в Storage
    {
        internal static readonly object ConnectionStringLocker = new object();
        internal static readonly object RandomLocker = new object();
        internal static readonly object InitializationTransactionLocker = new object();
        internal static readonly object BlockedRemoteIpsHashesLocker = new object();
        internal static readonly object AccountIdentifierRemoteIpLogLocker = new object();
        internal static readonly object TimerIsWorkingLocker = new object();
        internal static readonly object RemoteIpHashesAttemptsCounterLocker = new object();
        internal static readonly object LoginPasswordHashesDeltaLocker = new object();
        internal static readonly object LoginPasswordAccIdHashesLocker = new object();
        internal static readonly object NicksHashesLocker = new object();
        internal static readonly object LoginPasswordHashesLocker = new object();
        internal static readonly object PersonalPagesLocker = new object();
        internal static readonly object PersonalPagesDepthsLocker = new object();
        internal static readonly object RegistrationLineLocker = new object();
        internal static readonly object PreRegistrationLineLocker = new object();
        internal static readonly object DialogsToStartLocker = new object();
        internal static readonly object PersonalMessagesToPublishLocker = new object();
        internal static readonly object MessagesToPublishLocker = new object();
        internal static readonly object TopicsToStartLocker = new object();
        internal static readonly object CaptchaMessagesLocker = new object();
        internal static readonly object CaptchaMessagesRegistrationDataLocker = new object();
        internal static readonly object DialogPagesLocker = new object();
        internal static readonly object SectionPagesLocker = new object();
        internal static readonly object ThreadPagesLocker = new object();
        internal static readonly object EndPointPagesLocker = new object();
        internal static readonly object pagesLocker = new object();
        internal static readonly object DialogPagesPageDepthLocker = new object();
        internal static readonly object SectionPagesPageDepthLocker = new object();
        internal static readonly object ThreadPagesPageDepthLocker = new object();
        internal static readonly object SpecialLocker = new object();
        internal static readonly object CaptchaPageToReturnLocker = new object();
        internal static readonly object PageToReturnRegistrationDataLocker = new object();
        internal static readonly object MainPageLocker = new object();
        internal static readonly object MainContentLocker = new object();
        internal static readonly object tempLocker = new object();
        internal static readonly object threadsCountLocker = new object();
        internal static readonly object posLocker = new object();
        internal static readonly object DialogPagesLengthLocker = new object();
        internal static readonly object SectionPagesLengthLocker = new object();
        internal static readonly object OwnProfilePagesLocker = new object();
        internal static readonly object PublicProfilePagesLocker = new object();
        internal static readonly object PreSaveProfilesLineLocker = new object();
    }
}