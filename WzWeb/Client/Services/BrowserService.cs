using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using WzWeb.Shared;

namespace WzWeb.Client.Services
{
    public class BrowserService : IBrowserService
    {
        public BrowserConfig BrowserConfig { get; private set; }

        public bool HasInit { get; private set; }

        private readonly IJSRuntime jSRuntime;

        public BrowserService(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
            _ = Init();
        }

        public async Task Init()
        {
            if (!HasInit)
            {
                var json = await jSRuntime.InvokeAsync<string>("getBrowserConfig");
                BrowserConfig = JsonConvert.DeserializeObject<BrowserConfig>(json);
                HasInit = true;
            }

        }
    }
}
