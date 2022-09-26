using Microsoft.AspNetCore.Authentication.Cookies;
using MolinaInmobilaria.Repositorios;
using MolinaInmobilaria.Models;
var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();



    //builder.Services.AddScoped<IRepositorioUsuario, UsuarioRepositorio>();
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Usuario/Login";
            options.LogoutPath = "/Usuario/Logout";  
            options.AccessDeniedPath = "/Home/Prohibido";
        });
    builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("Empleado", policy => policy.RequireRole("Empleado"));
        });
  
    
    
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
