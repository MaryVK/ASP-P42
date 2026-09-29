namespace ASP_P42.Models.Rest
{
    public class RestMetaPagination
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int TotalItems {  get; set; } = 1;
    }
}
