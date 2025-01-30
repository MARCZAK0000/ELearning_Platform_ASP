using System.ComponentModel.DataAnnotations.Schema;

namespace ELearning_Platform.Domain.Database.Enitities
{
    [Table("Moderators", Schema = "Role")]
    public class Moderators
    {
        public string AccountID { get; set; }

        public DateTime ModifiedDate { get; set; }

        public User User {  get; set; } 
    }
}
