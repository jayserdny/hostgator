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
     [JsonObject(MemberSerialization.OptIn)] public class StudentReport 
    {
        [JsonProperty] public int Id { get; set; }    

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DisplayName("Document Date"), JsonProperty]
        public DateTime? DocumentDate { get; set; }

        [DisplayName("Document Type"), JsonProperty]
        public virtual DocumentType DocumentType { get; set; }


        [DisplayName("Comments"), JsonProperty]
        public string Comments { get; set; }

        public virtual Student Student { get; set; }

        [JsonProperty] public string DocumentLink { get; set; } 

    }
}