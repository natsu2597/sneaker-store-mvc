using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using SneakerStore.Services.Data;
using SneakerStore.Services.Extensions;
using SneakerStore.Services.Models;
using SneakerStore.Services.Repository;
using SneakerStore.Services.Seeders;
using SneakerStore.Services.Services;
using SneakerStore.Services.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    }

    );

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<UserSeeder>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


builder.Services.Configure<CloudinarySettings>(
        builder.Configuration.GetSection("Cloudinary")
    );

builder.Services.Configure<ApplicationSettings>(
        builder.Configuration.GetSection("Application")
    );

var emailSettings = builder.Configuration
    .GetSection("Email")
    .Get<EmailSettings>()
    ?? throw new InvalidOperationException("Email Configuration is missing");

builder.Services.AddSingleton(emailSettings);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDbAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
