using System;
using System.Collections.Generic;

namespace WzWeb.Shared.Character
{
    public class CharacterMotion
    {
        public string Name { get; set; }
        public IDictionary<string, CharacterAction> Motions { get; set; }
    }
}
