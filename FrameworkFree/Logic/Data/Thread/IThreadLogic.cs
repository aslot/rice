namespace Data
{
    public interface IThreadLogic
    {
        string GetNick(in int AccountId);
        void LoadThreadPages();
        string GetThreadPage(in int? Id, in int? page);
    }
}