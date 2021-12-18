using System;
using System.Timers;
namespace Data
{
    public interface INewTopicLogic
    {
        bool CheckText(in string message);
        int CountStringOccurrences
            (in string text, in string pattern);
        void Start(in string threadName, in int? endpointId, in Pair pair, in string message);
        void StartNextTopicByTimer();
    }
}