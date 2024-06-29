using Lanstaller_Shared;
using LanstallerAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

SoftwareClass.ConnectionString = Environment.GetEnvironmentVariable("DBSTRING");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "ApiKeyScheme";
    options.DefaultChallengeScheme = "ApiKeyScheme";
}).AddScheme<AuthenticationSchemeOptions, ApiKeyAuth>("ApiKeyScheme", options => { });


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Set newtonsoft as default json.
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//HTTPS redirect disabled.
//app.UseHttpsRedirection();

//Add MIME for DLL.
FileExtensionContentTypeProvider extensionProvider = new FileExtensionContentTypeProvider();
extensionProvider.Mappings.Add(".dll", "application/octet-stream");


//static files warning: folder does not auto generate when empty.

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
    RequestPath = "/StaticFiles",
    ContentTypeProvider = extensionProvider

});


//Generate tmp folder for client packages.
if (!Directory.Exists("./tmp"))
{
    Directory.CreateDirectory("./tmp");
}



app.UseAuthorization();

app.MapControllers();

app.Run();

Console.WriteLine("Lanstaller API Started");
