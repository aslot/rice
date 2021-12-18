using System;
using System.Timers;
namespace Data
{
    public interface INewPrivateDialogLogic
    {
        void StartNextDialogByTimer();
        void Start(in string acceptorNick, in Pair pair, in string message);
    }
}