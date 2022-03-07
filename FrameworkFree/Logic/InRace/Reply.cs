using System;
using Own.MarkupHandlers;
using Own.Permanent;
using Own.Types;
using Own.Database;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        private static readonly object replyLocker = new object();

        internal static void StartReplyVoid(in int? id, in Pair pair, in string t)
        {
            if (Fast.GetMessagesToPublishCountLocked() < Constants.MaxFirstLineLength)
            {
                MessageData messageData
                      = new MessageData
                      {
                          id = id,
                          pair = pair,
                          text = t
                      };
                Fast.MessagesToPublishEnqueueLocked(messageData);
            }
        }

        internal static int? GetAccountIdNullable(in Pair pair)
        {
            int? accId = Fast.GetLoginPasswordAccIdHashesLocked(pair);

            if (!accId.HasValue)
                lock (replyLocker)
                    return Own.Sequential.Unstable.GetAccIdAndStore(pair);

            return accId;
        }
    }
}
