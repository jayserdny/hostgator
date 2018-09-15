using System;
using System.Collections.Generic;    
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MVC5_Seneca.EntityModels
{
    [JsonObject(MemberSerialization.OptIn)] public class Student
    {
        [JsonProperty] public int Id { get; set; }

        [DisplayName("First Name"), JsonProperty]
        public string FirstName { get; set; }

        [DisplayName("M  /  F"), JsonProperty]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Birthday"), JsonProperty]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Grade Level")]
        [JsonProperty] public int? GradeLevel { get; set; }

        [DisplayName("In Special Class")]
        [JsonProperty] public bool SpecialClass { get; set; }    

        [DisplayName("School")]
        [JsonProperty] public virtual School School { get; set; }      

        [DisplayName("Parent")]
        [JsonProperty] public virtual Parent Parent { get; set; }       

        [JsonProperty] public virtual ICollection<StudentReport> Reports { get; set; }

        public virtual  ICollection<TutorNote> TutorNotes { get; set; }

        [DisplayName("Tutor")]
        [JsonProperty] public virtual ApplicationUser PrimaryTutor { get; set; }

        [DisplayName("Teacher")]
        [JsonProperty] public virtual Teacher Teacher { get; set; }

        [JsonProperty] public virtual ICollection<ApplicationUser> AssociateTutors { get; set; }
        
        // Fields for Who's Tutoring Whom cshtml report: 
        [NotMapped] [JsonProperty] public int? PrimaryNoteCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped] [JsonProperty] public DateTime? LastPrimaryNoteDate { get; set; }  
        [NotMapped] [JsonProperty] public int? AssociateNoteCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [NotMapped] [JsonProperty] public DateTime? LastAssociateNoteDate { get; set; }
    }
}