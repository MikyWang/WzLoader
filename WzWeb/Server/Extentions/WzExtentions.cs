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
                Value = wz_Node.Value?.ToString(),
                FullPath = wz_Node.FullPath,
                Type = wz_Node.GetNodeType()
            };
        }

        public static NodeType GetNodeType(this Wz_Node wz_Node)
        {
            var value = wz_Node.Value;
            if (value == null) return NodeType.Wz_Normal;
            if (value is Wz_File) return NodeType.Wz_File;
            if (value is Wz_Image) return NodeType.Wz_Image;
            if (value is Wz_Png) return NodeType.Wz_Png;
            if (value is Wz_Sound) return NodeType.Wz_Sound;
            if (value is Wz_Uol) return NodeType.Wz_Uol;
            if (value is Wz_Vector) return NodeType.Wz_Vector;
            return NodeType.Wz_Normal;
        }
    }
}
