using Microsoft.EntityFrameworkCore;
using tl2_proyecto_2024_nachoNota.Database;
using tl2_proyecto_2024_nachoNota.Filters;
using tl2_proyecto_2024_nachoNota.Models;
using tl2_proyecto_2024_nachoNota.Repositories;
using tl2_proyecto_2024_nachoNota.Services;

var builder = WebApplication.CreateBuilder(args);

// Habilitar servicios de sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(300); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Solo accesible desde HTTP, no JavaScript
    options.Cookie.IsEssential = true; // Necesario incluso si el usuario no acepta cookies
});

builder.Services.AddHttpContextAccessor();

// Add services to the container.
string cadenaConexion = builder.Configuration.GetConnectionString("MySqlConnection")!.ToString();

builder.Services.AddSingleton<string>(cadenaConexion);

builder.Services.AddSingleton<IConnectionProvider, MySqlConnectionProvider>();
builder.Services.AddSingleton<ICommandFactory, MySqlCommandFactory>();

builder.Services.AddScoped<ITableroRepository, TableroRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<KanbanContext>(options => 
            options.UseMySQL(builder.Configuration.GetConnectionString("MySqlConnection") ?? 
                            throw new Exception("Missing connection string")));    

var app = builder.Build();

app.UseSession();

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
    pattern: "{controller=Login}/{action=Index}/{id?}");


app.Run();
