using Microsoft.AspNetCore.Authentication.Cookies;
using SistemaILP.comisariato.Servicios;
using SistemaILP.comisariato.Servicios.Finanzas;
using SistemaILP.comisariato.Servicios.MercadeoVentas;
using SistemaILP.comisariato.Servicios.Operaciones;
using SistemaILP.comisariato.Servicios.Reportes;
using SistemaILP.comisariato.Servicios.Sistemas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    // Mantener las rutas por defecto
    options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");

    // Agregar las rutas personalizadas
    options.ViewLocationFormats.Add("/Views/Areas/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Areas/Finanzas/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Areas/MercadeoVentas/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Areas/Operaciones/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Areas/Reportes/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Areas/Sistemas/{1}/{0}.cshtml");
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Auth/Login";
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDatosDtoService, DatosDtoService>();
builder.Services.AddScoped<IEncryptService, EncryptService>();
builder.Services.AddScoped<IPermisosService, PermisosService>();
builder.Services.AddScoped<IBreadcrumbService, BreadcrumbService>();

builder.Services.AddTransient<IRepositorioComisariatos, RepositorioComisariatos>();
builder.Services.AddTransient<IRepositorioProgramas, RepositorioProgramas>();
builder.Services.AddTransient<IRepositorioRoles, RepositorioRoles>();
builder.Services.AddTransient<IRepositorioUsuario, RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioEmpleado, RepositorioEmpleados>();
builder.Services.AddTransient<IRepositorioProducto, RepositorioProductos>();
builder.Services.AddTransient<IRepositorioExistencias, RepositorioExistencias>();
builder.Services.AddTransient<IRepositorioFacturas, RepositorioFacturas>();
builder.Services.AddTransient<IRepositorioReportes, RepositorioReportes >();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "modulos",
    pattern: "Areas/{module}/{controller=Home}/{action=Index}/{id?}");

// Configura Rotativa
IWebHostEnvironment env = app.Environment;

Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa");

app.Run();
