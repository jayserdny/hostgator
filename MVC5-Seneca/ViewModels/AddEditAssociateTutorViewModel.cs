using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class AddEditAssociateTutorViewModel
    {
        public int Id { get; set; }
        public virtual IEnumerable<SelectListItem> Tutors { get; set; }    
        public virtual IEnumerable<SelectListItem> Students { get; set; }

        [DisplayName("Associate Tutor")]
        public ApplicationUser Tutor { get; set; }
        public Student Student { get; set; }          
    }
}