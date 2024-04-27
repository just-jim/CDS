using Amazon.Runtime;
using Amazon.SQS;
using CDS.Adapters.AssetDomainAdapter.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<HostOptions>(x => {
    x.ServicesStartConcurrently = true;
    x.ServicesStopConcurrently = false;
});

var amazonSqsClient = new AmazonSQSClient(
    new BasicAWSCredentials("ignore", "ignore"),
    new AmazonSQSConfig{
        ServiceURL = "http://localhost.localstack.cloud:4566"
    });
builder.Services.AddSingleton<IAmazonSQS>(_ => amazonSqsClient);
builder.Services.AddHostedService<AssetSqsConsumerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();