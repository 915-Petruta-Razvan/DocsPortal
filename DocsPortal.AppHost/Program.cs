IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<SqlServerServerResource> sql = builder
    .AddSqlServer("sql")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<SqlServerDatabaseResource> db = sql.AddDatabase("database");

builder
    .AddProject<Projects.DocsPortal_Api>("docsportal-api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
