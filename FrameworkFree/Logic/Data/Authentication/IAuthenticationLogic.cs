using System.Net;
using System;
namespace Data
{
    public interface IAuthenticationLogic
    {
        bool AccessGranted(in string token);
        Pair GetPair(in string token);
        string Accept(in IPAddress ip, in Pair pair);
        void FlushAccountIdentifierRemoteIpLogByTimer();
        Tuple<bool, int> AccessGrantedEntended(in string token);
    }
}