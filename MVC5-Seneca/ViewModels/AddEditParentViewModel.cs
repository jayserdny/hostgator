using System.Collections.Generic;
using System.ComponentModel;
using MVC5_Seneca.EntityModels;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace MVC5_Seneca.ViewModels
{
    public class AddEditParentViewModel
    {
        public int Id { get; set; }

        public virtual IEnumerable<SelectListItem> StaffMembers { get; set; }

        public string FirstName { get; set; }
        public string MotherFather { get; set; }

        [DisplayName ("Mother / Father")]
        public string SelectedMotherFather { get; set; }
        public string Address { get; set; } 

        [DisplayName ("Home Phone")]
        [Phone]
        public string HomePhone { get; set; }

        [DisplayName("Cell Phone")]
        [Phone]
        public string CellPhone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Case Manager")]
        public virtual Staff StaffMember { get; set; }     
        public string ErrorMessage { get; set; }
    }
}