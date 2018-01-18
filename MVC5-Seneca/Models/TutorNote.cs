using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using MVC5_Seneca.Models;

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

    }
}