using System;
using System.Net;
using System.Collections.Generic;
namespace Data
{//убрать полный проход по словарю
    internal sealed class AuthenticationLogic : IAuthenticationLogic
    {
        private static readonly object locker = new object();
        public readonly IStorage Storage;
        public AuthenticationLogic(IStorage storage)
        {
            Storage = storage;
        }
        public bool AccessGranted(in string token)
        {
            lock (locker)
            {
                Guid guid;
                bool result = false;

                try
                {
                    guid = new Guid(token);
                }
                catch (System.FormatException)
                {
                    guid = Guid.Empty;
                }

                if (guid == Guid.Empty)
                { }
                else
                    result = Storage.Fast.LoginPasswordHashesValuesContains(guid);

                return result;
            }
        }
        public Tuple<bool, int> AccessGrantedEntended(in string token)
        {
            lock (locker)
            {
                Guid guid;
                Tuple<bool, int> result = new Tuple<bool, int>(false, Constants.Zero);

                try
                {
                    guid = new Guid(token);
                }
                catch (System.FormatException)
                {
                    guid = Guid.Empty;
                }

                if (guid == Guid.Empty)
                { }
                else
                    result = Storage.Fast.CheckGuidAndGetOwnerAccountId(guid);

                return result;
            }
        }
        public void FlushAccountIdentifierRemoteIpLogByTimer()
        {
            Queue<AccountIdentifierRemoteIp> set = Storage.Fast.GetAccountIdentifierRemoteIps();

            foreach (var pair in set)
                Storage.Slow.PutAccountIdentifierIpHashInBaseIfNotExists(pair.AccountIdentifier, pair.RemoteIp);
            Storage.Fast.ClearAccountIdentifierRemoteIps();
        }
        public string Accept(in IPAddress ip, in Pair pair)
        {
            lock (locker)
            {
                Guid token = Guid.NewGuid();
                Storage.Fast.SetLoginPasswordHashesPairToken(pair, token);
                Storage.Fast.AccountIdentifierRemoteIpLogEnqueue
                (new AccountIdentifierRemoteIp
                {
                    AccountIdentifier = pair.LoginHash,
                    RemoteIp = ip
                });

                return token.ToString();
            }
        }

        public Pair GetPair(in string token)
        {
            lock (locker)
            {
                Guid guid;

                try
                {
                    guid = new Guid(token);
                }
                catch (System.FormatException)
                {
                    guid = Guid.Empty;
                }
                Pair result = new Pair();

                if (guid == Guid.Empty)
                { }
                else
                    Storage.Fast.LoginPasswordHashesThroughIterationCheck(ref result, guid);

                return result;
            }
        }
    }
}
