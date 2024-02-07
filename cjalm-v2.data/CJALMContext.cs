using Microsoft.EntityFrameworkCore;
using cjalm_v2.domain;

namespace cjalm_v2.data
{
    public class CJALMContext : DbContext
    {
        public DbSet<Entry> Entries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = CJALM-v2.Database"
                );
        }
    }
}
