using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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