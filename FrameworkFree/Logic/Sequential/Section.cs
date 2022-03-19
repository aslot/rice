using Own.Permanent;
using Own.Storage;
using Own.MarkupHandlers;
using System.Collections.Generic;
using Own.Types;
using System.Linq;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        private static void RemoveBrOfIncompleteSectionPagesVoid(in int number)
        {
            string temp = Fast.GetSectionPagesPageLocked(number,
                   Fast.GetSectionPagesArrayLocked(number).Length - Constants.One);
            int pos = temp.LastIndexOf(Constants.brMarker);
            temp = temp.Remove(pos, Constants.brMarker.Length);
            Fast.SetSectionPagesPageLocked(number,
                Fast.GetSectionPagesArrayLocked(number).Length - Constants.One, temp);
        }
        private static void AddSectionVoid(in int number)
        {
            int id = number + Constants.One;
            int count = Slow.CountThreadsById(id);
            string newTopicText = count == 0 ? Marker.GetLastPage(id)
            : Constants.buttonTxt;

            if (count == Constants.Zero)
                count++;
            int pagesCount = count / Constants.threadsOnPage;

            if (count - pagesCount * Constants.threadsOnPage > Constants.Zero)
                pagesCount++;
            Fast.SetSectionPagesArrayLocked
                            (number, new string[pagesCount]);
            Fast.
                SetSectionPagesPageDepthLocked(number, pagesCount);

            for (int i = Constants.Zero; i < pagesCount; i++)
                Fast.SetSectionPagesPageLocked(number, i, newTopicText);

            ProcessSectionReaderVoid(Slow.GetThreadIdNamesByIdNullable(id),
                number, pagesCount);
        }
        internal static void LoadSectionPagesVoid()
        {
            Fast.SetSectionPagesLengthLocked(Constants.len);
            Fast.InitializeSectionPagesPageDepthLocked(
                new int[Fast.GetSectionPagesLengthLocked()]);
            Fast.InitializeSectionPagesLocked
                (new string[Fast.GetSectionPagesLengthLocked()][]);

            for (int i = Constants.Zero;
                i < Fast.GetSectionPagesLengthLocked(); i++)
            {
                AddSectionVoid(i);
            }
        }
        private static void ProcessSectionReaderVoid
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
                        Fast.AddToSectionPagesPageLocked
                            (number, pageNumber, Marker.GetNav());
                    int id_ = idName.Id;

                    endpointHidden = Marker
                        .GetEndpointHidden(number + Constants.One);

                    Fast.AddToSectionPagesPageLocked
                        (number, pageNumber, Marker
                            .GetThreadLinkText(id_, idName.Name));
                    i++;

                    if (i == Constants.threadsOnPage)
                    {
                        Fast.AddToSectionPagesPageLocked
                            (number, pageNumber,
                            Marker.SetNavigationWithEndpoint(pageNumber,
                                pagesCount, number, endpointHidden));

                        i = Constants.Zero;
                        pageNumber++;
                    }
                    else
                        Fast.AddToSectionPagesPageLocked
                            (number, pageNumber, Constants.brMarker);
                }

                RemoveBrOfIncompleteSectionPagesVoid(number);

                if ((i < Constants.threadsOnPage) && (i > Constants.Zero))
                {
                    if (pageNumber > Constants.Zero)
                    {
                        Fast.AddToSectionPagesPageLocked
                            (number, pageNumber,
                            Marker
                                .SetSectionNavigation(pageNumber, pagesCount, number));
                    }
                    Fast.AddToSectionPagesPageLocked
                        (number, pageNumber, Marker.GetNavWithEndpoint(endpointHidden));
                }
            }
        }
    }
}