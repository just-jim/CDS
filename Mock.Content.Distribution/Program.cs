using Mock.Content.Distribution.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ContentDistributionController>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/content-distribution/cache", (ContentDistributionController controller) => {
    var contentDistribution = controller.Cache();
    return Results.Ok(contentDistribution);
});

app.Run();