using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace ELearning_Platform.Domain.Response.ClassResponse
{
    public class CreateClassResponse
    {
        public bool IsCreated {  get; set; }    
        
        public string Name { get; set; }    
    }
}
