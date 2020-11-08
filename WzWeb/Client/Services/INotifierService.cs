using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WzWeb.Client.Services
{
    interface INotifierService
    {
        public event Func<string, int, Task> Notify;
        public Task Update(string key = "", int value = 0);
    }
}
