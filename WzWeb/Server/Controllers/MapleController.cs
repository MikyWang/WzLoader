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
using System.Runtime.InteropServices.WindowsRuntime;

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

        [HttpGet("GetNode")]
        public Node Get(string path)
        {
            var head = wzLoader.HeadNode;
            var wz_node = head.SearchNode(path);
            return wz_node.ToNode();
        }

        [HttpPost("GetNodeList")]
        public IEnumerable<Node> GetNodeList(Node node)
        {
            var wz_Node = node.ToWzNode(wzLoader.HeadNode);
            return wz_Node.Nodes.Select(node => node.ToNode()).ToArray();
        }

        [HttpPost("GetFileInfo")]
        public MapleFileInfo GetFileInfo(Node node)
        {
            var wz_Node = node.ToWzNode(wzLoader.HeadNode);
            var wz_File = wz_Node.Value as Wz_File;
            return wz_File.GetFileInfo();
        }

        [HttpPost("GetPng")]
        public PngInfo GetPng(Node node)
        {
            var wz_Node = node.ToWzNode(wzLoader.HeadNode);
            var wz_png = wz_Node.GetValue<Wz_Png>();
            return wz_png.ToPngInfo();
        }

    }
}
