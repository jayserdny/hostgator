using Microsoft.AspNet.Identity.EntityFramework;

namespace SenecaHeights.EntityModels
{
    public class ApplicationRole :  IdentityRole
    {
        public ApplicationRole()
        { }
        public ApplicationRole(string name) : base(name) { }
    }
}