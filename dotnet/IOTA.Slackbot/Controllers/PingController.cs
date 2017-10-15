using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FluentScheduler;

namespace IOTA.Slackbot.Controllers
{
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Pong...";
        }

        [HttpGet("JobStatus")]
        public IEnumerable<string> JobStatus()
        {
            var result = new List<string> {"* All Schedules *"};
            result.AddRange(JobManager.AllSchedules.Select(x => $"{x.Name}:{x.NextRun}"));

            result.Add("* RunningSchedules *");
            result.AddRange(JobManager.RunningSchedules.Select(x => $"{x.Name}:{x.NextRun}"));

            return result;
        }
    }
}
