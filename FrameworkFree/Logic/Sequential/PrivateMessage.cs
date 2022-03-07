using Own.Permanent;
using Own.Types;
using Own.Storage;
using System.Collections.Generic;
using Own.MarkupHandlers;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void LoadPersonalPagesVoid()
        {
            int ownersCount = Slow.GetAccountsCount();

            for (int i = Constants.Zero; i < ownersCount; i++)
            {
                SetMessagesDictionaryVoid(i + Constants.One);
            }
        }
        internal static PrivateMessages
                    ProcessCompanionPrivateMessagesReader
                        (in IEnumerable<IdText> idTexts, in int companionId,
                        in int accountId, in int pagesCount)
        {
            string companionNick = Slow.GetNickByAccountIdNullable(companionId);
            string accountNick = Slow.GetNickByAccountIdNullable(accountId);

            return Marker.ProcessCompanionReaderMarkup(
                companionNick, companionId, idTexts, pagesCount, accountNick);
        }
        internal static PrivateMessages GetMessages(in int companionId, in int accountId)
        {
            int count = Slow.CountPrivateMessagesByIds(companionId, accountId);

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.five;

            if (count - pagesCount * Constants.five > Constants.Zero)
                pagesCount++;

            return ProcessCompanionPrivateMessagesReader
                            (Slow.GetPrivateMessagesByIdsNullable(companionId, accountId),
                             companionId, accountId, pagesCount);
        }

        internal static void SetMessagesDictionaryVoid(in int accountId)
        {
            CompanionId[] companions
                = Slow.GetCompanionsByAccountIdNullable(accountId);

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
            Fast.PersonalPagesAddLocked(new OwnerId { Id = accountId }, temp1);
            Fast.PersonalPagesDepthsAddLocked(new OwnerId { Id = accountId }, temp2);
        }
        internal static void AddNewCompanionsIfNotExistsVoid
                    (in int ownerId, in int companionId, in string ownerNick,
            in string companionNick, in bool notEqualFlag)
        {
            OwnerId ownerIdObj = new OwnerId { Id = ownerId };
            CompanionId companionIdObj = new CompanionId { Id = companionId };
            SetNewCompanionDepthVoid(ownerIdObj, companionIdObj);
            SetNewCompanionPageVoid(ownerIdObj, companionIdObj, companionNick);

            if (notEqualFlag)
            {
                ownerIdObj.Id = companionId;
                companionIdObj.Id = ownerId;
                SetNewCompanionDepthVoid(ownerIdObj, companionIdObj);
                SetNewCompanionPageVoid(ownerIdObj, companionIdObj, ownerNick);
            }
        }
        internal static void SetNewCompanionPageVoid
            (in OwnerId ownerId, in CompanionId companionId, in string companionNick)
        {
            bool flag = Fast.PersonalPagesKeysContainsLocked(ownerId);

            if (!Fast.PersonalPagesContainsKeyLocked(ownerId, companionId, flag))
                Fast.PersonalPagesAddLocked(ownerId, companionId, new string[Constants.One]
                {
                    Marker.SetNewCompanionPageMarkup(companionId.Id,
                    companionNick)
                }, flag);
        }
    }
}