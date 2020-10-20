using System;
using Microsoft.Extensions.Logging;
using WzLib;

namespace WzWeb.Server.Services
{
    public class WzLoader : IWzLoader
    {
        private readonly ILogger<WzLoader> logger;
        private static string FILEPATH = @"D:\文档\WzFile\Base.wz";

        public Wz_Node HeadNode { get; private set; }
        public WzLoader(ILogger<WzLoader> logger)
        {
            this.logger = logger;
            Wz_Structure wz_Structure = new Wz_Structure();
            wz_Structure.Load(FILEPATH);
            HeadNode = wz_Structure.WzNode;
        }

        public void OutPutNode(Wz_Node wz_Node)
        {
            logger.LogInformation(wz_Node.Text);
        }
    }
}
