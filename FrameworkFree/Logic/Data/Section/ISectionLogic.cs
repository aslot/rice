namespace Data
{
    public interface ISectionLogic
    {
        string GetSectionPage(in int? Id, in int? page);
        void LoadSectionPages();
    }
}