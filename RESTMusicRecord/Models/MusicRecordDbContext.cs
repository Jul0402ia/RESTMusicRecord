using Microsoft.EntityFrameworkCore;
namespace RESTMusicRecord.Models
{
    public class MusicRecordDbContext : DbContext // klasse der arver fra dbcontext og styrer forbindelsen til databse
    {

        // constructor modtager database indstillinger fx connection string 
        public MusicRecordDbContext(DbContextOptions<MusicRecordDbContext> options) : base(options)
        {
            //sender indstillingerne videre til dbcontex - ses lidt som det der håndtere db
        }

        //repræsenterer tabellen musicrecords i databsen 
        public DbSet<MusicRecord> MusicRecords { get; set; }
    }
}

