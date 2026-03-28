using Azure.Identity;
using Azure.Storage.Blobs;
using FileUploader.Models;
using FileUploader.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(_ =>
{
    var config = new BlobStorageConfig();
    builder.Configuration.GetSection("Cloud:BlobStorage").Bind(config);
    var blobStorageClient = new BlobServiceClient(config.Url);
    return blobStorageClient.GetBlobContainerClient(config.ContainerName);
});

builder.Services.AddScoped<BlobService>();

var app = builder.Build();
    
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
