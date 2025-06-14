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

builder
    .AddProject<Projects.DocsPortal_Api>("docsportal-api")
    .WithReference(db)
    .WithReference(redis)
    .WaitFor(db)
    .WaitFor(redis);

builder.Build().Run();
