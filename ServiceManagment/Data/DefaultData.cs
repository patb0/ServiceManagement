using Microsoft.AspNetCore.Identity;
using ServiceManagment.Models;

namespace ServiceManagment.Data
{
    public class DefaultData
    {
        public static async Task AdminAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Admin worker role and worker role
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(WorkerRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(WorkerRoles.Admin));
                if (!await roleManager.RoleExistsAsync(WorkerRoles.Worker))
                    await roleManager.CreateAsync(new IdentityRole(WorkerRoles.Worker));

                //Admin
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Worker>>();
                string adminUserEmail = "patbog@mail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                
                if (adminUser == null)
                {
                    var newAdminWorker = new Worker()
                    {
                        UserName = adminUserEmail,
                        Email = adminUserEmail,
                        Name = "Patryk Boguslawski",
                        CreatedAt = DateTime.Now,
                        PhoneNumber = "384234889",
                    };

                    await userManager.CreateAsync(newAdminWorker, "zaq1@WSX");
                    await userManager.AddToRoleAsync(newAdminWorker, WorkerRoles.Admin);
                }
            }
        }
    }
}
