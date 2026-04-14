
namespace RESTMusicRecord
{
    public interface IREPOMusicRecords
    {
        MusicRecord Add(MusicRecord musicRecord);
        IEnumerable<MusicRecord> GetAll(string? title = null, string? artist = null, int? duration = null, int? publicationYear = null, string? sortBy = null);
        MusicRecord? GetById(int id);
        MusicRecord? Remove(int id);
        MusicRecord? Update(int id, MusicRecord updatedMusicRecord);
    }
}