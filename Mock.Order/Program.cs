using Mock.Order.Controllers;
using Mock.Order.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<OrderController>();
builder.Services.AddSingleton<ISqsController, AwsSqsController>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/orders/original", async(ISqsController publisher) => {
    var order = OrderController.Original();
    await publisher.Publish("orders", order);
    app.Logger.LogInformation($"Order {order.OrderNumber} was published in the orders queue");
    return Results.Ok(order);
});

app.MapPost("/orders/random", async(OrderController controller, ISqsController publisher) => {
    var order = controller.Random();
    await publisher.Publish("orders", order);
    app.Logger.LogInformation($"Order {order.OrderNumber} was published in the orders queue");
    return Results.Ok(order);
});

app.Run();