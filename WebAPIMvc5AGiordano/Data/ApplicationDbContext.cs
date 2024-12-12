using Microsoft.EntityFrameworkCore;
using WebAPIMvc5AGiordano.Models;

namespace WebAPIMvc5AGiordano.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
