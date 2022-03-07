using System.Collections.Generic;
using System.Linq;
using Own.Permanent;
using Own.Types;
using Inclusions;
using Own.Storage;
using Own.MarkupHandlers;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static int? GetAccIdAndStore(Pair pair)
        {
            int? accountId = Slow.GetAccountIdFromBaseNullable
                            (pair.LoginHash, pair.PasswordHash);

            if (accountId.HasValue)
            {
                Fast.LoginPasswordAccIdHashesAddLocked(pair, accountId.Value);
                byte result;
                Fast.LoginPasswordHashesDeltaRemoveLocked(pair, out result);
                Fast.AddOrUpdateOwnProfilePageLocked(accountId.Value,
                            Marker
                                .GetOwnProfilePrimaryUnfilledMarkup(accountId.Value));
            }

            return accountId;
        }

        internal static bool CheckNickHashIfExists(in string nick)
        {
            bool result = false;

            if (Fast.NicksHashesKeysContainsLocked(XXHash32.Hash(nick)))
                result = true;
            else
                result = false;

            return result;
        }

        internal static void CheckAccountIdByTimerVoid()
        {
            Slow.CheckAccountIdVoid(Unstable.GetAccIdAndStore);
        }

        internal static void LoadAccountsVoid()
        {
            ProcessAccountsReader(Slow.GetPairsNullable());
        }

        internal static void LoadNicksVoid()
        {
            ProcessNicksReader(Slow.GetNicksNullable());
        }

        private static void ProcessAccountsReader(in ICollection<Pair> pairs)
        {
            Fast.InitializeLoginPasswordAccIdHashesLocked();
            Fast.InitializeLoginPasswordHashesLocked();
            Fast.InitializeLoginPasswordHashesDeltaLocked();

            if (pairs.Any())
            {
                foreach (Pair pair in pairs)
                {
                    if (pair.LoginHash != Constants.Zero && pair.PasswordHash != Constants.Zero)
                    {
                        if (!Fast.LoginPasswordAccIdHashesContainsKeyLocked(pair))
                        {
                            int? accountId = Slow.GetAccountIdFromBaseNullable
                                (pair.LoginHash, pair.PasswordHash);

                            if (accountId.HasValue)
                                Fast.LoginPasswordAccIdHashesAddLocked(pair, accountId.Value);
                        }

                        if (!Fast.LoginPasswordHashesContainsKeyLocked(pair))
                        {
                            Fast.LoginPasswordHashesAdd(pair, null);
                        }
                    }
                }
            }
        }

        private static void ProcessNicksReader(in IEnumerable<string> nicks)
        {
            Fast.InitializeNicksHashesLocked();

            if (nicks.Any())
                foreach (var nick in nicks)
                    Fast.NicksHashesAddLocked(XXHash32.Hash(nick), Constants.Zero);
        }
    }
}