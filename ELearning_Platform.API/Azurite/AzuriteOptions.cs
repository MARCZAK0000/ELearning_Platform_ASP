using System.ComponentModel.DataAnnotations;

namespace ELearning_Platform.API.Azurite
{
    public class AzuriteOptions
    {
        public string Verb {  get; set; }

        public string FileName { get; set; }

        public bool UseShellExecute { get; set; }
    }
}
