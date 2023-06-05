using CatalogServiceWebApp;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog Service");
    c.OAuthClientId(builder.Configuration["SwaaggerAzureAD:ClientId"]);
    c.OAuthUsePkce();
    c.OAuthScopeSeparator(" ");
});


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();

