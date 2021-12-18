using System.Collections.Generic;
using MarkupHandlers;
using System.Linq;
namespace Data
{
    internal sealed class SectionLogic : ISectionLogic
    {
        public readonly IStorage Storage;
        private readonly SectionMarkupHandler SectionMarkupHandler;
        private readonly NewTopicMarkupHandler NewTopicMarkupHandler;
        public SectionLogic(IStorage storage,
        SectionMarkupHandler sectionMarkupHandler,
        NewTopicMarkupHandler newTopicMarkupHandler)
        {
            Storage = storage;
            SectionMarkupHandler = sectionMarkupHandler;
            NewTopicMarkupHandler = newTopicMarkupHandler;
        }
        public void AddSection(in int number)
        {
            int id = number + Constants.One;
            int count = Storage.Slow.CountThreadsById(id);
            string newTopicText = count == 0 ? NewTopicMarkupHandler.GetLastPage(id)
            : Constants.buttonTxt;

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.threadsOnPage;

            if (count - pagesCount * Constants.threadsOnPage > Constants.Zero)
                pagesCount++;
            Storage.Fast.SetSectionPagesArrayLocked
                            (number, new string[pagesCount]);
            Storage.Fast.
                SetSectionPagesPageDepthLocked(number, pagesCount);

            for (int i = Constants.Zero; i < pagesCount; i++)
                Storage.Fast.SetSectionPagesPageLocked(number, i, newTopicText);

            ProcessSectionReader(Storage.Slow.GetThreadIdNamesById(id),
                number, pagesCount);
        }
        public void RemoveBrOfIncompletePages(in int number)
        {
            string temp = Storage.Fast.GetSectionPagesPageLocked(number,
                   Storage.Fast.GetSectionPagesArrayLocked(number).Length - Constants.One);
            int pos = temp.LastIndexOf(Constants.brMarker);
            temp = temp.Remove(pos, Constants.brMarker.Length);
            Storage.Fast.SetSectionPagesPageLocked(number,
                Storage.Fast.GetSectionPagesArrayLocked(number).Length - Constants.One, temp);
        }

        public void ProcessSectionReader
            (in IEnumerable<IdName> idNames, in int number, in int pagesCount)
        {
            if (idNames.Any())
            {
                string endpointHidden = Constants.SE;
                int pageNumber = Constants.Zero;
                int i = Constants.Zero;

                foreach (var idName in idNames)
                {
                    if (i == Constants.Zero)
                        Storage.Fast.AddToSectionPagesPageLocked
                            (number, pageNumber, SectionMarkupHandler.GetNav());
                    int id_ = idName.Id;

                    endpointHidden = SectionMarkupHandler
                        .GetEndpointHidden(number + Constants.One);

                    Storage.Fast.AddToSectionPagesPageLocked
                        (number, pageNumber, SectionMarkupHandler
                            .GetThreadLinkText(id_, idName.Name));
                    i++;

                    if (i == Constants.threadsOnPage)
                    {
                        Storage.Fast.AddToSectionPagesPageLocked
                            (number, pageNumber,
                            SectionMarkupHandler.SetNavigationWithEndpoint(pageNumber,
                                pagesCount, number, endpointHidden));

                        i = Constants.Zero;
                        pageNumber++;
                    }
                    else
                        Storage.Fast.AddToSectionPagesPageLocked
                            (number, pageNumber, Constants.brMarker);
                }

                RemoveBrOfIncompletePages(number);

                if ((i < Constants.threadsOnPage) && (i > Constants.Zero))
                {
                    if (pageNumber > Constants.Zero)
                    {
                        Storage.Fast.AddToSectionPagesPageLocked
                            (number, pageNumber,
                            SectionMarkupHandler
                                .SetNavigation(pageNumber, pagesCount, number));
                    }
                    Storage.Fast.AddToSectionPagesPageLocked
                        (number, pageNumber, SectionMarkupHandler.GetNavWithEndpoint(endpointHidden));
                }
            }
        }

        public string GetSectionPage(in int? Id, in int? page)
        {
            if (Id == null || page == null)
                return Constants.SE;
            else
            {
                if (Id > Constants.Zero && Id <= Storage.Fast.GetSectionPagesLengthLocked())
                {
                    int index = (int)Id - Constants.One;

                    if (page > Constants.Zero
                        && page <= Storage.Fast.GetSectionPagesPageDepthLocked(index))
                        return Storage.Fast.GetSectionPagesPageLocked
                            (index, (int)page - Constants.One);
                    else return Constants.SE;
                }
                else
                    return Constants.SE;
            }
        }

        public void LoadSectionPages()
        {
            Storage.Fast.SetSectionPagesLengthLocked(Constants.len);
            Storage.Fast.InitializeSectionPagesPageDepthLocked(
                new int[Storage.Fast.GetSectionPagesLengthLocked()]);
            Storage.Fast.InitializeSectionPagesLocked
                (new string[Storage.Fast.GetSectionPagesLengthLocked()][]);

            for (int i = Constants.Zero;
                i < Storage.Fast.GetSectionPagesLengthLocked(); i++)
            {
                AddSection(i);
            }
        }
    }
}
