using System.Collections.Generic;

namespace BusinessLayer.QueryObjects.Base.Results
{
    public class QueryResult<TDto>
    {
        public QueryResult(IList<TDto> items, int totalItemsCount, int pageSize = 10, int? requestedPageNumber = null)
        {
            TotalItemsCount = items.Count;
            RequestedPageNumber = requestedPageNumber;
            PageSize = pageSize;
            Items = items;
            TotalItemsCount = totalItemsCount;
        }

        /// <summary>
        /// Total number of items for the query
        /// </summary>
        public int TotalItemsCount { get; }

        /// <summary>
        /// Number of page (indexed from 1) which was requested
        /// </summary>
        public int? RequestedPageNumber { get; }

        /// <summary>
        /// Size of the page
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// The query results page
        /// </summary>
        public IList<TDto> Items { get; }

        public bool PagingEnabled => RequestedPageNumber != null;
    }
}