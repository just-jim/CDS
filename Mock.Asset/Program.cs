using Mock.Asset.Models;
using Mock.Asset.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AssetRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/assets", (AssetRepository repo) => {
    List<Asset> assets = repo.GetAll();
    return Results.Ok(assets);
});

app.MapPost("/assets/{id}", (AssetRepository repo, string id) => {
    var asset = repo.GetById(id);
    return Results.Ok(asset);
});

app.Run();
