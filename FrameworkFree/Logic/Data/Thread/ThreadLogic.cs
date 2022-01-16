using System.Collections.Generic;
using MarkupHandlers;
using System.Linq;
namespace Data
{
    internal sealed class ThreadLogic : IThreadLogic
    {
        private readonly IStorage Storage;
        private readonly ThreadMarkupHandler ThreadMarkupHandler;
        public ThreadLogic(IStorage storage,
        ThreadMarkupHandler threadMarkupHandler)
        {
            Storage = storage;
            ThreadMarkupHandler = threadMarkupHandler;
        }

        public string GetThreadPage(in int? id, in int? page)
        {
            if (id == null || page == null)
                return Constants.SE;
            else
            {
                int index = (int)id;

                if (index > Constants.Zero
                    && Storage.Fast.ThreadPagesContainsThreadIdLocked(index))
                {
                    if (page > Constants.Zero
                        && page <= Storage.Fast.GetThreadPagesPageDepthLocked(index))
                        return Storage.Fast
                            .GetThreadPagesPageLocked(index,
                                (int)page - Constants.One);
                    else
                        return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }

        public void LoadThreadPages()
        {
            int threadsCount = Storage.Slow.GetThreadsCount();
            Storage.Fast.InitializeThreadPagesLocked(threadsCount);
            Storage.Fast.InitializeThreadPagesPageDepthLocked(threadsCount);
            IEnumerable<int> threadsIds = Storage.Slow.GetExistingThreadsIds();

            foreach (int id in threadsIds)
            {
                AddThread(id);
            }
        }

        private void AddThread(in int number)
        {
            int count = Storage.Slow.CountMessagesByAmount(number);

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.five;

            if (count - pagesCount * Constants.five > Constants.Zero)
                pagesCount++;
            Storage.Fast.SetThreadPagesArrayLocked
                    (number, new string[pagesCount]);
            Storage.Fast.SetThreadPagesPageDepthLocked(number, pagesCount);
            ProcessThreadReader
                            (Storage.Slow.GetMessagesByAmount(number),
                             number, pagesCount, Storage.Slow.GetSectionNumById(number));
        }
        private void ProcessThreadReader
       (in IEnumerable<Message> messages, in int number,
           in int pagesCount, in int sectionNum)
        {

            int pageNumber = Constants.Zero;
            string threadName = Storage.Slow.GetThreadNameById(number);
            Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber,
                ThreadMarkupHandler.GetSectionHeader(number, sectionNum, threadName));
            bool first = false;

            if (messages.Any())
            {
                int i = Constants.Zero;

                foreach (var message in messages)
                {
                    if (i == Constants.Zero && first)
                    {
                        Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber,
                            ThreadMarkupHandler.GetSectionHeader(number, sectionNum, threadName));
                    }

                    int accountId = message.AccountId;

                    Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber,
                        ThreadMarkupHandler.GetArticle(accountId, GetNick(accountId), message.MsgText));
                    i++;

                    if (i == Constants.five)
                    {
                        string test = ThreadMarkupHandler.SetNavigation(pageNumber, pagesCount, number);
                        Storage.Fast.AddToThreadPagesPageLocked(number, pageNumber, test);

                        if (first)
                            Storage.Fast.AddToThreadPagesPageLocked
                                (number, pageNumber, ThreadMarkupHandler.GetIndicAndA());

                        i = Constants.Zero;
                        pageNumber++;
                    }

                    if (!first)
                        first = true;
                }
                if ((pageNumber >= Constants.Zero)
                        && (i < Constants.five) && (i > Constants.Zero))
                {
                    Storage.Fast.AddToThreadPagesPageLocked
                        (number, pageNumber,
                        ThreadMarkupHandler.SetNavigation(pageNumber, pagesCount, number));

                    if (first)
                        Storage.Fast.AddToThreadPagesPageLocked
                            (number, pageNumber, ThreadMarkupHandler.GetThreadDiv(i));
                }

            }
            if (!first)
                Storage.Fast.AddToThreadPagesPageLocked
                    (number, pageNumber, Constants.indicEnd);
        }
        public string GetNick(in int accountId)
        => Storage.Slow.GetNickByAccountId(accountId);
    }
}
