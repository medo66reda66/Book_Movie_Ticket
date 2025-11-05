using Book_Movie_Ticket.data;
using Book_Movie_Ticket.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Book_Movie_Ticket.Utilities
{
    public class DBInitializer : IBBInitializer
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DBInitializer> _logger;

        public DBInitializer(ApplicationDBContext dbContext, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager, ILogger<DBInitializer> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public void DBInitializ()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                    _dbContext.Database.Migrate();
                if(_roleManager.Roles.IsNullOrEmpty())
                {
                    _roleManager.CreateAsync(new(SD.SUPER_ADMIN_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.ADMIN_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.EMPLOYEE_ROLE)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new(SD.CUSTOMER_ROLE)).GetAwaiter().GetResult();

                    _userManager.CreateAsync(new ApplicationUser()
                    {
                        Email = "SuperAdmin@gmail.com",
                        UserName = "Supername",
                        EmailConfirmed = true,
                        Firestname = "Super",
                        Lasttname = "Admin",
                    }, "Admin123").GetAwaiter().GetResult();

                    var user = _userManager.FindByNameAsync("Supername").GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(user, SD.SUPER_ADMIN_ROLE).GetAwaiter().GetResult();
                }
            }catch(Exception ex)
            {
                _logger.LogError($"Error : {ex.Message}");
            }

        }
    }
}
