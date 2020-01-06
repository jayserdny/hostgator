using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SenecaHeights.ViewModels;

namespace SenecaHeights.EntityModels
{
    public class HfedEmail
    {
        [NotMapped] public string Title { get; set; }

        [NotMapped] public string EmailText { get; set; }

        [AllowHtml]
        [NotMapped] public string HtmlContent { get; set; }

        [NotMapped]                  
        public List<HfedEmailRecipient> Recipients { get; set; }     
    }
}