using CDB.Controllers;
using CDB.Data;
using Microsoft.EntityFrameworkCore;
using CDB.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using CDB.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DrawingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CDB")));
builder.Services.AddSignalR();
builder.Services.AddScoped<DrawingService>();
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
app.MapHub<DrawingHub>($"/Board");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
