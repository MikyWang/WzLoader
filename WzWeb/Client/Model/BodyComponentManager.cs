using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WzWeb.Shared.Character;
using WzWeb.Shared;
using System.Linq;

namespace WzWeb.Client.Model
{
    public class BodyComponentManager<T> : IBodyComponentManager where T : BodyComponentBase, new()
    {
        private readonly HttpClient httpClient;

        public BodyComponent Current { get; set; }
        public IDictionary<int, BodyComponent> Components { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int PageItemCount { get; set; } = 10;
        public bool HasNext { get; private set; } = true;

        public int PageCount => (int)MathF.Ceiling(Components.Count / PageItemCount);
        //public BodyComponent this[int key] => Components.ContainsKey(key) ? Components[key] : null;

        private BodyComponentManager() { }

        public BodyComponentManager(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<BodyComponent> GetDefaultComponent()
        {
            if (Current != null) return Current;

            var componentBase = new T();
            var component = new BodyComponent { ConfigType = componentBase.ConfigType };
            var resp = await httpClient.PostAsJsonAsync(CommonStrings.BODY_POST_COMPONENT, component);
            Current = await resp.Content.ReadFromJsonAsync<BodyComponent>();
            return Current;
        }

        public async Task<IDictionary<int, BodyComponent>> GetBodyComponentList(int number)
        {
            if (Components == null) Components = new Dictionary<int, BodyComponent>();
            if (!HasNext) return Components;

            var componentBase = new T();
            var component = new BodyComponent { ConfigType = componentBase.ConfigType };
            var req = new ListRequest<BodyComponent>
            {
                Num = number,
                Parameter = component,
                Start = Components.Count
            };
            var response = await httpClient.PostAsJsonAsync(CommonStrings.BODY_POST_COMPONENT_LIST, req);
            var components = await response.Content.ReadFromJsonAsync<ListResponse<BodyComponent>>();
            HasNext = components.HasNext;
            foreach (var item in components.Results)
            {
                if (Components.ContainsKey(item.ID)) continue;
                Components.Add(item.ID, item);
            }
            return Components;
        }


    }
}
