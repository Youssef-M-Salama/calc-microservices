using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HistoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1);
    options.ListenAnyIP(5001, o => o.Protocols = HttpProtocols.Http2);
});

var app = builder.Build();

app.MapGrpcService<HistoryGrpcService>();
app.Use(async (context, next) =>
{
    if (context.Request.ContentType != null &&
        context.Request.ContentType.Contains("application/grpc"))
    {
        return;
    }

    if (context.Request.Headers["X-Source"] != "api-gateway")
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Forbidden");
        return;
    }

    await next();
});


app.MapGet("/history", async (HistoryDbContext db) =>
{
    return await db.CalculationResults
                   .OrderByDescending(x => x.CreatedAt)
                   .Select(x => x.Result)
                   .FirstOrDefaultAsync();
});

app.MapGet("/", () => "Server is running");

app.Run();