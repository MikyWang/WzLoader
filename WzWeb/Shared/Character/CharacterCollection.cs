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

        public CharacterAction this[string key] => Actions.ContainsKey(key) ? Actions[key] : null;
    }

    public class CharacterAction
    {
        /// <summary>0无脸 1有脸</summary>

        public string HasFace { get; set; }
        public int Id { get; set; }
        public string Delay { get; set; }
        public Dictionary<string, CharacterConfig> Configs { get; set; }

        public CharacterConfig this[string key] => Configs.ContainsKey(key) ? Configs[key] : null;


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

        public Point this[string key] => Map.ContainsKey(key) ? Map[key] : new Point();
    }

    public enum ConfigType
    {
        Head,
        Body,
        Face
    }

    public enum EarType
    {
        Normal,
        Ear,
        LefEar,
        HighLefEar
    }
}
