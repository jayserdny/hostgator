using System.ComponentModel;

namespace MVC5_Seneca.EntityModels
{
    public class Teacher
    {
        public Teacher(string firstName)
        {
            FirstName = firstName; }

        public int Id { get; set; }

        [DisplayName ("Last Name")]
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