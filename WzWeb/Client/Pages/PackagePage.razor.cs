using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WzWeb.Client.Model;
using WzWeb.Shared;
using System.Linq;
using Microsoft.AspNetCore.Components;
using WzWeb.Client.Services;
using WzWeb.Client.Shared.Component;
using System.Threading;

namespace WzWeb.Client.Pages
{
    public partial class PackagePage : IDisplaySpinner, IDisposable
    {
        private IDictionary<string, Node> items;
        private IDictionary<string, Node> searchItems;
        private bool hasNext;
        private Node currentNode;
        private ListRequest<Node> req;
        private List<Media<Node>> mediaComponents;
        private LocalSearchComponent searchCompRef;
        private Timer mediatimer;

        public PackageManager Manager => BrowserService.PackageManager;
        private string Path => Manager.NodePath;
        public bool DisplaySpinner => Path == null || items == null;

        //public bool IsMedia => currentNode.Type != NodeType.Wz_File
        //                        && !items.Any(x => x.Value.Type == NodeType.Wz_File
        //                        || x.Value.Type == NodeType.Wz_Image
        //                        || x.Value.Type == NodeType.Wz_Null);

        public bool IsMedia => currentNode?.Type == NodeType.Wz_Null && searchItems.Any(item => item.Value.Type != NodeType.Wz_Null);

        protected async override Task OnInitializedAsync()
        {
            currentNode = await Manager.GetNode(Path);
            req = new ListRequest<Node> { Start = 0, Parameter = currentNode, Num = 100 };
            items = await LoadingItems(0);
            searchItems = items;
            mediatimer = new Timer(new TimerCallback(LoadingMedias), null, 100, 500);
            await BrowserService.DebugInfo("init");
        }

        private async Task RefreshList()
        {
            hasNext = false;
            Manager.NodePath = currentNode.FullPathToFile;
            items = await LoadingItems(0);
            searchItems = items;
            searchCompRef.Clear();
            mediaComponents = await RefreshMedia();
            StateHasChanged();
        }

        #region 计时器任务
        private void LoadingMedias(object obj)
        {
            if (!IsMedia) return;
            InvokeAsync(async () =>
            {
                if (mediaComponents == null)
                {
                    mediaComponents = await RefreshMedia();
                    StateHasChanged();
                    return;
                }
                if (mediaComponents.Count == searchItems.Count)
                {
                    mediatimer.Change(Timeout.Infinite, 500);
                    return;
                }
                if (mediaComponents.Count < searchItems.Count)
                {
                    var medias = await RefreshMedia(mediaComponents.Count);
                    mediaComponents.AddRange(medias);
                    StateHasChanged();
                    return;
                }

            });
        }
        #endregion

        private async Task<List<Media<Node>>> RefreshMedia(int start = 0)
        {
            var medias = new List<Media<Node>>();

            if (IsMedia)
            {
                foreach (var item in searchItems.Skip(start).Take(1))
                {
                    var media = new Media<Node>
                    {
                        Body = item.Value,
                        Title = item.Key
                    };
                    if (item.Value.Type == NodeType.Wz_Png)
                    {
                        media.PngInfo = await Manager.GetPngInfo(item.Value);
                    }
                    medias.Add(media);
                }
            }
            return medias;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (hasNext)
            {
                var results = await LoadingItems(items.Count);
                foreach (var item in results)
                {
                    if (items.ContainsKey(item.Key)) continue;
                    items.Add(item);
                }
                searchItems = items;
                StateHasChanged();
            }
            await BrowserService.DebugInfo("render");

        }

        private async Task<IDictionary<string, Node>> LoadingItems(int start)
        {
            req.Parameter = currentNode;
            req.Start = start;
            var resp = await Manager.GetNodeList(req);
            hasNext = resp.HasNext;
            return resp.Results.ToDictionary(x => x.Text);
        }

        private async Task NavigateCB()
        {
            mediatimer.Change(Timeout.Infinite, 100);
            currentNode = await Manager.GetNode(Path);
            mediatimer.Change(100, 500);
            await RefreshList();
        }

        private async Task SearchNode(string text)
        {
            await InvokeAsync(async () =>
            {
                mediatimer.Change(Timeout.Infinite, 0);
                if (string.IsNullOrEmpty(text))
                {
                    searchItems = items;
                }
                else
                {
                    var keys = items.Keys.Where(x => x.ToLower().Contains(text.ToLower()));
                    searchItems = new Dictionary<string, Node>();
                    foreach (var key in keys)
                    {
                        searchItems.Add(key, items[key]);
                    }
                }
                mediaComponents = await RefreshMedia();
                mediatimer.Change(100, 500);
                StateHasChanged();
            });
        }

        private async Task Navigate(Node node)
        {
            mediatimer.Change(Timeout.Infinite, 0);
            currentNode = node;
            mediatimer.Change(100, 500);
            await RefreshList();
        }

        public void Dispose()
        {
            mediatimer.Change(Timeout.Infinite, 100);
            mediatimer.Dispose();
        }
    }
}
