namespace RESTMusicRecord
{
    public class MusicRecord
    
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Artist { get; set; }
            public int Duration { get; set; }
            public int PublicationYear { get; set; }

            public override string ToString()
            {
                return $"Id:{Id},Title: {Title},Artist: {Artist}, Duration in seconds: {Duration}, Publication Year:{PublicationYear}";
            }
        }
    }



