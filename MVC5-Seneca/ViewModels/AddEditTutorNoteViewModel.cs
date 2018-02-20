using System;
using System.Collections.Generic;
using MVC5_Seneca.EntityModels;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MVC5_Seneca.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AddEditTutorNoteViewModel
    {
        [JsonProperty] public int Id { get; set; }

        [JsonProperty] public IEnumerable<SelectListItem> Users { get; set; }
        [JsonProperty] public IEnumerable<SelectListItem> Students { get; set; }

        // Must send TutorNotes as iCollection, and construct SelectListItems in jscript:
        [JsonProperty] public ICollection<TutorNote> TutorNotes { get; set; }

        [JsonProperty] public virtual ApplicationUser User { get; set; }   
        [JsonProperty] public int User_Id { get; set; }
        [JsonProperty] public virtual Student Student { get; set; }
        [JsonProperty] public int Student_Id { get; set; }
        [JsonProperty] public String SessionNote { get; set; }

        [JsonProperty] public virtual TutorNote TutorNote { get; set; }      
        [JsonProperty] public DateTime? Date { get; set; }
    }
}