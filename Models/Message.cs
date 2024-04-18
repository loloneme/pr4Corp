namespace pr4.Models;

public class Message
{
    public int ID { get; set; }
    public required string From { get; set; }
    public string To { get; set; }
    public string? Theme { get; set; }
    public string? Text { get; set; }
    public DateTimeOffset SentDate { get; set; }
    public string Status { get; set; }

}