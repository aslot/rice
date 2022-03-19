using Own.Permanent;
using Own.Types;
using Own.Storage;
namespace Own.InRace
{
    internal static partial class Unstable
    {
        internal static void StartNewTopicVoid
                (in string threadName, in int? endpointId, in Pair pair, in string message)
        {
            if (Fast.GetTopicsToStartCountLocked() < Constants.MaxFirstLineLength)
            {
                TopicData topicData = new TopicData
                {
                    threadName = threadName,
                    endpointId = endpointId,
                    pair = pair,
                    message = message
                };
                Fast.TopicsToStartEnqueueLocked(topicData);
            }
        }
    }
}