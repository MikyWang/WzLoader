using System;
using Microsoft.AspNetCore.Components;
using WzWeb.Client.Model;

namespace WzWeb.Client.Shared
{
    public partial class SpinnerContainer : IDisplaySpinner
    {
        [Parameter]
        public bool DisplaySpinner { get; set; } = false;
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
