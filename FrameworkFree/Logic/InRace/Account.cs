using Inclusions;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        private static Pair? CheckPairNullable(in string login, in string password)
        {
            var pair = new Pair
            {
                LoginHash = XXHash32.Hash(login),
                PasswordHash = XXHash32.Hash(password)
            };

            if (Fast.LoginPasswordHashesContainsKeyLocked(pair))
                return pair;
            else
                return null;
        }
    }
}
