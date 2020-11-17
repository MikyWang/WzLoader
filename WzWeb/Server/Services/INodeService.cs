using System;
using System.Collections.Generic;
using WzLib;
using WzWeb.Shared;

namespace WzWeb.Server.Services
{
    public interface INodeService
    {
        public Wz_Node BaseNode { get; }

        public IDictionary<string, string> GetNodeProperties(Node node);
    }
}
