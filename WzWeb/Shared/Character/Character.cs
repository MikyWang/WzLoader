using System;
using System.Collections.Generic;

namespace WzWeb.Shared.Character
{
    public class Character
    {
        public int Id { get; set; }
        public CharacterInfo Info { get; set; }
        public Dictionary<string, CharacterAction> Actions { get; set; }
    }

    public class CharacterInfo
    {
        public string Islot { get; set; }
        public string Vslot { get; set; }
        public string Cash { get; set; }
    }
}
