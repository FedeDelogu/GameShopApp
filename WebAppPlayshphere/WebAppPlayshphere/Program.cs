using Microsoft.AspNetCore.SignalR;
using Utility;
using WebAppPlayshphere.DAO;
using WebAppPlayshphere.Settings;

var builder = WebApplication.CreateBuilder(args);
// Carica le configurazioni da appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabase, Database>(); // Iniezione del database

builder.Services.AddTransient<IDAO, DAOAnagrafica>(); // DAOCarrello 
builder.Services.AddTransient<IDAO, DAOCarrello>(); // DAOVideogioco
builder.Services.AddTransient<IDAO, DAOOrdine>();
builder.Services.AddTransient<IDAO, DAORecensione>();
builder.Services.AddTransient<IDAO, DAOUtente>();
builder.Services.AddTransient<IDAO, DAOVideogioco>();


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // servizio per i controller MVC
builder.Services.AddEndpointsApiExplorer(); // necessario per Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

var app = builder.Build();
; // aggiunge Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Recensioni}/{action=Elenco}/{id?}");

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // Mappa i controller API

app.MapControllerRoute(name: "default", pattern: "{controller=Utenti}/{action=Registrazione}/{id?}");

app.Run();