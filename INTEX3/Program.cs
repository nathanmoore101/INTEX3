using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using INTEX3.Models;
using INTEX3.Data;
using INTEX3.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using INTEX3.Data.Repositories;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var conStrBuilder = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
conStrBuilder.Password = builder.Configuration["DBPassword"];
var connection = conStrBuilder.ConnectionString;

// Connection string from the first program.cs
var connectionString1 = builder.Configuration["ConnectionStrings:AZURE_SQL_CONNECTIONSTRING"];

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the DbContext from the first program.cs
builder.Services.AddDbContext<Intex2Context>(options =>
{
    options.UseSqlServer(connectionString1); // Assuming you're using SQL Server
});

// Register the DbContext from the second program.cs
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString1)); //put connection to azure

// Add repositories
builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IRecProductRepository, RecProductRepository>();


builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Authentication configuration from the second program.cs
services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
{
    microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
});

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    // Enable roles
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>() // Add this line to enable Role services
.AddEntityFrameworkStores<ApplicationDbContext>();

// Password settings from the second program.cs
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 10;
    options.Password.RequiredUniqueChars = 5;
});

var app = builder.Build();

// Assign roles to users
var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<IdentityUser>>();
var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();

// Create Roles
if (!await roleManager.RoleExistsAsync("admin"))
{
    var role = new IdentityRole("admin");
    await roleManager.CreateAsync(role);
}

if (!await roleManager.RoleExistsAsync("customer"))
{
    var role = new IdentityRole("customer");
    await roleManager.CreateAsync(role);
}

// Assign admin role to a user
var adminUser = await userManager.FindByEmailAsync("admin@email.com");
if (adminUser != null)
{
    await userManager.AddToRoleAsync(adminUser, "admin");
}

// Assign customer role to a user
var customerUser = await userManager.FindByEmailAsync("customer@email.com"); // Replace with the actual customer email
if (customerUser != null)
{
    await userManager.AddToRoleAsync(customerUser, "admin");
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enable HSTS
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Use Authentication before Authorization
app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; img-src 'self' https://*; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; font-src 'self' https://fonts.gstatic.com; frame-src https://*.googleapis.com");
    await next();
});


//additional security: if a view is under development...
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseCookiePolicy();

app.UseSession();

app.UseHsts(); // Enable HSTS


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomePage}/{id?}");
app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Home}/{action=AdminUsersPage}/{id?}");

app.MapRazorPages();

app.Run();
