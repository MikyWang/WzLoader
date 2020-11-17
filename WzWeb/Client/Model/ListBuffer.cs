using System;
namespace WzWeb.Client.Model
{
    public class ListBuffer<TKey, TValue> where TValue : new()
    {
        public TKey key { get; set; }
        public TValue List { get; set; } = new TValue();
        public bool used { get; set; } = false;
    }
}
