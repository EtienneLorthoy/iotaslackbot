using System;
using Microsoft.AspNetCore.Mvc;

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
    }
}
