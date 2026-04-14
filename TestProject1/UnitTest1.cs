using System;
using System.Collections.Generic;
using System.Linq;
using RESTMusicRecord;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        // Fake repository til test (i stedet for rigtig database)
        private class FakeMusicRepo : IREPOMusicRecords
        {
            // Liste der gemmer data i hukommelsen
            private readonly List<MusicRecord> _store = new();

            // Bruges til at give unikke Id'er
            private int _nextId = 1;

            public MusicRecord Add(MusicRecord musicRecord)
            {
                // Tjekker om input er null
                if (musicRecord == null) throw new ArgumentNullException(nameof(musicRecord));

                // Opretter en kopi (beskytter original data)
                var copy = new MusicRecord
                {
                    Id = _nextId++, // giver nyt id og øger tæller
                    Title = musicRecord.Title,
                    Artist = musicRecord.Artist,
                    Duration = musicRecord.Duration,
                    PublicationYear = musicRecord.PublicationYear
                };

                // Gemmer i listen
                _store.Add(copy);

                // Returnerer en ny kopi (beskytter intern liste)
                return new MusicRecord
                {
                    Id = copy.Id,
                    Title = copy.Title,
                    Artist = copy.Artist,
                    Duration = copy.Duration,
                    PublicationYear = copy.PublicationYear
                };
            }

            public IEnumerable<MusicRecord> GetAll(string? title = null, string? artist = null, int? duration = null, int? publicationYear = null, string? sortBy = null)
            {
                // Starter med kopi af alle elementer
                IEnumerable<MusicRecord> result = _store.Select(m => new MusicRecord
                {
                    Id = m.Id,
                    Title = m.Title,
                    Artist = m.Artist,
                    Duration = m.Duration,
                    PublicationYear = m.PublicationYear
                });

                // Filtrering
                if (title is not null)
                    result = result.Where(m => m.Title != null && m.Title.StartsWith(title, StringComparison.Ordinal));

                if (artist is not null)
                    result = result.Where(m => m.Artist != null && m.Artist.StartsWith(artist, StringComparison.Ordinal));

                if (duration is not null)
                    result = result.Where(m => m.Duration == duration);

                if (publicationYear is not null)
                    result = result.Where(m => m.PublicationYear == publicationYear);

                // Sortering
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    switch (sortBy.ToLowerInvariant())
                    {
                        case "id":
                            result = result.OrderBy(m => m.Id);
                            break;

                        case "title":
                            result = result.OrderBy(m => m.Title);
                            break;

                        case "artist":
                            result = result.OrderBy(m => m.Artist);
                            break;

                        case "duration":
                            result = result.OrderByDescending(m => m.Duration);
                            break;

                        case "publicationyear":
                            result = result.OrderByDescending(m => m.PublicationYear);
                            break;
                    }
                }

                // Returnerer som liste
                return result.ToList();
            }

            public MusicRecord? GetById(int id)
            {
                // Finder første match
                var m = _store.FirstOrDefault(x => x.Id == id);

                // Hvis ikke fundet → null
                if (m == null) return null;

                // Returnerer kopi
                return new MusicRecord
                {
                    Id = m.Id,
                    Title = m.Title,
                    Artist = m.Artist,
                    Duration = m.Duration,
                    PublicationYear = m.PublicationYear
                };
            }

            public MusicRecord? Remove(int id)
            {
                // Finder objekt
                var m = _store.FirstOrDefault(x => x.Id == id);

                // Hvis ikke fundet → null
                if (m == null) return null;

                // Fjerner fra liste
                _store.Remove(m);

                // Returnerer kopi
                return new MusicRecord
                {
                    Id = m.Id,
                    Title = m.Title,
                    Artist = m.Artist,
                    Duration = m.Duration,
                    PublicationYear = m.PublicationYear
                };
            }

            public MusicRecord? Update(int id, MusicRecord updatedMusicRecord)
            {
                // Finder eksisterende objekt
                var existing = _store.FirstOrDefault(x => x.Id == id);

                // Hvis ikke fundet → null
                if (existing == null) return null;

                // Opdaterer værdier
                existing.Title = updatedMusicRecord.Title;
                existing.Artist = updatedMusicRecord.Artist;
                existing.Duration = updatedMusicRecord.Duration;
                existing.PublicationYear = updatedMusicRecord.PublicationYear;

                // Returnerer kopi
                return new MusicRecord
                {
                    Id = existing.Id,
                    Title = existing.Title,
                    Artist = existing.Artist,
                    Duration = existing.Duration,
                    PublicationYear = existing.PublicationYear
                };
            }
        }

        [Fact]
        public void Add_ShouldAssignIdAndReturnCopy()
        {
            var repo = new FakeMusicRepo();

            // Opretter input objekt
            var input = new MusicRecord { Title = "Song", Artist = "Artist", Duration = 120, PublicationYear = 2022 };

            // Kalder Add
            var added = repo.Add(input);

            // Tjekker resultat
            Assert.NotNull(added);
            Assert.True(added.Id > 0);
            Assert.Equal("Song", added.Title);
            Assert.Equal("Artist", added.Artist);
        }

        [Fact]
        public void GetById_ReturnsNullWhenMissing_AndReturnsCopyWhenPresent()
        {
            var repo = new FakeMusicRepo();

            // Tester ikke-eksisterende id
            Assert.Null(repo.GetById(999));

            // Tilføjer data
            var added = repo.Add(new MusicRecord { Title = "T", Artist = "A", Duration = 60, PublicationYear = 2000 });

            // Henter igen
            var fetched = repo.GetById(added.Id);

            // Tjekker resultat
            Assert.NotNull(fetched);
            Assert.Equal(added.Id, fetched!.Id);
            Assert.Equal(added.Title, fetched.Title);
        }

        [Fact]
        public void GetAll_FiltersAndSortsCorrectly()
        {
            var repo = new FakeMusicRepo();

            // Tilføjer test data
            repo.Add(new MusicRecord { Title = "A1", Artist = "Band", Duration = 100, PublicationYear = 2010 });
            repo.Add(new MusicRecord { Title = "B1", Artist = "Band", Duration = 200, PublicationYear = 2011 });
            repo.Add(new MusicRecord { Title = "A2", Artist = "Solo", Duration = 150, PublicationYear = 2012 });

            // Filtrerer på titel
            var byTitlePrefix = repo.GetAll(title: "A").ToList();
            Assert.Equal(2, byTitlePrefix.Count);

            // Filtrerer på artist
            var byArtist = repo.GetAll(artist: "Band").ToList();
            Assert.Equal(2, byArtist.Count);

            // Sorterer på titel
            var sortedByTitle = repo.GetAll(sortBy: "title").ToList();
            Assert.Equal(3, sortedByTitle.Count);
            Assert.Equal("A1", sortedByTitle[0].Title);
        }

        [Fact]
        public void Update_WhenNotFound_ReturnsNull_AndWhenFound_Updates()
        {
            var repo = new FakeMusicRepo();

            // Test: findes ikke
            Assert.Null(repo.Update(5, new MusicRecord()));

            // Tilføj og opdater
            var added = repo.Add(new MusicRecord { Title = "Old", Artist = "X", Duration = 10, PublicationYear = 1999 });
            var updated = repo.Update(added.Id, new MusicRecord { Title = "New", Artist = "Y", Duration = 20, PublicationYear = 2000 });

            // Tjek resultat
            Assert.NotNull(updated);
            Assert.Equal("New", updated!.Title);

            var fetched = repo.GetById(added.Id);
            Assert.Equal("New", fetched!.Title);
        }

        [Fact]
        public void Remove_WhenExists_RemovesAndReturnsCopy_OtherwiseNull()
        {
            var repo = new FakeMusicRepo();

            // Test: findes ikke
            Assert.Null(repo.Remove(1234));

            // Tilføj og fjern
            var added = repo.Add(new MusicRecord { Title = "ToRemove", Artist = "Z", Duration = 30, PublicationYear = 2015 });
            var removed = repo.Remove(added.Id);

            // Tjek resultat
            Assert.NotNull(removed);
            Assert.Equal(added.Id, removed!.Id);

            // Skal ikke kunne findes bagefter
            Assert.Null(repo.GetById(added.Id));
        }
    }
}