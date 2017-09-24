using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IOTA.Slackbot.Slack;
using IOTA.Slackbot.Wallet;
using IOTA.Slackbot.Engine;
using IOTA.Slackbot.Iota;
using IOTA.Slackbot.Iota.Commands;
using IOTA.Slackbot.Iota.Repositories;

namespace IOTA.Slackbot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<IotaBotSettings>(Configuration.GetSection("IotaBotSettings"));

            // Repositories
            services.AddScoped<IWalletRepository>(x => new WalletRepository());
            services.AddScoped<IUniqueIndexRepository>(x => new UniqueIndexRepository());

            // Services
            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped<IIotaManager, IotaManager>();
            services.AddTransient<ISlackApiClient, SlackApiClient>();

            // Command
            services.AddTransient<GetNextDepositAddressCommand>();
            
            Engine.Startup.Initialization(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            Configuration = builder.Build();

            app.UseMvc();
        }
    }
}
