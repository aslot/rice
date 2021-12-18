namespace Data
{
    public interface IReplyLogic
    {
        int? GetAccountId(in Pair pair);
        void Start(in int? id, in Pair pair, in string t);
        void PublishNextMessageByTimer();
    }
}