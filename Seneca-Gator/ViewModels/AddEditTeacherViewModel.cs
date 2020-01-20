using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class AddEditTeacherViewModel
    {
        public int Id { get; set; }

        // Input fields needed by the Create and Edit .cshtml files
        public virtual IEnumerable<SelectListItem> Schools { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        public School School { get; set; }

        [DisplayName("Work Phone")]
        public string WorkPhone { get; set; }

        [DisplayName("Cell Phone")]
        public string CellPhone { get; set; }

        public string Email { get; set; }
    }
}
