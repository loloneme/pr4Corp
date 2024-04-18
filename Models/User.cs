namespace pr4.Models;

public class User
{
    public int ID { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronymic { get; set; }
}