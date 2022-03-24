using CollectIt.MVC.Account.IdentityEntities;
using CollectIt.MVC.Account.Infrastructure.Data;
using CollectIt.MVC.Resources.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = "/account/login";
    options.DefaultSignOutScheme = "/account/logout";
}).AddCookie(options =>
{
    options.Cookie.Name = "Cookie";
});
builder.Services.AddAuthorization();
builder.Services.AddAuthorization();
builder.Services.AddDbContext<PostgresqlIdentityDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings:Accounts:PostgresqlDevelopment"],
                      config =>
                      {
                          config.MigrationsAssembly("CollectIt.MVC.View");
                      });
});
builder.Services.AddIdentity<User, Role>(config =>
        {
            config.User = new UserOptions
                          {
                              RequireUniqueEmail = true,
                          };
            config.Password = new PasswordOptions
                              {
                                  RequireDigit = true,
                                  RequiredLength = 6,
                                  RequireLowercase = false,
                                  RequireUppercase = false,
                                  RequiredUniqueChars = 1,
                                  RequireNonAlphanumeric = false,
                              };
            config.SignIn = new SignInOptions
                            {
                                RequireConfirmedEmail = false,
                                RequireConfirmedAccount = false,
                                RequireConfirmedPhoneNumber = false,
                            };
        })
       .AddEntityFrameworkStores<PostgresqlIdentityDbContext>()
       .AddUserManager<UserManager>()
       .AddDefaultTokenProviders();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();