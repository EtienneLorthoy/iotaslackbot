using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentScheduler;
using IOTA.Slackbot.Engine.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace IOTA.Slackbot.Engine
{
    public class JobRegistry : Registry
    {
        private readonly IServiceCollection _serviceCollection;

        public JobRegistry(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
            // Startup Jobs 
            RegisterAndSchedule<ScheduleCheckTransactionStartupJob>().ToRunOnceIn(5).Seconds();

            // Recurring Jobs

            // One time jobs
            Register<CheckTransactionsJob>();
        }

        private Schedule RegisterAndSchedule<T>() where T : class, IJob
        {
            _serviceCollection.AddTransient<T>();
            return Schedule<T>();
        }

        private void Register<T>() where T : class, IJob
        {
            _serviceCollection.AddTransient<T>();
            Schedule<T>().Disable();
        }
    }

    public class UnityJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UnityJobFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IJob GetJobInstance<T>() where T : IJob
        {
            return this._serviceProvider.GetRequiredService<T>();
        }
    }
}
