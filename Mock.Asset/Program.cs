using Amazon.Runtime;
using Amazon.SQS;
using Mock.Asset.Models;
using Mock.Asset.Repositories;
using Mock.Asset.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AssetRepository>();

var sqsClient = new AmazonSQSClient(
    new BasicAWSCredentials("ignore", "ignore"), 
    new AmazonSQSConfig{
    ServiceURL = "http://localhost.localstack.cloud:4566"
});
var publisher = new SqsController(sqsClient);

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/assets", (AssetRepository repo) => {
    List<Asset> assets = repo.GetAll();
    return Results.Ok(assets);
});

app.MapPost("/assets/{id}", async (AssetRepository repo, string id) => {
    var asset = repo.GetById(id);

    if (asset == null) {
        return Results.NotFound($"The asset with id {id} doesn't exist");
    }
    
    await publisher.Publish("assets", asset);
    return Results.Ok(asset);
});

app.Run();
