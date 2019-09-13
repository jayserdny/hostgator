using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MVC5_Seneca.ViewModels
{
    public class HfedEmailViewModel
    {
        public string Title { get; set; }
        public string EmailText { get; set; }         
        public List<HfedEmailRecipient> Recipients { get; set; }
    }
}