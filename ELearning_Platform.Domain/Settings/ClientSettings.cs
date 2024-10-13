using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Domain.Settings
{
    public class ClientSettings
    {
        [Required]
        public string Host {  get; set; }   
    }
}
