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

        [HttpPost("GetFileInfo")]
        public FileInfo GetFileInfo(Node node)
        {

            var wz_Node = wzLoader.HeadNode.FindNodeByPath(node.FullPath);
            if (wz_Node == null)
            {
                if (wzLoader.HeadNode.FullPath == node.FullPath)
                {
                    wz_Node = wzLoader.HeadNode;
                }
            }
            var wz_File = wz_Node.Value as Wz_File;
            return wz_File.GetFileInfo();
        }
    }
}
