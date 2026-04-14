using System.Collections.Generic;
using System.Linq;

namespace RESTMusicRecord
{
    public class REPOMusicRecords : IREPOMusicRecords
    {
        private List<MusicRecord> m_musicRecords = new List<MusicRecord>();

        private static int nextid = 1;

        public REPOMusicRecords()
        {
        }

        public IEnumerable<MusicRecord> GetAll(string? title = null, string? artist = null, int? duration = null, int? publicationYear = null, string? sortBy = null)
        {
            List<MusicRecord> musicRecords = new List<MusicRecord>(m_musicRecords);

            if (title != null)
            {
                musicRecords = musicRecords.FindAll(m => m.Title != null && m.Title.StartsWith(title));
            }

            if (artist != null)
            {
                musicRecords = musicRecords.FindAll(m => m.Artist != null && m.Artist.StartsWith(artist));
            }

            if (duration != null)
            {
                musicRecords = musicRecords.FindAll(m => m.Duration == duration);
            }

            if (publicationYear != null)
            {
                musicRecords = musicRecords.FindAll(m => m.PublicationYear == publicationYear);
            }

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
            MusicRecord? musicRecord = m_musicRecords.FirstOrDefault(m => m.Id == id);

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
            MusicRecord newMusicRecord = new MusicRecord
            {
                Id = nextid++,
                Title = musicRecord.Title,
                Artist = musicRecord.Artist,
                Duration = musicRecord.Duration,
                PublicationYear = musicRecord.PublicationYear
            };

            m_musicRecords.Add(newMusicRecord);

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