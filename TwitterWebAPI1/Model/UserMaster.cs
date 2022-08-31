using System.ComponentModel.DataAnnotations;

namespace TwitterWebAPI1.Model
{
    public class UserMaster
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public DateTime CreatedTimestamp { get; set; }

    }

    public class UserCredentials
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
    }
}
