using Mock.Content.Distribution.Controllers;
using Mock.Content.Distribution.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ContentDistributionController>();
builder.Services.AddSingleton<ISqsController, AwsSqsController>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/content-distributions/original", async(ISqsController publisher) => {
    var contentDistribution = ContentDistributionController.Original();
    await publisher.Publish("contentDistributions", contentDistribution);
    app.Logger.LogInformation($"Content distribution for the date {contentDistribution.DistributionDate} was published in the contentDistributions queue");
    return Results.Ok(contentDistribution);
});

app.Run();