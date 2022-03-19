using Own.Storage;
using System.Collections.Generic;
using Own.Permanent;
using System.Linq;
using Own.MarkupHandlers;
using Own.Types;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void LoadThreadPagesVoid()
        {
            int threadsCount = Slow.GetThreadsCount();
            Fast.InitializeThreadPagesLocked(threadsCount);
            Fast.InitializeThreadPagesPageDepthLocked(threadsCount);
            IEnumerable<int> threadsIds = Slow.GetExistingThreadsIdsNullable();

            foreach (int id in threadsIds)
                AddThreadVoid(id);
        }
        private static void AddThreadVoid(in int number)
        {
            int count = Slow.CountMessagesByAmount(number);

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.five;

            if (count - pagesCount * Constants.five > Constants.Zero)
                pagesCount++;
            Fast.SetThreadPagesArrayLocked
                    (number, new string[pagesCount]);
            Fast.SetThreadPagesPageDepthLocked(number, pagesCount);
            ProcessThreadReaderVoid
                            (Slow.GetMessagesByAmountNullable(number),
                             number, pagesCount, Slow.GetSectionNumById(number));
        }
        private static void ProcessThreadReaderVoid
       (in IEnumerable<Message> messages, in int number,
           in int pagesCount, in int sectionNum)
        {

            int pageNumber = Constants.Zero;
            string threadName = Slow.GetThreadNameByIdNullable(number);
            Fast.AddToThreadPagesPageLocked(number, pageNumber,
                Marker.GetSectionHeader(number, sectionNum, threadName));
            bool first = false;

            if (messages.Any())
            {
                int i = Constants.Zero;

                foreach (var message in messages)
                {
                    if (i == Constants.Zero && first)
                    {
                        Fast.AddToThreadPagesPageLocked(number, pageNumber,
                            Marker.GetSectionHeader(number, sectionNum, threadName));
                    }

                    int accountId = message.AccountId;

                    Fast.AddToThreadPagesPageLocked(number, pageNumber,
                        Marker.GetArticle(accountId, Slow.GetNickByAccountIdNullable(accountId), message.MsgText));
                    i++;

                    if (i == Constants.five)
                    {
                        string test = Marker.SetThreadNavigation(pageNumber, pagesCount, number);
                        Fast.AddToThreadPagesPageLocked(number, pageNumber, test);

                        if (first)
                            Fast.AddToThreadPagesPageLocked
                                (number, pageNumber, Marker.GetIndicAndA());

                        i = Constants.Zero;
                        pageNumber++;
                    }

                    if (!first)
                        first = true;
                }
                if ((pageNumber >= Constants.Zero)
                        && (i < Constants.five) && (i > Constants.Zero))
                {
                    Fast.AddToThreadPagesPageLocked
                        (number, pageNumber,
                        Marker.SetThreadNavigation(pageNumber, pagesCount, number));

                    if (first)
                        Fast.AddToThreadPagesPageLocked
                            (number, pageNumber, Marker.GetThreadDiv(i));
                }

            }
            if (!first)
                Fast.AddToThreadPagesPageLocked
                    (number, pageNumber, Constants.indicEnd);
        }
    }
}