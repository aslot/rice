using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Inclusions;
using System.Linq;
using Microsoft.AspNetCore.ResponseCompression;
using App.Controllers;
using System.Threading.Tasks;
using System.IO.Compression;
var builder = WebApplication.CreateBuilder();
var initializer = Task.Run(() => FriendlyFire.InitializeVoid(builder.Configuration[
#if DEBUG
"TestConnectionString"
#elif RELEASE
"ProductionConnectionString"
#endif
]));
builder.Services.AddDbContext<TotalForumDbContext>();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "text/html", "text/css", "text/javascript",
        "image/x-icon", "text/plain", "image/jpeg" });
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});
var app = builder.Build();

#if RELEASE
app.UseHsts();
app.UseHttpsRedirection();
#endif

app.UseResponseCompression();
app.UseStaticFiles();
app.UseMvc(routes =>
{
    routes.MapRoute(
                      name: "",
                      template: "{action}/{i?}",
                      defaults: new { controller = "f", action = "t" }
                   );
});

await initializer;

await app.RunAsync();

