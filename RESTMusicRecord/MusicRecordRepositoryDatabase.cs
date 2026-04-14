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
            // Tjekker om objektet er null
            if (musicRecord == null)
            {
                throw new ArgumentNullException(nameof(musicRecord));
            }

            // Tilføjer til databasen
            _context.MusicRecords.Add(musicRecord);
            _context.SaveChanges();

            return musicRecord;
        }

        public IEnumerable<MusicRecord> GetAll(string? title = null, string? artist = null, int? duration = null, int? publicationYear = null, string? sortBy = null)
        {
            // Henter alle records fra databasen
            List<MusicRecord> musicRecords = _context.MusicRecords.ToList();

            // Filtrerer på title
            if (title != null)
            {
                musicRecords = musicRecords.FindAll(m => m.Title != null && m.Title.StartsWith(title));
            }

            // Filtrerer på artist
            if (artist != null)
            {
                musicRecords = musicRecords.FindAll(m => m.Artist != null && m.Artist.StartsWith(artist));
            }

            // Filtrerer på duration
            if (duration != null)
            {
                musicRecords = musicRecords.FindAll(m => m.Duration == duration);
            }

            // Filtrerer på publication year
            if (publicationYear != null)
            {
                musicRecords = musicRecords.FindAll(m => m.PublicationYear == publicationYear);
            }

            // Sortering
            if (sortBy != null)
            {
                switch (sortBy.ToLower())
                {
                    case "id":
                        musicRecords = musicRecords.OrderBy(m => m.Id).ToList();
                        break;

                    case "title":
                        musicRecords = musicRecords.OrderBy(m => m.Title).ToList();
                        break;

                    case "artist":
                        musicRecords = musicRecords.OrderBy(m => m.Artist).ToList();
                        break;

                    case "duration":
                        musicRecords = musicRecords.OrderByDescending(m => m.Duration).ToList();
                        break;

                    case "publicationyear":
                        musicRecords = musicRecords.OrderByDescending(m => m.PublicationYear).ToList();
                        break;

                    case "durationasc":
                        musicRecords = musicRecords.OrderBy(m => m.Duration).ToList();
                        break;

                    case "publicationyearasc":
                        musicRecords = musicRecords.OrderBy(m => m.PublicationYear).ToList();
                        break;
                }
            }

            return musicRecords;
        }

        public MusicRecord? GetById(int id)
        {
            return _context.MusicRecords.Find(id);
        }

        public MusicRecord? Remove(int id)
        {
            MusicRecord? musicRecord = _context.MusicRecords.Find(id);

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
            MusicRecord? existingMusicRecord = GetById(id);

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