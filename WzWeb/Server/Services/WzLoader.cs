using System;
using Microsoft.Extensions.Logging;
using WzLib;

namespace WzWeb.Server.Services
{
    public class WzLoader : IWzLoader
    {
        public WzLoader()
        {
        }

        public void OutPutNode(Wz_Node wz_Node, ILogger logger)
        {
            logger.LogDebug("asdad");
        }
    }
}
