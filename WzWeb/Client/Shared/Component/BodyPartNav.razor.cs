using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WzWeb.Shared.Character;

namespace WzWeb.Client.Shared.Component
{
    public partial class BodyPartNav
    {
        private Dictionary<ConfigType, string> Navs = new Dictionary<ConfigType, string>
        {
            {ConfigType.Face,"脸型" },{ConfigType.Hair,"发型"},{ConfigType.Coat,"上衣"},{ConfigType.Pants,"裤子"}
        };

        private Func<ConfigType, string> ActiveClass => nav => CurrentNav == nav ? "active" : "";

        [Parameter]
        public ConfigType CurrentNav { get; set; }
        [Parameter]
        public EventCallback<ConfigType> CurrentNavChanged { get; set; }
        [Parameter]
        public Func<ConfigType, Task> NavigateCB { get; set; }

        public async Task Navigate(ConfigType configType)
        {
            await CurrentNavChanged.InvokeAsync(configType);

            if (NavigateCB == null) return;
            await NavigateCB.Invoke(configType);
        }


    }

}
