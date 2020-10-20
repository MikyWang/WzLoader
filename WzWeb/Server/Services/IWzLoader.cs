using System;
using Microsoft.Extensions.Logging;
using WzLib;
namespace WzWeb.Server.Services
{
    public interface IWzLoader
    {
        public Wz_Node HeadNode { get; }
        public void OutPutNode(Wz_Node wz_Node);
    }
}
