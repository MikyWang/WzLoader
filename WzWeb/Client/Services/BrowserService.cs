using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WzWeb.Shared;
using WzWeb.Shared.Character;
using System.Net.Http.Json;
using System.Linq;

namespace WzWeb.Client.Services
{
    public class BrowserService : IBrowserService
    {
        public BrowserConfig Config { get; private set; }
        public bool HasInit { get; private set; }
        public Character CurrentCharacter { get; private set; }
        public string NodePath { get; set; }
        public IDictionary<int, IDictionary<string, CharacterCollection>> LoadedCharacters { get; set; } = new Dictionary<int, IDictionary<string, CharacterCollection>>();
        public IList<int> Skins { get; private set; }
        public IList<string> Actions { get; private set; }

        private readonly IJSRuntime jSRuntime;
        private readonly HttpClient httpClient;

        public BrowserService(IJSRuntime jSRuntime, HttpClient httpClient)
        {
            this.jSRuntime = jSRuntime;
            this.httpClient = httpClient;
            _ = Init();
        }
        public async Task Init()
        {
            if (!HasInit)
            {
                var json = await jSRuntime.InvokeAsync<string>("getBrowserConfig");
                Config = JsonConvert.DeserializeObject<BrowserConfig>(json);
                HasInit = true;
            }

        }
        public async Task<Character> GetDefaultCharacter()
        {
            if (CurrentCharacter == null)
            {
                var response = await httpClient.GetFromJsonAsync<CharacterResponse>("api/character");
                var collection = response.CharacterCollection;
                LoadedCharacters.Add(collection.Id, new Dictionary<string, CharacterCollection>());
                LoadedCharacters[collection.Id].Add(collection.HeadMotion.Name, collection);
                CurrentCharacter = new Character
                {
                    Id = collection.Id,
                    CurrentFrame = "0",
                    CurrentHeadMotion = collection.HeadMotion,
                    CurrentBodyMotion = collection.BodyMotion
                };
            }
            return CurrentCharacter;
        }
        public async Task<Character> GetCharacterAsync(int id, string motionName, int frame)
        {
            CurrentCharacter = await LoadingCharacterAsync(id, motionName, frame);
            return CurrentCharacter;
        }

        public async Task<Character> LoadingCharacterAsync(int id, string motionName, int frame)
        {
            if (!LoadedCharacters.ContainsKey(id))
            {
                LoadedCharacters.Add(id, new Dictionary<string, CharacterCollection>());
            }
            if (!LoadedCharacters[id].ContainsKey(motionName))
            {
                var request = new CharacterRequest { CharacterId = id, MotionName = motionName };
                var response = await httpClient.PostAsJsonAsync<CharacterRequest>("api/character/GetCharacter", request);
                var collection = (await response.Content.ReadFromJsonAsync<CharacterResponse>()).CharacterCollection;
                if (collection == null) return null;
                LoadedCharacters[id].Add(collection.HeadMotion.Name, collection);
            }
            var extcollection = LoadedCharacters[id][motionName];
            return new Character
            {
                Id = id,
                CurrentFrame = frame.ToString(),
                CurrentHeadMotion = extcollection.HeadMotion,
                CurrentBodyMotion = extcollection.BodyMotion
            };
        }

        public async Task<IList<int>> GetSkins()
        {
            if (Skins?.Count > 0) return Skins;

            Skins = await httpClient.GetFromJsonAsync<List<int>>("api/character/GetSkins");
            return Skins;
        }
        public async Task<IList<string>> GetActions(int characterId)
        {
            if (Actions?.Count > 0) return Actions;

            Actions = await httpClient.GetFromJsonAsync<List<string>>($"api/character/GetActions/{characterId}");
            return Actions;
        }
    }
}
