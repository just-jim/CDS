using Mock.Briefing.Models;
using Mock.Briefing.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<BriefingRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/briefings", (BriefingRepository repo) => {
    List<Briefing> assets = repo.GetAll();
    return Results.Ok(assets);
});

app.MapGet("/briefings/{name}", (BriefingRepository repo, string name) => {
    var asset = repo.GetByName(name);

    if (asset != null) {
        return Results.Ok(asset);
    }
    
    var error = new {
        error = "Briefing Not Found",
        message = $"The briefing with name '{name}' was not found"
    };
    return Results.NotFound(error);
});

app.Run();
