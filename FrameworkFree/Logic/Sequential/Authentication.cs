using System.Collections.Generic;
using Own.Types;
using Own.Storage;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void FlushAccountIdentifierRemoteIpLogByTimerVoid()
        {
            Queue<AccountIdentifierRemoteIp> set = Fast.GetAccountIdentifierRemoteIpsLocked();

            foreach (var pair in set)
                Slow.PutAccountIdentifierIpHashInBaseIfNotExistsVoid(pair.AccountIdentifier, pair.RemoteIp);
            Fast.ClearAccountIdentifierRemoteIpsLocked();
        }
    }
}