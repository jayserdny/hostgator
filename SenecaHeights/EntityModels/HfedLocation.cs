using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SenecaHeights.EntityModels
{
    public class HfedLocation
    {
        public int Id { get; set; }

        [DisplayName("Location Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Main Phone")]
        public string MainPhone { get; set; }

        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]
        public string LocationNote { get; set; }
    }
}