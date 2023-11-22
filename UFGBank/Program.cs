using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UFGBank.Areas.Identity.Data;
using UFGBank.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UFGBankDbContextConnection") ?? throw new InvalidOperationException("Connection string 'UFGBankDbContextConnection' not found.");

builder.Services.AddDbContext<UFGBankDbContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddIdentity<UFGBankUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UFGBankDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//seed roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UFGBankUser>>();
    string email = "admin@admin.com";
    string password = "P32ADdj1ejj2#!";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new UFGBankUser();
        user.UserName = email;
        user.FirstName = "Admin";
        user.LastName = "Admin";
        user.Email = email;
        
        await userManager.CreateAsync(user, password);
        
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();