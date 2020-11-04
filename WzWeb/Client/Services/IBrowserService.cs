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
        public IDictionary<int, IDictionary<string, CharacterCollection>> LoadedCharacters { get; set; }
        public IList<int> Skins { get; }
        public IList<string> Actions { get; }
        public Character CurrentCharacter { get; }
        public Task Init();
        public Task<Character> GetCharacter(int id, string MotionName, int Frame);
        public Task<Character> GetDefaultCharacter();
        public Task<IList<int>> GetSkins();
        public Task<IList<string>> GetActions(int characterId);
    }
}
