using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC5_Seneca.EntityModels
{        
    public class Login
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
    }
}