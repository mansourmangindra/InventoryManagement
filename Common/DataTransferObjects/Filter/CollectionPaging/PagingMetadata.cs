using Common.DataTransferObjects.Filter.CollectionPaging.Interfaces;
using Newtonsoft.Json;

namespace Common.DataTransferObjects.Filter.CollectionPaging
{
    public class PagingMetadata : IPagingMetadata
    {
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public bool HasPrevious { get; private set; }

        public bool HasNext { get; private set; }

        [JsonConstructor]
        public PagingMetadata(int currentPage, int totalPages, int pageSize, int totalCount, string pageClickEvent)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            PageSize = pageSize;
            TotalCount = totalCount;

            HasPrevious = currentPage > 1;
            HasNext = currentPage < totalPages;
        }

        public PagingMetadata(int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = totalCount;
            CurrentPage = pageNumber;
            PageSize = pageSize;
        }
    }
}