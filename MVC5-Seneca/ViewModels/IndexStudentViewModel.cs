using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC5_Seneca.Models;
using MVC5_Seneca.EntityModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MVC5_Seneca.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class IndexStudentViewModel  
    {
        // Input fields needed by the Index.cshtml file to display the form
        [JsonProperty] public List<School> AllSchools;
        [JsonProperty] public List<Parent> AllParents;

        // Output fields used by the .cshtml file to return form results
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("M/F")]
        public string Gender { get; set; }

        [DisplayName("Birthday")]
        [JsonProperty] public DateTime BirthDate { get; set; }

        [DisplayName("School"), JsonProperty]
        public int School_Id { get; set; } 
        
       [JsonProperty] public virtual School School { get; set; }
        
        //public List<SelectListItem> Students { get; set; } 
    }
}