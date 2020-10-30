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

        [HttpGet("GetNode")]
        public Node Get(string path)
        {
            var head = wzLoader.BaseNode;
            var wz_node = head.SearchNode(path);
            return wz_node.ToNode();
        }

        [HttpPost("GetNodeList")]
        public NodeListResponse GetNodeList(NodeListRequest req)
        {
            var node = req.Node;
            var wz_Node = node.ToWzNode(wzLoader.BaseNode);
            var resp = new NodeListResponse();
            resp.Nodes = wz_Node.Nodes.Select(node => node.ToNode()).Skip(req.Start).Take(req.Num).ToList();
            resp.HasNext = resp.Nodes.Count == req.Num;
            return resp;
        }

        [HttpPost("GetFileInfo")]
        public MapleFileInfo GetFileInfo(Node node)
        {
            var wz_Node = node.ToWzNode(wzLoader.BaseNode);
            var wz_File = wz_Node.Value as Wz_File;
            return wz_File.GetFileInfo();
        }

        [HttpPost("GetPng")]
        public PngInfo GetPng(Node node)
        {
            var wz_Node = node.ToWzNode(wzLoader.BaseNode);
            var wz_png = wz_Node.GetValue<Wz_Png>();
            return wz_png.ToPngInfo();
        }

        [HttpPost("GetUol")]
        public UolResponse GetUol(Node node)
        {
            var wz_Node = node.ToWzNode(wzLoader.BaseNode);

            if (wz_Node == null || !(wz_Node.Value is Wz_Uol)) return null;

            var wz_Uol = wz_Node.GetValue<Wz_Uol>();
            return new UolResponse
            {
                TargetNode = wz_Uol.HandleUol(wz_Node).ToNode(),
                Uol = wz_Uol.Uol
            };
        }

    }
}
