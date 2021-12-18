namespace Data
{
    public interface IEndPointLogic
    {
        string GetEndPointPage(in int? Id);
        void LoadEndPointPages();
    }
}