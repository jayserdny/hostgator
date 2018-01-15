using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.Models;
using Newtonsoft.Json;

namespace MVC5_Seneca.EntityModels
{
    [JsonObject(MemberSerialization.OptIn)] public class Student
    {
        public int Id { get; set; }

        [DisplayName("First Name"), JsonProperty]
        public string FirstName { get; set; }

        [DisplayName("M/F"), JsonProperty]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Birthday"), JsonProperty]
        public DateTime? BirthDate { get; set; }     
  
        [DisplayName("School")]
        [JsonProperty] public virtual School School { get; set; }      

        [DisplayName("Parent")]
        [JsonProperty] public virtual Parent Parent { get; set; }       

        [JsonProperty] public virtual ICollection<StudentReport> Reports { get; set; }

        public virtual  ICollection<TutorNote> TutorNotes { get; set; }

    }
}