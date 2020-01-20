using System.ComponentModel;

namespace MVC5_Seneca.EntityModels
{
    public class Staff
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
      
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Work Phone")]
        public string WorkPhone { get; set; }

        [DisplayName("Cell Phone")]
        public string CellPhone { get; set;  }

        [DisplayName("Email")]
        public string Email { get; set; } 

    }
}