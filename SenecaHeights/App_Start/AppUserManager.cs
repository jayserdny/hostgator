using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SenecaHeights.DataAccessLayer;
using SenecaHeights.EntityModels;

namespace SenecaHeights.App_Start
{
    public class AppUserManager  : UserManager<ApplicationUser>
    {
        public AppUserManager(IUserStore<ApplicationUser> store)
        : base(store)
        {
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(
                new UserStore<ApplicationUser>(context.Get<SenecaContext>()));

            // optionally configure your manager
            // ...

            return manager;
        }
    }
}