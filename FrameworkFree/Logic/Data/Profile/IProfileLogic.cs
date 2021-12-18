namespace Data
{
    public interface IProfileLogic
    {
        void Start(in int accountId, in string aboutMe, in bool[] flags, in byte[] file);
        void HandleAndSaveProfilesByTimer();
        void LoadProfiles();
    }
}