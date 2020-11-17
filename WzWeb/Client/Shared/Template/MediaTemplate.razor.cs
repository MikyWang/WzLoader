using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using WzWeb.Client.Model;
namespace WzWeb.Client.Shared.Template
{
    public partial class MediaTemplate<TItem> : IDisplaySpinner
    {
        [Parameter]
        public List<Media<TItem>> Components { get; set; }
        [Parameter]
        public EventCallback<List<Media<TItem>>> ComponentsChanged { get; set; }
        [Parameter]
        public RenderFragment<TItem> Body { get; set; }

        public bool DisplaySpinner => Components == null;
    }
}
