using CatalogServiceWebApp;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
/*builder.Logging.AddApplicationInsights(
        configureTelemetryConfiguration: (config) =>
            config.ConnectionString = builder.Configuration.GetConnectionString("APPLICATIONINSIGHTS_CONNECTION_STRING"),
            configureApplicationInsightsLoggerOptions: (options) => { }
    );
*/
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Service");
    c.OAuthClientId(builder.Configuration["SwaaggerAzureAD:ClientId"]);
    c.OAuthUsePkce();
//    c.OAuthScopes("Full access");
    c.OAuthScopeSeparator(" ");
});


app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();

