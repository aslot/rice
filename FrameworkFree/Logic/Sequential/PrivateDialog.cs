using Own.Storage;
using Own.Permanent;
using Own.MarkupHandlers;
using Own.Types;
using System.Collections.Generic;
using System.Linq;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void LoadDialogPagesVoid()
        {
            Fast.SetDialogPagesLengthLocked(Slow.GetAccountsCount());
            Fast.InitializeDialogPagesPageDepthLocked(
                new int[Fast.GetDialogPagesLengthLocked()]);
            Fast.InitializeDialogPagesLocked
                (new string[Fast.GetDialogPagesLengthLocked()][]);
            int len = Fast.GetDialogPagesLengthLocked();

            for (int i = Constants.Zero; i < len; i++)
                AddDialogVoid(i);
        }

        internal static void AddDialogVoid(in int Num)
        {
            int count;
            int number = Num;
            int id = number + Constants.One;

            count = Slow.CountPrivateMessages(id);

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.DialogsOnPage;

            if (count - pagesCount * Constants.DialogsOnPage > Constants.Zero)
                pagesCount++;
            Fast.SetDialogPagesArrayLocked
                            (number, new string[pagesCount]);
            Fast.SetDialogPagesPageDepthLocked(number, pagesCount);

            for (int i = Constants.Zero; i < pagesCount; i++)
                Own.Storage.Fast
                    .SetDialogPagesPageLocked(number, i, Constants.NewDialog);

            ProcessDialogReaderVoid(Slow.GetIdNicksByAccountIdNullable(id),
                number, pagesCount);
        }
        internal static void ProcessDialogReaderVoid
            (in IEnumerable<IdNick> idNicks, in int number, in int pagesCount)
        {
            if (idNicks.Any())
            {
                int pageNumber = Constants.Zero;
                var i = Constants.Zero;

                foreach (var idNick in idNicks)
                {
                    if (i == Constants.Zero)
                        Fast.AddToDialogPagesPageLocked
                            (number, pageNumber, string.Concat(Constants.navMarker,
                                                Constants.brMarker));
                    Fast.AddToDialogPagesPageLocked
                        (number, pageNumber,
                            Marker.GetPersonalLinkText(idNick.Id, idNick.Nick));
                    i++;

                    if (i == Constants.DialogsOnPage)
                    {
                        Fast.AddToDialogPagesPageLocked
                            (number, pageNumber,
                            string.Concat(Marker.GetArrows(pageNumber, pagesCount),
                            Constants.endNavMarker));

                        i = Constants.Zero;
                        pageNumber++;
                    }
                    else
                        Fast.AddToDialogPagesPageLocked
                            (number, pageNumber, Constants.brMarker);
                }

                RemoveBrOfIncompletePrivateDialogPagesVoid(number);

                if ((i < Constants.DialogsOnPage) && (i > Constants.Zero))
                {
                    if (pageNumber > Constants.Zero)
                    {
                        Fast.AddToDialogPagesPageLocked
                            (number, pageNumber,
                            Marker.GetArrows(pageNumber, pagesCount));
                    }
                    Fast.AddToDialogPagesPageLocked
                        (number, pageNumber, Constants.endNavMarker);
                }
            }
        }
        internal static void RemoveBrOfIncompletePrivateDialogPagesVoid(in int number)
        {
            string temp = Fast.GetDialogPagesPageLocked(number,
                   Own.Storage.Fast
                    .GetDialogPagesArrayLockedLocked(number).Length - Constants.One);
            int pos = temp.LastIndexOf(Constants.brMarker);
            temp = temp.Remove(pos, Constants.brMarker.Length);
            Fast.SetDialogPagesPageLocked(number,
                Own.Storage.Fast
                    .GetDialogPagesArrayLockedLocked(number).Length - Constants.One, temp);
        }
    }
}