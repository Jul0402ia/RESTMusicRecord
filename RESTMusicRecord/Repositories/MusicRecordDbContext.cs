using Microsoft.EntityFrameworkCore;
using RESTMusicRecord.Models;

namespace RESTMusicRecord.Repositories
{
    public class MusicRecordDbContext : DbContext
    {
        public MusicRecordDbContext(DbContextOptions<MusicRecordDbContext> options) : base(options)
        {

        }
        public DbSet<MusicRecord> MusicRecords { get; set; }
    }

}
