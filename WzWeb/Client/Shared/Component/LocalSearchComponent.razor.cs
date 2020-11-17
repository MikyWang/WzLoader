using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace WzWeb.Client.Shared.Component
{
    public partial class LocalSearchComponent
    {
        private string searchText;
        [Parameter]
        public bool HasNext { get; set; }
        [Parameter]
        public Func<string, Task> Search { get; set; }

        public void Clear()
        {
            searchText = string.Empty;
        }
    }
}
