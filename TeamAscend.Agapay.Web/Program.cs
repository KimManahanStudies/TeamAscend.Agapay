using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
            .AddRazorRuntimeCompilation()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Disables camelCase
            });

// Add Memory Cache
builder.Services.AddMemoryCache();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Enable CORS
app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthorization();

//.NET 9 Approach
//app.MapStaticAssets();

//Legacy Approach to load static HTML Files
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//.WithStaticAssets();//.NET 9 Approach


app.Run();
