using System.Collections.Generic;
using MarkupHandlers;
using System.Linq;

namespace Data
{
    internal sealed class PrivateDialogLogic : IPrivateDialogLogic
    {
        private readonly IStorage Storage;
        private readonly IReplyLogic ReplyLogic;
        private readonly PrivateDialogMarkupHandler PrivateDialogMarkupHandler;
        private static readonly object locker = new object();
        public PrivateDialogLogic(IStorage storage,
        IReplyLogic replyLogic,
        PrivateDialogMarkupHandler privateDialogMarkupHandler)
        {
            Storage = storage;
            ReplyLogic = replyLogic;
            PrivateDialogMarkupHandler = privateDialogMarkupHandler;
        }
        public void AddDialog(in int Num)
        {
            int count;
            int number = Num;
            int id = number + Constants.One;

            count = Storage.Slow.CountPrivateMessages(id);

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.DialogsOnPage;

            if (count - pagesCount * Constants.DialogsOnPage > Constants.Zero)
                pagesCount++;
            Storage.Fast.SetDialogPagesArrayLocked
                            (number, new string[pagesCount]);
            Storage.Fast.SetDialogPagesPageDepthLocked(number, pagesCount);

            for (int i = Constants.Zero; i < pagesCount; i++)
                Storage.Fast
                    .SetDialogPagesPageLocked(number, i, Constants.NewDialog);

            ProcessDialogReader(Storage.Slow.GetIdNicksByAccountId(id),
                number, pagesCount);
        }
        public void ProcessDialogReader
            (in IEnumerable<IdNick> idNicks, in int number, in int pagesCount)
        {
            if (idNicks.Any())
            {
                /*string endpointHidden = Constants.SE;
                */
                int pageNumber = Constants.Zero;
                var i = Constants.Zero;

                foreach (var idNick in idNicks)
                {
                    if (i == Constants.Zero)
                        Storage.Fast.AddToDialogPagesPageLocked
                            (number, pageNumber, string.Concat(Constants.navMarker,
                                                Constants.brMarker));
                    Storage.Fast.AddToDialogPagesPageLocked
                        (number, pageNumber,
                            PrivateDialogMarkupHandler.GetPersonalLinkText(idNick.Id, idNick.Nick));
                    i++;

                    if (i == Constants.DialogsOnPage)
                    {
                        Storage.Fast.AddToDialogPagesPageLocked
                            (number, pageNumber,
                            string.Concat(PrivateDialogMarkupHandler.GetArrows(pageNumber, pagesCount),
                            Constants.endNavMarker));
                        //+ endpointHidden);

                        i = Constants.Zero;
                        pageNumber++;
                    }
                    else
                        Storage.Fast.AddToDialogPagesPageLocked
                            (number, pageNumber, Constants.brMarker);
                }

                RemoveBrOfIncompletePages(number);

                if ((i < Constants.DialogsOnPage) && (i > Constants.Zero))
                {
                    if (pageNumber > Constants.Zero)
                    {
                        Storage.Fast.AddToDialogPagesPageLocked
                            (number, pageNumber,
                            PrivateDialogMarkupHandler.GetArrows(pageNumber, pagesCount));
                    }
                    Storage.Fast.AddToDialogPagesPageLocked
                        (number, pageNumber, Constants.endNavMarker);
                    //+ endpointHidden);
                }
            }
        }
        public void RemoveBrOfIncompletePages(in int number)
        {
            string temp = Storage.Fast.GetDialogPagesPageLocked(number,
                   Storage.Fast
                    .GetDialogPagesArrayLocked(number).Length - Constants.One);
            int pos = temp.LastIndexOf(Constants.brMarker);
            temp = temp.Remove(pos, Constants.brMarker.Length);
            Storage.Fast.SetDialogPagesPageLocked(number,
                Storage.Fast
                    .GetDialogPagesArrayLocked(number).Length - Constants.One, temp);
        }

        public string GetDialogPage
                            (in int? page, in Pair pair)
        {
            if (page == null)
                return Constants.SE;
            else
            {
                int? accountId = ReplyLogic.GetAccountId(pair);

                if (accountId.HasValue)
                {
                    int index = accountId.Value - Constants.One;

                    if (page > Constants.Zero
                        && page
                        <= Storage.Fast.GetDialogPagesPageDepthLocked(index))
                    {
                        return Storage.Fast
                          .GetDialogPagesPageLocked(index
                             , (int)page - Constants.One);
                    }
                    else
                        return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }

        public void LoadDialogPages()
        {
            Storage.Fast.SetDialogPagesLengthLocked(Storage.Slow.GetAccountsCount());
            Storage.Fast.InitializeDialogPagesPageDepthLocked(
                new int[Storage.Fast.GetDialogPagesLengthLocked()]);
            Storage.Fast.InitializeDialogPagesLocked
                (new string[Storage.Fast.GetDialogPagesLengthLocked()][]);
            int len = Storage.Fast.GetDialogPagesLengthLocked();

            for (int i = Constants.Zero;
                i < len; i++)
                AddDialog(i);
        }
    }
}
