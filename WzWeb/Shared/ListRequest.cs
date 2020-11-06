using System;
using System.Collections.Generic;
namespace WzWeb.Shared
{
    public class ListRequest<T>
    {
        public T Parameter { get; set; }
        public int Start { get; set; }
        public int Num { get; set; }
    }

    public class ListResponse<T>
    {
        public IEnumerable<T> Results { get; set; }
        public bool HasNext { get; set; }
    }
}
