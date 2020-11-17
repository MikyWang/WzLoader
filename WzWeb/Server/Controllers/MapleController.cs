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
    public class MapleController : ControllerBase
    {
        private readonly IWzLoader wzLoader;
        private readonly ILogger<MapleController> logger;
        private readonly object lockObject = new object();
        private readonly INodeService nodeService;

        public MapleController(ILogger<MapleController> logger, IWzLoader wzLoader, INodeService nodeService)
        {
            this.logger = logger;
            this.wzLoader = wzLoader;
            this.nodeService = nodeService;
        }

        [HttpGet("GetNode")]
        public Node Get(string path)
        {
            var head = wzLoader.BaseNode;
            var wz_node = head.SearchNode(path);
            return wz_node.ToNode();
        }

        [HttpPost("GetNodeList")]
        public ListResponse<Node> GetNodeList(ListRequest<Node> req)
        {
            lock (lockObject)
            {
                var node = req.Parameter;
                var wz_Node = node.ToWzNode(wzLoader.BaseNode);
                var resp = new ListResponse<Node>();
                resp.Results = wz_Node.Nodes.Select(node => node.ToNode()).Skip(req.Start).Take(req.Num).ToList();
                resp.HasNext = resp.Results.Count == req.Num;
                return resp;
            }
        }

        [HttpPost("GetNodeProps")]
        public IDictionary<string, string> GetNodeProps(Node node)
        {
            return nodeService.GetNodeProperties(node);
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
            lock (lockObject)
            {
                var wz_Node = node.ToWzNode(wzLoader.BaseNode);
                return wz_Node.GetPngInfo(wzLoader.BaseNode);
            }
        }

        [HttpPost("GetUol")]
        public UolResponse GetUol(UolRequest request)
        {
            var wz_Node = request.Node.ToWzNode(wzLoader.BaseNode);

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
