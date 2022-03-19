using Own.Types;
using Own.Storage;
using Own.Permanent;
using Own.Database;
using Own.MarkupHandlers;
using System;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        private static void
          CheckReplyAndPublishVoid(in int id, in Pair pair, in string text)
        {
            if (CheckReply(id, text))
                PublishReplyVoid(id, pair, text);
        }
        internal static void PublishNextMessageByTimerVoid()
        {
            if (Fast.GetMessagesToPublishCountLocked() != Constants.Zero)
            {
                MessageData temp = Fast.MessagesToPublishDequeueLocked();

                if (temp.id == null || temp.pair == null
                    || temp.text == null)
                { }
                else
                    CheckReplyAndPublishVoid
                        ((int)temp.id, temp.pair.Value, temp.text);
            }
        }
        private static void PublishReplyVoid
                (in int id, in Pair pair, in string text)
        {
            int? accId = Own.InRace.Unstable.GetAccountIdNullable(pair);

            if (accId.HasValue)
            {
                Slow.PutMessageInBaseVoid(new Msg
                { ThreadId = id, AccountId = accId.Value, MsgText = text });
                CorrectArrayVoid(id, accId.Value, text);
            }
        }

        private static void CorrectArrayVoid(in int id, in int accId, in string text)
        {
            string nick = Slow.GetNickByAccountIdNullable(accId);
            string page;
            string last =
                Fast.GetThreadPagesPageLocked
                    (id, Fast.GetThreadPagesPageDepthLocked(id)
                    - Constants.One);


            if (last.Contains(Constants.a))
            {
                string threadName = Slow.GetThreadNameByIdNullable(id);
                int sectionNum = Slow.GetSectionNumById(id);
                string[] temp = Fast.GetThreadPagesArrayLocked(id);
                Fast.AddToThreadPagesPageDepthLocked(id, Constants.One);
                Fast.SetThreadPagesArrayLocked
                    (id, new string[Fast.GetThreadPagesPageDepthLocked(id)]);
                temp.CopyTo(Fast.GetThreadPagesArrayLocked(id)
                            , Constants.Zero);
                page = Marker.GetPageWithHeader(id, sectionNum, threadName,
                    accId, nick, text);
                int len = Fast.GetThreadPagesPageDepthLocked(id);
                Fast.SetThreadPagesPageLocked(id, len - Constants.One, page);
                string[] thread = Fast.GetThreadPagesArrayLocked(id);
                int i;
                int start;
                int end;
                string navigation = Constants.SE;

                for (i = Constants.Zero; i < len; i++)
                {
                    navigation = Marker.GetThreadArrows(i, len, id);
                    start = thread[i].IndexOf(Constants.fullSpanMarker);
                    end = thread[i].LastIndexOf(Constants.spanEnd)
                            + Constants.spanEnd.Length;

                    if (start != -1)
                    {
                        thread[i] = thread[i].Remove(start, end - start);
                    }
                    else
                        start = thread[i].LastIndexOf(Constants.brMarker) + Constants.brMarker.Length;
                    thread[i] = thread[i].Insert(start, navigation);
                    Fast.SetThreadPagesPageLocked(id, i, thread[i]);
                }
            }
            else
            {
                int position = last.LastIndexOf(Constants.brMarker) + Constants.brMarker.Length;
                int pos = last.LastIndexOf(Constants.indic) + Constants.indic.Length;
                int start = pos;
                string countString = Constants.SE;

                while (last[pos] != Constants.TagStartSymbol)
                {
                    countString += last[pos];
                    pos++;
                }
                last = last.Remove(start, pos - start);
                last = last.Insert(start, (Convert.ToInt32(countString) - Constants.One).ToString());
                page = Marker.GetPage(accId, nick, text);
                last = last.Insert(position, page);
                Fast.SetThreadPagesPageLocked
                    (id, Fast.GetThreadPagesPageDepthLocked(id)
                    - Constants.One, last);
            }
        }
        private static bool CheckReply(in int id, in string text)
        {
            if (id > Constants.Zero
                && Fast.ThreadPagesContainsThreadIdLocked(id))
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