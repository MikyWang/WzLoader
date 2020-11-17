using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WzWeb.Client.Model;

namespace WzWeb.Client.Shared.Component
{
    public partial class BreadCrumbComponent : IDisplaySpinner
    {
        [Parameter]
        public string Path { get; set; }
        [Parameter]
        public EventCallback<string> PathChanged { get; set; }
        [Parameter]
        public Func<Task> NavigateCB { get; set; }

        public bool DisplaySpinner => Path == null;

        private IList<string> Links => (Path.StartsWith("Base") ? Path : Path.Insert(0, "Base\\")).Split('\\').ToList();

        private void Navigate(int index)
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(Links[0]);
            for (int i = 1; i <= index; i++)
            {
                strBuilder.Append($"\\{Links[i]}");
            }
            PathChanged.InvokeAsync(strBuilder.ToString());
            if (NavigateCB == null) return;
            NavigateCB.Invoke();
        }

    }
}
