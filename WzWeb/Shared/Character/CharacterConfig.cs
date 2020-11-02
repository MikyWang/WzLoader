using System;
using System.Collections.Generic;
using System.Drawing;

namespace WzWeb.Shared.Character
{
    public class CharacterConfig
    {
        public PngInfo PngInfo { get; set; }
        public Point Origin { get; set; }
        public IDictionary<string, Point> Map { get; set; }
        public string Z { get; set; }
        public string Group { get; set; }
        public string Hash { get; set; }
        public string Action { get; set; }
        public string Frame { get; set; }
        public string Delay { get; set; }
        public Point Move { get; set; }
        public string Rotate { get; set; }
        public Point Vector { get; set; }
        public string Flip { get; set; }
    }
}
