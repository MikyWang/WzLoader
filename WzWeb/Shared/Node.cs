using System;
using System.Collections.Generic;

namespace WzWeb.Shared
{
    public class Node
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public NodeType Type { get; set; }
        public Node parent { get; set; }
        public string FullPathToFile { get; set; }
    }

    public enum NodeType
    {
        Wz_File,
        Wz_Image,
        Wz_Sound,
        Wz_Png,
        Wz_Uol,
        Wz_Vector,
        Wz_Normal,
        Wz_Null
    }

}
