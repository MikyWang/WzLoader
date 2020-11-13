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
using WzWeb.Client.Model;

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

        public BodyComponent CurrentHair => GetBodyComponentManager<Hair>().Current;
        public BodyComponent CurrentFace => GetBodyComponentManager<Face>().Current;
        public BodyComponent CurrentCoat => GetBodyComponentManager<Coat>().Current;
        public BodyComponent CurrentPants => GetBodyComponentManager<Pants>().Current;

        public IList<IBodyComponentManager> ComponentManagers { get; set; }

        private readonly IJSRuntime jSRuntime;
        private readonly HttpClient httpClient;

        //public BodyComponentManager<Hair> HairManager { get; private set; }

        public BrowserService(IJSRuntime jSRuntime, HttpClient httpClient)
        {
            this.jSRuntime = jSRuntime;
            this.httpClient = httpClient;
            _ = Init();

            ComponentManagers = new List<IBodyComponentManager>
            {
                new BodyComponentManager<Hair>(httpClient),
                new BodyComponentManager<Face>(httpClient),
                new BodyComponentManager<Coat>(httpClient),
                new BodyComponentManager<Pants>(httpClient)
            };

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
            if (CurrentFace == null)
            {
                await GetBodyComponentManager<Face>().GetDefaultComponent();
            }

            if (CurrentHair == null)
            {
                await GetBodyComponentManager<Hair>().GetDefaultComponent();
            }
            if (CurrentCoat == null)
            {
                await GetBodyComponentManager<Coat>().GetDefaultComponent();
            }
            if (CurrentPants == null)
            {
                await GetBodyComponentManager<Pants>().GetDefaultComponent();
            }

            if (CurrentCharacter == null)
            {
                var response = await httpClient.GetFromJsonAsync<CharacterResponse>(CommonStrings.CHARACTER);
                var collection = response.CharacterCollection;
                LoadedCharacters.Add(collection.Id, new Dictionary<string, CharacterCollection>());
                LoadedCharacters[collection.Id].Add(collection.HeadMotion.Name, collection);
                CurrentCharacter = new Character
                {
                    Id = collection.Id,
                    CurrentFrame = "0",
                    CurrentFaceFrame = "0",
                    CurrentHeadMotion = collection.HeadMotion,
                    CurrentBodyMotion = collection.BodyMotion,
                    CurrentFaceMotion = CurrentFace.Motion,
                    CurrentHairMotion = CurrentHair.Motion,
                    CurrentCoatMotion = CurrentCoat.Motion,
                    CurrentPantsMotion = CurrentPants.Motion
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
                var response = await httpClient.PostAsJsonAsync<CharacterRequest>(CommonStrings.CHARACTER_POST_CHARACTER, request);
                var collection = (await response.Content.ReadFromJsonAsync<CharacterResponse>()).CharacterCollection;
                if (collection == null) return null;
                LoadedCharacters[id].Add(collection.HeadMotion.Name, collection);
            }
            var extcollection = LoadedCharacters[id][motionName];
            return new Character
            {
                Id = id,
                CurrentFrame = frame.ToString(),
                CurrentFaceFrame = "0",
                CurrentHeadMotion = extcollection.HeadMotion,
                CurrentBodyMotion = extcollection.BodyMotion,
                CurrentFaceMotion = CurrentFace.Motion,
                CurrentHairMotion = CurrentHair.Motion,
                CurrentCoatMotion = CurrentCoat.Motion,
                CurrentPantsMotion = CurrentPants.Motion
            };
        }


        public async Task<IList<int>> GetSkins()
        {
            if (Skins?.Count > 0) return Skins;

            Skins = await httpClient.GetFromJsonAsync<List<int>>(CommonStrings.CHARACTER_GET_SKIN_LIST);
            return Skins;
        }
        public async Task<IList<string>> GetActions(int characterId)
        {
            if (Actions?.Count > 0) return Actions;

            Actions = await httpClient.GetFromJsonAsync<List<string>>($"{CommonStrings.CHARACTER_GET_ACTION_LIST}/{characterId}");
            return Actions;
        }

        public BodyComponentManager<T> GetBodyComponentManager<T>() where T : BodyComponentBase, new()
        {
            foreach (var item in ComponentManagers)
            {
                var type = item.GetType();
                var typeargs = type.GetGenericArguments();
                foreach (var arg in typeargs)
                {
                    if (arg == typeof(T)) return item as BodyComponentManager<T>;
                }
            }
            return null;
        }
    }
}
