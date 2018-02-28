using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVC5_Seneca.EntityModels;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MVC5_Seneca.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)] public class DisplayStudentInfoViewModel
    {
        // Input fields needed by the .cshtml file to display the form
        [JsonProperty] public List<SelectListItem> Students { get; set; }
        [JsonProperty] public List<SelectListItem> Reports { get  ; set; }

        [JsonProperty] public List<Parent> Parents;
      
        [JsonProperty] public List<ApplicationUser> Users;
        [JsonProperty] public List<DocumentType> DocumentTypes { get; set; }

        // Output fields used by the .cshtml file to return form results
        [DisplayName("Student:")]
        [JsonProperty] public Student Student { get; set; }
        [JsonProperty] public ApplicationIdentity User { get; set; }  // to go with new Session Note  
        [JsonProperty] public string User_Id { get; set; }  // to go with new Session Note  

        [DisplayName("Parent:")]
        [JsonProperty] public Parent Parent { get; set; }

        [DisplayName("Author:")]
        [JsonProperty] public StudentReport Report { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [JsonProperty] public DateTime? SessionNoteDate { get; set; }

        [JsonProperty] public string SessionNote { get; set; }   

        public virtual string UpdateAllowed { get; set; }        
    }
}