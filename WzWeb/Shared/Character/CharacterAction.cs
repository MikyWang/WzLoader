using System;
using System.Collections.Generic;

namespace WzWeb.Shared.Character
{
    public class CharacterAction
    {
        public int Id { get; set; }
        public IDictionary<string, CharacterConfig> Configs { get; set; }

    }
}
