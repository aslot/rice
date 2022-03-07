using System;
using System.Net;
using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        private static readonly object authenticationLocker = new object();

        internal static bool AccessGranted(in string token)
        {
            lock (authenticationLocker)
                if (Guid.TryParse(token, out Guid guid))
                    return Fast.LoginPasswordHashesValuesContainsLocked(guid);
                else return false;
        }

        internal static Tuple<bool, int> AccessGrantedEntendedNullable(in string token)
        {
            lock (authenticationLocker)
                if (Guid.TryParse(token, out Guid guid))
                    return Fast.CheckGuidAndGetOwnerAccountIdLocked(guid);
                else return new Tuple<bool, int>(false, Constants.Zero);
        }

        internal static Pair GetPair(in string token)
        {
            lock (authenticationLocker)
            {
                Pair result;

                if (Guid.TryParse(token, out Guid guid))
                    Fast.LoginPasswordHashesThroughIterationCheckLocked(out result, guid);
                else return new Pair();

                return result;
            }
        }

        private static string AcceptNullable(in IPAddress ip, in Pair pair)
        {
            lock (authenticationLocker)
            {
                Guid token = Guid.NewGuid();
                Fast.SetLoginPasswordHashesPairTokenLocked(pair, token);
                Fast.AccountIdentifierRemoteIpLogEnqueueLocked
                (new AccountIdentifierRemoteIp
                {
                    AccountIdentifier = pair.LoginHash,
                    RemoteIp = ip
                });

                return token.ToString();
            }
        }
    }
}
