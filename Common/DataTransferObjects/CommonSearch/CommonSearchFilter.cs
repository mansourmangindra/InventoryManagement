
using Common.DataTransferObjects.Filter.CollectionPaging;

namespace Common.DataTransferObjects.CommonSearch
{
    public class CommonSearchFilter : PagingParameter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? SecondStartDate { get; set; }
        public DateTime? SecondEndDate { get; set; }
        public string SearchKeyword { get; set; }
        public string SecondSearchKeyword { get; set; }
        public string ThirdSearchKeyword { get; set; }
        public string FourthSearchKeyword { get; set; }
        public string FifthSearchKeyword { get; set; }
        public string SixthSearchKeyword { get; set; }
        public bool Export { get; set; }
    }
}
