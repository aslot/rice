using System.Collections.Generic;
using MarkupHandlers;
namespace Data
{
    internal sealed class PrivateMessageLogic : IPrivateMessageLogic
    {
        private readonly IStorage Storage;
        private readonly IThreadLogic ThreadLogic;
        private readonly IReplyLogic ReplyLogic;
        private readonly PrivateMessageMarkupHandler PrivateMessageMarkupHandler;
        public PrivateMessageLogic(IStorage storage,
        IThreadLogic threadLogic,
        IReplyLogic replyLogic,
        PrivateMessageMarkupHandler privateMessageMarkupHandler)
        {
            Storage = storage;
            ThreadLogic = threadLogic;
            ReplyLogic = replyLogic;
            PrivateMessageMarkupHandler = privateMessageMarkupHandler;
        }
        public
            PrivateMessages
                GetMessages(in int companionId, in int accountId)
        {
            int count = Storage.Slow.CountPrivateMessagesByIds(companionId, accountId);

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.five;

            if (count - pagesCount * Constants.five > Constants.Zero)
                pagesCount++;

            return ProcessCompanionPrivateMessagesReader
                            (Storage.Slow.GetPrivateMessagesByIds(companionId, accountId),
                             companionId, accountId, pagesCount);
        }

        public PrivateMessages
                    ProcessCompanionPrivateMessagesReader
                        (in IEnumerable<IdText> idTexts, in int companionId,
                        in int accountId, in int pagesCount)
        {
            string companionNick = ThreadLogic.GetNick(companionId);
            string accountNick = ThreadLogic.GetNick(accountId);

            return PrivateMessageMarkupHandler.ProcessCompanionReaderMarkup(
                companionNick, companionId, idTexts, pagesCount, accountNick);
        }
        public string GetPersonalPage
                            (in int? id, in int? page, in Pair pair)
        {
            if (page == null || id == null)
                return Constants.SE;
            else
            {
                int? index = ReplyLogic.GetAccountId(pair);

                if (index.HasValue)
                {
                    int Id = (int)id;
                    int limit = Storage.Fast.GetPersonalPagesPageDepth(index.Value, Id);

                    if (page > Constants.Zero
                        && page
                        <= limit)
                    {
                        return Storage.Fast.GetMessage(index.Value, Id, (int)page - Constants.One);
                    }
                    else
                        return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }
        public void LoadPersonalPages()
        {
            int ownersCount = Storage.Slow.GetAccountsCount();

            for (int i = Constants.Zero; i < ownersCount; i++)
            {
                SetMessagesDictionary(i + Constants.One);
            }
        }
        public void AddNewCompanionsIfNotExists
                    (in int ownerId, in int companionId, in string ownerNick,
            in string companionNick, in bool notEqualFlag)
        {
            OwnerId ownerIdObj = new OwnerId { Id = ownerId };
            CompanionId companionIdObj = new CompanionId { Id = companionId };
            SetNewCompanionDepth(ownerIdObj, companionIdObj);
            SetNewCompanionPage(ownerIdObj, companionIdObj, companionNick);

            if (notEqualFlag)
            {
                ownerIdObj.Id = companionId;
                companionIdObj.Id = ownerId;
                SetNewCompanionDepth(ownerIdObj, companionIdObj);
                SetNewCompanionPage(ownerIdObj, companionIdObj, ownerNick);
            }
        }
        public void SetNewCompanionPage
            (in OwnerId ownerId, in CompanionId companionId, in string companionNick)
        {
            bool flag = Storage.Fast.PersonalPagesKeysContains(ownerId);

            if (!Storage.Fast.PersonalPagesContainsKey(ownerId, companionId, flag))
                Storage.Fast.PersonalPagesAdd(ownerId, companionId, new string[Constants.One]
                {
                    PrivateMessageMarkupHandler.SetNewCompanionPageMarkup(companionId.Id,
                    companionNick)
                }, flag);
        }
        public void SetNewCompanionDepth
            (in OwnerId ownerId, in CompanionId companionId)
        {
            bool flag = Storage.Fast.PersonalPagesDepthsKeysContains(ownerId);

            if (!Storage.Fast.PersonalPagesDepthsContainsKey(ownerId, companionId, flag))
                Storage.Fast.PersonalPagesDepthsAdd(ownerId, companionId, flag);
        }
        public void SetMessagesDictionary(in int accountId)
        {
            CompanionId[] companions
                = Storage.Slow.GetCompanionsByAccountId(accountId);

            Dictionary<CompanionId, PrivateMessages>
                    temp1 = new();
            Dictionary<CompanionId, int>
                    temp2 = new();

            for (int i = Constants.Zero; i < companions.Length; i++)
            {
                PrivateMessages pm = GetMessages
                                    (companions[i].Id, accountId);

                temp1.Add(companions[i], pm);
                temp2.Add(companions[i], pm.Messages.Length);
            }
            Storage.Fast.PersonalPagesAdd(new OwnerId { Id = accountId }, temp1);
            Storage.Fast.PersonalPagesDepthsAdd(new OwnerId { Id = accountId }, temp2);
        }
    }
}
