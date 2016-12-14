using Microsoft.AspNet.Identity.EntityFramework;
using Solutions.Models;

namespace Solutions.Controllers.Admin
{
    internal class ApplicationUserManager<T>
    {
        private UserStore<ApplicationUser> userStore;

        public ApplicationUserManager(UserStore<ApplicationUser> userStore)
        {
            this.userStore = userStore;
        }
    }
}