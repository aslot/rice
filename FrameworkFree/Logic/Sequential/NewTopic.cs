using System;
using Own.Permanent;
using Own.Types;
using Own.Storage;
using Own.MarkupHandlers;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        private static void PublishTopicVoid
                (in string threadName, in int endpointId,
                in Pair pair, in string message)
        {
            int? accountId = Own.InRace.Unstable.GetAccountIdNullable(pair);

            if (accountId.HasValue)
            {
                string nick = Slow.GetNickByAccountIdNullable(accountId.Value);
                int threadId = Slow.PutThreadAndMessageInBase(
                    new Own.Database.Thread { Name = threadName, EndpointId = endpointId },
                        accountId.Value, message);
                CorrectSectionArrayVoid(endpointId, threadId, threadName);
                Fast.CorrectMessagesArrayLocked(Marker.GetNewPage, endpointId, threadId,
                        message, accountId.Value, threadName, nick);
            }
        }

        private static void MoveThreadAndSetArrowsVoid
                    (in int threadId, in int endpointId)
        {
            SetArrowsForSingleVoid(threadId, endpointId);
            MoveThreadToNextVoid();
        }

        private static void SetArrowsForSingleVoid(in int threadId, in int endpointId)
        {
            int index = endpointId - Constants.One;
            int pagesCount = Own.Storage.Fast
                .GetSectionPagesPageDepthLocked(index);
            string[] arrows = new string[pagesCount];
            int pos;

            for (int i = Constants.Zero; i < pagesCount; i++)
            {
                arrows[i] = Marker
                           .SetSectionNavigation(i, pagesCount, index);
            }

            for (int i = Constants.Zero; i < arrows.Length; i++)
            {
                pos = Fast.GetPageLocked(i).LastIndexOf(Constants.endNavMarker);
                Fast.SetPageLocked(i, Fast.GetPageLocked(i).Insert(pos, arrows[i]));
            }
        }

        private static void ThreadPreloadVoid
            (in int threadId, in string threadName, in int endpointId)
        {
            int len = Fast.GetPagesLengthLocked() + Constants.One;
            string[] temp = new string[len];
            Fast.GetPagesLocked().CopyTo(temp, Constants.Zero);
            Fast.SetPagesLocked(temp);
            Fast.SetSectionPagesPageDepthLocked(endpointId - Constants.One, len);
            Fast.SetLastPageLocked(Marker.GetLastPage(endpointId));
            AddThreadVoid(Constants.Zero, threadId, threadName);
        }

        private static void MoveThreadsAndSetArrowsVoid
                (in int threadId, in int endpointId)
        {
            SetArrowsVoid(threadId, endpointId);
            MoveThreadsInFullVoid(threadId, endpointId);
        }
        private static void MoveThreadsInFullVoid(in int threadId, in int endpointId)
        {
            int len = Fast.GetPagesLengthLocked();
            int i = Constants.Zero, j = Constants.One;

            while (j < len)
            {
                SetFirstThreadVoid(j++, i, GetLastThreadNullable(i));
            }
        }

        private static void SetArrowsVoid(in int threadId, in int endpointId)
        {
            int index = endpointId - Constants.One;
            int pagesCount = Own.Storage.Fast
                .GetSectionPagesPageDepthLocked(index);
            string arrows;

            for (int i = Constants.Zero; i < pagesCount; i++)
            {
                if (Fast.GetPageLocked(i).Contains(Constants.fullSpanMarker))
                {
                    if (Fast.GetPageLocked(i).Contains(Constants.endLinkMarker))
                    {
                        UpdateEndLinkVoid(i);
                    }
                    else
                    {
                        if (Fast.GetPageLocked(i).Contains(Constants.nextLinkMarker))
                        {
                            AddEndLinkAndUpdateNextLinkVoid(i, pagesCount, index);
                        }
                        else
                        {
                            AddEndLinkAndNextLinkVoid(i, pagesCount, index);
                        }
                    }
                }
                else
                {
                    arrows = Marker
                       .SetSectionNavigation(i, pagesCount, index);
                    InsertArrowsVoid(arrows);
                }
            }
        }
        private static void AddEndLinkAndNextLinkVoid(in int i, in int pagesCount,
           in int number)
        {
            AddEndLinkAndUpdateNextLinkVoid(i, pagesCount, number);
        }
        private static void AddEndLinkAndUpdateNextLinkVoid
            (in int i, in int pagesCount, in int number)
        {
            string arrows = Marker
                       .SetSectionNavigation(i, pagesCount, number);
            int pos1 = Fast.GetPageLocked(i).IndexOf(Constants.fullSpanMarker);
            int pos2 = Fast.GetPageLocked(i).LastIndexOf(Constants.brMarker) + Constants.brMarker.Length;
            Fast.SetPageLocked(i, Fast.GetPageLocked(i).Remove(pos1, pos2 - pos1));
            Fast.SetPageLocked(i, Fast.GetPageLocked(i).Insert(pos1, arrows));
        }
        private static void UpdateEndLinkVoid(in int i)
        {
            int pos = Fast.GetPageLocked(i).LastIndexOf(Constants.pageMarker)
                                    + Constants.pageMarker.Length;
            int newPos = pos;
            string pageNum;
            char c = Fast.GetPageLocked(i)[newPos];
            pageNum = c.ToString();

            while (true)
            {
                newPos++;
                c = Fast.GetPageLocked(i)[newPos];

                if (char.IsDigit(c))
                {
                    pageNum += c;
                }
                else
                    break;
            }
            int pageNumInt = Convert.ToInt32(pageNum);
            pageNumInt++;
            Fast.SetPageLocked(i, Fast.GetPageLocked(i).Remove(pos, pageNum.Length));
            pageNum = pageNumInt.ToString();
            Fast.SetPageLocked(i, Fast.GetPageLocked(i).Insert(pos, pageNum));
        }
        private static void InsertArrowsVoid(in string arrows)
        {
            int len = Fast.GetPagesLengthLocked() - Constants.One;
            int pos = Fast.GetPageLocked(len).IndexOf(Constants.navMarker)
                        + Constants.navMarker.Length;
            Fast.SetPageLocked(len, Fast.GetPageLocked(len).Insert(pos, arrows));
        }
        private static void AddThreadToBeginVoid
            (in int threadId, in string threadName)
        {
            AddThreadVoid(Constants.Zero, threadId, threadName);
            MoveThreadsVoid(threadId, threadName);
        }
        private static void MoveThreadsVoid(in int threadId, in string threadName)
        {
            int len = Fast.GetPagesLengthLocked() - Constants.One;
            int j;

            for (int i = Constants.Zero; i < len; i++)
            {
                j = i + Constants.One;
                Fast.SetPageLocked(j,
                Fast.GetPageLocked(i).Insert
                    (Fast.GetPageLocked(j).IndexOf(Constants.navMarker)
                        + Constants.navMarker.Length, GetLastThreadNullable(i)));
            }
        }
        private static string GetLastThreadNullable(in int i)
        {
            string page = Fast.GetPageLocked(i);
            int lastThreadPos = page.LastIndexOf(Constants.pMarker) - Constants.brMarker.Length;
            int count = page.LastIndexOf(Constants.spanMarker) - lastThreadPos;
            string result = page.Substring(lastThreadPos, count);
            Fast.SetPageLocked(i, page
                .Remove(lastThreadPos, count));

            return result;
        }

        private static void AddThreadVoid
            (in int i, in int threadId, in string threadName)
        {
            SetPosAndTempVoid(i, threadId, threadName);
            Fast.SetPageLocked(i, Fast.GetPageLocked(i)
                .Insert(Fast.GetPosLocked(), Fast.GetTempLocked()));
        }
        private static void
            CheckTopicAndPublishVoid(in string threadName, in int endpointId,
                        in Pair pair, in string message)
        {
            if (CheckTopicInfo(threadName, endpointId, message))
                PublishTopicVoid
                        (threadName, endpointId, pair, message);
        }
        internal static void StartNextTopicByTimerVoid()
        {
            if (Fast.GetTopicsToStartCountLocked() != Constants.Zero)
            {
                TopicData temp = Fast.TopicsToStartDequeueLocked();
                if (temp.endpointId == null || temp.message == null
                    || temp.threadName == null || temp.pair == null)
                { }
                else
                    CheckTopicAndPublishVoid
                     (temp.threadName, (int)temp.endpointId,
                        temp.pair.Value, temp.message);
            }
        }
        private static bool CheckTopicInfo
            (in string threadName, in int endpointId, in string message)
        {
            if (endpointId > Constants.Zero
                    && endpointId < Fast.GetSectionPagesLengthLocked() + Constants.One)
            {
                if (CheckText(message) && CheckThreadName(threadName))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private static bool CheckThreadName(in string message)
        {
            int messageLength = message.Length;

            if (messageLength < Constants.One
                || messageLength > Constants.MaxForumThreadNameTextLength)
                return false;
            char c;

            for (int i = Constants.Zero; i < messageLength; i++)
            {
                c = message[i];

                if (Constants.AlphabetRusLower.Contains(char.ToLowerInvariant(c))
                    || char.IsDigit(c) || Fast.SpecialSearchLocked(c))
                {

                }
                else
                    return false;
            }

            return true;
        }
        internal static bool CheckText(in string message)
        {
            int messageLength = message.Length;

            if (messageLength < Constants.One
            || messageLength > Constants.MaxReplyMessageTextLength)
                return false;
            char c;

            for (int i = Constants.Zero; i < messageLength; i++)
            {
                c = message[i];

                if (Constants.AlphabetRusLower.Contains(char.ToLowerInvariant(c))
                    || char.IsDigit(c) || Fast.SpecialSearchLocked(c))
                {
                }
                else
                    return false;
            }

            return true;
        }

        internal static int CountStringOccurrences
            (in string text, in string pattern)
        {
            int count = Constants.Zero;
            int i = Constants.Zero;

            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }

            return count;
        }
        private static void SetPosAndTempVoid
            (in int i, in int threadId, in string threadName)
        {
            Fast.SetPosLocked(Fast.GetPageLocked(i).IndexOf(Constants.navMarker)
                + Constants.navMarker.Length);
            Fast.SetTempLocked(Marker.GetTemp(threadId, threadName));
        }
        private static void SetFirstThreadVoid(in int next, in int current, in string lastThread)
        {
            string page = Fast.GetPageLocked(current);
            int pos = page.IndexOf(Constants.navMarker) + Constants.navMarker.Length;
            Fast.SetPageLocked(next, page.Insert(pos, lastThread));
        }

        private static void AddThreadToFullVoid
            (in int threadId, in string threadName, in int endpointId)
        {
            ThreadPreloadVoid(threadId, threadName, endpointId);
            MoveThreadsAndSetArrowsVoid(threadId, endpointId);
        }

        private static void MoveThreadToNextVoid()
        {
            SetFirstThreadVoid(Constants.One, Constants.One, GetLastThreadNullable(Constants.Zero));
        }

        private static void CorrectSectionArrayVoid
                                (in int endpointId, in int threadId, in string threadName)
        {
            Fast.SetSectionPagesArrayLocked(endpointId);
            Fast.SetThreadsCountLocked(CountStringOccurrences
                    (Fast.GetLastPageLocked(), Constants.pMarker));

            if (Fast.GetPagesLengthLocked() > Constants.One)
            {
                if (Fast.GetThreadsCountLocked() == Constants.threadsOnPage)
                {
                    AddThreadToFullVoid(threadId, threadName, endpointId);
                }
                else
                {
                    AddThreadToBeginVoid(threadId, threadName);
                }
            }
            else
            {
                if (Fast.GetThreadsCountLocked() == Constants.threadsOnPage)
                {
                    AddThreadToSingleVoid(threadId, threadName, endpointId);
                }
                else
                {
                    AddThreadVoid
                        (Constants.Zero, threadId, threadName);
                }
            }
            Fast.SetSectionPagesArrayLocked
                                (endpointId - Constants.One, Fast.GetPagesLocked());
        }
        private static void AddThreadToSingleVoid
            (in int threadId, in string threadName, in int endpointId)
        {
            ThreadPreloadVoid(threadId, threadName, endpointId);
            MoveThreadAndSetArrowsVoid(threadId, endpointId);
        }
    }
}