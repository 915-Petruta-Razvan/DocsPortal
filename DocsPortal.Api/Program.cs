using Azure.Storage.Blobs;
using DocsPortal.BLL.Context;
using DocsPortal.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Caching.Distributed;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StorageContext>(opts =>
    opts.UseSqlServer(
        builder.Configuration.GetConnectionString("database"),
        sqlOpts => sqlOpts.EnableRetryOnFailure()
    )
);

builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = builder.Configuration.GetConnectionString("redis");
    opts.InstanceName = "DocsPortalCache:";
});

builder.Services.AddAzureClients(clients =>
{
    clients.AddBlobServiceClient(builder.Configuration.GetConnectionString("blobs"));
});

builder.Services.AddScoped<BLContext>(provider =>
    new BLContext(
        provider.GetRequiredService<DALContext>(),
        provider.GetRequiredService<IDistributedCache>(),
        provider.GetRequiredService<BlobServiceClient>()
    )
);
builder.Services.AddScoped<DALContext>();

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DocsPortal API", Version = "v1" });
});

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocsPortal API v1"));
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        StorageContext db = scope.ServiceProvider.GetRequiredService<StorageContext>();
        logger.LogInformation("Applying database migrations...");
        db.Database.Migrate();
        logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while applying database migrations.");
    }
}


app.Run();
