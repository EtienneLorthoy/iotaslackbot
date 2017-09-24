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
using FluentScheduler;
using IOTA.Slackbot.Engine.Jobs;
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

            services.AddTransient<ISlackApiClient, SlackApiClient>();
            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddTransient<ITransactionManager, TransactionManager>();
            services.AddTransient<IIotaManager, IotaManager>();
            services.AddTransient<IUniqueIndexRepository, UniqueIndexRepository>();

            JobManager.Initialize(new JobRegistry());
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

    public class JobRegistry : Registry
    {
        public JobRegistry()
        {
            // Startup Jobs 
            Schedule<ScheduleCheckTransactionStartupJob>().ToRunOnceIn(5).Seconds();

            // Recurring Jobs
            Schedule<CheckTransactionsJob>().ToRunNow().AndEvery(5).Minutes();
        }
    }
}
