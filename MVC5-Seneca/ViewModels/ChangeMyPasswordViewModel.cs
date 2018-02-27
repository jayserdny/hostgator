using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC5_Seneca.ViewModels
{
    public class ChangeMyPasswordViewModel
    {
        // Input fields needed by the .cshtml file to display the form


        // Output fields used by the .cshtml file to return form results

        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        [Required(ErrorMessage = "This field is required.")]
        public string NewPassword1 { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Re-enter New Password")]
        [Required(ErrorMessage = "This field is required.")]
        public string NewPassword2 { get; set; }          
        public string ErrorMessage { get; set; }
    }
}