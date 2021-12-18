using System.Net;
using System;
namespace Data
{
    public interface IFriendlyFire
    {
        void ProfileLogic_Start(int accountId, string aboutMe,
            bool[] flags, byte[] file);
        string ForumLogic_GetMainPageLocked();
        string ThreadData_GetThreadPage(int? id, int? page);
        string ForumLogic_GetMainContentLocked();
        string GetPublicProfilePageIfExists(int accountId);
        string GetOwnProfilePage(int accountId);
        string SectionLogic_GetSectionPage(int? id, int? page);
        string EndPointLogic_GetEndPointPage(int? id);
        string LoginData_CheckAndAuth(IPAddress ip, string captcha, string login, string password);
        string GetRegistrationDataPageToReturn();
        bool AuthenticationLogic_AccessGranted(string token);
        void ReplyData_Start(int? id, Pair pair, string t);
        void RemoveAccountByNickIfExists(string uniqueNick);
        Pair AuthenticationLogic_GetPair(string token);
        string LoginData_GetPageToReturn();
        void RegistrationData_PreRegistration(string captcha, string login,
        string password, string email, string nick);
        void NewTopicData_Start(string t, int? id, Pair pair, string m);
        string PrivateDialogLogic_GetDialogPage(int? id, Pair pair);
        string PrivateMessageLogic_GetPersonalPage(int? id, int? page, Pair pair);
        int GetDialogPagesLengthFast();
        void NewPrivateMessageLogic_Start(int? id, Pair pair, string t);
        void NewPrivateDialogLogic_Start(string nick, Pair pair, string msg);
        bool CheckIp(IPAddress ip, byte incValue = Constants.Fifty);
        Tuple<bool, int> AuthneticationLogic_AccessGrantedExtended(string token);
    }
}