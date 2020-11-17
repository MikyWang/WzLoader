using System;
using System.Collections.Generic;
using WzLib;
using WzWeb.Server.Extentions;
using WzWeb.Shared;

namespace WzWeb.Server.Services
{
    public class NodeService : INodeService
    {
        private readonly IWzLoader wzLoader;
        private object _lock = new object();

        public NodeService(IWzLoader wzLoader)
        {
            this.wzLoader = wzLoader;
        }

        public Wz_Node BaseNode => wzLoader.BaseNode;

        public IDictionary<string, string> GetNodeProperties(Node cnode)
        {
            lock (_lock)
            {
                var node = cnode.ToWzNode(BaseNode);
                var props = new Dictionary<string, string>();
                var nodes = node?.Nodes;
                if (nodes == null || nodes.Count == 0)
                {
                    string value = node.GetValueString();
                    props.Add("self", value);
                    return props;
                }
                foreach (var item in nodes)
                {
                    string value = item.GetValueString();
                    props.Add(item.Text, value);
                }
                return props;
            }
        }
    }
}
