using Own.Types;
using Own.Storage;
using Own.Permanent;
using System;
using Own.MarkupHandlers;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void PublishNextPrivateMessageByTimerVoid()
        {
            if (Fast.GetPersonalMessagesToPublishCountLocked() != Constants.Zero)
            {
                MessageData temp = Fast.PersonalMessagesToPublishDequeueLocked();

                if (temp.id == null || temp.pair == null
                    || temp.text == null)
                { }
                else
                    CheckPersonalReplyAndPublishVoid
                        ((int)temp.id, temp.pair.Value, temp.text);
            }
        }

        private static void
            CheckPersonalReplyAndPublishVoid(in int id, in Pair pair, in string text)
        {
            if (CheckNewPrivateMessage(id, text))
                PublishPersonalReplyVoid(id, pair, text);
        }

        private static void PublishPersonalReplyVoid
                (in int id, in Pair pair, in string text)
        {
            int? accId = Own.InRace.Unstable.GetAccountIdNullable(pair);

            if (accId.HasValue)
            {
                Slow.PutPrivateMessageInBaseVoid(accId.Value, id, text);
                string ownerNick = Slow.GetNickByAccountIdNullable(accId.Value);
                string companionNick = Slow.GetNickByAccountIdNullable(id);
                byte order = Constants.One;
                CorrectArrayVoid(id, accId.Value, text, ownerNick, companionNick, order);

                if (id != accId.Value)
                {
                    order = 2;
                    CorrectArrayVoid(accId.Value, id, text, companionNick, ownerNick, order);
                }
            }
        }

        private static void CorrectArrayVoid(in int companionId,
            in int ownerId, in string text,
            in string ownerNick, in string companionNick, in byte order)
        {
            int depth;
            string last = Fast.GetMessageLocked(ownerId, companionId,
               Fast.GetPersonalPagesPageDepthLocked(ownerId, companionId) - Constants.One); ;

            if (last.Contains(Constants.a))
            {
                string[] temp = Fast.GetMessagesLocked(ownerId, companionId);
                Fast.AddToPersonalPagesDepthLocked(companionId, ownerId);
                depth = Own.Storage.Fast
                                .GetPersonalPagesDepthLocked(companionId, ownerId);
                string[] personalPagesArray = new string[depth];
                temp.CopyTo(personalPagesArray, Constants.Zero);
                Fast.SetPersonalPagesMessagesArrayLocked
                            (companionId, ownerId, personalPagesArray);
                Fast.SetPersonalPagesPageLocked
                        (companionId, ownerId, depth - Constants.One,
                        Marker.GenerateNewPrivateMessagePageIfEmpty
                        (companionId, companionNick, order, ownerId, ownerNick, text));
                string[] pages = Fast.GetMessagesLocked(ownerId, companionId);
                int i;
                int start;
                int end;
                string navigation = Constants.SE;

                for (i = Constants.Zero; i < depth; i++)
                {
                    navigation = Marker.GetPrivateMessageArrows(i, depth, companionId);
                    start = pages[i].IndexOf(Constants.fullSpanMarker);

                    if (start != -1)
                    {
                        end = pages[i].LastIndexOf(Constants.spanEnd)
                            + Constants.spanEnd.Length;
                        pages[i] = pages[i].Remove(start, end - start);
                    }
                    else
                    {
                        start = pages[i].IndexOf(Constants.answerMarker);
                    }
                    pages[i] = pages[i].Insert(start, navigation);
                    Fast.SetPersonalPagesPageLocked(companionId, ownerId, i, pages[i]);
                }
            }
            else
            {
                int position = last.LastIndexOf(Constants.brMarker) + Constants.brMarker.Length;
                int pos = last.LastIndexOf(Constants.indic) + Constants.indic.Length;
                int start = pos;
                string countString = Constants.SE;

                while (last[pos] != '<')
                {
                    countString += last[pos];
                    pos++;
                }
                last = last.Remove(start, pos - start);
                last = last.Insert(start, (Convert.ToInt32(countString) - Constants.One).ToString());
                last = last.Insert(position,
                    Marker.GenerateNewPrivateMessagePage(order, ownerId,
                        ownerNick, companionId, companionNick, text));
                depth = Fast.GetPersonalPagesDepthLocked(companionId, ownerId);
                Fast.SetPersonalPagesPageLocked
                    (companionId, ownerId, depth
                    - Constants.One, last);
            }
        }

        private static bool CheckNewPrivateMessage(in int id, in string text)
        {
            int limit = Fast.GetDialogPagesLengthLocked();
            if (id > Constants.Zero
                && id <= limit)
            {
                int textLength = text.Length;

                if (textLength < Constants.One
                || textLength > Constants.MaxReplyMessageTextLength)
                    return false;
                char c;

                for (int i = Constants.Zero; i < textLength; i++)
                {
                    c = text[i];

                    if (Constants.AlphabetRusLower.Contains(char.ToLowerInvariant(c))
                        || char.IsDigit(c) || Fast.SpecialSearchLocked(c))
                    {
                    }
                    else
                        return false;
                }

                return true;
            }
            else
                return false;
        }
    }
}