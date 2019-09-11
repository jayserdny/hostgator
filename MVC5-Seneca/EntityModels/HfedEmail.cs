using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.EntityModels
{
    public class HfedEmail
    {
        [NotMapped]
        public string Title { get; set; }

        [NotMapped]
        public string EmailText { get; set; }

        [NotMapped]
        public Collection<HfedEmailRecipient> Recipients { get; set; }
    }
}