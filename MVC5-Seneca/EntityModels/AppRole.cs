using Microsoft.AspNet.Identity.EntityFramework;

namespace MVC5_Seneca.EntityModels
{
    public class ApplicationRole :  IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
    }
}