using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MVC5_Seneca.ViewModels
{
    public class HfedEmailViewModel
    {
        public string Title { get; set; }

        [AllowHtml ]
        public string EmailText { get; set; }
        public  Collection<HfedEmailRecipient> Recipients { get; set; }
    }
}