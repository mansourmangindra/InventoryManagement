using Common.DataTransferObjects.Filter.CollectionPaging.Interfaces;

namespace Common.DataTransferObjects.Filter.CollectionPaging
{
    public class PagedList<TEntity> : IPagingMetadata
    {
        public PagedList()
        {

        }

        public PagedList(List<TEntity> items, PagingMetadata pagingMetadata)
        {
            CurrentPage = pagingMetadata.CurrentPage;
            TotalCount = pagingMetadata.TotalCount;
            PageSize = pagingMetadata.PageSize == 0 ? TotalCount : pagingMetadata.PageSize;
            TotalPages = pagingMetadata.PageSize == 0 ? 1 : (int)Math.Ceiling(pagingMetadata.TotalCount / (double)pagingMetadata.PageSize);
            HasPrevious = CurrentPage > 1;
            HasNext = CurrentPage < TotalPages;
            Items = items;
        }

        public List<TEntity> Items { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public bool HasPrevious { get; set; }

        public bool HasNext { get; set; }

        public static PagedList<TEntity> ToPagedList(IEnumerable<TEntity> source, int pageNumber, int pageSize)
        {
            int count;
            List<TEntity> items;

            if (pageSize != 0)
            {
                count = source.Count();
                items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                items = source.ToList();
                count = items.Count;
            }

            return new PagedList<TEntity>(items, new PagingMetadata(count, pageNumber, pageSize));
        }
    }
}