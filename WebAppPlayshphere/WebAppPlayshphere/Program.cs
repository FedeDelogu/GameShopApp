using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // servizio per i controller MVC
builder.Services.AddEndpointsApiExplorer(); // necessario per Swagger
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

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

//app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // Mappa i controller API



//app.MapRazorPages();
app.MapControllerRoute(name: "dafault", pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();