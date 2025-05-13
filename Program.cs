using Microsoft.EntityFrameworkCore;
using UserManagementApp.Data;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options => {
        options.LoginPath = "/Account/Login"; 
    });


builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


app.UseSession(); 

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();
