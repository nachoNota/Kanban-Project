using tl2_proyecto_2024_nachoNota.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var cadenaConexion = builder.Configuration.GetConnectionString("MySqlConnection")!.ToString();
builder.Services.AddSingleton<string>(cadenaConexion);

builder.Services.AddSingleton<IConnectionProvider, MySqlConnectionProvider>();
builder.Services.AddSingleton<ICommandFactory, MySqlCommandFactory>();

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
