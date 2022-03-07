using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static void StartNewPrivateDialogVoid
            (in string acceptorNick, in Pair pair, in string message)
        {
            if (Fast.GetDialogsToStartCountLocked() < Constants.MaxFirstLineLength)
            {
                DialogData dialogData = new DialogData
                {
                    acceptorNick = acceptorNick,
                    pair = pair,
                    message = message
                };
                Fast.DialogsToStartEnqueueLocked(dialogData);
            }
        }
    }
}
