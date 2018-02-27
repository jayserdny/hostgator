using Microsoft.AspNet.Identity.EntityFramework;

namespace MVC5_Seneca.EntityModels
{
    public class ApplicationRole :  IdentityRole
    {
        public ApplicationRole()
        { }
        public ApplicationRole(string name) : base(name) { }
    }
}