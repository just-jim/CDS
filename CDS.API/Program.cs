using Amazon.Runtime;
using Amazon.SQS;
using CDS.Adapters.AssetDomainAdapter.Consumers;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set up the AWS sqs client
var amazonSqsClient = new AmazonSQSClient(
    new BasicAWSCredentials("ignore", "ignore"),
    new AmazonSQSConfig{ ServiceURL = configuration["LocalStackHost"] }
 );
builder.Services.AddSingleton<IAmazonSQS>(_ => amazonSqsClient);

// To allow polling within IHostedServices we need to allow concurrent running of services
builder.Services.Configure<HostOptions>(x => {
    x.ServicesStartConcurrently = true;
    x.ServicesStopConcurrently = false;
});

// Add the sqs consumers to consume messages from the external domains
builder.Services.AddHostedService<AssetSqsConsumerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();