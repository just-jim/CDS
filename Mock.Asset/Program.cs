using Mock.Asset.Models;
using Mock.Asset.Repositories;
using Mock.Asset.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AssetRepository>();
builder.Services.AddSingleton<ISqsController, AwsSqsController>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/assets", (AssetRepository repo) => {
    List<Asset> assets = repo.GetAll();
    return Results.Ok(assets);
});

app.MapPost("/assets/{id}", async (AssetRepository repo, ISqsController publisher, string id) => {
    var asset = repo.GetById(id);

    if (asset == null) {
        return Results.NotFound($"The asset with id {id} doesn't exist");
    }
    
    await publisher.Publish(configuration["SqsQueueName"]!, asset);
    app.Logger.LogInformation($"Asset {id} was published in the assets queue");
    return Results.Ok(asset);
});

app.Run();
