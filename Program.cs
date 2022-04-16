using Collector.Services;
using NATS.Client;
using NATS.Client.JetStream;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

// Create a new connection factory to create
// a connection.
var cf = new ConnectionFactory();

// Creates a live connection to the default
// NATS Server running locally
var c = cf.CreateConnection();
var jetStream = c.CreateJetStreamContext();
builder.Services.AddTransient<IJetStream>(_ => jetStream);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CollectorService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
