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
        private object _lock = new object();

        [Required]
        public string Host
        {
            get 
            {
                lock (_lock)
                {
                    { return _host; }
                }
            }
            set 
            { 
                lock (_lock) 
                { 
                    _host = value; 
                } 
            }
        }
        private string _host;

    }
}
