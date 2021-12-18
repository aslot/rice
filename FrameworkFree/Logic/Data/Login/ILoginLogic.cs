using System.Net;
namespace Data
{
    public interface ILoginLogic
    {
        void InitPage();
        string CheckAndAuth
        (in IPAddress ip, in string captcha, in string login, in string password);
    }
}