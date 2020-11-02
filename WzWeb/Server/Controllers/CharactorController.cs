using WzWeb.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WzWeb.Server.Services;
using WzLib;
using WzWeb.Server.Extentions;

namespace WzWeb.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactorController : Controller
    {

        private readonly IWzLoader wzLoader;
        private readonly ILogger<CharactorController> logger;


        public CharactorController(IWzLoader wzLoader, ILogger<CharactorController> logger)
        {
            this.logger = logger;
            this.wzLoader = wzLoader;
        }

        [HttpGet]
        public string Get()
        {
            return "value";
        }

    }
}
