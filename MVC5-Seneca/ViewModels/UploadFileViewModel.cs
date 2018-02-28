using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC5_Seneca.ViewModels
{
    public class UploadFileViewModel
    { 
        // Input Fields for cshtml file //
        public virtual List<SelectListItem> Students { get; set; }             
        public virtual List<SelectListItem> DocumentTypes { get; set; }
        public virtual string ErrorMessage { get; set; }
    }
}