using System;
using System.Collections.Generic;
using System.Drawing;

namespace WzWeb.Shared.Character
{
    public class CharacterConfig
    {
        public string PngData { get; set; }
        public Point Origin { get; set; }
        public Dictionary<string, Point> Map { get; set; }
        public string Z { get; set; }
        public string Group { get; set; }
        public string Hash { get; set; }
    }
}
