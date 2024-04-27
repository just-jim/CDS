using Amazon.Runtime;
using Amazon.SQS;
using Mock.Order.Controllers;
using Mock.Order.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<OrderController>();

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

app.MapPost("/orders/original", async() => {
    var order = OrderController.Original();
    await publisher.Publish("orders", order);
    return Results.Ok(order);
});

app.MapPost("/orders/random", async(OrderController controller) => {
    var order = controller.Random();
    await publisher.Publish("orders", order);
    return Results.Ok(order);
});

app.Run();