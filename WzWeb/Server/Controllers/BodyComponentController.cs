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
using WzWeb.Shared.Character;

namespace WzWeb.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BodyComponentController : Controller
    {

        private readonly IBodyComponetService bodyComponetService;

        public BodyComponentController(IBodyComponetService bodyComponetService)
        {
            this.bodyComponetService = bodyComponetService;
        }

        [HttpPost("GetBodyComponent")]
        public BodyComponent GetBodyComponent([FromBody] BodyComponent bodyComponent, [FromQuery] bool isDefault = true)
        {
            return bodyComponetService.GetBodyComponent(bodyComponent, isDefault);
        }

        [HttpPost("GetBodyComponentList")]
        public ListResponse<BodyComponent> GetBodyComponentList(ListRequest<BodyComponent> request)
        {
            return bodyComponetService.GetBodyComponentList(request);
        }

    }
}
