using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using SportEventsApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(SportEventsApp.Startup))]

namespace SportEventsApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            app.UseCors(CorsOptions.AllowAll);
            OAuthAuthorizationServerOptions option = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(90),
                AllowInsecureHttp = true
            };
            app.UseOAuthAuthorizationServer(option);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));



            // In Startup iam creating first Admin Role and creating a default Admin User 
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website				
                var user = new ApplicationUser();
                user.UserName = "mamdouh";
                user.Email = "mamdouhahmed633@yahoo.com";
                string userPWD = "A7Ayash3b56789";
                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }
            if (!roleManager.RoleExists("Player"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Player";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("StoreManager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "StoreManager";
                roleManager.Create(role);
            }

            context.SaveChanges();

        }
    }
}
