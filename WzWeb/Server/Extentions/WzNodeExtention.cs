using System;
using WzLib;
using WzWeb.Shared;
using System.Linq;

namespace WzWeb.Server.Extentions
{
    public static class WzNodeExtention
    {
        public static Node ToNode(this Wz_Node wz_Node)
        {
            return new Node
            {
                ParentNode = wz_Node.ParentNode?.ToNode(),
                Text = wz_Node.Text,
                Value = wz_Node.Value?.ToString()
            };
        }
    }
}
