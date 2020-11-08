using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WzWeb.Client.Services
{
    public class NotifierService : INotifierService
    {
        public event Func<string, int, Task> Notify;
        public async Task Update(string key, int value)
        {
            if (Notify != null)
            {
                await Notify.Invoke(key, value);
            }
        }
    }
}
