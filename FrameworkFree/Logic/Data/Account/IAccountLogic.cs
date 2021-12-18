using System.Timers;
using System;
namespace Data
{
    public interface IAccountLogic
    {
        void CheckAccountIdByTimer();
        void LoadAccounts();
        void LoadNicks();
        bool CheckNickHashIfExists(in string nick);
        int? GetAccIdAndStore(Pair pair);
        Pair? CheckPair(in string login, in string password);
    }
}