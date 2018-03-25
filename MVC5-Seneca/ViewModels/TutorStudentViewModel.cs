using System.Collections.Generic;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class TutorStudentViewModel
    {
        public ApplicationUser Tutor { get; set; }
        public List<Student> Students { get; set; }
    }
}