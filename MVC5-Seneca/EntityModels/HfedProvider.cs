using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC5_Seneca.EntityModels
{
    public class HfedProvider
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Address")]
        public string Address { get; set; }

        [DisplayName("Main Phone")]
        public string MainPhone { get; set; }

        [DisplayName("Fax")]
        public string Fax { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Contact Name")]
        public string ContactName { get; set; }

        [DisplayName("Contact Email")]
        public string ContactEmail { get; set; }

        [DisplayName("Contact Phone")]
        public string ContactPhone { get; set; }

        [DisplayName("Note")]
        [DataType(DataType.MultilineText)]
        public string ProviderNote { get; set; }

        [DisplayName("Box Weight")]
        public decimal? BoxWeight { get; set; }  
    }
}