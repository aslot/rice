using Own.Storage;
using Own.Types;
using Own.MarkupHandlers;
using Own.Permanent;
using System;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static bool CheckNickIfExists(in string nick)
        => Own.Sequential.Unstable.CheckNickHashIfExists(nick)
                && Slow.CheckNickInBase(nick);

        internal static bool CheckDialogInfo
          (in string acceptorNick, in string message)
        {
            if (acceptorNick != null && message != null)
            {
                if (Own.Sequential.Unstable.CheckText(message)
                    && Own.Sequential.Unstable.CheckNick(acceptorNick)
                    && Own.Sequential.Unstable.CheckNickIfExists(acceptorNick))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        internal static void StartNextDialogByTimerVoid()
        {
            if (Fast.GetDialogsToStartCountLocked() != Constants.Zero)
            {
                DialogData temp = Fast.DialogsToStartDequeueLocked();

                if (temp.acceptorNick == null
                    || temp.message == null
                    || temp.pair == null)
                { }
                else
                    CheckDialogAndPublishVoid
                         (temp.pair.Value, temp.acceptorNick, temp.message);
            }
        }

        internal static void SetNewCompanionDepthVoid
            (in OwnerId ownerId, in CompanionId companionId)
        {
            bool flag = Fast.PersonalPagesDepthsKeysContainsLocked(ownerId);

            if (!Fast.PersonalPagesDepthsContainsKeyLocked(ownerId, companionId, flag))
                Fast.PersonalPagesDepthsAddLocked(ownerId, companionId, flag);
        }

        internal static void
            CheckDialogAndPublishVoid(in Pair pair, in string acceptorNick, in string message)
        {
            if (CheckDialogInfo(acceptorNick, message))
                PublishDialogVoid
                        (acceptorNick, pair, message);
        }

        internal static void PublishDialogVoid
                (in string acceptorNick, in Pair pair, in string message)
        {
            int acceptorId =
                Slow.GetIdByNick(acceptorNick);
            int? accountId =
                Own.InRace.Unstable.GetAccountIdNullable(pair);

            if (accountId.HasValue)
            {
                string nick =
                    Slow.GetNickByAccountIdNullable(accountId.Value);
                Slow.PutPrivateMessageInBaseVoid
                    (accountId.Value, acceptorId, message);
                bool notEqualFlag = accountId != acceptorId;
                CorrectDialogPagesVoid(acceptorId, accountId.Value, nick,
                    acceptorNick, notEqualFlag);
                CorrectMessagesPagesVoid(accountId.Value, acceptorId, nick,
                    acceptorNick, message, notEqualFlag);
            }
        }

        internal static void CorrectMessagesPagesVoid(in int accountId,
         in int acceptorId, in string accountNick, in string acceptorNick,
         in string message, in bool notEqualFlag)
        {
            Unstable
                .AddNewCompanionsIfNotExistsVoid(accountId, acceptorId,
                accountNick, acceptorNick, notEqualFlag);
            CorrectMessagesArrayVoid(acceptorId, accountId,
                        accountNick, acceptorNick, message, accountNick, accountId);

            if (notEqualFlag)
                CorrectMessagesArrayVoid(accountId, acceptorId,
                    acceptorNick, accountNick, message, accountNick, accountId);
        }
        internal static void CorrectMessagesArrayVoid(in int companionId,
            in int ownerId, in string ownerNick, in string companionNick,
            in string message, in string accountNick, in int accountId)
        {
            int depth;
            string last = Fast.GetMessageLocked(ownerId, companionId,
               Fast.GetPersonalPagesPageDepthLocked(ownerId, companionId) - Constants.One);

            if (last.Contains(Constants.a))
            {
                Fast.AddToPersonalPagesDepthLocked(companionId, ownerId);
                depth = Own.Storage.Fast
                                .GetPersonalPagesDepthLocked(companionId, ownerId);
                string[] personalPagesArray = new string[depth];
                Fast.GetMessagesLocked(ownerId, companionId).CopyTo(personalPagesArray, Constants.Zero);
                Fast.SetPersonalPagesMessagesArrayLocked
                            (companionId, ownerId, personalPagesArray);

                Fast.SetPersonalPagesPageLocked
                        (companionId, ownerId, depth - Constants.One,
                            Marker.GenerateNewPrivateDialogPage(companionId,
                                companionNick, ownerId, ownerNick, message));
                string[] pages = Fast.GetMessagesLocked(ownerId, companionId);
                int start;

                for (int i = Constants.Zero; i < depth; i++)
                {
                    start = pages[i].IndexOf(Constants.fullSpanMarker);

                    if (start != -1)
                        pages[i] = pages[i].Remove(start, pages[i].LastIndexOf(Constants.endSpanMarker)
                                + Constants.endSpanMarker.Length - start);
                    else
                        start = pages[i].IndexOf(Constants.answerMarker);
                    pages[i] = pages[i].Insert(start, Marker.GetPrivateMessageArrows(i, depth, companionId));
                    Fast.SetPersonalPagesPageLocked(companionId, ownerId, i, pages[i]);
                }
            }
            else
            {
                int pos = last.LastIndexOf(Constants.indic) + Constants.indic.Length;
                int start = pos;
                string countString = Constants.SE;

                while (last[pos] != '<')
                {
                    countString += last[pos];
                    pos++;
                }
                last = last.Remove(start, pos - start).Insert(start, (Convert.ToInt32(countString) - Constants.One).ToString());
                int temp = last.LastIndexOf(Constants.brMarker);
                Fast.SetPersonalPagesPageLocked
                    (companionId, ownerId, Fast.GetPersonalPagesDepthLocked(companionId, ownerId) - Constants.One,
                    last.Insert(temp != -1
                        ? temp + Constants.brMarker.Length
                        : last.IndexOf(Constants.h2End) + Constants.h2End.Length,
                         Marker.GenerateNewPrivateDialogArticle(accountId, accountNick, message)));
            }
        }

        internal static void CorrectDialogPagesVoid
            (in int acceptorId, in int accountId, in string accountNick,
            in string acceptorNick, in bool notEqualFlag)
        {
            CorrectPagesVoid(accountId, accountNick,
                    acceptorId, acceptorNick);

            if (notEqualFlag)
                CorrectPagesVoid(acceptorId, acceptorNick,
                        accountId, accountNick);
        }

        internal static void CorrectPagesVoid
            (in int firstId, in string firstNick,
            in int secondId, in string secondNick)
        {
            int index = firstId - Constants.One;
            string[] pages = Own.Storage.Fast
                .GetDialogPagesArrayLockedLocked(index);
            int depth = Own.Storage.Fast
                .GetDialogPagesPageDepthLocked(index);
            bool containsNick = false;

            for (int i = Constants.Zero; i < depth; i++)
                if (containsNick)
                    break;
                else
                    if (pages[i].Contains(string.Concat(">", secondNick, "<")))
                    containsNick = true;

            if (!containsNick)
            {
                int nicksCount = Own.Sequential.Unstable
                    .CountStringOccurrences
                    (pages[depth - Constants.One], Constants.pMarker);

                if (depth > Constants.One)
                {
                    if (nicksCount == Constants.DialogsOnPage)
                    {
                        AddDialogToFullVoid
                            (pages, depth, firstId,
                            secondId, secondNick);
                    }
                    else
                    {
                        AddDialogToPartialVoid
                            (pages, depth, secondId,
                            secondNick, firstId);
                    }
                }
                else
                {
                    if (nicksCount == Constants.DialogsOnPage)
                    {
                        AddDialogToFullVoid
                            (pages, depth, firstId,
                            secondId, secondNick);
                    }
                    else
                    {
                        AddDialogToPartialVoid
                           (pages, depth, secondId,
                           secondNick, firstId);
                    }
                }
            }
        }

        internal static void AddDialogToPartialVoid
            (in string[] pages, in int depth, in int secondId,
            in string secondNick, in int firstId)
        {
            string[] pagesArray = pages;
            int index = depth - Constants.One;
            int startPos = pages[index]
                .IndexOf(Constants.fullSpanMarker);

            if (startPos == -1)
                startPos = pages[index].IndexOf(Constants.endNavMarker);
            pagesArray[index] = pagesArray[index]
                                    .Insert(startPos,
                                        Marker.GenerateNewPrivateDialogEntry
                                            (secondId, secondNick));//doublepost ERROR
            Own.Storage.Fast
                        .SetDialogPagesArrayLocked
                        (firstId - Constants.One, pagesArray);
        }
        internal static void AddDialogToFullVoid
           (in string[] pages, in int depth, in int firstId,
            in int secondId, in string secondNick)
        {
            int len = depth + Constants.One;
            string[] newPagesArray = new string[len];
            pages.CopyTo(newPagesArray, Constants.Zero);
            newPagesArray[depth] = Constants.NewDialog;
            int index = firstId - Constants.One;
            newPagesArray = InsertArrowsNullable(newPagesArray, len);
            newPagesArray = InsertDialogToFullNullable
                (newPagesArray, depth, secondId, secondNick);
            Own.Storage.Fast
                        .SetDialogPagesArrayLocked
                        (index, newPagesArray);
            Fast.SetDialogPagesPageDepthLocked(index, len);
        }
        internal static string[] InsertDialogToFullNullable
                (in string[] newPagesArray, in int depth,
                in int secondId, in string secondNick)
        {
            int startPos = newPagesArray[depth]
                                        .IndexOf(Constants.navMarker)
                                        + Constants.navMarker.Length;
            newPagesArray[depth] = newPagesArray[depth]
                                .Insert(startPos,
                                Marker
                                .GenerateNewPrivateDialogEntryWithEnd
                                    (secondId, secondNick));

            return newPagesArray;
        }

        internal static string[] InsertArrowsNullable
                (in string[] newPagesArray, in int len)
        {
            for (int i = Constants.Zero; i < len; i++)
            {
                string arrows =
                    Marker.GetArrows(i, len);
                int tempPos = newPagesArray[i].IndexOf(Constants.fullSpanMarker);

                if (tempPos != -1)
                    newPagesArray[i] = newPagesArray[i].Remove(tempPos,
                                                newPagesArray[i].LastIndexOf(Constants.endSpanMarker)
                                                + Constants.endSpanMarker.Length
                                                + Constants.brMarker.Length - tempPos)
                                           .Insert(tempPos, arrows);
                else
                    newPagesArray[i] = newPagesArray[i]
                                        .Insert(newPagesArray[i]
                                        .IndexOf(Constants.endNavMarker), arrows);

            }

            return newPagesArray;
        }
    }
}