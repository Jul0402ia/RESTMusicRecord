namespace RESTMusicRecord
{
    public class REPOMusicRecords
    {
        private List<MusicRecord> m_musicRecords = new List<MusicRecord>();

        private static int nextid = 1;

        public REPOMusicRecords() { }

        // læs om readonly collections i C# og overvej at bruge det i stedet for at returnere en kopi af listen, tænk på pladsen listen bruger i hukommelsen, og om det er nødvendigt at returnere en kopi af listen, eller om det er nok at returnere en readonly collection, som ikke kan ændres uden for klassen.
        public IEnumerable<MusicRecord> GetAll()
        {
            List<MusicRecord> musicRecords = new List<MusicRecord>(m_musicRecords);
            return musicRecords;
        }

        public MusicRecord? GetById(int id)
        {
            MusicRecord musicRecord = m_musicRecords.FirstOrDefault(m => m.Id == id);
            if (musicRecord == null)
            {
                return null;
            }
            MusicRecord musicRecordCopy = new MusicRecord
            {
                Id = musicRecord.Id,
                Title = musicRecord.Title,
                Artist = musicRecord.Artist,
                Duration = musicRecord.Duration,
                PublicationYear = musicRecord.PublicationYear

            };
            return musicRecordCopy;

        }
        public MusicRecord Add(MusicRecord musicRecord)
        {
            musicRecord.Id = nextid++;
            m_musicRecords.Add(musicRecord);

            MusicRecord musicRecordCopy = new MusicRecord
            {
                Id = musicRecord.Id,
                Title = musicRecord.Title,
                Artist = musicRecord.Artist,
                Duration = musicRecord.Duration,
                PublicationYear = musicRecord.PublicationYear
            };
            return musicRecordCopy;

        }
        public MusicRecord? Remove(int id)
        {
            MusicRecord? musicRecord = m_musicRecords.FirstOrDefault(m => m.Id == id);
            if (musicRecord == null)
            {
                return null;
            }
            m_musicRecords.Remove(musicRecord);

            MusicRecord musicRecordCopy = new MusicRecord
            {
                Id = musicRecord.Id,
                Title = musicRecord.Title,
                Artist = musicRecord.Artist,
                Duration = musicRecord.Duration,
                PublicationYear = musicRecord.PublicationYear
            };
            return musicRecordCopy;

        }
        public MusicRecord? Update(int id, MusicRecord updatedMusicRecord)
        {
            MusicRecord? existingMusicRecord = m_musicRecords.FirstOrDefault(m => m.Id == id);
            if (existingMusicRecord == null)
            {
                return null;
            }
            existingMusicRecord.Title = updatedMusicRecord.Title;
            existingMusicRecord.Artist = updatedMusicRecord.Artist;
            existingMusicRecord.Duration = updatedMusicRecord.Duration;
            existingMusicRecord.PublicationYear = updatedMusicRecord.PublicationYear;
            MusicRecord musicRecordCopy = new MusicRecord
            {
                Id = existingMusicRecord.Id,
                Title = existingMusicRecord.Title,
                Artist = existingMusicRecord.Artist,
                Duration = existingMusicRecord.Duration,
                PublicationYear = existingMusicRecord.PublicationYear
            };
            return musicRecordCopy;
        }
    }
}
