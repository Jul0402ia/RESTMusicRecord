namespace RESTMusicRecord.Models
{
    public class MusicRecordRepositoryDatabase : IMusicRecordRepository
    {
        private readonly MusicRecordDbContext _context;

        public MusicRecordRepositoryDatabase(MusicRecordDbContext context) 
        {
            _context = context;
        }

        public MusicRecord Add(MusicRecord musicRecord)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MusicRecord> GetAll(string title = null, string artist = null, int? duration = null, int? publicationYear = null, string sortBy = null)
        {
            throw new NotImplementedException();
        }

        public MusicRecord? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MusicRecord> GetMusicRecords()
        {
            return _context.MusicRecords;
        }

        public MusicRecord? Remove(int id)
        {
            throw new NotImplementedException();
        }

        public MusicRecord? Update(int id, MusicRecord updatedMusicRecord)
        {
            throw new NotImplementedException();
        }
    }

}
