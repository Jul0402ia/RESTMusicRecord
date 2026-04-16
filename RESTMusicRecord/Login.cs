namespace RESTMusicRecord
{
    /// <summary>
    /// This class is used to receive login data from the client.
    /// </summary>
    public class Login
    {
        // Username entered by the user
        public string Username { get; set; }

        // Password entered by the user
        public string Password { get; set; }

        // Constructor ensures values are never null
        public Login()
        {
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}