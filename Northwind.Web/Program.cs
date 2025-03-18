using Northwind.DataContext.SqlServer;
using Northwind.EntityModels;

#region Konfigurera web server host och tjänster
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddNorthwindContext();
builder.Services.AddRequestDecompression(); // Lägger till compresson tjänster (Accept-Encoding: gzip, deflate)


var app = builder.Build();
#endregion

#region Konfiguration av http pipline och routing
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.Use(async (HttpContext context, Func<Task> next) =>
{
   RouteEndpoint? rep = context.GetEndpoint() as RouteEndpoint;
    if (rep is not null)
    {
        WriteLine($"Endpoint name: {rep.DisplayName}");
        WriteLine($"Endpoint route pattern: {rep.RoutePattern.RawText}");
    }

    if (context.Request.Path == "/bonjour")
    {
        //ifall att vi har ett match på URL path, detta avbryter pipeline och skickar inte anrop till nästa middleware.
        await context.Response.WriteAsync("Bonjour Monde!");
        return;
    }
    // Vi kan modifiera request och response här innan anrop går vidare.
    await next();
    //Vi kan modifiera response här efter att vi har kallat nest delegate och innan den skickas till klienten.
});

app.UseHttpsRedirection();
app.UseRequestDecompression(); // Anrop till att använda decompression middleware i pipeline

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRazorPages();

app.MapGet("/hello", () => $"Environment is {app.Environment.EnvironmentName}");
#endregion

app.Run();
WriteLine("Detta exekiveras efter att web server har stoppats");
