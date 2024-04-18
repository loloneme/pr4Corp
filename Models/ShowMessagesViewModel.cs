using Microsoft.Identity.Client;
using pr4.Models;

public class ShowMessagesViewModel
{
    public IEnumerable<Message> Messages { get; set; }
    public FiltersModel Filters { get; set; }

    public string Login {get; set; }
}