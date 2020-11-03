using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WzWeb.Shared;
using WzWeb.Shared.Character;
using System.Net.Http.Json;

namespace WzWeb.Client.Services
{
    public class BrowserService : IBrowserService
    {
        public BrowserConfig Config { get; private set; }

        public bool HasInit { get; private set; }

        public string NodePath { get; set; }
        public IDictionary<int, CharacterCollection> LoadedCharacters { get; set; } = new Dictionary<int, CharacterCollection>();

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

        public async Task<Character> GetCharacter(int id, string motionName, int frame)
        {
            if (!LoadedCharacters.ContainsKey(id))
            {
                var request = new CharacterRequest { CharacterId = id, MotionName = motionName };
                var response = await httpClient.PostAsJsonAsync<CharacterRequest>("api/character/GetCharacter", request);
                var collection = (await response.Content.ReadFromJsonAsync<CharacterResponse>()).CharacterCollection;
                LoadedCharacters.Add(collection.Id, collection);
            }
            var extcollection = LoadedCharacters[id];

            return new Character
            {
                Id = id,
                CurrentFrame = frame,
                CurrentHeadMotion = extcollection.HeadMotion,
                CurrentBodyMotion = extcollection.BodyMotion
            };
        }
    }
}
