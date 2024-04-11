namespace INTEX3.Models.ViewModels
{
    public class PaginationInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrntPage { get; set; }
        public int TotalPages => (int) (Math.Ceiling((decimal) TotalItems / ItemsPerPage));
    }
}
