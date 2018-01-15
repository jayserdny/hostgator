using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.Models;
using Newtonsoft.Json;

namespace MVC5_Seneca.EntityModels 
{
    [JsonObject(MemberSerialization.OptIn)] public class Parent
    {
        [JsonProperty] public int Id { get; set; }

        [DisplayName("Mother/Father (M/F)")]
        [JsonProperty] public string MotherFather { get; set; }   // M or F

        [DisplayName("First Name")]
        [JsonProperty] public string FirstName { get; set; }

        [DisplayName("Address"), JsonProperty]
        public string Address { get; set; }

        [DisplayName("Home Phone"), JsonProperty]
        [Phone]
        public string HomePhone { get; set; }

        [DisplayName("Cell Phone"), JsonProperty]
        [Phone]
        public string CellPhone { get; set; }

                
        [DisplayName("Email"), JsonProperty]
        public string Email { get; set; }

    }
    
}