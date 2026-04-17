using System.Collections.Generic;
using System.Linq;

namespace RESTMusicRecord
{
    public class REPOMusicRecords : IREPOMusicRecords
    {
        // Liste som gemmer alle music records i hukommelsen
        private List<MusicRecord> m_musicRecords = new List<MusicRecord>();

        // Holder styr på næste ledige id
        private static int nextid = 1;

        // Constructor
        public REPOMusicRecords()
        {
        }

        // Henter alle records med mulighed for filter og sortering
        public IEnumerable<MusicRecord> GetAll(string? title = null, string? artist = null, int? duration = null, int? publicationYear = null, string? sortBy = null)
        {
            // Laver kopi af listen så originalen ikke ændres
            List<MusicRecord> musicRecords = new List<MusicRecord>(m_musicRecords);

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

            // Sorterer listen hvis brugeren ønsker det
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

        // Finder en record ud fra id
        public MusicRecord? GetById(int id)
        {
            MusicRecord? musicRecord = m_musicRecords.FirstOrDefault(m => m.Id == id);

            // Hvis ikke fundet returneres null
            if (musicRecord == null)
            {
                return null;
            }

            // Returnerer kopi af objektet
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

        // Tilføjer ny music record
        public MusicRecord Add(MusicRecord musicRecord)
        {
            // Opretter nyt objekt med nyt id
            MusicRecord newMusicRecord = new MusicRecord
            {
                Id = nextid++,
                Title = musicRecord.Title,
                Artist = musicRecord.Artist,
                Duration = musicRecord.Duration,
                PublicationYear = musicRecord.PublicationYear
            };

            // Tilføjer til listen
            m_musicRecords.Add(newMusicRecord);

            // Returnerer kopi
            MusicRecord musicRecordCopy = new MusicRecord
            {
                Id = newMusicRecord.Id,
                Title = newMusicRecord.Title,
                Artist = newMusicRecord.Artist,
                Duration = newMusicRecord.Duration,
                PublicationYear = newMusicRecord.PublicationYear
            };

            return musicRecordCopy;
        }

        // Sletter record ud fra id
        public MusicRecord? Remove(int id)
        {
            MusicRecord? musicRecord = m_musicRecords.FirstOrDefault(m => m.Id == id);

            // Hvis ikke fundet returneres null
            if (musicRecord == null)
            {
                return null;
            }

            // Fjerner fra listen
            m_musicRecords.Remove(musicRecord);

            // Returnerer kopi af det slettede objekt
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

        // Opdaterer eksisterende record
        public MusicRecord? Update(int id, MusicRecord updatedMusicRecord)
        {
            MusicRecord? existingMusicRecord = m_musicRecords.FirstOrDefault(m => m.Id == id);

            // Hvis ikke fundet returneres null
            if (existingMusicRecord == null)
            {
                return null;
            }

            // Overskriver værdier
            existingMusicRecord.Title = updatedMusicRecord.Title;
            existingMusicRecord.Artist = updatedMusicRecord.Artist;
            existingMusicRecord.Duration = updatedMusicRecord.Duration;
            existingMusicRecord.PublicationYear = updatedMusicRecord.PublicationYear;

            // Returnerer kopi af det opdaterede objekt
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