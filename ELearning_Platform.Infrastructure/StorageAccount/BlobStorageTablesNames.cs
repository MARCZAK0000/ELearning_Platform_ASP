using System.ComponentModel.DataAnnotations;

namespace ELearning_Platform.Infrastructure.StorageAccount
{
    public class BlobStorageTablesNames
    {
        [Required]
        public string profileImage { get; set; }

        [Required]
        public string video { get; set; }

        [Required]
        public string lessonImage { get; set; }
    }
}
