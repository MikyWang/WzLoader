using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WzWeb.Client.Model;
using WzWeb.Shared;

namespace WzWeb.Client.Shared.Component
{
    public partial class PropertiesComponent : IDisplaySpinner
    {
        [Parameter]
        public Node Node
        {
            get
            {
                return node;
            }
            set
            {
                node = value;
                _ = RefreshItems();
            }
        }
        [Parameter]
        public EventCallback<Node> NodeChanged { get; set; }

        [Parameter]
        public Func<Node, Task> NavigateCB { get; set; }

        private Node node;

        private IDictionary<string, string> Items;
        private PackageManager Manager => BrowserService.PackageManager;

        public bool DisplaySpinner => Node == null || Items == null;

        private async Task RefreshItems()
        {
            Items = await Manager.GetNodeProps(Node);
            StateHasChanged();
        }

        private async Task Navigate(string link)
        {
            var node = await Manager.GetNode(link);
            await NavigateCB.Invoke(node);
        }

    }
}
