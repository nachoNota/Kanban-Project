using tl2_proyecto_2024_nachoNota.Database;
using tl2_proyecto_2024_nachoNota.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string cadenaConexion = builder.Configuration.GetConnectionString("MySqlConnection")!.ToString();
if (cadenaConexion is null)
{
    Console.WriteLine("La cadena de conexión no se encuentra.");
}
else
{
    Console.WriteLine("Cadena de conexión encontrada: " + cadenaConexion);
}

builder.Services.AddSingleton<string>(cadenaConexion);

builder.Services.AddSingleton<IConnectionProvider, MySqlConnectionProvider>();
builder.Services.AddSingleton<ICommandFactory, MySqlCommandFactory>();

builder.Services.AddScoped<ITableroRepository, TableroRepository>();
builder.Services.AddScoped<ITareaRepository, TareaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITablaRepository, TablaRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
