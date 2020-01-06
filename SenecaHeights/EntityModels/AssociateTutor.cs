using System.ComponentModel;
using Newtonsoft.Json;

namespace SenecaHeights.EntityModels
{
    [JsonObject(MemberSerialization.OptIn)]  public class AssociateTutor
    {
        [JsonProperty] public int Id { get; set; }

        [DisplayName("Tutor")]     
        [JsonProperty] public ApplicationUser Tutor { get; set; }
                                                                                                         
        [DisplayName("Student")]
        [JsonProperty] public Student Student { get; set; }
    }
}