using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;
using Newtonsoft.Json;

namespace MVC5_Seneca.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class UpdateMyScheduleViewModel
    {  
        [JsonProperty] public string Id { get; set; }

        [JsonProperty, DisplayName("First Name")]
        public string FirstName { get; set; }

        [JsonProperty, DisplayName("Last Name")]
        public string LastName { get; set; }

        public virtual List<SelectListItem> Students { get; set; }
    }
}