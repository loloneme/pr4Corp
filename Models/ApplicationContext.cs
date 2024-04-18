using Microsoft.EntityFrameworkCore;
namespace pr4.Models;


public class ApplicationContext : DbContext{
    public DbSet<User> User { get; set; }
    public DbSet<Message> Message { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }
}
