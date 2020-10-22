using System;
using Microsoft.Extensions.Logging;
using WzLib;
using WzWeb.Server.Extentions;
using WzWeb.Shared;

namespace WzWeb.Server.Services
{
    public class WzLoader : IWzLoader
    {
        private readonly ILogger<WzLoader> logger;
        private static string FILEPATH = @"D:\文档\WzFile\Base.wz";
        //private static string FILEPATH = @"/Volumes/数据/MapleStory/Base.wz";


        public Wz_Node HeadNode { get; private set; }
        public WzLoader(ILogger<WzLoader> logger)
        {
            this.logger = logger;
            Wz_Structure wz_Structure = new Wz_Structure();
            wz_Structure.Load(FILEPATH);
            HeadNode = wz_Structure.WzNode;
        }

        public Node OutPutNode(Wz_Node wz_Node)
        {
            return wz_Node.ToNode();
        }

    }
}
