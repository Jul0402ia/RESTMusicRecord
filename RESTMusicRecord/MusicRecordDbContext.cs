using Microsoft.EntityFrameworkCore;

namespace RESTMusicRecord
{
    public class MusicRecordDbContext : DbContext
    {
        public MusicRecordDbContext(DbContextOptions<MusicRecordDbContext> options) : base(options)
        {

        }
        public DbSet<MusicRecord> MusicRecords { get; set; }
    }

}
