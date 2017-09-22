﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IOTA.Slackbot.Slack;
using IOTA.Slackbot.Wallet;
using IOTA.Slackbot.Engine;
using IOTA.Slackbot.Iota;
using FluentScheduler;

namespace IOTA.Slackbot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<IotaBotSettings>(Configuration.GetSection("IotaBotSettings"));

            if (HostingEnvironment.IsDevelopment())
            {
                services.AddTransient<ISlackApiClient, FakeSlackApiClient>();
            }
            else
            {
                services.AddTransient<ISlackApiClient, SlackApiClient>();
                
            }
            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddTransient<ITransactionManager, TransactionManager>();
            services.AddTransient<IIotaManager, IotaManager>();

            JobManager.Initialize(new JobRegistry());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            HostingEnvironment = env;

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
            Schedule<CheckTransactionsJob>().ToRunNow().AndEvery(5).Minutes();
        }
    }
}
