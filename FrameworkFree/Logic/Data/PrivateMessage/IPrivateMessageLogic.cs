namespace Data
{
    public interface IPrivateMessageLogic
    {
        void LoadPersonalPages();
        string GetPersonalPage(in int? id, in int? page, in Pair pair);
        void AddNewCompanionsIfNotExists
            (in int ownerId, in int companionId, in string ownerNick,
                in string companionNick, in bool notEqualFlag);
    }
}