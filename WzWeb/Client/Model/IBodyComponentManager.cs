using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WzWeb.Shared.Character;

namespace WzWeb.Client.Model
{

    public interface IBodyComponentManager
    {

        public BodyComponent Current { get; set; }
        public IDictionary<int, BodyComponent> Components { get; set; }

        public int CurrentPage { get; set; }
        public int PageItemCount { get; set; }
        public bool HasNext { get; }
        public int PageCount { get; }

        public Task<BodyComponent> GetDefaultComponent();
        public Task<IDictionary<int, BodyComponent>> GetBodyComponentList(int number);

        public BodyComponent this[int key] => Components.ContainsKey(key) ? Components[key] : null;
        public bool PageEnoughed => (CurrentPage + 1) * PageItemCount < Components.Count;

    }

}
