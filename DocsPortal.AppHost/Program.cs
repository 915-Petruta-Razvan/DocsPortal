using Microsoft.Identity.Client.Extensions.Msal;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<SqlServerServerResource> sql = builder
    .AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithContainerName("docsportal-sql");
IResourceBuilder<SqlServerDatabaseResource> db = sql.AddDatabase("database");

IResourceBuilder<RedisResource> redis = builder
    .AddRedis("redis")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithContainerName("docsportal-redis");

var azureStorage = builder
    .AddAzureStorage("azure-storage")
    .RunAsEmulator();

var blobs = azureStorage.AddBlobs("blobs");

builder
    .AddProject<Projects.DocsPortal_Api>("docsportal-api")
    .WithReference(db)
    .WithReference(redis)
    .WithReference(blobs)
    .WaitFor(db)
    .WaitFor(redis)
    .WaitFor(blobs);

builder.Build().Run();
