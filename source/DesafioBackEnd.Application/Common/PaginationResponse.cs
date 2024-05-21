namespace DesafioBackEnd.Application.Common;

public class PaginationResponse<TItems>
{
    public List<TItems> Items { get; set; } = new List<TItems>();
    public int CurrentPage { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }
}
