using System;
using Microsoft.AspNetCore.Components;
using WzWeb.Shared;

namespace WzWeb.Client.Model
{
    public class Media<T>
    {
        public T Body { get; set; }
        public PngInfo PngInfo { get; set; }
        public string Title { get; set; }
    }
}
