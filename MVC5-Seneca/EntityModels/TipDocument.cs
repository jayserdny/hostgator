using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace MVC5_Seneca.EntityModels
{
    [JsonObject(MemberSerialization.OptIn)] public class TipDocument
    {
        [JsonProperty] public int Id { get; set; }
        [JsonProperty] public virtual TipsCategory Category { get; set; }
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public string DocumentLink { get; set; }                           
        [JsonProperty] public virtual ApplicationUser User { get; set; }    // who submitted this tip

        [JsonProperty] public virtual ICollection<TipsCategory> Categories { get; set; }        
    }
}