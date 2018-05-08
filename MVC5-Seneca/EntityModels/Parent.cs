using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace MVC5_Seneca.EntityModels 
{
    [JsonObject(MemberSerialization.OptIn)] public class Parent
    {
        [JsonProperty] public int Id { get; set; }

        [DisplayName("Mother/Father (M/F)")]
        [JsonProperty] public string MotherFather { get; set; }

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

        [DisplayName("Case Manager"), JsonProperty]
        public virtual ApplicationUser CaseManager { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void SetCaseManager(ApplicationUser cm)
        {
            CaseManager = cm;
        }
    } 
}
