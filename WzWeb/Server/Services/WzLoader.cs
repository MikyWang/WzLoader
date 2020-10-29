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


        public Wz_Node BaseNode { get; private set; }
        public Wz_Node CharacterNode { get; private set; }
        public Wz_Node StringNode { get; private set; }

        public WzLoader(ILogger<WzLoader> logger)
        {
            this.logger = logger;
            Wz_Structure wz_Structure = new Wz_Structure();
            wz_Structure.Load(FILEPATH);
            BaseNode = wz_Structure.WzNode;
            CharacterNode = BaseNode.SearchNode("Character");
            StringNode = BaseNode.SearchNode("String");
            this.logger.LogInformation($"已加载Wz,baseNode={BaseNode.Text},character={CharacterNode.Text},string={StringNode.Text}");
        }

    }
}
