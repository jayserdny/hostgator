using System.Collections.Generic;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.ViewModels
{
    public class UpdateMyScheduleViewModel
    {  
        public string Id { get; set; }
        public ApplicationUser Tutor { get; set; }
        public Student  Student { get; set; }
        public string DayName { get; set; }
        public string TimeOfDay { get; set; }
       
        public virtual List<SelectListItem> Students { get; set; }
        public virtual List<Student> MyTutees { get; set; } 
    }
}