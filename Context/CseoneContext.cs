using Microsoft.EntityFrameworkCore;
 
namespace Cseone.Models
{
    public class CseoneContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public CseoneContext(DbContextOptions<CseoneContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Participant> Participants { get; set; }
        
    }
}