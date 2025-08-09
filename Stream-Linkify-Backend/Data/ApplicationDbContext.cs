using Microsoft.EntityFrameworkCore;
using Stream_Linkify_Backend.Models;

namespace Stream_Linkify_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base (options)
        {
            
        }
        public DbSet<TrackModel> Tracks { get; set; }
    }
}
