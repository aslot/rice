using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Data;
using System.Linq;
using Microsoft.AspNetCore.ResponseCompression;
using MarkupHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TotalForumDbContext>();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();

    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "image/svg+xml", "text/html", "text/css", "text/javascript",
        "image/x-icon", "text/plain", "image/gif", "image/jpeg" });
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});
builder.Services.AddSingleton<EndPointMarkupHandler>();
builder.Services.AddSingleton<ForumMarkupHandler>();
builder.Services.AddSingleton<LoginMarkupHandler>();
builder.Services.AddSingleton<CaptchaMarkupHandler>();
builder.Services.AddSingleton<NewPrivateDialogMarkupHandler>();
builder.Services.AddSingleton<NewPrivateMessageMarkupHandler>();
builder.Services.AddSingleton<NewTopicMarkupHandler>();
builder.Services.AddSingleton<PrivateDialogMarkupHandler>();
builder.Services.AddSingleton<PrivateMessageMarkupHandler>();
builder.Services.AddSingleton<RegistrationMarkupHandler>();
builder.Services.AddSingleton<ReplyMarkupHandler>();
builder.Services.AddSingleton<SectionMarkupHandler>();
builder.Services.AddSingleton<ThreadMarkupHandler>();
builder.Services.AddSingleton<ProfileMarkupHandler>();
builder.Services.AddSingleton<IStorage, Storage>();
builder.Services.AddSingleton<IMemory, Memory>();
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddSingleton<IAccountLogic, AccountLogic>();
builder.Services.AddSingleton<IPrivateDialogLogic, PrivateDialogLogic>();
builder.Services.AddSingleton<IPrivateMessageLogic, PrivateMessageLogic>();
builder.Services.AddSingleton<IEndPointLogic, EndPointLogic>();
builder.Services.AddSingleton<IForumLogic, ForumLogic>();
builder.Services.AddSingleton<INewPrivateDialogLogic, NewPrivateDialogLogic>();
builder.Services.AddSingleton<INewPrivateMessageLogic, NewPrivateMessageLogic>();
builder.Services.AddSingleton<ISectionLogic, SectionLogic>();
builder.Services.AddSingleton<INewTopicLogic, NewTopicLogic>();
builder.Services.AddSingleton<IThreadLogic, ThreadLogic>();
builder.Services.AddSingleton<IReplyLogic, ReplyLogic>();
builder.Services.AddSingleton<IRegistrationLogic, RegistrationLogic>();
builder.Services.AddSingleton<ILoginLogic, LoginLogic>();
builder.Services.AddSingleton<ICaptchaGenerator, CaptchaGenerator>();
builder.Services.AddSingleton<Captcha>();
builder.Services.AddSingleton<IAuthenticationLogic, AuthenticationLogic>();
builder.Services.AddSingleton<IProfileLogic, ProfileLogic>();
builder.Services.AddSingleton<IFriendlyFire, FriendlyFire>();

var app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();
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

app.Run();
