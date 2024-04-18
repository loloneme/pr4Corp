using Microsoft.Identity.Client;
using pr4.Models;

public class FiltersModel
{
    // public string TypeOfDispatchFilter {get; set; }
    public string SenderFilter { get; set; }
    public string StatusFilter { get; set; }
    public DateTimeOffset StartDate {get; set; }
    public DateTimeOffset EndDate {get; set; }
}