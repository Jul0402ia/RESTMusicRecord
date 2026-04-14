using System.Collections.Generic;
using System.Linq;
using RESTMusicRecord;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Add_ShouldAssignIdAndReturnMusicRecord()
        {
            // Arrange
            REPOMusicRecords repo = new REPOMusicRecords();
            MusicRecord input = new MusicRecord
            {
                Title = "Song",
                Artist = "Artist",
                Duration = 120,
                PublicationYear = 2022
            };

            // Act
            MusicRecord addedMusicRecord = repo.Add(input);

            // Assert
            Assert.NotNull(addedMusicRecord);
            Assert.True(addedMusicRecord.Id > 0);
            Assert.Equal("Song", addedMusicRecord.Title);
            Assert.Equal("Artist", addedMusicRecord.Artist);
            Assert.Equal(120, addedMusicRecord.Duration);
            Assert.Equal(2022, addedMusicRecord.PublicationYear);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Arrange
            REPOMusicRecords repo = new REPOMusicRecords();

            // Act
            MusicRecord? result = repo.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetById_ShouldReturnMusicRecord_WhenIdExists()
        {
            // Arrange
            REPOMusicRecords repo = new REPOMusicRecords();
            MusicRecord addedMusicRecord = repo.Add(new MusicRecord
            {
                Title = "Hello",
                Artist = "Adele",
                Duration = 300,
                PublicationYear = 2015
            });

            // Act
            MusicRecord? foundMusicRecord = repo.GetById(addedMusicRecord.Id);

            // Assert
            Assert.NotNull(foundMusicRecord);
            Assert.Equal(addedMusicRecord.Id, foundMusicRecord.Id);
            Assert.Equal("Hello", foundMusicRecord.Title);
        }

        [Fact]
        public void GetAll_ShouldFilterByTitle()
        {
            // Arrange
            REPOMusicRecords repo = new REPOMusicRecords();

            repo.Add(new MusicRecord { Title = "Alpha", Artist = "Band1", Duration = 100, PublicationYear = 2010 });
            repo.Add(new MusicRecord { Title = "Beta", Artist = "Band2", Duration = 200, PublicationYear = 2011 });
            repo.Add(new MusicRecord { Title = "Alone", Artist = "Band3", Duration = 150, PublicationYear = 2012 });

            // Act
            List<MusicRecord> result = repo.GetAll(title: "Al").ToList();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Update_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Arrange
            REPOMusicRecords repo = new REPOMusicRecords();

            // Act
            MusicRecord? result = repo.Update(100, new MusicRecord
            {
                Title = "New",
                Artist = "Artist",
                Duration = 180,
                PublicationYear = 2020
            });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Update_ShouldChangeMusicRecord_WhenIdExists()
        {
            // Arrange
            REPOMusicRecords repo = new REPOMusicRecords();
            MusicRecord addedMusicRecord = repo.Add(new MusicRecord
            {
                Title = "Old",
                Artist = "OldArtist",
                Duration = 100,
                PublicationYear = 2000
            });

            // Act
            MusicRecord? updatedMusicRecord = repo.Update(addedMusicRecord.Id, new MusicRecord
            {
                Title = "New",
                Artist = "NewArtist",
                Duration = 200,
                PublicationYear = 2024
            });

            // Assert
            Assert.NotNull(updatedMusicRecord);
            Assert.Equal("New", updatedMusicRecord.Title);
            Assert.Equal("NewArtist", updatedMusicRecord.Artist);
            Assert.Equal(200, updatedMusicRecord.Duration);
            Assert.Equal(2024, updatedMusicRecord.PublicationYear);
        }

        [Fact]
        public void Remove_ShouldReturnNull_WhenIdDoesNotExist()
        {
            // Arrange
            REPOMusicRecords repo = new REPOMusicRecords();

            // Act
            MusicRecord? result = repo.Remove(1234);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Remove_ShouldDeleteMusicRecord_WhenIdExists()
        {
            // Arrange
            REPOMusicRecords repo = new REPOMusicRecords();
            MusicRecord addedMusicRecord = repo.Add(new MusicRecord
            {
                Title = "DeleteMe",
                Artist = "Artist",
                Duration = 220,
                PublicationYear = 2021
            });

            // Act
            MusicRecord? removedMusicRecord = repo.Remove(addedMusicRecord.Id);
            MusicRecord? foundAfterDelete = repo.GetById(addedMusicRecord.Id);

            // Assert
            Assert.NotNull(removedMusicRecord);
            Assert.Equal(addedMusicRecord.Id, removedMusicRecord.Id);
            Assert.Null(foundAfterDelete);
        }
    }
}