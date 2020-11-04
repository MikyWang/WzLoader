using System.Collections.Generic;
using System.Drawing;

namespace WzWeb.Shared.Character
{
    public class CharacterRequest
    {
        public int CharacterId { get; set; }
        public string MotionName { get; set; }
    }

    public class CharacterResponse
    {
        public CharacterCollection CharacterCollection { get; set; }
    }

    public class CharacterCollection
    {
        public int Id { get; set; }
        public CharacterInfo HeadInfo { get; set; }
        public CharacterInfo BodyInfo { get; set; }
        public CharacterMotion HeadMotion { get; set; }
        public CharacterMotion BodyMotion { get; set; }
    }

    public class CharacterInfo
    {
        public string Islot { get; set; }
        public string Vslot { get; set; }
        public string Cash { get; set; }
    }

    public class CharacterMotion
    {
        public string Name { get; set; }
        public Dictionary<string, CharacterAction> Actions { get; set; }
    }

    public class CharacterAction
    {
        public int Id { get; set; }
        public Dictionary<string, CharacterConfig> Configs { get; set; }
    }

    public class CharacterConfig
    {
        public string Name { get; set; }
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
