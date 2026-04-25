using UpcAiPc1.Application;
using UpcAiPc1.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();
var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
startupLogger.LogDebug("La aplicacion inicia en {EnvironmentName}.", app.Environment.EnvironmentName);

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
app.MapControllers();
app.Run();
