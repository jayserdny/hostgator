using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MVC5_Seneca.Models;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class AddEditParentViewModel
    {
        public int Id { get; set; } 
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
    }
}