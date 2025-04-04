using Northwind.DataContext.SqlServer;
using Northwind.EntityModels;

#region Konfigurera web server host och tj�nster
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddNorthwindContext();
builder.Services.AddRequestDecompression(); // L�gger till compresson tj�nster (Accept-Encoding: gzip, deflate)


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
        //ifall att vi har ett match p� URL path, detta avbryter pipeline och skickar inte anrop till n�sta middleware.
        await context.Response.WriteAsync("Bonjour Monde!");
        return;
    }
    // Vi kan modifiera request och response h�r innan anrop g�r vidare.
    await next();
    //Vi kan modifiera response h�r efter att vi har kallat nest delegate och innan den skickas till klienten.
});

app.UseHttpsRedirection();
app.UseRequestDecompression(); // Anrop till att anv�nda decompression middleware i pipeline

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRazorPages();

app.MapGet("/hello", () => $"Environment is {app.Environment.EnvironmentName}");
#endregion

app.Run();
WriteLine("Detta exekiveras efter att web server har stoppats");
