using Lanstaller_Shared;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
var extensionProvider = new FileExtensionContentTypeProvider();
extensionProvider.Mappings.Add(".dll", "application/octet-stream");


//static files warning: folder does not auto generate when empty.

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "StaticFiles")),
    RequestPath = "/StaticFiles",
    ContentTypeProvider = extensionProvider

});


app.UseAuthorization();

app.MapControllers();


SoftwareClass.ConnectionString = app.Configuration.GetValue<string>("ConnectionString");

app.Run();
