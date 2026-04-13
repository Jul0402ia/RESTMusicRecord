using RESTMusicRecord.Models;

namespace RESTMusicRecord.Repository
{
    // in memory repository klasse - data gemmes via en liste og ikke i db
    public class REPOMusicRecords : IMusicRecordRepository
    {
        // privat liste der gemmer alle musicrecords objekter - det betyder at andre klasser ikke kan ændre i listen direkte
        private List<MusicRecord> m_musicRecords = new List<MusicRecord>();

        // bruges til automatisk at give nye objekter et unikt id
        private static int nextid = 1;

        // tim constructor 
        public REPOMusicRecords() { }

        // læs om readonly collections i C# og overvej at bruge det i stedet for at returnere en kopi af listen, tænk på pladsen listen bruger i hukommelsen, og om det er nødvendigt at returnere en kopi af listen, eller om det er nok at returnere en readonly collection, som ikke kan ændres uden for klassen.
        // henter alle music records, og parametren bruges til filtrering 
        public IEnumerable<MusicRecord> GetAll(string title = null, string artist = null,int? duration = null,int? publicationYear =null, string sortBy = null)
        {
            // vi laver en kopi af listen - så andre ikke arbejder direkte på den orginale liste i klassen
            List<MusicRecord> musicRecords = new List<MusicRecord>(m_musicRecords);

            // filtrer på titel hvis titel ikke er null
            if (title != null)
            {
                musicRecords = musicRecords.FindAll(m => m.Title.StartsWith (title));
            }
            // samme her 
            if (artist != null) 
            {
                musicRecords = musicRecords.FindAll(m => m.Artist.StartsWith(artist));
            }
            if (duration != null)
            {
                musicRecords = musicRecords.FindAll(m => m.Duration == duration);

            }
            if (publicationYear != null) 
            {
                musicRecords = musicRecords.FindAll(m => m.PublicationYear == publicationYear);
            }
            //returnerer den filtrerede liste
            return musicRecords;
        }

        // finder et objekt ud fra id
        public MusicRecord? GetById(int id)
        {
            // leder efter første objekt med matchende id
            MusicRecord musicRecord = m_musicRecords.FirstOrDefault(m => m.Id == id);

            //Hvis intet objekt findes, returneres null
            if (musicRecord == null)
            {
                return null;
            }

            // vi laver en kopi af objektet - så vi ikke retunerer den orginale reference
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
