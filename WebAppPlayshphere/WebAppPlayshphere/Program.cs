using Microsoft.AspNetCore.SignalR;
using WebAppPlayshphere.Hubs;
using WebAppPlayshphere.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR(); // singal r ( per la chat)
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // servizio per i controller MVC
builder.Services.AddEndpointsApiExplorer(); // necessario per Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Usato per memorizzare i dati della sessione in memoria
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo di inattività prima della scadenza della sessione
    options.Cookie.HttpOnly = true; // Impedisce l'accesso ai cookie da JavaScript
    options.Cookie.IsEssential = true; // Necessario per il GDPR
});

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
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "-1";
    await next();
});

app.MapHub<ChatHub>("/chathub"); // 

//app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // Mappa i controller API



//app.MapRazorPages();
app.MapControllerRoute(name: "dafault", pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();