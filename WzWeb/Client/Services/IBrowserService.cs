using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WzWeb.Client.Model;
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
        #region 状态相关
        public Character CurrentCharacter { get; }
        public BodyComponent CurrentHair { get; }
        public BodyComponent CurrentFace { get; }
        public BodyComponent CurrentCoat { get; }
        public IList<IBodyComponentManager> ComponentManagers { get; set; }
        #endregion
        public Task Init();
        public Task<Character> GetCharacterAsync(int id, string MotionName, int Frame);
        public Task<Character> LoadingCharacterAsync(int id, string motionName, int frame);
        public Task<Character> GetDefaultCharacter();
        public Task<IList<int>> GetSkins();
        public Task<IList<string>> GetActions(int characterId);
        public BodyComponentManager<T> GetBodyComponentManager<T>() where T : BodyComponentBase, new();
    }
}
