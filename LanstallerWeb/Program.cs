using Lanstaller_Shared;
using LanstallerWeb;
using LanstallerWeb.Components;

LanstallerShared.ConnectionString = Environment.GetEnvironmentVariable("DBSTRING");
if (string.IsNullOrEmpty(LanstallerShared.ConnectionString))
{
    LanstallerShared.ConnectionString = "Data Source=192.168.88.3,1433;Initial Catalog=lanstaller;Integrated Security = true";
}

LanstallerWebSettings.serverAddress = Environment.GetEnvironmentVariable("ADDRESS");
if (string.IsNullOrEmpty(LanstallerWebSettings.serverAddress))
{
    LanstallerWebSettings.serverAddress = "localhost:5236";
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//add controllers.
builder.Services.AddControllers();


//Set newtonsoft as default json.
//builder.Services.AddControllers().AddNewtonsoftJson();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();


Maintenance.CleanupTemp();

app.Run();
