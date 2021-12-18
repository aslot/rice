using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Data;
using MarkupHandlers;

namespace App
{
    public class Startup
    {
        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            WebHostEnvironment = webHostEnvironment;
            ConfigurationSettings = configuration;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }
        public IConfiguration ConfigurationSettings { get; }

        public static string ConnectionString;

        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => {
            options.LoginPath = new PathString("/u/");            
        });*/
            //string path = WebHostEnvironment.WebRootPath.Replace('/', System.IO.Path.DirectorySeparatorChar);
            services.AddDbContext<TotalForumDbContext>(options => options.UseSqlServer(ConfigurationSettings["ConnectionStrings:DefaultConnection"]));

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();

    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml", "text/html" });
});

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest;
            });

            services.AddSingleton<IStorage, Storage>();
            services.AddSingleton<IMemory, Memory>();
            services.AddSingleton<IDatabase, Database>();
            services.AddSingleton<IFriendlyFire, FriendlyFire>();
            services.AddSingleton<IAccountLogic, AccountLogic>();
            services.AddSingleton<IPrivateDialogLogic, PrivateDialogLogic>();
            services.AddSingleton<IPrivateMessageLogic, PrivateMessageLogic>();
            services.AddSingleton<IEndPointLogic, EndPointLogic>();
            services.AddSingleton<IForumLogic, ForumLogic>();
            services.AddSingleton<INewPrivateDialogLogic, NewPrivateDialogLogic>();
            services.AddSingleton<INewPrivateMessageLogic, NewPrivateMessageLogic>();
            services.AddSingleton<ISectionLogic, SectionLogic>();
            services.AddSingleton<INewTopicLogic, NewTopicLogic>();
            services.AddSingleton<IThreadLogic, ThreadLogic>();
            services.AddSingleton<IReplyLogic, ReplyLogic>();
            services.AddSingleton<IRegistrationLogic, RegistrationLogic>();
            services.AddSingleton<ILoginLogic, LoginLogic>();
            services.AddSingleton<Captcha>();
            services.AddSingleton<IAuthenticationLogic, AuthenticationLogic>();
            services.AddSingleton<IProfileLogic, ProfileLogic>();
            services.AddSingleton<EndPointMarkupHandler>();
            services.AddSingleton<ForumMarkupHandler>();
            services.AddSingleton<LoginMarkupHandler>();
            services.AddSingleton<NewPrivateDialogMarkupHandler>();
            services.AddSingleton<NewPrivateMessageMarkupHandler>();
            services.AddSingleton<NewTopicMarkupHandler>();
            services.AddSingleton<PrivateDialogMarkupHandler>();
            services.AddSingleton<PrivateMessageMarkupHandler>();
            services.AddSingleton<RegistrationMarkupHandler>();
            services.AddSingleton<ReplyMarkupHandler>();
            services.AddSingleton<SectionMarkupHandler>();
            services.AddSingleton<ThreadMarkupHandler>();
            services.AddSingleton<ProfileMarkupHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
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
        }
    }
}
