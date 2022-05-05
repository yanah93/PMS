using System.ComponentModel.DataAnnotations;

namespace PMS.Model
{
    public class UserAccountModel
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsProjectManager { get; set; }
        public DateTime RegistrationTime { get; set; }

    }
}
