using WzWeb.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WzWeb.Server.Services;

namespace WzWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapleController : ControllerBase
    {
        private readonly IWzLoader wzLoader;
        private readonly ILogger<MapleController> logger;

        public MapleController(ILogger<MapleController> logger, IWzLoader wzLoader)
        {
            this.logger = logger;
            this.wzLoader = wzLoader;
        }

        [HttpGet]
        public Node Get()
        {
            return wzLoader.OutPutNode(wzLoader.HeadNode);
        }
    }
}
