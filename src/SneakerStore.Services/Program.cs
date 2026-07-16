using SneakerStore.Services.Data;
using SneakerStore.Services.Extensions;
using SneakerStore.Services.Services;
using SneakerStore.Services.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.Configure<CloudinarySettings>(
        builder.Configuration.GetSection("Cloudinary")
    );
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
