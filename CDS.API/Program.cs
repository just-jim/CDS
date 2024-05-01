using CDS.API.Controllers;
using CDS.Application;
using CDS.Infrastructure;
using CDS.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApi()
        .AddApplication()
        .AddInfrastructure(builder.Configuration,builder.Host);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    // Run the database migrations
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<CdsDbContext>();
        dbContext.Database.Migrate();
    }

    app.MapControllers();

    app.Run();
}

