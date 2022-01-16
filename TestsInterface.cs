using Data;
using XXHash;
using MarkupHandlers;
namespace TestsInterface
{
    public static class TestsCalls // доступ по ключу?
    {
        private static IFriendlyFire TestForce;
        private static bool accessGranted = false;
        public static void Initialize(string accessKey)
        {
            if (TestForce == null
                && XXHash32.Hash(accessKey) == Constants.TestsInterfaceAccessKeyHash)
            {
                accessGranted = true;
                var memory = new Memory();
                var storage = new Storage(memory, new Database(memory));
                var threadMarkupHandler = new ThreadMarkupHandler();
                var replyMarkupHandler = new ReplyMarkupHandler();
                var endPointMarkupHandler = new EndPointMarkupHandler();
                var privateDialogMarkupHandler = new PrivateDialogMarkupHandler();
                var privateMessageMarkupHandler = new PrivateMessageMarkupHandler();
                var forumMarkupHandler = new ForumMarkupHandler();
                var newPrivateDialogMarkupHandler = new NewPrivateDialogMarkupHandler();
                var registrationMarkupHandler = new RegistrationMarkupHandler();
                var newPrivateMessageMarkupHandler = new NewPrivateMessageMarkupHandler();
                var sectionMarkupHandler = new SectionMarkupHandler();
                var newTopicMarkupHandler = new NewTopicMarkupHandler();
                var loginMarkupHandler = new LoginMarkupHandler();
                var profileMarkupHandler = new ProfileMarkupHandler();

                var accountLogic = new AccountLogic(storage, profileMarkupHandler);
                var threadLogic = new ThreadLogic(storage,
                threadMarkupHandler);
                var replyLogic = new ReplyLogic(storage,
                accountLogic,
                threadLogic,
                replyMarkupHandler,
                threadMarkupHandler
                );
                var endPointLogic = new EndPointLogic(storage,
                endPointMarkupHandler);
                var privateDialogLogic = new PrivateDialogLogic(storage,
                replyLogic, privateDialogMarkupHandler);
                var privateMessageLogic = new PrivateMessageLogic(storage,
                threadLogic,
                replyLogic,
                privateMessageMarkupHandler);
                var forumLogic = new ForumLogic(storage,
                forumMarkupHandler);
                var captchaGenerator = new CaptchaGenerator();
                var captchaMarkupHandler = new CaptchaMarkupHandler();
                var captcha = new Captcha(captchaGenerator,
                storage,
                captchaMarkupHandler);
                var registrationLogic = new RegistrationLogic(storage,
                registrationMarkupHandler,
                captcha);
                var sectionLogic = new SectionLogic(storage,
                sectionMarkupHandler,
                newTopicMarkupHandler);
                var newTopicLogic = new NewTopicLogic(storage,
                sectionLogic,
                threadLogic,
                replyLogic,
                newTopicMarkupHandler,
                sectionMarkupHandler);
                var newPrivateDialogLogic = new NewPrivateDialogLogic(storage,
                accountLogic,
                privateMessageLogic,
                threadLogic,
                replyLogic,
                registrationLogic,
                newTopicLogic,
                newPrivateDialogMarkupHandler,
                privateDialogMarkupHandler,
                privateMessageMarkupHandler);
                var newPrivateMessageLogic = new NewPrivateMessageLogic(storage,
                threadLogic,
                replyLogic,
                newPrivateMessageMarkupHandler,
                privateMessageMarkupHandler);
                var authenticationLogic = new AuthenticationLogic(storage);
                var loginLogic = new LoginLogic(storage,
                registrationLogic,
                accountLogic,
                authenticationLogic,
                captcha,
                loginMarkupHandler);
                var profileLogic = new ProfileLogic(storage,
                profileMarkupHandler);

                TestForce = new FriendlyFire(accountLogic,
                storage,
                endPointLogic,
                privateDialogLogic,
                privateMessageLogic,
                forumLogic,
                newPrivateDialogLogic,
                newPrivateMessageLogic,
                sectionLogic,
                newTopicLogic,
                threadLogic,
                replyLogic,
                registrationLogic,
                loginLogic,
                captcha,
                authenticationLogic,
                profileLogic);
            }
        }
        public static void Register(string captcha, string login,
            string password, string email, string nick)
        {
            if (accessGranted)
                TestForce.RegistrationData_PreRegistration(captcha,
                    login, password, email, nick);
        }
        public static string Authenticate(string captcha, string login, string password)
        {
            if (accessGranted)
                return TestForce.LoginData_CheckAndAuth(new System.Net.IPAddress(0x2414188f/*143.24.20.36*/), captcha, login, password);
            else
                return Constants.SE;
        }
        public static int GetXXHashCode(string text)
        {
            if (accessGranted)
                return unchecked((int)XXHash32.Hash(text));
            else
                return int.MinValue;
        }
        public static void RemoveAccountByNickIfExists(string uniqueNick)
        {
            if (accessGranted)
                TestForce.RemoveAccountByNickIfExists(uniqueNick);
        }
        public static string RetrieveEndPointMarkup(int? id)
        {
            if (accessGranted)
                return TestForce.EndPointLogic_GetEndPointPage(id);
            else
                return Constants.SE;
        }
    }
}