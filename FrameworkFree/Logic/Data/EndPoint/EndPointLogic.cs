using System.Linq;
using System.Collections.Generic;
using MarkupHandlers;
namespace Data
{
    internal sealed class EndPointLogic : IEndPointLogic
    {
        public readonly IStorage Storage;
        public readonly EndPointMarkupHandler EndPointMarkupHandler;
        public EndPointLogic(IStorage storage, EndPointMarkupHandler endPointMarkupHandler)
        {
            Storage = storage;
            EndPointMarkupHandler = endPointMarkupHandler;
        }
        private static readonly object locker = new object();

        public string GetEndPointPage(in int? Id)
        {
            if (Id == null)
                return Constants.SE;
            else
            {
                if (Id > Constants.Zero && Id <= Constants.EndPointPagesCount)
                    return Storage.Fast.GetEndPointPageLocked((int)Id - Constants.One);
                else
                    return Constants.SE;
            }
        }

        public void LoadEndPointPages()
        {
            Storage.Fast.InitializeEndPointPagesLocked(Constants.EndPointPagesCount);

            for (int i = Constants.Zero;
                    i < Constants.EndPointPagesCount; i++)
            {
                ProcessEndPointReader(Storage.Slow.GetEndpointIdNames(i + Constants.One), i);
            }
        }

        public void ProcessEndPointReader
            (in IEnumerable<IdName> idNames, in int number)
        {
            if (idNames.Any())
                Storage.Fast.SetEndPointPageLocked(number,
                    EndPointMarkupHandler.GenerateEndPointLinks(idNames));
        }

        public string GetEndPointPageLocked(in int index)
        {
            lock (locker)
                return Storage.Fast.GetEndPointPageLocked(index);
        }
    }
}
