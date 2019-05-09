using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC5_Seneca.EntityModels
{
    public class HfedLocation
    {
        public int Id { get; set; }

        [DisplayName("Location Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]
        public string LocationNote { get; set; }
    }
}