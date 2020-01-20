using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace MVC5_Seneca.EntityModels
{
    [JsonObject(MemberSerialization.OptIn)] public class TutorNote
    {  
        [JsonProperty] public int Id { get; set; }

        [DisplayName("User Id"), JsonProperty]    
        public virtual ApplicationUser ApplicationUser { get; set; } 
                                          
        public virtual Student Student { get; set; }
        
        [JsonProperty] public DateTime? Date { get; set; }

        [DisplayName("Session Note"), JsonProperty]
        public String SessionNote { get; set; }

        [JsonProperty] public virtual Boolean UpdateAllowed { get; set; }

    }
}