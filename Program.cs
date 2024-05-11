using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using T3_RevillaFernandezHectorRolando.Data;
using T3_RevillaFernandezHectorRolando.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<T3_RevillaFernandezHectorRolandoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("T3_RevillaFernandezHectorRolandoContext") ?? throw new InvalidOperationException("Connection string 'T3_RevillaFernandezHectorRolandoContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEmpleadoService,EmpleadoServiceImpl>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

app.Run();
