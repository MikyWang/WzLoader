using System;
using WzLib;
using WzWeb.Shared;
using System.Linq;
using WzWeb.Server.Services;

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

        public static FileInfo GetFileInfo(this Wz_File wz_File)
        {
            return new FileInfo
            {
                Signature = wz_File.Header.Signature,
                Copyright = wz_File.Header.Copyright,
                FileName = wz_File.Header.FileName,
                HeaderSize = wz_File.Header.HeaderSize,
                DataSize = wz_File.Header.DataSize,
                FileSize = wz_File.Header.FileSize,
                EncryptedVersion = wz_File.Header.EncryptedVersion,
                VersionChecked = wz_File.Header.VersionChecked,
                TextEncoding = wz_File.TextEncoding.ToString(),
                ImageCount = wz_File.ImageCount
            };
        }

        public static Wz_Node ToWzNode(this Node node, Wz_Node headNode)
        {
            var wz_Node = headNode.FindNodeByPath(node.FullPath);
            if (wz_Node == null)
            {
                if (headNode.FullPath == node.FullPath)
                {
                    wz_Node = headNode;
                }
            }
            return wz_Node;
        }
    }
}
