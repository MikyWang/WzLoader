using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WzWeb.Client.Model;
using WzWeb.Shared.Character;

namespace WzWeb.Client.Shared.Component
{
    public partial class BodyPartComponent : IDisplaySpinner
    {
        [Parameter]
        public IBodyComponentManager Manager
        {
            get => manager;
            set
            {
                manager = value;
                _ = CalculatePage();
            }
        }
        [Parameter]
        public EventCallback<IBodyComponentManager> ManagerChanged { get; set; }

        private IDictionary<int, BodyComponent> componentList;
        private IBodyComponentManager manager;

        private Func<BodyComponent, string> IsActive => (BodyComponent comp) => comp.ID == Manager.Current?.ID ? "active" : string.Empty;
        private int CurrentPage => Manager.CurrentPage;
        private int PageItemCount => Manager.PageItemCount;
        private int CurrentPageIndex => (CurrentPage - 1) * PageItemCount;
        public bool DisplaySpinner => componentList == null;

        protected async override Task OnInitializedAsync()
        {
            componentList = await Manager.GetBodyComponentList(10);

        }

        private async Task ChooseComponent(BodyComponent component)
        {
            Manager.Current = component;
            StateHasChanged();
            await Notifier.Update();
        }

        private async Task CalculatePage()
        {
            await BrowserService.DebugInfo(Manager.PageEnoughed);
            await InvokeAsync(() =>
            {
                componentList = Manager.Components.Skip(CurrentPageIndex).Take(PageItemCount).ToDictionary(item => item.Key, item => item.Value);
                if (Manager.PageEnoughed) return;
                _ = GetItems();
            });
        }

        private async Task GetItems()
        {
            if (!Manager.HasNext) return;
            await Manager.GetBodyComponentList(Manager.PageItemCount);
        }

        private Task Search(string searchText)
        {
            return null;
        }

        private Task SearchFromServer(string searchText)
        {
            return null;
        }
    }
}
