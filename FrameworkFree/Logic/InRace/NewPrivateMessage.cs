using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static void StartNewPrivateMessageVoid(in int? id, in Pair pair, in string t)
        {
            if (Fast.GetPersonalMessagesToPublishCountLocked() < Constants.MaxFirstLineLength)
            {
                MessageData personalMessageData = new MessageData
                {
                    id = id,
                    pair = pair,
                    text = t
                };
                Fast.PersonalMessagesToPublishEnqueueLocked(personalMessageData);
            }
        }
    }
}
