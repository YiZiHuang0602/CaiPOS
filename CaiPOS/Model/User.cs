using System.ComponentModel.DataAnnotations;
namespace CaiPOS.Model
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public string UserNumber { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
