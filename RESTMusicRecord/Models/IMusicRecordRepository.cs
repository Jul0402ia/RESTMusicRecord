namespace RESTMusicRecord.Models
{
    public interface IMusicRecordRepository
    {
        IEnumerable<MusicRecord> GetAll(string title = null, string artist = null, int? duration = null, int? publicationYear = null, string sortBy = null);

        MusicRecord? GetById(int id);

        MusicRecord Add(MusicRecord musicRecord);

        MusicRecord? Remove(int id);

        MusicRecord? Update(int id, MusicRecord updatedMusicRecord);



    }
}
