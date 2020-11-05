using System;
using WzLib;
using WzWeb.Shared;
using WzWeb.Server.Services;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using WzWeb.Shared.Character;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WzWeb.Server.Extentions
{
    public static class WzNodeExtention
    {
        public static Node ToNode(this Wz_Node wz_Node)
        {

            return new Node
            {
                Text = wz_Node.Text,
                Value = wz_Node.Value?.ToString(),
                FullPathToFile = wz_Node.FullPathToFile,
                Type = wz_Node.GetNodeType()
            };
        }
        public static NodeType GetNodeType(this Wz_Node wz_Node)
        {
            var value = wz_Node.Value;
            if (value == null) return NodeType.Wz_Null;
            if (value is Wz_File) return NodeType.Wz_File;
            if (value is Wz_Image) return NodeType.Wz_Image;
            if (value is Wz_Png) return NodeType.Wz_Png;
            if (value is Wz_Sound) return NodeType.Wz_Sound;
            if (value is Wz_Uol) return NodeType.Wz_Uol;
            if (value is Wz_Vector) return NodeType.Wz_Vector;
            return NodeType.Wz_Normal;
        }

        public static Wz_Node GetImageNode(this Wz_Node wz_Node)
        {
            var image = wz_Node.GetValue<Wz_Image>();
            if (image.TryExtract())
            {
                return image.Node;
            }
            return null;
        }

        public static MapleFileInfo GetFileInfo(this Wz_File wz_File)
        {
            return new MapleFileInfo
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
            var wz_Node = headNode.SearchNode(node.FullPathToFile);
            if (wz_Node == null)
            {
                if (headNode.FullPathToFile == node.FullPathToFile)
                {
                    wz_Node = headNode;
                }
            }
            return wz_Node;
        }

        public static Wz_Node SearchNode(this Wz_Node wz_Node, string fullPathToFile)
        {
            if (fullPathToFile == null || wz_Node.FullPathToFile == fullPathToFile) return wz_Node;
            var pathes = fullPathToFile.Split('\\').ToList();
            if (pathes[0] != wz_Node.FullPathToFile) pathes.Insert(0, wz_Node.FullPathToFile);
            return SearchNode(wz_Node, pathes);

        }

        public static PngInfo ToPngInfo(this Wz_Png wz_Png)
        {
            string base64;
            using (var bmp = wz_Png.ExtractPng())
            {
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] data = ms.ToArray();
                    base64 = Convert.ToBase64String(data);
                }
            }
            return new PngInfo
            {
                Width = wz_Png.Width,
                Height = wz_Png.Height,
                Base64Data = base64,
                DataLength = wz_Png.DataLength,
                Form = wz_Png.Form
            };
        }

        private static Wz_Node SearchNode(this Wz_Node wz_Node, List<string> pathes)
        {
            Wz_Node node;
            pathes.RemoveAt(0);
            if (pathes.Count == 0) return wz_Node;
            node = wz_Node.FindNodeByPath(pathes[0]);
            if (node == null)
            {
                var value = wz_Node.GetValue<Wz_Image>();
                if (value != null)
                {
                    value.TryExtract();
                    wz_Node = value.Node;
                    node = wz_Node.FindNodeByPath(pathes[0]);
                }
            }
            else
            {
                var value = node.GetValue<Wz_Image>();
                if (value != null)
                {
                    value.TryExtract();
                    node = value.Node;

                }
            }
            return SearchNode(node, pathes);
        }

        public static CharacterInfo GetCharacterInfo(this Wz_Node wz_Node)
        {
            var infoNode = wz_Node.Nodes["info"];
            if (infoNode == null) return null;

            return new CharacterInfo
            {
                Islot = infoNode.Nodes["islot"].Value.ToString(),
                Vslot = infoNode.Nodes["vslot"].Value.ToString(),
                Cash = infoNode.Nodes["cash"].Value.ToString()
            };
        }

        public static PngInfo GetPngInfo(this Wz_Node wz_Node, Wz_Node baseNode)
        {
            var nodes = wz_Node.Nodes;
            var inLinkNode = nodes["_inlink"];
            var outLinkNode = nodes["_outlink"];
            PngInfo pngInfo;
            if (inLinkNode != null)
            {
                var link = inLinkNode.Value.ToString().Replace('/', '\\');
                var node = wz_Node.GetNodeWzImage().Node.SearchNode(link);
                pngInfo = node.GetValue<Wz_Png>().ToPngInfo();
            }
            else if (outLinkNode != null)
            {
                var link = outLinkNode.Value.ToString().Replace('/', '\\');
                var node = baseNode.SearchNode(link);
                pngInfo = node.GetValue<Wz_Png>().ToPngInfo();
            }
            else
            {
                pngInfo = wz_Node.GetValue<Wz_Png>()?.ToPngInfo();
            }
            return pngInfo;
        }

        public static CharacterMotion GetCharacterMotion(this Wz_Node wz_Node, Wz_Node baseNode)
        {
            var nodes = wz_Node.Nodes;
            var actions = new Dictionary<string, CharacterAction>();
            foreach (var acNode in nodes)
            {
                actions.Add(acNode.Text, acNode.GetCharacterAction(baseNode));
            }
            return new CharacterMotion { Name = wz_Node.Text, Actions = actions };
        }

        public static CharacterAction GetCharacterAction(this Wz_Node wz_Node, Wz_Node baseNode)
        {
            var nodes = wz_Node.Nodes.Where(node => node.Text != "face" || node.Text != "delay");
            var configs = new Dictionary<string, CharacterConfig>();
            foreach (var acNode in nodes)
            {
                configs.Add(acNode.Text, acNode.GetCharacterConfig(baseNode));
            }

            return new CharacterAction
            {
                Id = int.Parse(wz_Node.Text),
                Configs = configs,
                Delay = wz_Node.Nodes["delay"]?.Value.ToString(),
                HasFace = wz_Node.Nodes["face"]?.Value.ToString()
            };
        }

        public static CharacterConfig GetCharacterConfig(this Wz_Node wz_Node, Wz_Node baseNode)
        {
            if (wz_Node.Value != null && wz_Node.Value is Wz_Uol)
            {
                wz_Node = wz_Node.GetValue<Wz_Uol>().HandleUol(wz_Node);
            }
            var nodes = wz_Node.Nodes;
            var config = new CharacterConfig
            {
                Name = wz_Node.Text,
                Origin = nodes["origin"]?.GetValue<Wz_Vector>(),
                PngInfo = wz_Node.GetPngInfo(baseNode),
                Group = nodes["group"]?.Value?.ToString(),
                Hash = nodes["_hash"]?.Value?.ToString(),
                Map = wz_Node.GetMap(),
                Z = nodes["z"]?.Value?.ToString(),
                Action = nodes["action"]?.Value?.ToString(),
                Delay = nodes["delay"]?.Value?.ToString(),
                Frame = nodes["frame"]?.Value?.ToString(),
                Move = nodes["move"]?.GetValue<Wz_Vector>(),
                Rotate = nodes["rotate"]?.Value?.ToString(),
                Vector = nodes["vector"]?.GetValue<Wz_Vector>(),
                Flip = nodes["flip"]?.Value?.ToString()
            };
            if (config.Action != null)
            {
                var link = $"{config.Action}\\{config.Frame}";
                var node = wz_Node.GetNodeWzImage().Node.SearchNode(link);
                var baseConfig = node.GetCharacterConfig(baseNode);
                config.Origin = baseConfig.Origin;
                config.Map = baseConfig.Map;
                config.Z = baseConfig.Z;
                config.Group = baseConfig.Group;
                config.Hash = baseConfig.Hash;
                config.PngInfo = baseConfig.PngInfo;
            }
            return config;
        }

        public static IDictionary<string, Point> GetMap(this Wz_Node wz_Node)
        {
            var map = new Dictionary<string, Point>();
            var mapNode = wz_Node.Nodes["map"];
            if (mapNode == null) return null;
            foreach (var node in mapNode.Nodes)
            {
                map.Add(node.Text, node.GetValue<Wz_Vector>());
            }
            return map;
        }
    }
}
