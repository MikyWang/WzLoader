using System;
using Microsoft.Extensions.Logging;
using WzLib;
using WzWeb.Shared;

namespace WzWeb.Server.Services
{
    public interface IWzLoader
    {
        public Wz_Node BaseNode { get; }
        public Node OutPutNode(Wz_Node wz_Node);
    }
}
