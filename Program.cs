using Board_Game_Manager.DAL;
using Board_Game_Manager.BLL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Board_Game_Manager {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BoardGameContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>() // Add support for roles
                .AddEntityFrameworkStores<BoardGameContext>();

            //repo
            builder.Services.AddScoped<PlayerRepository>();
            builder.Services.AddScoped<GameRepository>();
            builder.Services.AddScoped<TournamentRepository>();
            builder.Services.AddScoped<GameSessionRepository>();
            builder.Services.AddScoped<GameMasterRepository>();

            //service
            builder.Services.AddScoped<PlayerService>();
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<TournamentService>();
            builder.Services.AddScoped<GameSessionService>();
            builder.Services.AddScoped<GameMasterService>();

            var app = builder.Build();

            //seed roles and admin user here
            seedRolesAndAdminUserAsync(app.Services).GetAwaiter().GetResult();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();

            static async Task seedRolesAndAdminUserAsync(IServiceProvider serviceProvider) {
                using (IServiceScope scope = serviceProvider.CreateScope()) {
                    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    //define roles
                    string[] roles = { "Admin","GM", "Player" };
                    foreach (string role in roles) {
                        if (!await roleManager.RoleExistsAsync(role)) {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }
                    //create an admin user
                    IdentityUser admin = new IdentityUser {
                        UserName = "admin@123.com",
                        Email = "admin@123.com",
                        EmailConfirmed = true
                    };
                    if (await userManager.FindByEmailAsync(admin.Email) == null) {
                        await userManager.CreateAsync(admin, "AdminPassword123!");
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                    // GM
                    IdentityUser GM = new IdentityUser {
                        UserName = "gm@123.com",
                        Email = "gm@123.com",
                        EmailConfirmed = true
                    };
                    if (await userManager.FindByEmailAsync(GM.Email) == null) {
                        await userManager.CreateAsync(GM, "gmPassword123!");
                        await userManager.AddToRoleAsync(GM, "GM");
                    }
                    // player
                    IdentityUser player = new IdentityUser {
                        UserName = "player@123.com",
                        Email = "player@123.com",
                        EmailConfirmed = true
                    };
                    if (await userManager.FindByEmailAsync(player.Email) == null) {
                        await userManager.CreateAsync(player, "playerPassword123!");
                        await userManager.AddToRoleAsync(player, "Player");
                    }
                }
            }
        }
    }
}
