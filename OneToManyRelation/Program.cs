using Application;
using Infrastructure.CustomMiddleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog services to the container and read the configuration from appsettings.
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
// Add services to the container using the DependencyInjection class
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddControllersWithViews();
DependencyInjection.ConfigureServices(builder.Services, builder.Configuration);
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

// Add the custom error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure Serilog for logging.
app.UseSerilogRequestLogging(); // Adds request logging using Serilog.

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

try
{
    Log.Information("Starting web application"); // Logs a startup message.
    app.Run(); // Runs the application.
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly"); // Logs a fatal error if the application crashes.
}
finally
{
    Log.CloseAndFlush(); // It ensures all logs are flushed and closed properly.
}
