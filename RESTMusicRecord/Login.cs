namespace RESTMusicRecord
{
    public class Login
    {
        // brugernavn
        public string Username { get; set; }

        // kode
        public string Password { get; set; }

        // constructor
        public Login()
        {
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}