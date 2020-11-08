using System;
using Microsoft.Extensions.Logging;
using WzLib;
using WzWeb.Server.Extentions;
using WzWeb.Shared;
using System.Runtime.InteropServices;

namespace WzWeb.Server.Services
{
    public class WzLoader : IWzLoader
    {
        private readonly ILogger<WzLoader> logger;
        private readonly string FILEPATH = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"D:\文档\WzFile\Base.wz" : @"/Volumes/数据/MapleStory/Base.wz";


        public Wz_Node BaseNode { get; private set; }
        public Wz_Node CharacterNode { get; private set; }
        public Wz_Node StringNode { get; private set; }
        private Wz_Structure wz_Structure;

        public WzLoader(ILogger<WzLoader> logger)
        {
            this.logger = logger;
            wz_Structure = new Wz_Structure();
            wz_Structure.Load(FILEPATH);
            BaseNode = wz_Structure.WzNode;
            CharacterNode = BaseNode.SearchNode("Character");
            CharacterNode.Nodes.SortByImgID();
            StringNode = BaseNode.SearchNode("String");
            this.logger.LogInformation($"已加载Wz,baseNode={BaseNode.Text},character={CharacterNode.Text},string={StringNode.Text}");
        }
        public void Dispose()
        {
            wz_Structure.Clear();
        }
    }
}
