using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static void StartProfileVoid(in int accountId, in string aboutMe, in bool[] flags, in byte[] file)
        {
            if (Fast.GetProfilesOnPreSaveLineCountLocked() < Constants.MaxFirstLineLength)
            {
                Fast.PreSaveProfilesLineEnqueueLocked(
                    new PreProfile
                    {
                        AccountId = accountId,
                        AboutMe = aboutMe,
                        Flags = flags,
                        File = file
                    }
                );
            }
        }
    }
}