using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WzWeb.Shared;
using WzWeb.Shared.Character;

namespace WzWeb.Client.Services
{
    public interface IBrowserService
    {
        public BrowserConfig Config { get; }
        public bool HasInit { get; }
        public string NodePath { get; set; }
        public IDictionary<int, CharacterCollection> LoadedCharacters { get; set; }

        public Task Init();
        public Task<Character> GetCharacter(int id, string MotionName, int Frame);
    }
}
