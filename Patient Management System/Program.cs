using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Patient_Management_System.Data;
using Patient_Management_System.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PMSContext") ?? throw new InvalidOperationException("Connection string 'PMSContext' not found.")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<PMSContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
