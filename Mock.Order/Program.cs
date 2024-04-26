using Mock.Order.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<OrderController>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/orders/original", () => {
    var order = OrderController.Original();
    return Results.Ok(order);
});

app.MapPost("/orders/random", (OrderController controller) => {
    var order = controller.Random();
    return Results.Ok(order);
});

app.Run();