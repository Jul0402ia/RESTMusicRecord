
namespace RESTMusicRecord
{
    public class MusicRecordRepositoryDatabase : IREPOMusicRecords
    {
        private readonly MusicRecordDbContext _context;

        public MusicRecordRepositoryDatabase(MusicRecordDbContext context)
        {
            _context = context;
        }

        public MusicRecord Add(MusicRecord musicRecord)
        {
            if(musicRecord == null)
            {
                throw new ArgumentNullException(nameof(musicRecord));
            }
            _context.MusicRecords.Add(musicRecord);
            _context.SaveChanges();
            return musicRecord;
        }

        public IEnumerable<MusicRecord> GetAll(string? title = null, string? artist = null, int? duration = null, int? publicationYear = null, string? sortBy = null)
        {
            return _context.MusicRecords;
        }

        public MusicRecord? GetById(int id)
        {
            return _context.MusicRecords.Find(id);
        }

        public MusicRecord? Remove(int id)
        {
            var musicRecord = _context.MusicRecords.Find(id);
            if (musicRecord == null)
            {
                return null;
            }
            _context.MusicRecords.Remove(musicRecord);
            _context.SaveChanges();
            return musicRecord;
        }

        public MusicRecord? Update(int id, MusicRecord updatedMusicRecord)
        {
            var existingMusicRecord = GetById(id);
            if (existingMusicRecord != null)
            {
                existingMusicRecord.Title = updatedMusicRecord.Title;
                existingMusicRecord.Artist = updatedMusicRecord.Artist;
                existingMusicRecord.Duration = updatedMusicRecord.Duration;
                existingMusicRecord.PublicationYear = updatedMusicRecord.PublicationYear;
                _context.SaveChanges();
                return existingMusicRecord;
            }
            return null;
        }
    }
}
