using System;
using System.Timers;
namespace Data
{
    public interface IRegistrationLogic
    {
        bool CheckNick(in string nick);
        void RegisterInBaseByTimer();
        void PutRegInfoByTimer();
        void PreRegistration
            (in string captcha, in string login, in string password, in string email, in string nick);
        bool CheckPassword(in string password);
        void RefreshLogRegPagesByTimer();
        bool CheckLogin(in string login);
    }
}