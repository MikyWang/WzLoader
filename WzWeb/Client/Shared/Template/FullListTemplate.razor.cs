using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WzWeb.Client.Model;

namespace WzWeb.Client.Shared.Template
{

    public partial class FullListTemplate<TIndex, TItem> : IDisplaySpinner
    {
        [Parameter]
        public IDictionary<TIndex, TItem> Items { get; set; }
        [Parameter]
        public EventCallback<IDictionary<TIndex, TItem>> ItemsChanged { get; set; }
        [Parameter]
        public int ListSizePercentage { get; set; }
        [Parameter]
        public RenderFragment<KeyValuePair<TIndex, TItem>> ItemTemplate { get; set; }
        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        public bool DisplaySpinner => Items == null;

    }

}
