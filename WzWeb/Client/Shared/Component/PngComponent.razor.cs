using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WzWeb.Client.Model;
using WzWeb.Client.Services;
using WzWeb.Shared;

namespace WzWeb.Client.Shared.Component
{
    public partial class PngComponent : IDisplaySpinner
    {

        [Parameter]
        public Node CurrentNode { get; set; }

        private PngInfo png;

        public bool DisplaySpinner => png == null;

        private PackageManager Manager => BrowserService.PackageManager;

        protected override async Task OnInitializedAsync()
        {
            png = await Manager.GetPngInfo(CurrentNode);
        }

    }
}
