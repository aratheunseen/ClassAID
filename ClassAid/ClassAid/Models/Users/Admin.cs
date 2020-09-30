using ClassAid.Models.Engines;


namespace ClassAid.Models.Users
{
    public class Admin : Shared
    {
        public Admin(string Username, string Password)
        {
            this.Username = Username.ToLower();
            this.Password = Password;
            Key = Cryptography.EncryptString(this.Username, this.Password);
        }
        public Admin() { }

    }
}
