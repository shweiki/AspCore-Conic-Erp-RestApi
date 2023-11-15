namespace RestApi.Models;

public class SearchOptions
{
    //public int Draw { get; set; }
    //public int Start { get; set; }
    //public int Length { get; set; }
    //public string SortBy { get; set; } = "";
    //public bool IsDesc { get; set; }
    //public string CreatedBy { get; set; } = "";

    public int Limit { get; set; }
    public string Sort { get; set; }
    public int Page { get; set; }
    public int? Status { get; set; }
    public string Any { get; set; }
}
