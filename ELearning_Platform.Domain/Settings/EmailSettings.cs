using System.ComponentModel.DataAnnotations;

namespace ELearning_Platform.Domain.Settings
{
    public class EmailSettings
    {
        [Required]
        public string Host = "smtp.gmail.com";
        [AllowedValues(values: [587])]
        public int Port = 587;
        [Required]
        public string Email = "systemhr193@gmail.com";
        [Required]
        public string Password = "ujgu dadn zmoq tcpt ";
    }
}
