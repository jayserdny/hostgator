using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class AddEditTipDocumentViewModel
    {
        public int Id { get; set; }
        public virtual IEnumerable<SelectListItem> Categories { get; set; }
        public virtual TipsCategory Category { get; set; }

        [DisplayName("Title")]
        public string Name { get; set; }
        public string DocumentLink { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual string ErrorMessage { get; set; }
    }
}