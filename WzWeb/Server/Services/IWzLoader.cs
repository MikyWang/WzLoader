using System;
using Microsoft.Extensions.Logging;
using WzLib;
using WzWeb.Shared;

namespace WzWeb.Server.Services
{
    public interface IWzLoader : IDisposable
    {
        public Wz_Node BaseNode { get; }
        public Wz_Node CharacterNode { get; }
        public Wz_Node StringNode { get; }
    }
}
