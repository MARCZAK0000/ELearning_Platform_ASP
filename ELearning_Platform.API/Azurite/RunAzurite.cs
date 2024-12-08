using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ELearning_Platform.API.Azurite
{
    public class RunAzurite(AzuriteOptions options)
    {
        private readonly AzuriteOptions _options = options;
        public async Task RunEmulator()
        {
            try
            {
                var proccess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = $@"{_options.FileName}",
                        RedirectStandardError = false,
                        Verb = _options.Verb,
                        RedirectStandardOutput = false,
                        CreateNoWindow = false,
                        UseShellExecute = _options.UseShellExecute,

                    }
                };
                await Task.Run(() =>
                {
                    proccess.Start();
                    
                });
            }

            catch (Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine(ex);
                System.Diagnostics.Debugger.Break();   
            }
            
            
        }
    }
}
