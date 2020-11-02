using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WzWeb.Shared.Character
{
    public class Character
    {
        public int Id { get; set; }
        public CharacterInfo HeadInfo { get; set; }
        public CharacterInfo BodyInfo { get; set; }
        public IDictionary<string, CharacterAction> HeadActions { get; set; }
        public IDictionary<string, CharacterAction> BodyActions { get; set; }

    }

    public class CharacterInfo
    {
        public string Islot { get; set; }
        public string Vslot { get; set; }
        public string Cash { get; set; }
    }
}
