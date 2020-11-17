using System;
using WzWeb.Shared;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace WzWeb.Client.Model
{
    public class PackageManager
    {
        private readonly HttpClient Http;

        public string NodePath { get; set; } = "Base";
        public int PageNum { get; set; } = 300;

        public PackageManager(HttpClient http)
        {
            Http = http;
        }

        public async Task<Node> GetNode(string path)
        {
            var node = await Http.GetFromJsonAsync<Node>($"{CommonStrings.MAPLE_GET_NODE}?path={path}");
            return node;
        }

        public async Task<ListResponse<Node>> GetNodeList(ListRequest<Node> request)
        {
            var content = await Http.PostAsJsonAsync(CommonStrings.MAPLE_POST_NODELIST, request);
            var resp = await content.Content.ReadFromJsonAsync<ListResponse<Node>>();
            return resp;
        }

        public async Task<PngInfo> GetPngInfo(Node node)
        {
            var resp = await Http.PostAsJsonAsync(CommonStrings.MAPLE_POST_PNG, node);
            return await resp.Content.ReadFromJsonAsync<PngInfo>();
        }

        public async Task<IDictionary<string, string>> GetNodeProps(Node node)
        {
            var resp = await Http.PostAsJsonAsync(CommonStrings.MAPLE_POST_GETPROPS, node);
            return await resp.Content.ReadFromJsonAsync<IDictionary<string, string>>();
        }
    }
}
