using System;
using Models;
using MarkupHandlers;
namespace Data
{
    internal sealed class ReplyLogic : IReplyLogic
    {
        private static readonly object locker = new object();
        private readonly IStorage Storage;
        private readonly IAccountLogic AccountLogic;
        private readonly IThreadLogic ThreadLogic;
        private readonly ReplyMarkupHandler ReplyMarkupHandler;
        private readonly ThreadMarkupHandler ThreadMarkupHandler;
        public ReplyLogic(IStorage storage,
        IAccountLogic accountLogic,
        IThreadLogic threadLogic,
        ReplyMarkupHandler replyMarkupHandler,
        ThreadMarkupHandler threadMarkupHandler)
        {
            Storage = storage;
            AccountLogic = accountLogic;
            ThreadLogic = threadLogic;
            ReplyMarkupHandler = replyMarkupHandler;
            ThreadMarkupHandler = threadMarkupHandler;
        }
        private void
            CheckReplyAndPublish(in int id, in Pair pair, in string text)
        {
            if (Check(id, text))
                PublishReply(id, pair, text);
        }

        public void Start(in int? id, in Pair pair, in string t)
        {
            if (Storage.Fast.GetMessagesToPublishCount() < Constants.MaxFirstLineLength)
            {
                MessageData messageData
                      = new MessageData
                      {
                          id = id,
                          pair = pair,
                          text = t
                      };
                Storage.Fast.MessagesToPublishEnqueue(messageData);
            }
        }
        public void PublishNextMessageByTimer()
        {
            if (Storage.Fast.GetMessagesToPublishCount() != Constants.Zero)
            {
                MessageData temp = Storage.Fast.MessagesToPublishDequeue();

                if (temp.id == null || temp.pair == null
                    || temp.text == null)
                { }
                else
                    CheckReplyAndPublish
                        ((int)temp.id, temp.pair.Value, temp.text);
            }
        }

        public int? GetAccountId(in Pair pair)
        {
            int? accId = null;

            try
            {
                accId = Storage.Fast.GetLoginPasswordAccIdHashes(pair);
            }
            catch
            {
                lock (locker)
                    accId = AccountLogic.GetAccIdAndStore(pair);
            }

            return accId;
        }

        private void PublishReply
                (in int id, in Pair pair, in string text)
        {
            int? accId = GetAccountId(pair);

            if (accId.HasValue)
            {
                Storage.Slow.PutMessageInBase(new Msg
                { ThreadId = id, AccountId = accId.Value, MsgText = text });
                CorrectArray(id, accId.Value, text);
            }
        }

        private void CorrectArray(in int id, in int accId, in string text)
        {
            string nick = ThreadLogic.GetNick(accId);
            string page;
            string last =
                Storage.Fast.GetThreadPagesPageLocked
                    (id, Storage.Fast.GetThreadPagesPageDepthLocked(id)
                    - Constants.One);


            if (last.Contains(Constants.a))
            {
                int incrementedId = id + Constants.One;
                string threadName = Storage.Slow.GetThreadNameById(incrementedId);
                int sectionNum = Storage.Slow.GetSectionNumById(incrementedId);
                string[] temp = Storage.Fast.GetThreadPagesArrayLocked(id);
                Storage.Fast.AddToThreadPagesPageDepthLocked(id, Constants.One);
                Storage.Fast.SetThreadPagesArrayLocked
                    (id, new string[Storage.Fast.GetThreadPagesPageDepthLocked(id)]);
                temp.CopyTo(Storage.Fast.GetThreadPagesArrayLocked(id)
                            , Constants.Zero);
                page = ReplyMarkupHandler.GetPageWithHeader(id, sectionNum, threadName,
                    accId, nick, text);
                int len = Storage.Fast.GetThreadPagesPageDepthLocked(id);
                Storage.Fast.SetThreadPagesPageLocked(id, len - Constants.One, page);
                string[] thread = Storage.Fast.GetThreadPagesArrayLocked(id);
                int i;
                int start;
                int end;
                string navigation = Constants.SE;

                for (i = Constants.Zero; i < len; i++)
                {
                    navigation = ThreadMarkupHandler.GetArrows(i, len, id);
                    start = thread[i].LastIndexOf(Constants.spanIndicator);
                    end = thread[i].LastIndexOf(Constants.spanEnd)
                            + Constants.spanEnd.Length;

                    if (start != -1)
                    {
                        thread[i] = thread[i].Remove(start, end - start);
                    }
                    else
                        start = thread[i].LastIndexOf(Constants.brMarker) + Constants.brMarker.Length;
                    thread[i] = thread[i].Insert(start, navigation);
                    Storage.Fast.SetThreadPagesPageLocked(id, i, thread[i]);
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
                page = ReplyMarkupHandler.GetPage(accId, nick, text);
                last = last.Insert(position, page);
                Storage.Fast.SetThreadPagesPageLocked
                    (id, Storage.Fast.GetThreadPagesPageDepthLocked(id)
                    - Constants.One, last);
            }
        }
        private bool Check(in int id, in string text)
        {
            if (id > Constants.Zero
                && Storage.Fast.ThreadPagesContainsThreadIdLocked(id))
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
                    || char.IsDigit(c) || Storage.Fast.SpecialSearch(c))
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
